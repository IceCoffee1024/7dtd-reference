using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

namespace Platform.Steam
{
	// Token: 0x02001CCE RID: 7374
	public class NetworkServerSteam : IPlatformNetworkServer, INetworkServer
	{
		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600DAA6 RID: 55974 RVA: 0x004E4066 File Offset: 0x004E2266
		public bool ServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.protoManager.IsServer && this.handlerThread != null;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x0600DAA7 RID: 55975 RVA: 0x004E4080 File Offset: 0x004E2280
		public bool SteamGameServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.owner.ServerListAnnouncer.GameServerInitialized;
			}
		}

		// Token: 0x0600DAA8 RID: 55976 RVA: 0x004E4094 File Offset: 0x004E2294
		public NetworkServerSteam(IPlatform _owner, IProtocolManagerProtocolInterface _protoManager)
		{
			this.owner = _owner;
			this.protoManager = _protoManager;
			this.owner.Api.ClientApiInitialized += delegate
			{
				this.m_P2PSessionRequest = Callback<P2PSessionRequest_t>.CreateGameServer(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.P2PSessionRequest));
			};
		}

		// Token: 0x0600DAA9 RID: 55977 RVA: 0x004E4148 File Offset: 0x004E2348
		public NetworkConnectionError StartServer(int _basePort, string _password)
		{
			if (this.ServerRunning)
			{
				Log.Error("[Steamworks.NET] NET: Server already running");
				return NetworkConnectionError.AlreadyConnectedToServer;
			}
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
			this.handlerThread = ThreadManager.StartThread("SteamNetworkingServer", new ThreadManager.ThreadFunctionDelegate(this.threadHandlerMethod), null, null, true, false);
			Log.Out("[Steamworks.NET] NET: Server started");
			return NetworkConnectionError.NoError;
		}

		// Token: 0x0600DAAA RID: 55978 RVA: 0x004E41A7 File Offset: 0x004E23A7
		public void SetServerPassword(string _password)
		{
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
		}

		// Token: 0x0600DAAB RID: 55979 RVA: 0x004E41BC File Offset: 0x004E23BC
		public void StopServer()
		{
			if (!this.ServerRunning)
			{
				return;
			}
			this.handlerThread.WaitForEnd(30);
			this.handlerThread = null;
			this.checkConnections.Clear();
			foreach (KeyValuePair<CSteamID, NetworkServerSteam.ConnectionInformation> keyValuePair in this.connections)
			{
				if (keyValuePair.Value.State != NetworkServerSteam.EConnectionState.Disconnected)
				{
					keyValuePair.Value.State = NetworkServerSteam.EConnectionState.Disconnected;
					SteamGameServerNetworking.CloseP2PSessionWithUser(keyValuePair.Key);
				}
			}
			this.connections.Clear();
			this.sendBufs.Clear();
			this.sendBufsUnreliable.Clear();
			this.acceptQueue.Clear();
			this.dropQueue.Clear();
			this.disconnectQueue.Clear();
			Log.Out("[Steamworks.NET] NET: Server stopped");
		}

		// Token: 0x0600DAAC RID: 55980 RVA: 0x004E42A4 File Offset: 0x004E24A4
		public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
		{
			CSteamID csteamID = new CSteamID(((UserIdentifierSteam)_clientInfo.PlatformId).SteamId);
			ThreadManager.StartCoroutine(this.dropLater(csteamID, 0.2f));
		}

		// Token: 0x0600DAAD RID: 55981 RVA: 0x004E42DA File Offset: 0x004E24DA
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator dropLater(CSteamID _id, float _delay)
		{
			yield return new WaitForSeconds(_delay);
			if (!this.ServerRunning)
			{
				yield break;
			}
			NetworkServerSteam.ConnectionInformation connectionInformation;
			if (this.connections.TryGetValue(_id, out connectionInformation))
			{
				string text = "[Steamworks.NET] NET: Dropping client: ";
				CSteamID csteamID = _id;
				Log.Out(text + csteamID.ToString());
				connectionInformation.State = NetworkServerSteam.EConnectionState.Disconnected;
				this.OnPlayerDisconnected(_id);
			}
			this.dropQueue.Enqueue(_id);
			yield break;
		}

		// Token: 0x0600DAAE RID: 55982 RVA: 0x004E42F8 File Offset: 0x004E24F8
		public NetworkError SendData(ClientInfo _clientInfo, int _channel, ArrayListMP<byte> _data, bool reliableDelivery = true)
		{
			if (this.ServerRunning)
			{
				CSteamID csteamID = new CSteamID(((UserIdentifierSteam)_clientInfo.PlatformId).SteamId);
				_data[_data.Count - 1] = (byte)_channel;
				if (GameManager.unreliableNetPackets && !reliableDelivery && _data.Count <= this.GetMaximumPacketSize(_clientInfo, false))
				{
					this.sendBufsUnreliable.Enqueue(new NetworkCommonSteam.SendInfo(csteamID, _data));
				}
				else
				{
					this.sendBufs.Enqueue(new NetworkCommonSteam.SendInfo(csteamID, _data));
				}
				this.signalThread.Set();
			}
			else
			{
				Log.Warning("[Steamworks.NET] NET: Tried to send a package to a client while not being a server");
			}
			return NetworkError.Ok;
		}

		// Token: 0x0600DAAF RID: 55983 RVA: 0x004E438E File Offset: 0x004E258E
		[PublicizedFrom(EAccessModifier.Private)]
		public void P2PSessionRequest(P2PSessionRequest_t _par)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			Log.Out("[Steamworks.NET] NET: P2PSessionRequest from: " + _par.m_steamIDRemote.m_SteamID.ToString());
			this.acceptQueue.Enqueue(_par.m_steamIDRemote);
		}

		// Token: 0x0600DAB0 RID: 55984 RVA: 0x004E43CC File Offset: 0x004E25CC
		public void Update()
		{
			if (!this.ServerRunning)
			{
				return;
			}
			while (this.disconnectQueue.HasData())
			{
				CSteamID csteamID = this.disconnectQueue.Dequeue();
				this.OnPlayerDisconnected(csteamID);
			}
		}

		// Token: 0x0600DAB1 RID: 55985 RVA: 0x004E4402 File Offset: 0x004E2602
		public void LateUpdate()
		{
			this.flushBuffers = true;
			this.signalThread.Set();
		}

		// Token: 0x0600DAB2 RID: 55986 RVA: 0x004E441C File Offset: 0x004E261C
		public string GetIP(ClientInfo _cInfo)
		{
			NetworkServerSteam.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(new CSteamID(((UserIdentifierSteam)_cInfo.PlatformId).SteamId), out connectionInformation))
			{
				return string.Empty;
			}
			return NetworkUtils.ToAddr(connectionInformation.Ip);
		}

		// Token: 0x0600DAB3 RID: 55987 RVA: 0x004E4460 File Offset: 0x004E2660
		public int GetPing(ClientInfo _cInfo)
		{
			NetworkServerSteam.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(new CSteamID(((UserIdentifierSteam)_cInfo.PlatformId).SteamId), out connectionInformation))
			{
				return -1;
			}
			int num = 0;
			for (int i = 0; i < 50; i++)
			{
				num += connectionInformation.Pings[i];
			}
			return num / 50;
		}

		// Token: 0x0600DAB4 RID: 55988 RVA: 0x004E44B4 File Offset: 0x004E26B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerDisconnected(CSteamID _id)
		{
			UserIdentifierSteam userIdentifierSteam = new UserIdentifierSteam(_id);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(userIdentifierSteam);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}

		// Token: 0x0600DAB5 RID: 55989 RVA: 0x0003223F File Offset: 0x0003043F
		public string GetServerPorts(int _basePort)
		{
			return "";
		}

		// Token: 0x0600DAB6 RID: 55990 RVA: 0x004E44E4 File Offset: 0x004E26E4
		[PublicizedFrom(EAccessModifier.Private)]
		public void threadHandlerMethod(ThreadManager.ThreadInfo _threadinfo)
		{
			while (!_threadinfo.TerminationRequested())
			{
				this.signalThread.WaitOne(6);
				if (this.ServerRunning && this.SteamGameServerRunning)
				{
					while (this.acceptQueue.HasData())
					{
						CSteamID csteamID = this.acceptQueue.Dequeue();
						UserIdentifierSteam userIdentifierSteam = new UserIdentifierSteam(csteamID);
						this.connections[csteamID] = new NetworkServerSteam.ConnectionInformation
						{
							State = NetworkServerSteam.EConnectionState.Authenticating,
							UserIdentifier = userIdentifierSteam
						};
						SteamGameServerNetworking.AcceptP2PSessionWithUser(csteamID);
					}
					this.CheckConnections();
					this.ReceivePackets();
					while (this.sendBufs.HasData())
					{
						NetworkCommonSteam.SendInfo sendInfo = this.sendBufs.Dequeue();
						CSteamID recipient = sendInfo.Recipient;
						NetworkServerSteam.ConnectionInformation connectionInformation;
						if (this.connections.TryGetValue(recipient, out connectionInformation) && connectionInformation.State == NetworkServerSteam.EConnectionState.Connected)
						{
							if (!SteamGameServerNetworking.SendP2PPacket(recipient, sendInfo.Data.Items, (uint)sendInfo.Data.Count, EP2PSend.k_EP2PSendReliableWithBuffering, 0))
							{
								string text = "[Steamworks.NET] NET: Could not send package to client ";
								CSteamID csteamID2 = recipient;
								Log.Error(text + csteamID2.ToString());
							}
							else
							{
								connectionInformation.PacketsPendingSend = true;
							}
						}
					}
					while (this.sendBufsUnreliable.HasData())
					{
						NetworkCommonSteam.SendInfo sendInfo2 = this.sendBufsUnreliable.Dequeue();
						CSteamID recipient2 = sendInfo2.Recipient;
						NetworkServerSteam.ConnectionInformation connectionInformation2;
						if (this.connections.TryGetValue(recipient2, out connectionInformation2) && connectionInformation2.State == NetworkServerSteam.EConnectionState.Connected)
						{
							if (!SteamGameServerNetworking.SendP2PPacket(recipient2, sendInfo2.Data.Items, (uint)sendInfo2.Data.Count, EP2PSend.k_EP2PSendUnreliable, 0))
							{
								string text2 = "[Steamworks.NET] NET: Could not send package to client ";
								CSteamID csteamID2 = recipient2;
								Log.Error(text2 + csteamID2.ToString());
							}
							else
							{
								connectionInformation2.PacketsPendingSend = true;
							}
						}
					}
					if (this.flushBuffers)
					{
						this.flushBuffers = false;
						Utils.GetBytes(this.mswPing.ElapsedMilliseconds, NetworkServerSteam.timeData, 0);
						using (Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>.Enumerator enumerator = this.connections.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<CSteamID, NetworkServerSteam.ConnectionInformation> keyValuePair = enumerator.Current;
								if (keyValuePair.Value.State == NetworkServerSteam.EConnectionState.Connected && keyValuePair.Value.PacketsPendingSend)
								{
									keyValuePair.Value.PacketsPendingSend = false;
									this.FlushBuffer(keyValuePair.Key);
								}
							}
							goto IL_0244;
						}
						goto IL_0233;
					}
					IL_0244:
					if (!this.dropQueue.HasData())
					{
						continue;
					}
					IL_0233:
					SteamGameServerNetworking.CloseP2PSessionWithUser(this.dropQueue.Dequeue());
					goto IL_0244;
				}
				this.signalThread.WaitOne(100);
			}
		}

		// Token: 0x0600DAB7 RID: 55991 RVA: 0x004E4760 File Offset: 0x004E2960
		[PublicizedFrom(EAccessModifier.Private)]
		public void CheckConnections()
		{
			if (this.checkConnections.Count == 0)
			{
				foreach (KeyValuePair<CSteamID, NetworkServerSteam.ConnectionInformation> keyValuePair in this.connections)
				{
					if (keyValuePair.Value.State != NetworkServerSteam.EConnectionState.Disconnected)
					{
						this.checkConnections.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
				this.checkPerFrame = (this.checkConnections.Count + 9) / 10;
			}
			int num = 0;
			while (num < this.checkPerFrame && this.checkConnections.Count != 0)
			{
				Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>.Enumerator enumerator2 = this.checkConnections.GetEnumerator();
				enumerator2.MoveNext();
				KeyValuePair<CSteamID, NetworkServerSteam.ConnectionInformation> keyValuePair2 = enumerator2.Current;
				enumerator2.Dispose();
				if (keyValuePair2.Value.State != NetworkServerSteam.EConnectionState.Disconnected)
				{
					P2PSessionState_t p2PSessionState_t;
					if (SteamGameServerNetworking.GetP2PSessionState(keyValuePair2.Key, out p2PSessionState_t))
					{
						if (p2PSessionState_t.m_bConnectionActive == 0 && p2PSessionState_t.m_bConnecting == 0)
						{
							Log.Out("[Steamworks.NET] NET: Connection closed: " + keyValuePair2.Key.ToString());
							SteamGameServerNetworking.CloseP2PSessionWithUser(keyValuePair2.Key);
							keyValuePair2.Value.State = NetworkServerSteam.EConnectionState.Disconnected;
							this.disconnectQueue.Enqueue(keyValuePair2.Key);
						}
						else if (keyValuePair2.Value.Ip == 0U)
						{
							keyValuePair2.Value.Ip = p2PSessionState_t.m_nRemoteIP;
						}
					}
					else
					{
						Log.Out("[Steamworks.NET] NET: No connection to client: " + keyValuePair2.Key.ToString());
						SteamGameServerNetworking.CloseP2PSessionWithUser(keyValuePair2.Key);
						keyValuePair2.Value.State = NetworkServerSteam.EConnectionState.Disconnected;
						this.disconnectQueue.Enqueue(keyValuePair2.Key);
					}
				}
				num++;
				this.checkConnections.Remove(keyValuePair2.Key);
			}
		}

		// Token: 0x0600DAB8 RID: 55992 RVA: 0x004E4950 File Offset: 0x004E2B50
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReceivePackets()
		{
			long elapsedMilliseconds = this.mswPing.ElapsedMilliseconds;
			uint num;
			CSteamID csteamID;
			bool flag = SteamGameServerNetworking.ReadP2PPacket(this.recvBuf, (uint)this.recvBuf.Length, out num, out csteamID, 0);
			while (flag)
			{
				NetworkServerSteam.ConnectionInformation connectionInformation;
				if (!this.connections.TryGetValue(csteamID, out connectionInformation) || connectionInformation.State == NetworkServerSteam.EConnectionState.Disconnected)
				{
					string text = "[Steamworks.NET] NET: Received package from an unconnected client: ";
					CSteamID csteamID2 = csteamID;
					Log.Out(text + csteamID2.ToString());
				}
				else if (num != 0U)
				{
					num -= 1U;
					NetworkCommonSteam.ESteamNetChannels esteamNetChannels = (NetworkCommonSteam.ESteamNetChannels)this.recvBuf[(int)num];
					if (esteamNetChannels > NetworkCommonSteam.ESteamNetChannels.NetpackageChannel1)
					{
						if (esteamNetChannels != NetworkCommonSteam.ESteamNetChannels.Authentication)
						{
							if (esteamNetChannels != NetworkCommonSteam.ESteamNetChannels.Ping)
							{
								string text2 = "[Steamworks.NET] NET: Received package on an unknown channel from: ";
								CSteamID csteamID2 = csteamID;
								Log.Out(text2 + csteamID2.ToString());
							}
							else
							{
								this.UpdatePing(csteamID, this.recvBuf, elapsedMilliseconds);
							}
						}
						else if (connectionInformation.State == NetworkServerSteam.EConnectionState.Authenticating)
						{
							string @string = Encoding.UTF8.GetString(this.recvBuf, 0, (int)num);
							bool flag2;
							if (this.Authenticate(csteamID, @string))
							{
								flag2 = SteamGameServerNetworking.SendP2PPacket(csteamID, NetworkServerSteam.passwordValidPacket, (uint)NetworkServerSteam.passwordValidPacket.Length, EP2PSend.k_EP2PSendReliable, 0);
							}
							else
							{
								flag2 = SteamGameServerNetworking.SendP2PPacket(csteamID, NetworkServerSteam.passwordInvalidPacket, (uint)NetworkServerSteam.passwordInvalidPacket.Length, EP2PSend.k_EP2PSendReliable, 0);
							}
							if (!flag2)
							{
								string text3 = "[Steamworks.NET] NET: Could not send package to client ";
								CSteamID csteamID2 = csteamID;
								Log.Error(text3 + csteamID2.ToString());
							}
						}
					}
					else if (connectionInformation.State == NetworkServerSteam.EConnectionState.Connected)
					{
						if (num > 0U)
						{
							byte[] array = MemoryPools.poolByte.Alloc((int)num);
							Array.Copy(this.recvBuf, array, (long)((ulong)num));
							ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(connectionInformation.UserIdentifier);
							SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DataReceivedServer(clientInfo, (int)esteamNetChannels, array, (int)num);
						}
					}
					else
					{
						string text4 = "[Steamworks.NET] NET: Received package from an unauthenticated client: ";
						CSteamID csteamID2 = csteamID;
						Log.Out(text4 + csteamID2.ToString());
					}
				}
				flag = SteamGameServerNetworking.ReadP2PPacket(this.recvBuf, (uint)this.recvBuf.Length, out num, out csteamID, 0);
			}
		}

		// Token: 0x0600DAB9 RID: 55993 RVA: 0x004E4B44 File Offset: 0x004E2D44
		[PublicizedFrom(EAccessModifier.Private)]
		public bool Authenticate(CSteamID _id, string _password)
		{
			bool flag = string.IsNullOrEmpty(this.serverPassword) || _password == this.serverPassword;
			Log.Out(string.Concat(new string[]
			{
				"[Steamworks.NET] NET: Received authentication package from ",
				_id.ToString(),
				": ",
				flag ? "valid" : "invalid",
				" password"
			}));
			if (!flag)
			{
				this.connections[_id].State = NetworkServerSteam.EConnectionState.Authenticating;
				return false;
			}
			this.connections[_id].State = NetworkServerSteam.EConnectionState.Connected;
			this.OnPlayerConnected(_id);
			return true;
		}

		// Token: 0x0600DABA RID: 55994 RVA: 0x004E4BEC File Offset: 0x004E2DEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerConnected(CSteamID _id)
		{
			ClientInfo clientInfo = new ClientInfo
			{
				PlatformId = new UserIdentifierSteam(_id),
				network = this,
				netConnection = new INetConnection[2]
			};
			for (int i = 0; i < 2; i++)
			{
				clientInfo.netConnection[i] = new NetConnectionSteam(i, clientInfo, null, clientInfo.InternalId.CombinedString);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.AddClient(clientInfo);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerConnected(clientInfo);
		}

		// Token: 0x0600DABB RID: 55995 RVA: 0x004E4C5C File Offset: 0x004E2E5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void FlushBuffer(CSteamID _id)
		{
			if (!SteamGameServerNetworking.SendP2PPacket(_id, NetworkServerSteam.timeData, (uint)NetworkServerSteam.timeData.Length, EP2PSend.k_EP2PSendReliable, 0))
			{
				string text = "[Steamworks.NET] NET: Could not flush the buffer to client ";
				CSteamID csteamID = _id;
				Log.Error(text + csteamID.ToString());
			}
		}

		// Token: 0x0600DABC RID: 55996 RVA: 0x004E4CA0 File Offset: 0x004E2EA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdatePing(CSteamID _sourceId, byte[] _data, long _curTime)
		{
			long num = BitConverter.ToInt64(_data, 0);
			int num2 = (int)(_curTime - num);
			NetworkServerSteam.ConnectionInformation connectionInformation = this.connections[_sourceId];
			connectionInformation.LastPingIndex++;
			if (connectionInformation.LastPingIndex >= 50)
			{
				connectionInformation.LastPingIndex = 0;
			}
			connectionInformation.Pings[connectionInformation.LastPingIndex] = num2;
		}

		// Token: 0x0600DABD RID: 55997 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600DABE RID: 55998 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600DABF RID: 55999 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600DAC0 RID: 56000 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600DAC1 RID: 56001 RVA: 0x0003223F File Offset: 0x0003043F
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600DAC2 RID: 56002 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0600DAC3 RID: 56003 RVA: 0x004E4CF4 File Offset: 0x004E2EF4
		public int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false)
		{
			return 1200;
		}

		// Token: 0x0600DAC4 RID: 56004 RVA: 0x00010E62 File Offset: 0x0000F062
		public int GetBadPacketCount(ClientInfo _cInfo)
		{
			return 0;
		}

		// Token: 0x0400A595 RID: 42389
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A596 RID: 42390
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A597 RID: 42391
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PingCount = 50;

		// Token: 0x0400A598 RID: 42392
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch mswPing = new MicroStopwatch();

		// Token: 0x0400A599 RID: 42393
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo handlerThread;

		// Token: 0x0400A59A RID: 42394
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent signalThread = new AutoResetEvent(false);

		// Token: 0x0400A59B RID: 42395
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufs = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A59C RID: 42396
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufsUnreliable = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A59D RID: 42397
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> acceptQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A59E RID: 42398
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> dropQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A59F RID: 42399
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool flushBuffers;

		// Token: 0x0400A5A0 RID: 42400
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> disconnectQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A5A1 RID: 42401
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation> connections = new Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>();

		// Token: 0x0400A5A2 RID: 42402
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<P2PSessionRequest_t> m_P2PSessionRequest;

		// Token: 0x0400A5A3 RID: 42403
		[PublicizedFrom(EAccessModifier.Private)]
		public string serverPassword;

		// Token: 0x0400A5A4 RID: 42404
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation> checkConnections = new Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>();

		// Token: 0x0400A5A5 RID: 42405
		[PublicizedFrom(EAccessModifier.Private)]
		public int checkPerFrame;

		// Token: 0x0400A5A6 RID: 42406
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] passwordValidPacket = new byte[] { 1, 50 };

		// Token: 0x0400A5A7 RID: 42407
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] passwordInvalidPacket = new byte[] { 0, 50 };

		// Token: 0x0400A5A8 RID: 42408
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] recvBuf = new byte[1048576];

		// Token: 0x0400A5A9 RID: 42409
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] timeData = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 60 };

		// Token: 0x02001CCF RID: 7375
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EConnectionState
		{
			// Token: 0x0400A5AB RID: 42411
			Disconnected,
			// Token: 0x0400A5AC RID: 42412
			Authenticating,
			// Token: 0x0400A5AD RID: 42413
			Connected
		}

		// Token: 0x02001CD0 RID: 7376
		[PublicizedFrom(EAccessModifier.Private)]
		public class ConnectionInformation
		{
			// Token: 0x0400A5AE RID: 42414
			public NetworkServerSteam.EConnectionState State;

			// Token: 0x0400A5AF RID: 42415
			public uint Ip;

			// Token: 0x0400A5B0 RID: 42416
			public bool PacketsPendingSend;

			// Token: 0x0400A5B1 RID: 42417
			public UserIdentifierSteam UserIdentifier;

			// Token: 0x0400A5B2 RID: 42418
			public int LastPingIndex = -1;

			// Token: 0x0400A5B3 RID: 42419
			public readonly int[] Pings = new int[50];
		}
	}
}
