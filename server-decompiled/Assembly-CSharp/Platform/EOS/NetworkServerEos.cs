using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using UnityEngine;
using UnityEngine.Networking;

namespace Platform.EOS
{
	// Token: 0x02001CFA RID: 7418
	public class NetworkServerEos : IPlatformNetworkServer, INetworkServer
	{
		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x0600DC03 RID: 56323 RVA: 0x004EA642 File Offset: 0x004E8842
		public bool ServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.serverStarted;
			}
		}

		// Token: 0x0600DC04 RID: 56324 RVA: 0x004EA64C File Offset: 0x004E884C
		public NetworkServerEos(IPlatform _owner, IProtocolManagerProtocolInterface _protoManager)
		{
			this.owner = _owner;
			this.protoManager = _protoManager;
			this.owner.Api.ClientApiInitialized += delegate
			{
				if (!GameManager.IsDedicatedServer)
				{
					EosHelpers.AssertMainThread("P2PS.Init");
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

		// Token: 0x0600DC05 RID: 56325 RVA: 0x004EA6BF File Offset: 0x004E88BF
		public NetworkConnectionError StartServer(int _basePort, string _password)
		{
			if (this.ServerRunning)
			{
				Log.Error("[EOS-P2PS] Server already running");
				return NetworkConnectionError.AlreadyConnectedToServer;
			}
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
			this.serverStarted = true;
			Log.Out("[EOS-P2PS] Server started");
			return NetworkConnectionError.NoError;
		}

		// Token: 0x0600DC06 RID: 56326 RVA: 0x004EA6FA File Offset: 0x004E88FA
		public void SetServerPassword(string _password)
		{
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
		}

		// Token: 0x0600DC07 RID: 56327 RVA: 0x004EA710 File Offset: 0x004E8910
		public void StopServer()
		{
			if (!this.ServerRunning)
			{
				return;
			}
			this.serverStarted = false;
			this.connections.Clear();
			EosHelpers.AssertMainThread("P2PS.Stop");
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
				Log.Error("[EOS-P2PS] Failed closing connections: " + result.ToStringCached<Result>());
			}
			Log.Out("[EOS-P2PS] Server stopped");
		}

		// Token: 0x0600DC08 RID: 56328 RVA: 0x004EA7CC File Offset: 0x004E89CC
		public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
		{
			ProductUserId productUserId = ((UserIdentifierEos)_clientInfo.CrossplatformId).ProductUserId;
			ThreadManager.StartCoroutine(this.dropLater(productUserId, 0.2f));
		}

		// Token: 0x0600DC09 RID: 56329 RVA: 0x004EA7FC File Offset: 0x004E89FC
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator dropLater(ProductUserId _id, float _delay)
		{
			yield return new WaitForSeconds(_delay);
			if (!this.ServerRunning)
			{
				yield break;
			}
			NetworkServerEos.ConnectionInformation connectionInformation;
			if (this.connections.TryGetValue(_id, out connectionInformation))
			{
				Log.Out("[EOS-P2PS] Dropping client: " + ((_id != null) ? _id.ToString() : null));
				connectionInformation.State = NetworkServerEos.EConnectionState.Disconnected;
				this.OnPlayerDisconnected(_id);
			}
			CloseConnectionOptions closeConnectionOptions = new CloseConnectionOptions
			{
				SocketId = new SocketId?(this.socketId),
				LocalUserId = this.localUser,
				RemoteUserId = _id
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.ptpInterface.CloseConnection(ref closeConnectionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error(string.Format("[EOS-P2PS] Failed closing connection: {0}: {1}", _id, result.ToStringCached<Result>()));
			}
			yield break;
		}

		// Token: 0x0600DC0A RID: 56330 RVA: 0x004EA819 File Offset: 0x004E8A19
		public NetworkError SendData(ClientInfo _clientInfo, int _channel, ArrayListMP<byte> _data, bool _reliableDelivery = true)
		{
			if (this.ServerRunning)
			{
				_data[0] = (byte)_channel;
				this.sendBufs.Enqueue(new NetworkCommonEos.SendInfo(_clientInfo, _data));
			}
			else
			{
				Log.Warning("[EOS-P2PS] Tried to send a package to a client while not being a server");
			}
			return NetworkError.Ok;
		}

		// Token: 0x0600DC0B RID: 56331 RVA: 0x004EA84C File Offset: 0x004E8A4C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionRequestHandler(ref OnIncomingConnectionRequestInfo _data)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			ProductUserId remoteUserId = _data.RemoteUserId;
			UserIdentifierEos userIdentifierEos = new UserIdentifierEos(remoteUserId);
			if (_data.SocketId.Value.SocketName != "Game")
			{
				Log.Warning(string.Concat(new string[]
				{
					"[EOS-P2PS] P2P session request from ",
					userIdentifierEos.ProductUserIdString,
					" with invalid socket name '",
					_data.SocketId.Value.SocketName,
					"'"
				}));
				return;
			}
			Log.Out("[EOS-P2PS] P2PSessionRequest from: " + userIdentifierEos.ProductUserIdString);
			AcceptConnectionOptions acceptConnectionOptions = new AcceptConnectionOptions
			{
				SocketId = new SocketId?(this.socketId),
				LocalUserId = this.localUser,
				RemoteUserId = remoteUserId
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.ptpInterface.AcceptConnection(ref acceptConnectionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-P2PS] Failed accepting session: " + result.ToStringCached<Result>());
				return;
			}
			this.connections[remoteUserId] = new NetworkServerEos.ConnectionInformation
			{
				State = NetworkServerEos.EConnectionState.Authenticating,
				UserIdentifier = userIdentifierEos
			};
		}

		// Token: 0x0600DC0C RID: 56332 RVA: 0x004EA9A0 File Offset: 0x004E8BA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionEstablishedHandler(ref OnPeerConnectionEstablishedInfo _data)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			Log.Out(string.Format("[EOS-P2PS] Connection established: {0}", _data.RemoteUserId));
		}

		// Token: 0x0600DC0D RID: 56333 RVA: 0x004EA9C0 File Offset: 0x004E8BC0
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionClosedHandler(ref OnRemoteConnectionClosedInfo _data)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			ProductUserId remoteUserId = _data.RemoteUserId;
			Log.Out(string.Format("[EOS-P2PS] Connection closed by {0}: ", remoteUserId) + _data.Reason.ToStringCached<ConnectionClosedReason>());
			NetworkServerEos.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(remoteUserId, out connectionInformation) || connectionInformation.State != NetworkServerEos.EConnectionState.Connected)
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
				Log.Error(string.Format("[EOS-P2PS] Failed closing connection to {0}: {1}", remoteUserId, result.ToStringCached<Result>()));
			}
			connectionInformation.State = NetworkServerEos.EConnectionState.Disconnected;
			this.OnPlayerDisconnected(remoteUserId);
		}

		// Token: 0x0600DC0E RID: 56334 RVA: 0x004EAAB0 File Offset: 0x004E8CB0
		[PublicizedFrom(EAccessModifier.Private)]
		public void IncomingPacketQueueFullHandler(ref OnIncomingPacketQueueFullInfo _data)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			Log.Error(string.Format("[EOS-P2PS] Packet queue full: Chn={0}, IncSize={1}, Used={2}, Max={3}", new object[] { _data.OverflowPacketChannel, _data.OverflowPacketSizeBytes, _data.PacketQueueCurrentSizeBytes, _data.PacketQueueMaxSizeBytes }));
		}

		// Token: 0x0600DC0F RID: 56335 RVA: 0x004EAB14 File Offset: 0x004E8D14
		public void Update()
		{
			if (!this.ServerRunning)
			{
				return;
			}
			long num = (long)(Time.unscaledTime * 1000f);
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
				uint num2;
				lock (obj)
				{
					byte b;
					result = this.ptpInterface.ReceivePacket(ref receivePacketOptions, ref productUserId, ref socketId, out b, this.receiveBuffer, out num2);
				}
				if (result != Result.Success)
				{
					break;
				}
				NetworkServerEos.ConnectionInformation connectionInformation;
				if (!this.connections.TryGetValue(productUserId, out connectionInformation) || connectionInformation.State == NetworkServerEos.EConnectionState.Disconnected)
				{
					string text = "[EOS-P2PS] Received package from an unconnected client: ";
					ProductUserId productUserId2 = productUserId;
					Log.Out(text + ((productUserId2 != null) ? productUserId2.ToString() : null));
				}
				else if (num2 != 0U)
				{
					NetworkCommonEos.ESteamNetChannels esteamNetChannels = (NetworkCommonEos.ESteamNetChannels)this.receiveBuffer.Array[0];
					if (esteamNetChannels > NetworkCommonEos.ESteamNetChannels.NetpackageChannel1)
					{
						if (esteamNetChannels != NetworkCommonEos.ESteamNetChannels.Authentication)
						{
							if (esteamNetChannels != NetworkCommonEos.ESteamNetChannels.Ping)
							{
								string text2 = "[EOS-P2PS] Received package on an unknown channel from: ";
								ProductUserId productUserId3 = productUserId;
								Log.Out(text2 + ((productUserId3 != null) ? productUserId3.ToString() : null));
							}
							else
							{
								this.UpdatePing(productUserId, this.receiveBuffer.Array, num);
							}
						}
						else if (connectionInformation.State == NetworkServerEos.EConnectionState.Authenticating)
						{
							string @string = Encoding.UTF8.GetString(this.receiveBuffer.Array, 1, (int)(num2 - 1U));
							bool flag2 = this.Authenticate(productUserId, @string);
							SendPacketOptions sendPacketOptions = new SendPacketOptions
							{
								SocketId = new SocketId?(this.socketId),
								LocalUserId = this.localUser,
								RemoteUserId = productUserId,
								Channel = 0,
								Reliability = PacketReliability.ReliableOrdered,
								AllowDelayedDelivery = true,
								Data = (flag2 ? NetworkServerEos.passwordValidPacket : NetworkServerEos.passwordInvalidPacket)
							};
							obj = AntiCheatCommon.LockObject;
							Result result2;
							lock (obj)
							{
								result2 = this.ptpInterface.SendPacket(ref sendPacketOptions);
							}
							if (result2 != Result.Success)
							{
								Log.Error(string.Format("[EOS-P2PS] Could not send package to client {0}: {1}", productUserId, result2.ToStringCached<Result>()));
							}
						}
					}
					else if (connectionInformation.State == NetworkServerEos.EConnectionState.Connected)
					{
						byte[] array = MemoryPools.poolByte.Alloc((int)num2);
						Array.Copy(this.receiveBuffer.Array, array, (long)((ulong)num2));
						ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(connectionInformation.UserIdentifier);
						SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DataReceivedServer(clientInfo, (int)esteamNetChannels, array, (int)num2);
					}
					else
					{
						string text3 = "[EOS-P2PS] Received package from an unauthenticated client: ";
						ProductUserId productUserId4 = productUserId;
						Log.Out(text3 + ((productUserId4 != null) ? productUserId4.ToString() : null));
					}
				}
			}
			if (result != Result.NotFound)
			{
				Log.Error("[EOS-P2PS] Error reading packages: " + result.ToStringCached<Result>());
			}
			if (result == Result.InvalidAuth)
			{
				this.StopServer();
				return;
			}
		}

		// Token: 0x0600DC10 RID: 56336 RVA: 0x004EAE10 File Offset: 0x004E9010
		public void LateUpdate()
		{
			if (!this.ServerRunning)
			{
				return;
			}
			this.sendBuffers(this.sendBufs, PacketReliability.ReliableOrdered);
			this.sendBuffers(this.sendBufsUnreliable, PacketReliability.UnreliableUnordered);
			Utils.GetBytes((long)(Time.unscaledTime * 1000f), NetworkServerEos.timeData.Array, 1);
			foreach (KeyValuePair<ProductUserId, NetworkServerEos.ConnectionInformation> keyValuePair in this.connections)
			{
				if (keyValuePair.Value.State == NetworkServerEos.EConnectionState.Connected)
				{
					this.FlushBuffer(keyValuePair.Key);
				}
			}
		}

		// Token: 0x0600DC11 RID: 56337 RVA: 0x004EAEBC File Offset: 0x004E90BC
		[PublicizedFrom(EAccessModifier.Private)]
		public void sendBuffers(BlockingQueue<NetworkCommonEos.SendInfo> _buffers, PacketReliability _queue)
		{
			while (_buffers.HasData())
			{
				NetworkCommonEos.SendInfo sendInfo = _buffers.Dequeue();
				ProductUserId productUserId = ((UserIdentifierEos)sendInfo.Recipient.CrossplatformId).ProductUserId;
				NetworkServerEos.ConnectionInformation connectionInformation;
				if (this.connections.TryGetValue(productUserId, out connectionInformation) && connectionInformation.State == NetworkServerEos.EConnectionState.Connected)
				{
					SendPacketOptions sendPacketOptions = new SendPacketOptions
					{
						SocketId = new SocketId?(this.socketId),
						LocalUserId = this.localUser,
						RemoteUserId = productUserId,
						Channel = 0,
						Reliability = _queue,
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
						Log.Error(string.Format("[EOS-P2PS] Could not send package to client {0}: {1}", productUserId, result.ToStringCached<Result>()));
					}
				}
			}
		}

		// Token: 0x0600DC12 RID: 56338 RVA: 0x004EAFE0 File Offset: 0x004E91E0
		public string GetIP(ClientInfo _cInfo)
		{
			NetworkServerEos.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(((UserIdentifierEos)_cInfo.CrossplatformId).ProductUserId, out connectionInformation))
			{
				return string.Empty;
			}
			return NetworkUtils.ToAddr(connectionInformation.Ip);
		}

		// Token: 0x0600DC13 RID: 56339 RVA: 0x004EB020 File Offset: 0x004E9220
		public int GetPing(ClientInfo _cInfo)
		{
			NetworkServerEos.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(((UserIdentifierEos)_cInfo.CrossplatformId).ProductUserId, out connectionInformation))
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

		// Token: 0x0600DC14 RID: 56340 RVA: 0x004EB06C File Offset: 0x004E926C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerDisconnected(ProductUserId _id)
		{
			UserIdentifierEos userIdentifierEos = new UserIdentifierEos(_id);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(userIdentifierEos);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}

		// Token: 0x0600DC15 RID: 56341 RVA: 0x00032147 File Offset: 0x00030347
		public string GetServerPorts(int _basePort)
		{
			return "";
		}

		// Token: 0x0600DC16 RID: 56342 RVA: 0x004EB09C File Offset: 0x004E929C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool Authenticate(ProductUserId _id, string _password)
		{
			bool flag = string.IsNullOrEmpty(this.serverPassword) || _password == this.serverPassword;
			Log.Out(string.Format("[EOS-P2PS] Received authentication package from {0}: {1} password", _id, flag ? "valid" : "invalid"));
			if (!flag)
			{
				this.connections[_id].State = NetworkServerEos.EConnectionState.Authenticating;
				return false;
			}
			this.connections[_id].State = NetworkServerEos.EConnectionState.Connected;
			this.OnPlayerConnected(_id);
			return true;
		}

		// Token: 0x0600DC17 RID: 56343 RVA: 0x004EB118 File Offset: 0x004E9318
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerConnected(ProductUserId _id)
		{
			ClientInfo clientInfo = new ClientInfo
			{
				CrossplatformId = new UserIdentifierEos(_id),
				network = this,
				netConnection = new INetConnection[2]
			};
			for (int i = 0; i < 2; i++)
			{
				clientInfo.netConnection[i] = new NetConnectionSimple(i, clientInfo, null, clientInfo.InternalId.CombinedString, 1, 1120);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.AddClient(clientInfo);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerConnected(clientInfo);
		}

		// Token: 0x0600DC18 RID: 56344 RVA: 0x004EB190 File Offset: 0x004E9390
		[PublicizedFrom(EAccessModifier.Private)]
		public void FlushBuffer(ProductUserId _id)
		{
			SendPacketOptions sendPacketOptions = new SendPacketOptions
			{
				SocketId = new SocketId?(this.socketId),
				LocalUserId = this.localUser,
				RemoteUserId = _id,
				Channel = 0,
				Reliability = PacketReliability.ReliableOrdered,
				AllowDelayedDelivery = true,
				Data = NetworkServerEos.timeData
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.ptpInterface.SendPacket(ref sendPacketOptions);
			}
			if (result != Result.Success)
			{
				Log.Error(string.Format("[EOS-P2PS] Could not send ping package to client {0}: {1}", _id, result.ToStringCached<Result>()));
			}
		}

		// Token: 0x0600DC19 RID: 56345 RVA: 0x004EB248 File Offset: 0x004E9448
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdatePing(ProductUserId _sourceId, byte[] _data, long _curTime)
		{
			long num = BitConverter.ToInt64(_data, 1);
			int num2 = (int)(_curTime - num);
			NetworkServerEos.ConnectionInformation connectionInformation = this.connections[_sourceId];
			connectionInformation.LastPingIndex++;
			if (connectionInformation.LastPingIndex >= 50)
			{
				connectionInformation.LastPingIndex = 0;
			}
			connectionInformation.Pings[connectionInformation.LastPingIndex] = num2;
		}

		// Token: 0x0600DC1A RID: 56346 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600DC1B RID: 56347 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600DC1C RID: 56348 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600DC1D RID: 56349 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600DC1E RID: 56350 RVA: 0x00032147 File Offset: 0x00030347
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600DC1F RID: 56351 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0600DC20 RID: 56352 RVA: 0x004EB29C File Offset: 0x004E949C
		public int GetMaximumPacketSize(ClientInfo _cInfo, bool _reliable = false)
		{
			return 1170;
		}

		// Token: 0x0600DC21 RID: 56353 RVA: 0x00010E62 File Offset: 0x0000F062
		public int GetBadPacketCount(ClientInfo _cInfo)
		{
			return 0;
		}

		// Token: 0x0600DC22 RID: 56354 RVA: 0x004EB2A4 File Offset: 0x004E94A4
		// Note: this type is marked as 'beforefieldinit'.
		[PublicizedFrom(EAccessModifier.Private)]
		static NetworkServerEos()
		{
			byte[] array = new byte[2];
			array[0] = 50;
			NetworkServerEos.passwordInvalidPacket = new ArraySegment<byte>(array);
			byte[] array2 = new byte[9];
			array2[0] = 60;
			NetworkServerEos.timeData = new ArraySegment<byte>(array2);
		}

		// Token: 0x0400A64E RID: 42574
		[PublicizedFrom(EAccessModifier.Private)]
		public const string socketName = "Game";

		// Token: 0x0400A64F RID: 42575
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A650 RID: 42576
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A651 RID: 42577
		[PublicizedFrom(EAccessModifier.Private)]
		public P2PInterface ptpInterface;

		// Token: 0x0400A652 RID: 42578
		[PublicizedFrom(EAccessModifier.Private)]
		public bool serverStarted;

		// Token: 0x0400A653 RID: 42579
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PingCount = 50;

		// Token: 0x0400A654 RID: 42580
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ProductUserId, NetworkServerEos.ConnectionInformation> connections = new Dictionary<ProductUserId, NetworkServerEos.ConnectionInformation>();

		// Token: 0x0400A655 RID: 42581
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonEos.SendInfo> sendBufs = new BlockingQueue<NetworkCommonEos.SendInfo>();

		// Token: 0x0400A656 RID: 42582
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonEos.SendInfo> sendBufsUnreliable = new BlockingQueue<NetworkCommonEos.SendInfo>();

		// Token: 0x0400A657 RID: 42583
		[PublicizedFrom(EAccessModifier.Private)]
		public string serverPassword;

		// Token: 0x0400A658 RID: 42584
		[PublicizedFrom(EAccessModifier.Private)]
		public ProductUserId localUser;

		// Token: 0x0400A659 RID: 42585
		[PublicizedFrom(EAccessModifier.Private)]
		public SocketId socketId;

		// Token: 0x0400A65A RID: 42586
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1170]);

		// Token: 0x0400A65B RID: 42587
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ArraySegment<byte> passwordValidPacket = new ArraySegment<byte>(new byte[] { 50, 1 });

		// Token: 0x0400A65C RID: 42588
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ArraySegment<byte> passwordInvalidPacket;

		// Token: 0x0400A65D RID: 42589
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ArraySegment<byte> timeData;

		// Token: 0x02001CFB RID: 7419
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EConnectionState
		{
			// Token: 0x0400A65F RID: 42591
			Disconnected,
			// Token: 0x0400A660 RID: 42592
			Authenticating,
			// Token: 0x0400A661 RID: 42593
			Connected
		}

		// Token: 0x02001CFC RID: 7420
		[PublicizedFrom(EAccessModifier.Private)]
		public class ConnectionInformation
		{
			// Token: 0x0400A662 RID: 42594
			public NetworkServerEos.EConnectionState State;

			// Token: 0x0400A663 RID: 42595
			public uint Ip;

			// Token: 0x0400A664 RID: 42596
			public UserIdentifierEos UserIdentifier;

			// Token: 0x0400A665 RID: 42597
			public int LastPingIndex = -1;

			// Token: 0x0400A666 RID: 42598
			public readonly int[] Pings = new int[50];
		}
	}
}
