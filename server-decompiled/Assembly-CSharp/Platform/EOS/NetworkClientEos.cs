using System;
using System.Collections;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using UnityEngine.Networking;

namespace Platform.EOS
{
	// Token: 0x02001CF5 RID: 7413
	public class NetworkClientEos : IPlatformNetworkClient, INetworkClient
	{
		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x0600DBE4 RID: 56292 RVA: 0x004E9A37 File Offset: 0x004E7C37
		public bool IsConnected
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.serverId != null && (this.protoManager.IsClient || this.connecting) && this.owner.User.UserStatus == EUserStatus.LoggedIn;
			}
		}

		// Token: 0x0600DBE5 RID: 56293 RVA: 0x004E9A74 File Offset: 0x004E7C74
		public NetworkClientEos(IPlatform _owner, IProtocolManagerProtocolInterface _protoManager)
		{
			this.owner = _owner;
			this.protoManager = _protoManager;
			this.owner.Api.ClientApiInitialized += delegate
			{
				if (!GameManager.IsDedicatedServer)
				{
					EosHelpers.AssertMainThread("P2P.Init");
					this.localUser = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
					this.socketId = new SocketId
					{
						SocketName = "Game"
					};
					object obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						this.ptpInterface = ((Api)this.owner.Api).PlatformInterface.GetP2PInterface();
					}
					AddNotifyPeerConnectionRequestOptions addNotifyPeerConnectionRequestOptions = new AddNotifyPeerConnectionRequestOptions
					{
						LocalUserId = this.localUser,
						SocketId = new SocketId?(this.socketId)
					};
					obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						this.ptpInterface.AddNotifyPeerConnectionRequest(ref addNotifyPeerConnectionRequestOptions, null, new OnIncomingConnectionRequestCallback(this.ConnectionRequestHandler));
					}
					AddNotifyPeerConnectionEstablishedOptions addNotifyPeerConnectionEstablishedOptions = new AddNotifyPeerConnectionEstablishedOptions
					{
						LocalUserId = this.localUser,
						SocketId = new SocketId?(this.socketId)
					};
					obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						this.ptpInterface.AddNotifyPeerConnectionEstablished(ref addNotifyPeerConnectionEstablishedOptions, null, new OnPeerConnectionEstablishedCallback(this.ConnectionEstablishedHandler));
					}
					AddNotifyPeerConnectionClosedOptions addNotifyPeerConnectionClosedOptions = new AddNotifyPeerConnectionClosedOptions
					{
						LocalUserId = this.localUser,
						SocketId = new SocketId?(this.socketId)
					};
					obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						this.ptpInterface.AddNotifyPeerConnectionClosed(ref addNotifyPeerConnectionClosedOptions, null, new OnRemoteConnectionClosedCallback(this.ConnectionClosedHandler));
					}
					AddNotifyIncomingPacketQueueFullOptions addNotifyIncomingPacketQueueFullOptions = default(AddNotifyIncomingPacketQueueFullOptions);
					obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						this.ptpInterface.AddNotifyIncomingPacketQueueFull(ref addNotifyIncomingPacketQueueFullOptions, null, new OnIncomingPacketQueueFullCallback(this.IncomingPacketQueueFullHandler));
					}
				}
			};
		}

		// Token: 0x0600DBE6 RID: 56294 RVA: 0x004E9AD4 File Offset: 0x004E7CD4
		public void Connect(GameServerInfo _gsi)
		{
			this.disconnectEventReceived = false;
			Log.Out("[EOS-P2PC] Trying to connect to: " + _gsi.GetValue(GameInfoString.IP) + ":" + _gsi.GetValue(GameInfoInt.Port).ToString());
			if (string.IsNullOrEmpty(_gsi.GetValue(GameInfoString.CombinedPrimaryId)))
			{
				Log.Out("[EOS-P2PC] Resolving EOS ID for IP " + _gsi.GetValue(GameInfoString.IP) + ":" + _gsi.GetValue(GameInfoInt.Port).ToString());
				ServerInformationTcpClient.RequestRules(_gsi, false, new ServerInformationTcpClient.RulesRequestDone(this.RulesRequestTcpDone));
				this.connecting = true;
				return;
			}
			this.ConnectInternal(_gsi);
		}

		// Token: 0x0600DBE7 RID: 56295 RVA: 0x004E9B6D File Offset: 0x004E7D6D
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesRequestTcpDone(bool _success, string _message, GameServerInfo _gsi)
		{
			if (_success && this.connecting)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo = _gsi;
				this.ConnectInternal(_gsi);
				return;
			}
			this.Disconnect();
			ThreadManager.StartCoroutine(this.connectionFailedLater(_message));
		}

		// Token: 0x0600DBE8 RID: 56296 RVA: 0x004E9BA0 File Offset: 0x004E7DA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectInternal(GameServerInfo _gsi)
		{
			string value = _gsi.GetValue(GameInfoString.CombinedPrimaryId);
			if (string.IsNullOrEmpty(value))
			{
				Log.Error("Server info does not have a CombinedPrimaryId");
				this.Disconnect();
				ThreadManager.StartCoroutine(this.connectionFailedLater(Localization.Get("netSteamNetworking_NoServerID", false, null)));
				return;
			}
			if (_gsi.AllowsCrossplay && !PermissionsManager.IsCrossplayAllowed())
			{
				this.Disconnect();
				this.protoManager.ConnectionFailedEv(Localization.Get("auth_noCrossplay", false, null));
				return;
			}
			UserIdentifierEos userIdentifierEos = PlatformUserIdentifierAbs.FromCombinedString(value, true) as UserIdentifierEos;
			if (userIdentifierEos == null)
			{
				this.Disconnect();
				ThreadManager.StartCoroutine(this.connectionFailedLater(Localization.Get("netSteamNetworking_NoServerID", false, null)));
				return;
			}
			string password = ServerInfoCache.Instance.GetPassword(_gsi);
			ArrayListMP<byte> arrayListMP;
			if (!string.IsNullOrEmpty(password))
			{
				int byteCount = Encoding.UTF8.GetByteCount(password);
				arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, byteCount + 1)
				{
					Count = byteCount + 1
				};
				Encoding.UTF8.GetBytes(password, 0, password.Length, arrayListMP.Items, 1);
			}
			else
			{
				arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, 1)
				{
					Count = 1
				};
			}
			EosHelpers.AssertMainThread("P2P.ConInt.PUID");
			this.serverId = userIdentifierEos.ProductUserId;
			string text = "[EOS-P2PC] Connecting to EOS ID ";
			ProductUserId productUserId = this.serverId;
			Log.Out(text + ((productUserId != null) ? productUserId.ToString() : null));
			this.connecting = true;
			this.SendData(50, arrayListMP);
		}

		// Token: 0x0600DBE9 RID: 56297 RVA: 0x004E9CF4 File Offset: 0x004E7EF4
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator connectionFailedLater(string _message)
		{
			yield return null;
			yield return null;
			this.protoManager.ConnectionFailedEv(_message);
			yield break;
		}

		// Token: 0x0600DBEA RID: 56298 RVA: 0x004E9D0C File Offset: 0x004E7F0C
		public void Disconnect()
		{
			this.connecting = false;
			this.sendBufs.Clear();
			EosHelpers.AssertMainThread("P2P.CloseCons");
			CloseConnectionsOptions closeConnectionsOptions = new CloseConnectionsOptions
			{
				SocketId = new SocketId?(this.socketId),
				LocalUserId = this.localUser
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.ptpInterface.CloseConnections(ref closeConnectionsOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-P2PC] Failed closing connections: " + result.ToStringCached<Result>());
			}
			this.serverId = null;
		}

		// Token: 0x0600DBEB RID: 56299 RVA: 0x004E9DBC File Offset: 0x004E7FBC
		public NetworkError SendData(int _channel, ArrayListMP<byte> _data)
		{
			if (this.IsConnected)
			{
				_data[0] = (byte)_channel;
				this.sendBufs.Enqueue(new NetworkCommonEos.SendInfo(null, _data));
			}
			else
			{
				Log.Warning("[EOS-P2PC] Tried to send a package while not connected to a server");
			}
			return NetworkError.Ok;
		}

		// Token: 0x0600DBEC RID: 56300 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionRequestHandler(ref OnIncomingConnectionRequestInfo _data)
		{
		}

		// Token: 0x0600DBED RID: 56301 RVA: 0x004E9DEE File Offset: 0x004E7FEE
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionEstablishedHandler(ref OnPeerConnectionEstablishedInfo _data)
		{
			Log.Out(string.Format("[EOS-P2PC] Connection established: {0}", _data.RemoteUserId));
		}

		// Token: 0x0600DBEE RID: 56302 RVA: 0x004E9E08 File Offset: 0x004E8008
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionClosedHandler(ref OnRemoteConnectionClosedInfo _data)
		{
			ProductUserId remoteUserId = _data.RemoteUserId;
			if (this.connecting)
			{
				this.Disconnect();
				Log.Out(string.Format("[EOS-P2PC] P2PSessionConnectFail to: {0} - Error: {1}", _data.RemoteUserId, _data.Reason.ToStringCached<ConnectionClosedReason>()));
				string text = Localization.Get("netSteamNetworkingSessionError_" + _data.Reason.ToStringCached<ConnectionClosedReason>(), false, null);
				this.protoManager.ConnectionFailedEv(text);
				return;
			}
			if (!this.IsConnected)
			{
				return;
			}
			Log.Out(string.Format("[EOS-P2PC] Connection closed by {0}: ", remoteUserId) + _data.Reason.ToStringCached<ConnectionClosedReason>());
			if (_data.Reason == ConnectionClosedReason.ClosedByLocalUser)
			{
				return;
			}
			CloseConnectionOptions closeConnectionOptions = new CloseConnectionOptions
			{
				SocketId = new SocketId?(this.socketId),
				LocalUserId = this.localUser,
				RemoteUserId = remoteUserId
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.ptpInterface.CloseConnection(ref closeConnectionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error(string.Format("[EOS-P2PC] Failed closing connection to {0}: {1}", remoteUserId, result.ToStringCached<Result>()));
			}
			this.OnDisconnectedFromServer();
		}

		// Token: 0x0600DBEF RID: 56303 RVA: 0x004E9F38 File Offset: 0x004E8138
		[PublicizedFrom(EAccessModifier.Private)]
		public void IncomingPacketQueueFullHandler(ref OnIncomingPacketQueueFullInfo _data)
		{
			if (!this.IsConnected)
			{
				return;
			}
			Log.Error(string.Format("[EOS-P2PC] Packet queue full: Chn={0}, IncSize={1}, Used={2}, Max={3}", new object[] { _data.OverflowPacketChannel, _data.OverflowPacketSizeBytes, _data.PacketQueueCurrentSizeBytes, _data.PacketQueueMaxSizeBytes }));
		}

		// Token: 0x0600DBF0 RID: 56304 RVA: 0x004E9F9C File Offset: 0x004E819C
		public void Update()
		{
			if (!this.IsConnected)
			{
				return;
			}
			Result result;
			for (;;)
			{
				ReceivePacketOptions receivePacketOptions = new ReceivePacketOptions
				{
					LocalUserId = this.localUser,
					MaxDataSizeBytes = 1170U
				};
				ProductUserId productUserId = new ProductUserId();
				SocketId socketId = default(SocketId);
				object obj = AntiCheatCommon.LockObject;
				uint num;
				lock (obj)
				{
					byte b;
					result = this.ptpInterface.ReceivePacket(ref receivePacketOptions, ref productUserId, ref socketId, out b, this.receiveBuffer, out num);
				}
				if (result != Result.Success)
				{
					break;
				}
				if (num > 0U)
				{
					NetworkCommonEos.ESteamNetChannels esteamNetChannels = (NetworkCommonEos.ESteamNetChannels)this.receiveBuffer.Array[0];
					if (esteamNetChannels > NetworkCommonEos.ESteamNetChannels.NetpackageChannel1)
					{
						if (esteamNetChannels != NetworkCommonEos.ESteamNetChannels.Authentication)
						{
							if (esteamNetChannels == NetworkCommonEos.ESteamNetChannels.Ping)
							{
								SendPacketOptions sendPacketOptions = new SendPacketOptions
								{
									SocketId = new SocketId?(this.socketId),
									LocalUserId = this.localUser,
									RemoteUserId = this.serverId,
									Channel = 0,
									Reliability = PacketReliability.ReliableOrdered,
									AllowDelayedDelivery = true,
									Data = this.receiveBuffer
								};
								obj = AntiCheatCommon.LockObject;
								Result result2;
								lock (obj)
								{
									result2 = this.ptpInterface.SendPacket(ref sendPacketOptions);
								}
								if (result2 != Result.Success)
								{
									Log.Error("[EOS-P2PC] Could not send ping package to server: " + result2.ToStringCached<Result>());
								}
							}
						}
						else
						{
							if (this.connecting)
							{
								this.connecting = false;
								Log.Out("[EOS-P2PC] Connection established");
							}
							if (this.receiveBuffer.Array[1] == 0)
							{
								Log.Out("[EOS-P2PC] Received invalid password package");
								ThreadManager.AddSingleTaskMainThread("SteamNetInvalidPassword", delegate(object _info)
								{
									this.protoManager.InvalidPasswordEv();
								}, null);
							}
							else
							{
								Log.Out("[EOS-P2PC] Password accepted");
								this.OnConnectedToServer();
							}
						}
					}
					else
					{
						byte[] array = MemoryPools.poolByte.Alloc((int)num);
						Array.Copy(this.receiveBuffer.Array, array, (long)((ulong)num));
						SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DataReceivedClient((int)esteamNetChannels, array, (int)num);
					}
				}
			}
			if (result != Result.NotFound)
			{
				Log.Error("[EOS-P2PS] Error reading packages: " + result.ToStringCached<Result>());
				return;
			}
		}

		// Token: 0x0600DBF1 RID: 56305 RVA: 0x004EA1E4 File Offset: 0x004E83E4
		public void LateUpdate()
		{
			if (!this.IsConnected)
			{
				return;
			}
			while (this.sendBufs.HasData())
			{
				NetworkCommonEos.SendInfo sendInfo = this.sendBufs.Dequeue();
				ProductUserId productUserId = this.serverId;
				SendPacketOptions sendPacketOptions = new SendPacketOptions
				{
					SocketId = new SocketId?(this.socketId),
					LocalUserId = this.localUser,
					RemoteUserId = productUserId,
					Channel = 0,
					Reliability = PacketReliability.ReliableOrdered,
					AllowDelayedDelivery = true,
					Data = new ArraySegment<byte>(sendInfo.Data.Items, 0, sendInfo.Data.Count)
				};
				object lockObject = AntiCheatCommon.LockObject;
				Result result;
				lock (lockObject)
				{
					result = this.ptpInterface.SendPacket(ref sendPacketOptions);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-P2PC] Could not send package to server: " + result.ToStringCached<Result>());
				}
			}
		}

		// Token: 0x0600DBF2 RID: 56306 RVA: 0x004EA2E4 File Offset: 0x004E84E4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisconnectedFromServer()
		{
			this.Disconnect();
			this.protoManager.DisconnectedFromServerEv(Localization.Get("netSteamNetworking_ConnectionClosedByServer", false, null));
		}

		// Token: 0x0600DBF3 RID: 56307 RVA: 0x004EA304 File Offset: 0x004E8504
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnConnectedToServer()
		{
			INetConnection[] array = new INetConnection[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = new NetConnectionSimple(i, null, this, null, 1, 1120);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SetConnectionToServer(array);
		}

		// Token: 0x0600DBF4 RID: 56308 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600DBF5 RID: 56309 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600DBF6 RID: 56310 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600DBF7 RID: 56311 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600DBF8 RID: 56312 RVA: 0x00032147 File Offset: 0x00030347
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600DBF9 RID: 56313 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0400A637 RID: 42551
		[PublicizedFrom(EAccessModifier.Private)]
		public const string socketName = "Game";

		// Token: 0x0400A638 RID: 42552
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A639 RID: 42553
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A63A RID: 42554
		[PublicizedFrom(EAccessModifier.Private)]
		public P2PInterface ptpInterface;

		// Token: 0x0400A63B RID: 42555
		[PublicizedFrom(EAccessModifier.Private)]
		public ProductUserId localUser;

		// Token: 0x0400A63C RID: 42556
		[PublicizedFrom(EAccessModifier.Private)]
		public SocketId socketId;

		// Token: 0x0400A63D RID: 42557
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonEos.SendInfo> sendBufs = new BlockingQueue<NetworkCommonEos.SendInfo>();

		// Token: 0x0400A63E RID: 42558
		[PublicizedFrom(EAccessModifier.Private)]
		public ProductUserId serverId;

		// Token: 0x0400A63F RID: 42559
		[PublicizedFrom(EAccessModifier.Private)]
		public bool connecting;

		// Token: 0x0400A640 RID: 42560
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disconnectEventReceived;

		// Token: 0x0400A641 RID: 42561
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1170]);
	}
}
