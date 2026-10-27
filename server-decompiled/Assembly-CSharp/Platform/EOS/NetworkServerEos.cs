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
	// Token: 0x02001D5D RID: 7517
	public class NetworkServerEos : IPlatformNetworkServer, INetworkServer
	{
		// Token: 0x17001C5D RID: 7261
		// (get) Token: 0x0600DE92 RID: 56978 RVA: 0x004F2D2A File Offset: 0x004F0F2A
		public bool ServerRunning
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.serverStarted;
			}
		}

		// Token: 0x0600DE93 RID: 56979 RVA: 0x004F2D34 File Offset: 0x004F0F34
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

		// Token: 0x0600DE94 RID: 56980 RVA: 0x004F2DA7 File Offset: 0x004F0FA7
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

		// Token: 0x0600DE95 RID: 56981 RVA: 0x004F2DE2 File Offset: 0x004F0FE2
		public void SetServerPassword(string _password)
		{
			this.serverPassword = (string.IsNullOrEmpty(_password) ? null : _password);
		}

		// Token: 0x0600DE96 RID: 56982 RVA: 0x004F2DF8 File Offset: 0x004F0FF8
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

		// Token: 0x0600DE97 RID: 56983 RVA: 0x004F2EB4 File Offset: 0x004F10B4
		public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
		{
			ProductUserId productUserId = ((UserIdentifierEos)_clientInfo.CrossplatformId).ProductUserId;
			ThreadManager.StartCoroutine(this.dropLater(productUserId, 0.2f));
		}

		// Token: 0x0600DE98 RID: 56984 RVA: 0x004F2EE4 File Offset: 0x004F10E4
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

		// Token: 0x0600DE99 RID: 56985 RVA: 0x004F2F01 File Offset: 0x004F1101
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

		// Token: 0x0600DE9A RID: 56986 RVA: 0x004F2F34 File Offset: 0x004F1134
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

		// Token: 0x0600DE9B RID: 56987 RVA: 0x004F3088 File Offset: 0x004F1288
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectionEstablishedHandler(ref OnPeerConnectionEstablishedInfo _data)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			Log.Out(string.Format("[EOS-P2PS] Connection established: {0}", _data.RemoteUserId));
		}

		// Token: 0x0600DE9C RID: 56988 RVA: 0x004F30A8 File Offset: 0x004F12A8
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

		// Token: 0x0600DE9D RID: 56989 RVA: 0x004F3198 File Offset: 0x004F1398
		[PublicizedFrom(EAccessModifier.Private)]
		public void IncomingPacketQueueFullHandler(ref OnIncomingPacketQueueFullInfo _data)
		{
			if (!this.ServerRunning)
			{
				return;
			}
			Log.Error(string.Format("[EOS-P2PS] Packet queue full: Chn={0}, IncSize={1}, Used={2}, Max={3}", new object[] { _data.OverflowPacketChannel, _data.OverflowPacketSizeBytes, _data.PacketQueueCurrentSizeBytes, _data.PacketQueueMaxSizeBytes }));
		}

		// Token: 0x0600DE9E RID: 56990 RVA: 0x004F31FC File Offset: 0x004F13FC
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

		// Token: 0x0600DE9F RID: 56991 RVA: 0x004F34F8 File Offset: 0x004F16F8
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

		// Token: 0x0600DEA0 RID: 56992 RVA: 0x004F35A4 File Offset: 0x004F17A4
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

		// Token: 0x0600DEA1 RID: 56993 RVA: 0x004F36C8 File Offset: 0x004F18C8
		public string GetIP(ClientInfo _cInfo)
		{
			NetworkServerEos.ConnectionInformation connectionInformation;
			if (!this.connections.TryGetValue(((UserIdentifierEos)_cInfo.CrossplatformId).ProductUserId, out connectionInformation))
			{
				return string.Empty;
			}
			return NetworkUtils.ToAddr(connectionInformation.Ip);
		}

		// Token: 0x0600DEA2 RID: 56994 RVA: 0x004F3708 File Offset: 0x004F1908
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

		// Token: 0x0600DEA3 RID: 56995 RVA: 0x004F3754 File Offset: 0x004F1954
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerDisconnected(ProductUserId _id)
		{
			UserIdentifierEos userIdentifierEos = new UserIdentifierEos(_id);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(userIdentifierEos);
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}

		// Token: 0x0600DEA4 RID: 56996 RVA: 0x000321FF File Offset: 0x000303FF
		public string GetServerPorts(int _basePort)
		{
			return "";
		}

		// Token: 0x0600DEA5 RID: 56997 RVA: 0x004F3784 File Offset: 0x004F1984
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

		// Token: 0x0600DEA6 RID: 56998 RVA: 0x004F3800 File Offset: 0x004F1A00
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

		// Token: 0x0600DEA7 RID: 56999 RVA: 0x004F3878 File Offset: 0x004F1A78
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

		// Token: 0x0600DEA8 RID: 57000 RVA: 0x004F3930 File Offset: 0x004F1B30
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

		// Token: 0x0600DEA9 RID: 57001 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600DEAA RID: 57002 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600DEAB RID: 57003 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600DEAC RID: 57004 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600DEAD RID: 57005 RVA: 0x000321FF File Offset: 0x000303FF
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600DEAE RID: 57006 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0600DEAF RID: 57007 RVA: 0x004F3984 File Offset: 0x004F1B84
		public int GetMaximumPacketSize(ClientInfo _cInfo, bool _reliable = false)
		{
			return 1170;
		}

		// Token: 0x0600DEB0 RID: 57008 RVA: 0x00010E62 File Offset: 0x0000F062
		public int GetBadPacketCount(ClientInfo _cInfo)
		{
			return 0;
		}

		// Token: 0x0600DEB1 RID: 57009 RVA: 0x004F398C File Offset: 0x004F1B8C
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

		// Token: 0x0400A80C RID: 43020
		[PublicizedFrom(EAccessModifier.Private)]
		public const string socketName = "Game";

		// Token: 0x0400A80D RID: 43021
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A80E RID: 43022
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A80F RID: 43023
		[PublicizedFrom(EAccessModifier.Private)]
		public P2PInterface ptpInterface;

		// Token: 0x0400A810 RID: 43024
		[PublicizedFrom(EAccessModifier.Private)]
		public bool serverStarted;

		// Token: 0x0400A811 RID: 43025
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PingCount = 50;

		// Token: 0x0400A812 RID: 43026
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ProductUserId, NetworkServerEos.ConnectionInformation> connections = new Dictionary<ProductUserId, NetworkServerEos.ConnectionInformation>();

		// Token: 0x0400A813 RID: 43027
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonEos.SendInfo> sendBufs = new BlockingQueue<NetworkCommonEos.SendInfo>();

		// Token: 0x0400A814 RID: 43028
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonEos.SendInfo> sendBufsUnreliable = new BlockingQueue<NetworkCommonEos.SendInfo>();

		// Token: 0x0400A815 RID: 43029
		[PublicizedFrom(EAccessModifier.Private)]
		public string serverPassword;

		// Token: 0x0400A816 RID: 43030
		[PublicizedFrom(EAccessModifier.Private)]
		public ProductUserId localUser;

		// Token: 0x0400A817 RID: 43031
		[PublicizedFrom(EAccessModifier.Private)]
		public SocketId socketId;

		// Token: 0x0400A818 RID: 43032
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1170]);

		// Token: 0x0400A819 RID: 43033
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ArraySegment<byte> passwordValidPacket = new ArraySegment<byte>(new byte[] { 50, 1 });

		// Token: 0x0400A81A RID: 43034
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ArraySegment<byte> passwordInvalidPacket;

		// Token: 0x0400A81B RID: 43035
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ArraySegment<byte> timeData;

		// Token: 0x02001D5E RID: 7518
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EConnectionState
		{
			// Token: 0x0400A81D RID: 43037
			Disconnected,
			// Token: 0x0400A81E RID: 43038
			Authenticating,
			// Token: 0x0400A81F RID: 43039
			Connected
		}

		// Token: 0x02001D5F RID: 7519
		[PublicizedFrom(EAccessModifier.Private)]
		public class ConnectionInformation
		{
			// Token: 0x0400A820 RID: 43040
			public NetworkServerEos.EConnectionState State;

			// Token: 0x0400A821 RID: 43041
			public uint Ip;

			// Token: 0x0400A822 RID: 43042
			public UserIdentifierEos UserIdentifier;

			// Token: 0x0400A823 RID: 43043
			public int LastPingIndex = -1;

			// Token: 0x0400A824 RID: 43044
			public readonly int[] Pings = new int[50];
		}
	}
}
