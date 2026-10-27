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
	// Token: 0x02001CFF RID: 7423
	public class NetworkServerSteam : IPlatformNetworkServer, INetworkServer
	{
		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x0600DBBA RID: 56250 RVA: 0x004E7C06 File Offset: 0x004E5E06
		public bool ServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.protoManager.IsServer && this.handlerThread != null;
			}
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x0600DBBB RID: 56251 RVA: 0x004E7C20 File Offset: 0x004E5E20
		public bool SteamGameServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.owner.ServerListAnnouncer.GameServerInitialized;
			}
		}

		// Token: 0x0600DBBC RID: 56252 RVA: 0x004E7C34 File Offset: 0x004E5E34
		public NetworkServerSteam(IPlatform _owner, IProtocolManagerProtocolInterface _protoManager)
		{
			this.owner = _owner;
			this.protoManager = _protoManager;
			this.owner.Api.ClientApiInitialized += delegate
			{
				this.m_P2PSessionRequest = Callback<P2PSessionRequest_t>.CreateGameServer(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.P2PSessionRequest));
			};
		}

		// Token: 0x0600DBBD RID: 56253 RVA: 0x004E7CE8 File Offset: 0x004E5EE8
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

		// Token: 0x0600DBBE RID: 56254 RVA: 0x004E7D47 File Offset: 0x004E5F47
		public void SetServerPassword(string _password)
		{
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
		}

		// Token: 0x0600DBBF RID: 56255 RVA: 0x004E7D5C File Offset: 0x004E5F5C
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

		// Token: 0x0600DBC0 RID: 56256 RVA: 0x004E7E44 File Offset: 0x004E6044
		public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
		{
			CSteamID csteamID = new CSteamID(((UserIdentifierSteam)_clientInfo.PlatformId).SteamId);
			ThreadManager.StartCoroutine(this.dropLater(csteamID, 0.2f));
		}

		// Token: 0x0600DBC1 RID: 56257 RVA: 0x004E7E7A File Offset: 0x004E607A
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

		// Token: 0x0600DBC2 RID: 56258 RVA: 0x004E7E98 File Offset: 0x004E6098
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

		// Token: 0x0600DBC3 RID: 56259 RVA: 0x004E7F2E File Offset: 0x004E612E
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

		// Token: 0x0600DBC4 RID: 56260 RVA: 0x004E7F6C File Offset: 0x004E616C
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

		// Token: 0x0600DBC5 RID: 56261 RVA: 0x004E7FA2 File Offset: 0x004E61A2
		public void LateUpdate()
		{
			this.flushBuffers = true;
			this.signalThread.Set();
		}

		// Token: 0x0600DBC6 RID: 56262 RVA: 0x004E7FBC File Offset: 0x004E61BC
		public string GetIP(ClientInfo _cInfo)
		{
			NetworkServerSteam.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(new CSteamID(((UserIdentifierSteam)_cInfo.PlatformId).SteamId), out connectionInformation))
			{
				return string.Empty;
			}
			return NetworkUtils.ToAddr(connectionInformation.Ip);
		}

		// Token: 0x0600DBC7 RID: 56263 RVA: 0x004E8000 File Offset: 0x004E6200
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

		// Token: 0x0600DBC8 RID: 56264 RVA: 0x004E8054 File Offset: 0x004E6254
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerDisconnected(CSteamID _id)
		{
			UserIdentifierSteam userIdentifierSteam = new UserIdentifierSteam(_id);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(userIdentifierSteam);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}

		// Token: 0x0600DBC9 RID: 56265 RVA: 0x000321FF File Offset: 0x000303FF
		public string GetServerPorts(int _basePort)
		{
			return "";
		}

		// Token: 0x0600DBCA RID: 56266 RVA: 0x004E8084 File Offset: 0x004E6284
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

		// Token: 0x0600DBCB RID: 56267 RVA: 0x004E8300 File Offset: 0x004E6500
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

		// Token: 0x0600DBCC RID: 56268 RVA: 0x004E84F0 File Offset: 0x004E66F0
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

		// Token: 0x0600DBCD RID: 56269 RVA: 0x004E86E4 File Offset: 0x004E68E4
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

		// Token: 0x0600DBCE RID: 56270 RVA: 0x004E878C File Offset: 0x004E698C
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

		// Token: 0x0600DBCF RID: 56271 RVA: 0x004E87FC File Offset: 0x004E69FC
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

		// Token: 0x0600DBD0 RID: 56272 RVA: 0x004E8840 File Offset: 0x004E6A40
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

		// Token: 0x0600DBD1 RID: 56273 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600DBD2 RID: 56274 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600DBD3 RID: 56275 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600DBD4 RID: 56276 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600DBD5 RID: 56277 RVA: 0x000321FF File Offset: 0x000303FF
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600DBD6 RID: 56278 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0600DBD7 RID: 56279 RVA: 0x004E8894 File Offset: 0x004E6A94
		public int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false)
		{
			return 1200;
		}

		// Token: 0x0600DBD8 RID: 56280 RVA: 0x00010E62 File Offset: 0x0000F062
		public int GetBadPacketCount(ClientInfo _cInfo)
		{
			return 0;
		}

		// Token: 0x0400A67E RID: 42622
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A67F RID: 42623
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A680 RID: 42624
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PingCount = 50;

		// Token: 0x0400A681 RID: 42625
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch mswPing = new MicroStopwatch();

		// Token: 0x0400A682 RID: 42626
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo handlerThread;

		// Token: 0x0400A683 RID: 42627
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent signalThread = new AutoResetEvent(false);

		// Token: 0x0400A684 RID: 42628
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufs = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A685 RID: 42629
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufsUnreliable = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A686 RID: 42630
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> acceptQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A687 RID: 42631
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> dropQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A688 RID: 42632
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool flushBuffers;

		// Token: 0x0400A689 RID: 42633
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> disconnectQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A68A RID: 42634
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation> connections = new Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>();

		// Token: 0x0400A68B RID: 42635
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<P2PSessionRequest_t> m_P2PSessionRequest;

		// Token: 0x0400A68C RID: 42636
		[PublicizedFrom(EAccessModifier.Private)]
		public string serverPassword;

		// Token: 0x0400A68D RID: 42637
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation> checkConnections = new Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>();

		// Token: 0x0400A68E RID: 42638
		[PublicizedFrom(EAccessModifier.Private)]
		public int checkPerFrame;

		// Token: 0x0400A68F RID: 42639
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] passwordValidPacket = new byte[] { 1, 50 };

		// Token: 0x0400A690 RID: 42640
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] passwordInvalidPacket = new byte[] { 0, 50 };

		// Token: 0x0400A691 RID: 42641
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] recvBuf = new byte[1048576];

		// Token: 0x0400A692 RID: 42642
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] timeData = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 60 };

		// Token: 0x02001D00 RID: 7424
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EConnectionState
		{
			// Token: 0x0400A694 RID: 42644
			Disconnected,
			// Token: 0x0400A695 RID: 42645
			Authenticating,
			// Token: 0x0400A696 RID: 42646
			Connected
		}

		// Token: 0x02001D01 RID: 7425
		[PublicizedFrom(EAccessModifier.Private)]
		public class ConnectionInformation
		{
			// Token: 0x0400A697 RID: 42647
			public NetworkServerSteam.EConnectionState State;

			// Token: 0x0400A698 RID: 42648
			public uint Ip;

			// Token: 0x0400A699 RID: 42649
			public bool PacketsPendingSend;

			// Token: 0x0400A69A RID: 42650
			public UserIdentifierSteam UserIdentifier;

			// Token: 0x0400A69B RID: 42651
			public int LastPingIndex = -1;

			// Token: 0x0400A69C RID: 42652
			public readonly int[] Pings = new int[50];
		}
	}
}
