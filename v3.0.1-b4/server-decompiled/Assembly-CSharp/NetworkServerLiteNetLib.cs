using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using UnityEngine.Networking;

// Token: 0x0200087F RID: 2175
public class NetworkServerLiteNetLib : INetworkServer
{
	// Token: 0x06003F46 RID: 16198 RVA: 0x0018CA47 File Offset: 0x0018AC47
	public NetworkServerLiteNetLib(IProtocolManagerProtocolInterface _protoManager)
	{
		this.protoManager = _protoManager;
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x0018CA6C File Offset: 0x0018AC6C
	public void Update()
	{
		ClientInfo clientInfo;
		do
		{
			clientInfo = null;
			List<ClientInfo> list = this.dropClientsQueue;
			lock (list)
			{
				int num = this.dropClientsQueue.Count - 1;
				if (num >= 0)
				{
					clientInfo = this.dropClientsQueue[num];
					this.dropClientsQueue.RemoveAt(num);
				}
			}
			if (clientInfo != null)
			{
				this.DropClient(clientInfo, false);
			}
		}
		while (clientInfo != null);
		NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer liteNetLibAuthWrapperServer = this.authWrapper;
		if (liteNetLibAuthWrapperServer == null)
		{
			return;
		}
		liteNetLibAuthWrapperServer.Update();
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x000027FC File Offset: 0x000009FC
	public void LateUpdate()
	{
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x0018CAF4 File Offset: 0x0018ACF4
	public NetworkConnectionError StartServer(int _basePort, string _password)
	{
		this.serverPassword = (string.IsNullOrEmpty(_password) ? "" : _password);
		EventBasedNetListener eventBasedNetListener = new EventBasedNetListener();
		this.server = new NetManager(eventBasedNetListener, null, true);
		this.authWrapper = new NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer(this);
		NetworkCommonLiteNetLib.InitConfig(this.server);
		eventBasedNetListener.ConnectionRequestEvent += this.authWrapper.ConnectionRequestCheck;
		eventBasedNetListener.PeerConnectedEvent += this.authWrapper.OnPeerConnectedEvent;
		eventBasedNetListener.PeerDisconnectedEvent += this.authWrapper.OnPeerDisconnectedEvent;
		eventBasedNetListener.NetworkReceiveEvent += this.authWrapper.OnNetworkReceiveEvent;
		eventBasedNetListener.NetworkErrorEvent += delegate(IPEndPoint _endpoint, SocketError _code)
		{
			Log.Error("NET: LiteNetLib: Network error: {0}", new object[] { _code });
		};
		if (this.server.Start(_basePort + 2))
		{
			Log.Out("NET: LiteNetLib server started");
			return NetworkConnectionError.NoError;
		}
		Log.Out("NET: LiteNetLib server could not be started");
		return NetworkConnectionError.CreateSocketOrThreadFailure;
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x0018CBEC File Offset: 0x0018ADEC
	public void SetServerPassword(string _password)
	{
		this.serverPassword = _password ?? "";
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0018CC00 File Offset: 0x0018AE00
	public void StopServer()
	{
		NetManager netManager = this.server;
		if (netManager != null && netManager.IsRunning)
		{
			List<NetPeer> list = new List<NetPeer>();
			this.server.GetPeersNonAlloc(list, ConnectionState.Any);
			for (int i = 0; i < list.Count; i++)
			{
				this.server.DisconnectPeer(list[i], NetworkServerLiteNetLib.disconnectServerShutdown);
			}
			this.server.Stop();
			this.server = null;
			this.authWrapper = null;
		}
		Log.Out("NET: LiteNetLib server stopped");
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0018CC80 File Offset: 0x0018AE80
	public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
	{
		this.OnPlayerDisconnected(_clientInfo.litenetPeerConnectId);
		NetPeer peerByConnectId = this.GetPeerByConnectId(_clientInfo.litenetPeerConnectId);
		if (peerByConnectId != null)
		{
			this.server.DisconnectPeer(peerByConnectId, NetworkServerLiteNetLib.disconnectFromClientSide);
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x0018CCBC File Offset: 0x0018AEBC
	public NetworkError SendData(ClientInfo _cInfo, int _channel, ArrayListMP<byte> _data, bool _reliableDelivery = true)
	{
		NetPeer peerByConnectId = this.GetPeerByConnectId(_cInfo.litenetPeerConnectId);
		if (peerByConnectId == null)
		{
			Log.Warning("NET: LiteNetLib: SendData requested for unknown client {0}", new object[] { _cInfo.ToString() });
			List<ClientInfo> list = this.dropClientsQueue;
			lock (list)
			{
				if (!this.dropClientsQueue.Contains(_cInfo))
				{
					this.dropClientsQueue.Add(_cInfo);
				}
			}
			return NetworkError.WrongConnection;
		}
		_data[0] = (byte)_channel;
		if (ConnectionManager.VerboseNetLogging)
		{
			Log.Out("Sending data to peer {2}: ch={0}, size={1}", new object[]
			{
				_channel,
				_data.Count,
				_cInfo.InternalId.CombinedString
			});
		}
		peerByConnectId.Send(_data.Items, 0, _data.Count, _reliableDelivery ? DeliveryMethod.ReliableOrdered : DeliveryMethod.Unreliable);
		return NetworkError.Ok;
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x0018CD9C File Offset: 0x0018AF9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PeerConnectedEvent(NetPeer _peer)
	{
		Log.Out(string.Format("NET: LiteNetLib: Connect from: {0} / {1}", _peer, _peer.Id));
		ClientInfo clientInfo = new ClientInfo
		{
			litenetPeerConnectId = (long)_peer.Id,
			network = this,
			netConnection = new INetConnection[2]
		};
		for (int i = 0; i < 2; i++)
		{
			clientInfo.netConnection[i] = new NetConnectionSimple(i, clientInfo, null, _peer.Id.ToString(), 1, 0);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.AddClient(clientInfo);
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerConnected(clientInfo);
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x0018CE2C File Offset: 0x0018B02C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PeerDisconnectedEvent(NetPeer _peer, DisconnectInfo _info)
	{
		Log.Out(string.Format("NET: LiteNetLib: Client disconnect from: {0} / {1} ({2})", _peer, _peer.Id, _info.Reason.ToStringCached<DisconnectReason>()));
		if (_info.Reason == DisconnectReason.Timeout)
		{
			Log.Out(string.Format("NET: LiteNetLib: TimeSinceLastPacket: {0}", _peer.TimeSinceLastPacket));
		}
		ThreadManager.AddSingleTaskMainThread("PlayerDisconnectLiteNetLib", delegate(object _)
		{
			Log.Out(string.Format("NET: LiteNetLib: MT: Client disconnect from: {0} / {1} ({2})", _peer, _peer.Id, _info.Reason.ToStringCached<DisconnectReason>()));
			this.OnPlayerDisconnected((long)_peer.Id);
		}, null);
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x0018CED0 File Offset: 0x0018B0D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void NetworkReceiveEvent(NetPeer _peer, NetPacketReader _reader, DeliveryMethod _deliveryMethod)
	{
		ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForLiteNetPeer((long)_peer.Id);
		if (clientInfo == null)
		{
			Log.Out("NET: LiteNetLib: Received package from an unknown client: " + ((_peer != null) ? _peer.ToString() : null));
			return;
		}
		if (_reader.AvailableBytes == 0)
		{
			string text = "NET: LiteNetLib: Received package with zero size from: ";
			ClientInfo clientInfo2 = clientInfo;
			Log.Out(text + ((clientInfo2 != null) ? clientInfo2.ToString() : null));
			return;
		}
		int availableBytes = _reader.AvailableBytes;
		byte[] array = MemoryPools.poolByte.Alloc(availableBytes);
		_reader.GetBytes(array, availableBytes);
		if (ConnectionManager.VerboseNetLogging)
		{
			Log.Out("Received data from peer {2}: ch={0}, size={1}", new object[]
			{
				array[0],
				availableBytes,
				clientInfo.InternalId.CombinedString
			});
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DataReceivedServer(clientInfo, (int)array[0], array, availableBytes);
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x0018CFA0 File Offset: 0x0018B1A0
	public string GetIP(ClientInfo _cInfo)
	{
		NetPeer peerByConnectId = this.GetPeerByConnectId(_cInfo.litenetPeerConnectId);
		if (peerByConnectId == null)
		{
			Log.Warning("NET: LiteNetLib: IP requested for unknown client {0}", new object[] { _cInfo.ToString() });
			return string.Empty;
		}
		return peerByConnectId.Address.ToString();
	}

	// Token: 0x06003F52 RID: 16210 RVA: 0x0018CFE8 File Offset: 0x0018B1E8
	public int GetPing(ClientInfo _cInfo)
	{
		NetPeer peerByConnectId = this.GetPeerByConnectId(_cInfo.litenetPeerConnectId);
		if (peerByConnectId == null)
		{
			Log.Warning("NET: LiteNetLib: Ping requested for unknown client {0}", new object[] { _cInfo.ToString() });
			return -1;
		}
		return peerByConnectId.Ping;
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x0018D028 File Offset: 0x0018B228
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPlayerDisconnected(long _peerConnectId)
	{
		ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForLiteNetPeer(_peerConnectId);
		if (clientInfo != null)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}
	}

	// Token: 0x06003F54 RID: 16212 RVA: 0x0018D054 File Offset: 0x0018B254
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPeer GetPeerByConnectId(long _connectId)
	{
		if (this.server == null)
		{
			return null;
		}
		List<NetPeer> list = this.getPeersList;
		lock (list)
		{
			this.server.GetPeersNonAlloc(this.getPeersList, ConnectionState.Any);
			for (int i = 0; i < this.getPeersList.Count; i++)
			{
				if ((long)this.getPeersList[i].Id == _connectId)
				{
					return this.getPeersList[i];
				}
			}
		}
		return null;
	}

	// Token: 0x06003F55 RID: 16213 RVA: 0x0018D0E8 File Offset: 0x0018B2E8
	public int GetBadPacketCount(ClientInfo _cInfo)
	{
		NetPeer peerByConnectId = this.GetPeerByConnectId(_cInfo.litenetPeerConnectId);
		if (peerByConnectId != null)
		{
			return peerByConnectId.badPacketCount;
		}
		return 0;
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x0018D110 File Offset: 0x0018B310
	public string GetServerPorts(int _basePort)
	{
		return (_basePort + 2).ToString() + "/UDP";
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x0018D132 File Offset: 0x0018B332
	public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
	{
		if (this.server != null)
		{
			this.server.SimulateLatency = _enable;
			this.server.SimulationMinLatency = _minLatency;
			this.server.SimulationMaxLatency = _maxLatency;
		}
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x0018D160 File Offset: 0x0018B360
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		if (this.server != null)
		{
			this.server.SimulatePacketLoss = _enable;
			this.server.SimulationPacketLossChance = _chance;
		}
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x0018D182 File Offset: 0x0018B382
	public void EnableStatistics()
	{
		if (this.server != null)
		{
			this.server.EnableStatistics = true;
		}
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x0018D198 File Offset: 0x0018B398
	public void DisableStatistics()
	{
		if (this.server != null)
		{
			this.server.EnableStatistics = false;
		}
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x0018D1AE File Offset: 0x0018B3AE
	public string PrintNetworkStatistics()
	{
		if (this.server != null)
		{
			return this.server.Statistics.ToString();
		}
		return "no server!";
	}

	// Token: 0x06003F5C RID: 16220 RVA: 0x0018D1CE File Offset: 0x0018B3CE
	public void ResetNetworkStatistics()
	{
		if (this.server != null)
		{
			this.server.Statistics.Reset();
		}
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x0018D1E8 File Offset: 0x0018B3E8
	public int GetMaximumPacketSize(ClientInfo _cInfo, bool _reliable = false)
	{
		int num = -1;
		NetPeer peerByConnectId = this.GetPeerByConnectId(_cInfo.litenetPeerConnectId);
		if (peerByConnectId != null)
		{
			num = peerByConnectId.GetMaxSinglePacketSize(_reliable ? DeliveryMethod.ReliableOrdered : DeliveryMethod.Unreliable);
		}
		return num;
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x0018D218 File Offset: 0x0018B418
	// Note: this type is marked as 'beforefieldinit'.
	[PublicizedFrom(EAccessModifier.Private)]
	static NetworkServerLiteNetLib()
	{
		byte[] array = new byte[2];
		array[0] = 1;
		NetworkServerLiteNetLib.rejectRateLimit = array;
		byte[] array2 = new byte[2];
		array2[0] = 2;
		NetworkServerLiteNetLib.rejectPendingConnection = array2;
		byte[] array3 = new byte[2];
		array3[0] = 3;
		NetworkServerLiteNetLib.disconnectServerShutdown = array3;
		byte[] array4 = new byte[2];
		array4[0] = 4;
		NetworkServerLiteNetLib.disconnectFromClientSide = array4;
	}

	// Token: 0x040032D8 RID: 13016
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] rejectInvalidPassword = new byte[2];

	// Token: 0x040032D9 RID: 13017
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] rejectRateLimit;

	// Token: 0x040032DA RID: 13018
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] rejectPendingConnection;

	// Token: 0x040032DB RID: 13019
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] disconnectServerShutdown;

	// Token: 0x040032DC RID: 13020
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] disconnectFromClientSide;

	// Token: 0x040032DD RID: 13021
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IProtocolManagerProtocolInterface protoManager;

	// Token: 0x040032DE RID: 13022
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverPassword;

	// Token: 0x040032DF RID: 13023
	[PublicizedFrom(EAccessModifier.Private)]
	public NetManager server;

	// Token: 0x040032E0 RID: 13024
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer authWrapper;

	// Token: 0x040032E1 RID: 13025
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ClientInfo> dropClientsQueue = new List<ClientInfo>();

	// Token: 0x040032E2 RID: 13026
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NetPeer> getPeersList = new List<NetPeer>();

	// Token: 0x02000880 RID: 2176
	[PublicizedFrom(EAccessModifier.Private)]
	public class LiteNetLibAuthWrapperServer
	{
		// Token: 0x06003F5F RID: 16223 RVA: 0x0018D26C File Offset: 0x0018B46C
		public LiteNetLibAuthWrapperServer(NetworkServerLiteNetLib _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0018D29C File Offset: 0x0018B49C
		public void Update()
		{
			DateTime now = DateTime.Now;
			if (now < this.nextStateChecksRun)
			{
				return;
			}
			this.nextStateChecksRun = now + NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.ConnectionStateCheckInterval;
			Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> dictionary = this.authStates;
			lock (dictionary)
			{
				this.clearList.Clear();
				foreach (KeyValuePair<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> keyValuePair in this.authStates)
				{
					int num;
					NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState lnlauthConnectionState;
					keyValuePair.Deconstruct(out num, out lnlauthConnectionState);
					int num2 = num;
					NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState lnlauthConnectionState2 = lnlauthConnectionState;
					if (lnlauthConnectionState2.State == NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState.EState.Authenticating && !(lnlauthConnectionState2.Age <= NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.MaxDurationInAuthState))
					{
						this.clearList.Add(num2);
					}
				}
				foreach (int num3 in this.clearList)
				{
					this.authStates.Remove(num3);
				}
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0018D3C8 File Offset: 0x0018B5C8
		public void ConnectionRequestCheck(ConnectionRequest _request)
		{
			string text = _request.RemoteEndPoint.Address.ToString();
			DateTime now = DateTime.Now;
			DateTime dateTime;
			this.lastConnectAttemptTimes.TryGetValue(text, out dateTime);
			TimeSpan timeSpan = now - dateTime;
			this.lastConnectAttemptTimes[text] = now;
			if (timeSpan.TotalMilliseconds < 500.0)
			{
				Log.Out("NET: Rejecting connection request from " + text + ": Limiting connect rate from that IP!");
				_request.Reject(NetworkServerLiteNetLib.rejectRateLimit);
				return;
			}
			foreach (ClientInfo clientInfo in SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List)
			{
				if (!clientInfo.loginDone && clientInfo.ip == text)
				{
					Log.Out("NET: Rejecting connection request from " + text + ": A connection attempt from that IP is currently being processed!");
					_request.Reject(NetworkServerLiteNetLib.rejectPendingConnection);
					return;
				}
			}
			if (_request.Data.GetString() != this.owner.serverPassword)
			{
				_request.Reject(NetworkServerLiteNetLib.rejectInvalidPassword);
				return;
			}
			_request.Accept();
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x0018D4F8 File Offset: 0x0018B6F8
		public void OnPeerConnectedEvent(NetPeer _peer)
		{
			Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> dictionary = this.authStates;
			NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState lnlauthConnectionState;
			lock (dictionary)
			{
				if (this.authStates.TryGetValue(_peer.Id, out lnlauthConnectionState) && ConnectionManager.VerboseNetLogging)
				{
					Log.Out(string.Format("NET: LiteNetLib: New peer {0} with previously used peer id {1}", _peer, _peer.Id));
				}
				lnlauthConnectionState = new NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState();
				this.authStates[_peer.Id] = lnlauthConnectionState;
			}
			byte[] array = new byte[17];
			array[0] = 202;
			lnlauthConnectionState.Challenge.WriteToBuffer(array, 1);
			if (ConnectionManager.VerboseNetLogging)
			{
				Log.Out(string.Format("NET: LiteNetLib: Sending challenge {0} to new peer {1} / {2}", lnlauthConnectionState.Challenge, _peer, _peer.Id));
			}
			_peer.Send(array, 0, array.Length, DeliveryMethod.ReliableOrdered);
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0018D5D8 File Offset: 0x0018B7D8
		public void OnPeerDisconnectedEvent(NetPeer _peer, DisconnectInfo _disconnectInfo)
		{
			Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> dictionary = this.authStates;
			NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState lnlauthConnectionState;
			lock (dictionary)
			{
				if (!this.authStates.TryGetValue(_peer.Id, out lnlauthConnectionState))
				{
					return;
				}
			}
			dictionary = this.authStates;
			lock (dictionary)
			{
				this.authStates.Remove(_peer.Id);
			}
			if (lnlauthConnectionState.State == NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState.EState.Authenticating)
			{
				Log.Out(string.Format("NET: LiteNetLib: Peer disconnected in auth state: {0} / {1}", _peer, _peer.Id));
				return;
			}
			this.owner.PeerDisconnectedEvent(_peer, _disconnectInfo);
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x0018D698 File Offset: 0x0018B898
		public void OnNetworkReceiveEvent(NetPeer _peer, NetPacketReader _reader, byte _channel, DeliveryMethod _deliveryMethod)
		{
			Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> dictionary = this.authStates;
			NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState lnlauthConnectionState;
			lock (dictionary)
			{
				if (!this.authStates.TryGetValue(_peer.Id, out lnlauthConnectionState))
				{
					return;
				}
			}
			if (lnlauthConnectionState.State == NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState.EState.Authenticating)
			{
				int availableBytes = _reader.AvailableBytes;
				if (availableBytes != 17)
				{
					Log.Out(string.Format("NET: LiteNetLib: Received invalid package (length={0}) from an client in auth state: {1} / {2}", availableBytes, _peer, _peer.Id));
					_peer.Disconnect();
					return;
				}
				byte[] array = MemoryPools.poolByte.Alloc(availableBytes);
				_reader.GetBytes(array, availableBytes);
				if (array[0] != 202)
				{
					Log.Out(string.Format("NET: LiteNetLib: Received invalid package (channel={0}) from an client in auth state: {1} / {2}", array[0], _peer, _peer.Id));
					_peer.Disconnect();
					MemoryPools.poolByte.Free(array);
					return;
				}
				byte[] array2 = new byte[16];
				Array.Copy(array, 1, array2, 0, 16);
				Guid guid = new Guid(array2);
				if (guid != lnlauthConnectionState.Challenge)
				{
					Log.Out(string.Format("NET: LiteNetLib: Received invalid challenge {0} from an client in auth state: {1} / {2}", guid, _peer, _peer.Id));
					_peer.Disconnect();
					MemoryPools.poolByte.Free(array);
					return;
				}
				if (ConnectionManager.VerboseNetLogging)
				{
					Log.Out(string.Format("NET: LiteNetLib: Received correct challenge {0} from new peer {1} / {2}, accepting", guid, _peer, _peer.Id));
				}
				lnlauthConnectionState.State = NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState.EState.Connected;
				this.owner.PeerConnectedEvent(_peer);
				MemoryPools.poolByte.Free(array);
				return;
			}
			else
			{
				this.owner.NetworkReceiveEvent(_peer, _reader, _deliveryMethod);
			}
		}

		// Token: 0x040032E3 RID: 13027
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan ConnectionStateCheckInterval = TimeSpan.FromSeconds(10.0);

		// Token: 0x040032E4 RID: 13028
		[PublicizedFrom(EAccessModifier.Private)]
		public const int ConnectionRateLimitMilliseconds = 500;

		// Token: 0x040032E5 RID: 13029
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan MaxDurationInAuthState = new TimeSpan(0, 0, 0, 10);

		// Token: 0x040032E6 RID: 13030
		[PublicizedFrom(EAccessModifier.Private)]
		public const int ChallengePackageSize = 17;

		// Token: 0x040032E7 RID: 13031
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NetworkServerLiteNetLib owner;

		// Token: 0x040032E8 RID: 13032
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, DateTime> lastConnectAttemptTimes = new Dictionary<string, DateTime>();

		// Token: 0x040032E9 RID: 13033
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> authStates = new Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState>();

		// Token: 0x040032EA RID: 13034
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<int> clearList = new List<int>();

		// Token: 0x040032EB RID: 13035
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime nextStateChecksRun;

		// Token: 0x02000881 RID: 2177
		[PublicizedFrom(EAccessModifier.Private)]
		public class LNLAuthConnectionState
		{
			// Token: 0x17000690 RID: 1680
			// (get) Token: 0x06003F66 RID: 16230 RVA: 0x0018D864 File Offset: 0x0018BA64
			public TimeSpan Age
			{
				get
				{
					return DateTime.Now - this.StartTime;
				}
			}

			// Token: 0x040032EC RID: 13036
			public NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState.EState State;

			// Token: 0x040032ED RID: 13037
			public readonly Guid Challenge = Guid.NewGuid();

			// Token: 0x040032EE RID: 13038
			public readonly DateTime StartTime = DateTime.Now;

			// Token: 0x02000882 RID: 2178
			public enum EState
			{
				// Token: 0x040032F0 RID: 13040
				Authenticating,
				// Token: 0x040032F1 RID: 13041
				Connected
			}
		}
	}
}
