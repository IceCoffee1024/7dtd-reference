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
	// Token: 0x02001C9C RID: 7324
	public class NetworkServerSteam : IPlatformNetworkServer, INetworkServer
	{
		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x0600D92C RID: 55596 RVA: 0x004DF51E File Offset: 0x004DD71E
		public bool ServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.protoManager.IsServer && this.handlerThread != null;
			}
		}

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x0600D92D RID: 55597 RVA: 0x004DF538 File Offset: 0x004DD738
		public bool SteamGameServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.owner.ServerListAnnouncer.GameServerInitialized;
			}
		}

		// Token: 0x0600D92E RID: 55598 RVA: 0x004DF54C File Offset: 0x004DD74C
		public NetworkServerSteam(IPlatform _owner, IProtocolManagerProtocolInterface _protoManager)
		{
			this.owner = _owner;
			this.protoManager = _protoManager;
			this.owner.Api.ClientApiInitialized += delegate
			{
				this.m_P2PSessionRequest = Callback<P2PSessionRequest_t>.CreateGameServer(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.P2PSessionRequest));
			};
		}

		// Token: 0x0600D92F RID: 55599 RVA: 0x004DF600 File Offset: 0x004DD800
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

		// Token: 0x0600D930 RID: 55600 RVA: 0x004DF65F File Offset: 0x004DD85F
		public void SetServerPassword(string _password)
		{
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
		}

		// Token: 0x0600D931 RID: 55601 RVA: 0x004DF674 File Offset: 0x004DD874
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

		// Token: 0x0600D932 RID: 55602 RVA: 0x004DF75C File Offset: 0x004DD95C
		public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
		{
			CSteamID csteamID = new CSteamID(((UserIdentifierSteam)_clientInfo.PlatformId).SteamId);
			ThreadManager.StartCoroutine(this.dropLater(csteamID, 0.2f));
		}

		// Token: 0x0600D933 RID: 55603 RVA: 0x004DF792 File Offset: 0x004DD992
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

		// Token: 0x0600D934 RID: 55604 RVA: 0x004DF7B0 File Offset: 0x004DD9B0
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

		// Token: 0x0600D935 RID: 55605 RVA: 0x004DF846 File Offset: 0x004DDA46
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

		// Token: 0x0600D936 RID: 55606 RVA: 0x004DF884 File Offset: 0x004DDA84
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

		// Token: 0x0600D937 RID: 55607 RVA: 0x004DF8BA File Offset: 0x004DDABA
		public void LateUpdate()
		{
			this.flushBuffers = true;
			this.signalThread.Set();
		}

		// Token: 0x0600D938 RID: 55608 RVA: 0x004DF8D4 File Offset: 0x004DDAD4
		public string GetIP(ClientInfo _cInfo)
		{
			NetworkServerSteam.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(new CSteamID(((UserIdentifierSteam)_cInfo.PlatformId).SteamId), out connectionInformation))
			{
				return string.Empty;
			}
			return NetworkUtils.ToAddr(connectionInformation.Ip);
		}

		// Token: 0x0600D939 RID: 55609 RVA: 0x004DF918 File Offset: 0x004DDB18
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

		// Token: 0x0600D93A RID: 55610 RVA: 0x004DF96C File Offset: 0x004DDB6C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerDisconnected(CSteamID _id)
		{
			UserIdentifierSteam userIdentifierSteam = new UserIdentifierSteam(_id);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(userIdentifierSteam);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}

		// Token: 0x0600D93B RID: 55611 RVA: 0x00032147 File Offset: 0x00030347
		public string GetServerPorts(int _basePort)
		{
			return "";
		}

		// Token: 0x0600D93C RID: 55612 RVA: 0x004DF99C File Offset: 0x004DDB9C
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

		// Token: 0x0600D93D RID: 55613 RVA: 0x004DFC18 File Offset: 0x004DDE18
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

		// Token: 0x0600D93E RID: 55614 RVA: 0x004DFE08 File Offset: 0x004DE008
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

		// Token: 0x0600D93F RID: 55615 RVA: 0x004DFFFC File Offset: 0x004DE1FC
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

		// Token: 0x0600D940 RID: 55616 RVA: 0x004E00A4 File Offset: 0x004DE2A4
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

		// Token: 0x0600D941 RID: 55617 RVA: 0x004E0114 File Offset: 0x004DE314
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

		// Token: 0x0600D942 RID: 55618 RVA: 0x004E0158 File Offset: 0x004DE358
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

		// Token: 0x0600D943 RID: 55619 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600D944 RID: 55620 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600D945 RID: 55621 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600D946 RID: 55622 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600D947 RID: 55623 RVA: 0x00032147 File Offset: 0x00030347
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600D948 RID: 55624 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0600D949 RID: 55625 RVA: 0x004E01AC File Offset: 0x004DE3AC
		public int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false)
		{
			return 1200;
		}

		// Token: 0x0600D94A RID: 55626 RVA: 0x00010E62 File Offset: 0x0000F062
		public int GetBadPacketCount(ClientInfo _cInfo)
		{
			return 0;
		}

		// Token: 0x0400A4C0 RID: 42176
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A4C1 RID: 42177
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A4C2 RID: 42178
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PingCount = 50;

		// Token: 0x0400A4C3 RID: 42179
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch mswPing = new MicroStopwatch();

		// Token: 0x0400A4C4 RID: 42180
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo handlerThread;

		// Token: 0x0400A4C5 RID: 42181
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent signalThread = new AutoResetEvent(false);

		// Token: 0x0400A4C6 RID: 42182
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufs = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A4C7 RID: 42183
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufsUnreliable = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A4C8 RID: 42184
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> acceptQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A4C9 RID: 42185
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> dropQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A4CA RID: 42186
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool flushBuffers;

		// Token: 0x0400A4CB RID: 42187
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<CSteamID> disconnectQueue = new BlockingQueue<CSteamID>();

		// Token: 0x0400A4CC RID: 42188
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation> connections = new Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>();

		// Token: 0x0400A4CD RID: 42189
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<P2PSessionRequest_t> m_P2PSessionRequest;

		// Token: 0x0400A4CE RID: 42190
		[PublicizedFrom(EAccessModifier.Private)]
		public string serverPassword;

		// Token: 0x0400A4CF RID: 42191
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation> checkConnections = new Dictionary<CSteamID, NetworkServerSteam.ConnectionInformation>();

		// Token: 0x0400A4D0 RID: 42192
		[PublicizedFrom(EAccessModifier.Private)]
		public int checkPerFrame;

		// Token: 0x0400A4D1 RID: 42193
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] passwordValidPacket = new byte[] { 1, 50 };

		// Token: 0x0400A4D2 RID: 42194
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] passwordInvalidPacket = new byte[] { 0, 50 };

		// Token: 0x0400A4D3 RID: 42195
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] recvBuf = new byte[1048576];

		// Token: 0x0400A4D4 RID: 42196
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] timeData = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 60 };

		// Token: 0x02001C9D RID: 7325
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EConnectionState
		{
			// Token: 0x0400A4D6 RID: 42198
			Disconnected,
			// Token: 0x0400A4D7 RID: 42199
			Authenticating,
			// Token: 0x0400A4D8 RID: 42200
			Connected
		}

		// Token: 0x02001C9E RID: 7326
		[PublicizedFrom(EAccessModifier.Private)]
		public class ConnectionInformation
		{
			// Token: 0x0400A4D9 RID: 42201
			public NetworkServerSteam.EConnectionState State;

			// Token: 0x0400A4DA RID: 42202
			public uint Ip;

			// Token: 0x0400A4DB RID: 42203
			public bool PacketsPendingSend;

			// Token: 0x0400A4DC RID: 42204
			public UserIdentifierSteam UserIdentifier;

			// Token: 0x0400A4DD RID: 42205
			public int LastPingIndex = -1;

			// Token: 0x0400A4DE RID: 42206
			public readonly int[] Pings = new int[50];
		}
	}
}
