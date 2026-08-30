using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LiteNetLib;
using UnityEngine.Networking;

// Token: 0x02000885 RID: 2181
public class NetworkClientLiteNetLib : INetworkClient
{
	// Token: 0x06003F90 RID: 16272 RVA: 0x0018E3EB File Offset: 0x0018C5EB
	public NetworkClientLiteNetLib(IProtocolManagerProtocolInterface _protoManager)
	{
		this.protoManager = _protoManager;
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x000027FC File Offset: 0x000009FC
	public void LateUpdate()
	{
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x0018E3FC File Offset: 0x0018C5FC
	public void Connect(GameServerInfo _gsi)
	{
		string value = _gsi.GetValue(GameInfoString.IP);
		int value2 = _gsi.GetValue(GameInfoInt.Port);
		string text = ServerInfoCache.Instance.GetPassword(_gsi);
		if (text == null)
		{
			text = "";
		}
		if (string.IsNullOrEmpty(value))
		{
			Log.Out("NET: Skipping LiteNetLib connection attempt, no IP given");
			this.protoManager.ConnectionFailedEv(Localization.Get("netConnectionFailedNoIp", false, null));
			return;
		}
		if (_gsi.AllowsCrossplay && !PermissionsManager.IsCrossplayAllowed())
		{
			this.Disconnect();
			this.protoManager.ConnectionFailedEv(Localization.Get("auth_noCrossplay", false, null));
			return;
		}
		Log.Out("NET: LiteNetLib trying to connect to: " + value + ":" + value2.ToString());
		if (this.client != null)
		{
			this.Disconnect();
		}
		EventBasedNetListener eventBasedNetListener = new EventBasedNetListener();
		this.client = new NetManager(eventBasedNetListener, null, true);
		this.authWrapper = new NetworkClientLiteNetLib.LiteNetLibAuthWrapperClient(this);
		NetworkCommonLiteNetLib.InitConfig(this.client);
		eventBasedNetListener.PeerConnectedEvent += this.authWrapper.OnPeerConnectedEvent;
		eventBasedNetListener.PeerDisconnectedEvent += this.authWrapper.OnPeerDisconnectedEvent;
		eventBasedNetListener.NetworkReceiveEvent += this.authWrapper.OnNetworkReceiveEvent;
		eventBasedNetListener.NetworkErrorEvent += delegate(IPEndPoint _endpoint, SocketError _code)
		{
			Log.Error("NET: LiteNetLib: Network error: {0}", new object[] { _code });
		};
		this.client.Start();
		this.client.Connect(value, value2 + 2, text);
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x0018E562 File Offset: 0x0018C762
	public void Disconnect()
	{
		this.connected = false;
		if (this.client != null && this.client.IsRunning)
		{
			this.client.Stop();
		}
		this.client = null;
		this.serverPeer = null;
		this.authWrapper = null;
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x0018E5A0 File Offset: 0x0018C7A0
	public NetworkError SendData(int _channel, ArrayListMP<byte> _data)
	{
		if (this.serverPeer == null)
		{
			Log.Warning("NET: LiteNetLib: SendData requested without active connection");
			return NetworkError.WrongOperation;
		}
		_data[0] = (byte)_channel;
		if (ConnectionManager.VerboseNetLogging)
		{
			Log.Out("Sending data to server: ch={0}, size={1}", new object[] { _channel, _data.Count });
		}
		this.serverPeer.Send(_data.Items, 0, _data.Count, DeliveryMethod.ReliableOrdered);
		return NetworkError.Ok;
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x0018E614 File Offset: 0x0018C814
	[PublicizedFrom(EAccessModifier.Private)]
	public void NetworkReceiveEvent(NetPeer _peer, NetPacketReader _reader, DeliveryMethod _deliveryMethod)
	{
		if (_reader.AvailableBytes == 0)
		{
			Log.Out("NET: LiteNetLib: Received package with zero size from");
			return;
		}
		int availableBytes = _reader.AvailableBytes;
		byte[] array = MemoryPools.poolByte.Alloc(availableBytes);
		_reader.GetBytes(array, availableBytes);
		if (ConnectionManager.VerboseNetLogging)
		{
			Log.Out("Received data from server: ch={0}, size={1}", new object[]
			{
				array[0],
				availableBytes
			});
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DataReceivedClient((int)array[0], array, availableBytes);
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x0018E68C File Offset: 0x0018C88C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPeerConnectedEvent(NetPeer _peer)
	{
		Log.Out("NET: LiteNetLib: Accepted by server");
		this.serverPeer = _peer;
		this.connected = true;
		INetConnection[] array = new INetConnection[2];
		for (int i = 0; i < 2; i++)
		{
			array[i] = new NetConnectionSimple(i, null, this, null, 1, 0);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SetConnectionToServer(array);
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x0018E6E0 File Offset: 0x0018C8E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisconnectedFromServer(NetPeer _peer, DisconnectInfo _info)
	{
		NetworkCommonLiteNetLib.EAdditionalDisconnectCause additionalDisconnectCause = NetworkCommonLiteNetLib.EAdditionalDisconnectCause.InvalidPassword;
		string text = null;
		bool hasDisconnectInfo = !_info.AdditionalData.IsNull && _info.AdditionalData.AvailableBytes != 0;
		if (hasDisconnectInfo)
		{
			int availableBytes = _info.AdditionalData.AvailableBytes;
			byte[] array = MemoryPools.poolByte.Alloc(availableBytes);
			_info.AdditionalData.GetBytes(array, availableBytes);
			additionalDisconnectCause = (NetworkCommonLiteNetLib.EAdditionalDisconnectCause)array[0];
			if (((availableBytes >= 2) ? array[1] : 0) > 0)
			{
				text = Encoding.UTF8.GetString(array, 2, (int)array[1]);
			}
			MemoryPools.poolByte.Free(array);
		}
		DisconnectReason reason = _info.Reason;
		string displayMessage = (hasDisconnectInfo ? string.Format(Localization.Get("netLiteNetLibDisconnectReason_" + additionalDisconnectCause.ToStringCached<NetworkCommonLiteNetLib.EAdditionalDisconnectCause>(), false, null), text) : Localization.Get("netLiteNetLibDisconnectReason_" + reason.ToStringCached<DisconnectReason>(), false, null));
		ThreadManager.AddSingleTaskMainThread("DisconnectLiteNetLib", delegate(object _taskInfo)
		{
			if (!this.connected)
			{
				Log.Out("NET: LiteNetLib: Connection failed: {0}", new object[] { reason.ToStringCached<DisconnectReason>() });
				if (reason == DisconnectReason.ConnectionRejected)
				{
					if (additionalDisconnectCause == NetworkCommonLiteNetLib.EAdditionalDisconnectCause.InvalidPassword)
					{
						this.protoManager.InvalidPasswordEv();
					}
					else
					{
						Log.Out("NET: LiteNetLib: Reject cause: {0}", new object[] { additionalDisconnectCause.ToStringCached<NetworkCommonLiteNetLib.EAdditionalDisconnectCause>() });
						this.protoManager.ConnectionFailedEv(displayMessage);
					}
				}
				else
				{
					this.protoManager.ConnectionFailedEv(displayMessage);
				}
			}
			else
			{
				Log.Out("NET: LiteNetLib: Connection closed: " + reason.ToStringCached<DisconnectReason>());
				if (hasDisconnectInfo && additionalDisconnectCause != NetworkCommonLiteNetLib.EAdditionalDisconnectCause.ClientSideDisconnect)
				{
					Log.Out("NET: LiteNetLib: Cause: {0}", new object[] { additionalDisconnectCause.ToStringCached<NetworkCommonLiteNetLib.EAdditionalDisconnectCause>() });
				}
				if (additionalDisconnectCause != NetworkCommonLiteNetLib.EAdditionalDisconnectCause.ClientSideDisconnect)
				{
					this.protoManager.DisconnectedFromServerEv(displayMessage);
				}
			}
			this.Disconnect();
		}, null);
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x0018E7F6 File Offset: 0x0018C9F6
	public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
	{
		if (this.client != null)
		{
			this.client.SimulateLatency = _enable;
			this.client.SimulationMinLatency = _minLatency;
			this.client.SimulationMaxLatency = _maxLatency;
		}
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x0018E824 File Offset: 0x0018CA24
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		if (this.client != null)
		{
			this.client.SimulatePacketLoss = _enable;
			this.client.SimulationPacketLossChance = _chance;
		}
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x0018E846 File Offset: 0x0018CA46
	public void EnableStatistics()
	{
		if (this.client != null)
		{
			this.client.EnableStatistics = true;
		}
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x0018E85C File Offset: 0x0018CA5C
	public void DisableStatistics()
	{
		if (this.client != null)
		{
			this.client.EnableStatistics = false;
		}
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x0018E872 File Offset: 0x0018CA72
	public string PrintNetworkStatistics()
	{
		if (this.client != null)
		{
			return this.client.Statistics.ToString();
		}
		return "No client!";
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x0018E892 File Offset: 0x0018CA92
	public void ResetNetworkStatistics()
	{
		if (this.client != null)
		{
			this.client.Statistics.Reset();
		}
	}

	// Token: 0x04003342 RID: 13122
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IProtocolManagerProtocolInterface protoManager;

	// Token: 0x04003343 RID: 13123
	[PublicizedFrom(EAccessModifier.Private)]
	public bool connected;

	// Token: 0x04003344 RID: 13124
	[PublicizedFrom(EAccessModifier.Private)]
	public NetManager client;

	// Token: 0x04003345 RID: 13125
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkClientLiteNetLib.LiteNetLibAuthWrapperClient authWrapper;

	// Token: 0x04003346 RID: 13126
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPeer serverPeer;

	// Token: 0x02000886 RID: 2182
	[PublicizedFrom(EAccessModifier.Private)]
	public class LiteNetLibAuthWrapperClient
	{
		// Token: 0x06003F9F RID: 16287 RVA: 0x0018E8AC File Offset: 0x0018CAAC
		public LiteNetLibAuthWrapperClient(NetworkClientLiteNetLib _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x0018E8BB File Offset: 0x0018CABB
		public void OnPeerConnectedEvent(NetPeer _peer)
		{
			Log.Out("NET: LiteNetLib: Connected to server");
			this.owner.OnPeerConnectedEvent(_peer);
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0018E8D3 File Offset: 0x0018CAD3
		public void OnPeerDisconnectedEvent(NetPeer _peer, DisconnectInfo _info)
		{
			this.owner.OnDisconnectedFromServer(_peer, _info);
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x0018E8E4 File Offset: 0x0018CAE4
		public void OnNetworkReceiveEvent(NetPeer _peer, NetPacketReader _reader, byte _channel, DeliveryMethod _deliveryMethod)
		{
			if (_reader.PeekByte() == 202)
			{
				int availableBytes = _reader.AvailableBytes;
				byte[] array = MemoryPools.poolByte.Alloc(availableBytes);
				_reader.GetBytes(array, availableBytes);
				if (ConnectionManager.VerboseNetLogging)
				{
					Log.Out("NET: LiteNetLib: Sending challenge reply to server");
				}
				_peer.Send(array, 0, availableBytes, DeliveryMethod.ReliableOrdered);
				MemoryPools.poolByte.Free(array);
				return;
			}
			this.owner.NetworkReceiveEvent(_peer, _reader, _deliveryMethod);
		}

		// Token: 0x04003347 RID: 13127
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NetworkClientLiteNetLib owner;
	}
}
