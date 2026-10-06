using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using UnityEngine.Networking;

// Token: 0x0200088E RID: 2190
public class NetworkServerLiteNetLib : INetworkServer
{
	// Token: 0x06003F39 RID: 16185 RVA: 0x0018E907 File Offset: 0x0018CB07
	public NetworkServerLiteNetLib(IProtocolManagerProtocolInterface _protoManager)
	{
		this.protoManager = _protoManager;
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x0018E92C File Offset: 0x0018CB2C
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

	// Token: 0x06003F3B RID: 16187 RVA: 0x000027FC File Offset: 0x000009FC
	public void LateUpdate()
	{
	}

	// Token: 0x06003F3C RID: 16188 RVA: 0x0018E9B4 File Offset: 0x0018CBB4
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

	// Token: 0x06003F3D RID: 16189 RVA: 0x0018EAAC File Offset: 0x0018CCAC
	public void SetServerPassword(string _password)
	{
		this.serverPassword = _password ?? "";
	}

	// Token: 0x06003F3E RID: 16190 RVA: 0x0018EAC0 File Offset: 0x0018CCC0
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

	// Token: 0x06003F3F RID: 16191 RVA: 0x0018EB40 File Offset: 0x0018CD40
	public void DropClient(ClientInfo _clientInfo, bool _clientDisconnect)
	{
		this.OnPlayerDisconnected(_clientInfo.litenetPeerConnectId);
		NetPeer peerByConnectId = this.GetPeerByConnectId(_clientInfo.litenetPeerConnectId);
		if (peerByConnectId != null)
		{
			this.server.DisconnectPeer(peerByConnectId, NetworkServerLiteNetLib.disconnectFromClientSide);
		}
	}

	// Token: 0x06003F40 RID: 16192 RVA: 0x0018EB7C File Offset: 0x0018CD7C
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

	// Token: 0x06003F41 RID: 16193 RVA: 0x0018EC5C File Offset: 0x0018CE5C
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

	// Token: 0x06003F42 RID: 16194 RVA: 0x0018ECEC File Offset: 0x0018CEEC
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

	// Token: 0x06003F43 RID: 16195 RVA: 0x0018ED90 File Offset: 0x0018CF90
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

	// Token: 0x06003F44 RID: 16196 RVA: 0x0018EE60 File Offset: 0x0018D060
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

	// Token: 0x06003F45 RID: 16197 RVA: 0x0018EEA8 File Offset: 0x0018D0A8
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

	// Token: 0x06003F46 RID: 16198 RVA: 0x0018EEE8 File Offset: 0x0018D0E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPlayerDisconnected(long _peerConnectId)
	{
		ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForLiteNetPeer(_peerConnectId);
		if (clientInfo != null)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Net_PlayerDisconnected(clientInfo);
		}
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x0018EF14 File Offset: 0x0018D114
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

	// Token: 0x06003F48 RID: 16200 RVA: 0x0018EFA8 File Offset: 0x0018D1A8
	public int GetBadPacketCount(ClientInfo _cInfo)
	{
		NetPeer peerByConnectId = this.GetPeerByConnectId(_cInfo.litenetPeerConnectId);
		if (peerByConnectId != null)
		{
			return peerByConnectId.badPacketCount;
		}
		return 0;
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x0018EFD0 File Offset: 0x0018D1D0
	public string GetServerPorts(int _basePort)
	{
		return (_basePort + 2).ToString() + "/UDP";
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x0018EFF2 File Offset: 0x0018D1F2
	public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
	{
		if (this.server != null)
		{
			this.server.SimulateLatency = _enable;
			this.server.SimulationMinLatency = _minLatency;
			this.server.SimulationMaxLatency = _maxLatency;
		}
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0018F020 File Offset: 0x0018D220
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		if (this.server != null)
		{
			this.server.SimulatePacketLoss = _enable;
			this.server.SimulationPacketLossChance = _chance;
		}
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0018F042 File Offset: 0x0018D242
	public void EnableStatistics()
	{
		if (this.server != null)
		{
			this.server.EnableStatistics = true;
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x0018F058 File Offset: 0x0018D258
	public void DisableStatistics()
	{
		if (this.server != null)
		{
			this.server.EnableStatistics = false;
		}
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x0018F06E File Offset: 0x0018D26E
	public string PrintNetworkStatistics()
	{
		if (this.server != null)
		{
			return this.server.Statistics.ToString();
		}
		return "no server!";
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x0018F08E File Offset: 0x0018D28E
	public void ResetNetworkStatistics()
	{
		if (this.server != null)
		{
			this.server.Statistics.Reset();
		}
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x0018F0A8 File Offset: 0x0018D2A8
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

	// Token: 0x06003F51 RID: 16209 RVA: 0x0018F0D8 File Offset: 0x0018D2D8
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

	// Token: 0x04003344 RID: 13124
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] rejectInvalidPassword = new byte[2];

	// Token: 0x04003345 RID: 13125
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] rejectRateLimit;

	// Token: 0x04003346 RID: 13126
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] rejectPendingConnection;

	// Token: 0x04003347 RID: 13127
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] disconnectServerShutdown;

	// Token: 0x04003348 RID: 13128
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] disconnectFromClientSide;

	// Token: 0x04003349 RID: 13129
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IProtocolManagerProtocolInterface protoManager;

	// Token: 0x0400334A RID: 13130
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverPassword;

	// Token: 0x0400334B RID: 13131
	[PublicizedFrom(EAccessModifier.Private)]
	public NetManager server;

	// Token: 0x0400334C RID: 13132
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer authWrapper;

	// Token: 0x0400334D RID: 13133
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ClientInfo> dropClientsQueue = new List<ClientInfo>();

	// Token: 0x0400334E RID: 13134
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NetPeer> getPeersList = new List<NetPeer>();

	// Token: 0x0200088F RID: 2191
	[PublicizedFrom(EAccessModifier.Private)]
	public class LiteNetLibAuthWrapperServer
	{
		// Token: 0x06003F52 RID: 16210 RVA: 0x0018F12C File Offset: 0x0018D32C
		public LiteNetLibAuthWrapperServer(NetworkServerLiteNetLib _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0018F15C File Offset: 0x0018D35C
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

		// Token: 0x06003F54 RID: 16212 RVA: 0x0018F288 File Offset: 0x0018D488
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

		// Token: 0x06003F55 RID: 16213 RVA: 0x0018F3B8 File Offset: 0x0018D5B8
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

		// Token: 0x06003F56 RID: 16214 RVA: 0x0018F498 File Offset: 0x0018D698
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

		// Token: 0x06003F57 RID: 16215 RVA: 0x0018F558 File Offset: 0x0018D758
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

		// Token: 0x0400334F RID: 13135
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan ConnectionStateCheckInterval = TimeSpan.FromSeconds(10.0);

		// Token: 0x04003350 RID: 13136
		[PublicizedFrom(EAccessModifier.Private)]
		public const int ConnectionRateLimitMilliseconds = 500;

		// Token: 0x04003351 RID: 13137
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan MaxDurationInAuthState = new TimeSpan(0, 0, 0, 10);

		// Token: 0x04003352 RID: 13138
		[PublicizedFrom(EAccessModifier.Private)]
		public const int ChallengePackageSize = 17;

		// Token: 0x04003353 RID: 13139
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NetworkServerLiteNetLib owner;

		// Token: 0x04003354 RID: 13140
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, DateTime> lastConnectAttemptTimes = new Dictionary<string, DateTime>();

		// Token: 0x04003355 RID: 13141
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState> authStates = new Dictionary<int, NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState>();

		// Token: 0x04003356 RID: 13142
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<int> clearList = new List<int>();

		// Token: 0x04003357 RID: 13143
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime nextStateChecksRun;

		// Token: 0x02000890 RID: 2192
		[PublicizedFrom(EAccessModifier.Private)]
		public class LNLAuthConnectionState
		{
			// Token: 0x170006BB RID: 1723
			// (get) Token: 0x06003F59 RID: 16217 RVA: 0x0018F724 File Offset: 0x0018D924
			public TimeSpan Age
			{
				get
				{
					return DateTime.Now - this.StartTime;
				}
			}

			// Token: 0x04003358 RID: 13144
			public NetworkServerLiteNetLib.LiteNetLibAuthWrapperServer.LNLAuthConnectionState.EState State;

			// Token: 0x04003359 RID: 13145
			public readonly Guid Challenge = Guid.NewGuid();

			// Token: 0x0400335A RID: 13146
			public readonly DateTime StartTime = DateTime.Now;

			// Token: 0x02000891 RID: 2193
			public enum EState
			{
				// Token: 0x0400335C RID: 13148
				Authenticating,
				// Token: 0x0400335D RID: 13149
				Connected
			}
		}
	}
}
