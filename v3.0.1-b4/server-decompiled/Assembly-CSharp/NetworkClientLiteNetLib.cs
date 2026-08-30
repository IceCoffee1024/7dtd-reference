using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LiteNetLib;
using UnityEngine.Networking;

// Token: 0x02000879 RID: 2169
public class NetworkClientLiteNetLib : INetworkClient
{
	// Token: 0x06003F2B RID: 16171 RVA: 0x0018C31B File Offset: 0x0018A51B
	public NetworkClientLiteNetLib(IProtocolManagerProtocolInterface _protoManager)
	{
		this.protoManager = _protoManager;
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x000027FC File Offset: 0x000009FC
	public void LateUpdate()
	{
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x0018C32C File Offset: 0x0018A52C
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

	// Token: 0x06003F2F RID: 16175 RVA: 0x0018C492 File Offset: 0x0018A692
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

	// Token: 0x06003F30 RID: 16176 RVA: 0x0018C4D0 File Offset: 0x0018A6D0
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

	// Token: 0x06003F31 RID: 16177 RVA: 0x0018C544 File Offset: 0x0018A744
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

	// Token: 0x06003F32 RID: 16178 RVA: 0x0018C5BC File Offset: 0x0018A7BC
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

	// Token: 0x06003F33 RID: 16179 RVA: 0x0018C610 File Offset: 0x0018A810
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

	// Token: 0x06003F34 RID: 16180 RVA: 0x0018C726 File Offset: 0x0018A926
	public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
	{
		if (this.client != null)
		{
			this.client.SimulateLatency = _enable;
			this.client.SimulationMinLatency = _minLatency;
			this.client.SimulationMaxLatency = _maxLatency;
		}
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x0018C754 File Offset: 0x0018A954
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		if (this.client != null)
		{
			this.client.SimulatePacketLoss = _enable;
			this.client.SimulationPacketLossChance = _chance;
		}
	}

	// Token: 0x06003F36 RID: 16182 RVA: 0x0018C776 File Offset: 0x0018A976
	public void EnableStatistics()
	{
		if (this.client != null)
		{
			this.client.EnableStatistics = true;
		}
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x0018C78C File Offset: 0x0018A98C
	public void DisableStatistics()
	{
		if (this.client != null)
		{
			this.client.EnableStatistics = false;
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x0018C7A2 File Offset: 0x0018A9A2
	public string PrintNetworkStatistics()
	{
		if (this.client != null)
		{
			return this.client.Statistics.ToString();
		}
		return "No client!";
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x0018C7C2 File Offset: 0x0018A9C2
	public void ResetNetworkStatistics()
	{
		if (this.client != null)
		{
			this.client.Statistics.Reset();
		}
	}

	// Token: 0x040032C1 RID: 12993
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IProtocolManagerProtocolInterface protoManager;

	// Token: 0x040032C2 RID: 12994
	[PublicizedFrom(EAccessModifier.Private)]
	public bool connected;

	// Token: 0x040032C3 RID: 12995
	[PublicizedFrom(EAccessModifier.Private)]
	public NetManager client;

	// Token: 0x040032C4 RID: 12996
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkClientLiteNetLib.LiteNetLibAuthWrapperClient authWrapper;

	// Token: 0x040032C5 RID: 12997
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPeer serverPeer;

	// Token: 0x0200087A RID: 2170
	[PublicizedFrom(EAccessModifier.Private)]
	public class LiteNetLibAuthWrapperClient
	{
		// Token: 0x06003F3A RID: 16186 RVA: 0x0018C7DC File Offset: 0x0018A9DC
		public LiteNetLibAuthWrapperClient(NetworkClientLiteNetLib _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0018C7EB File Offset: 0x0018A9EB
		public void OnPeerConnectedEvent(NetPeer _peer)
		{
			Log.Out("NET: LiteNetLib: Connected to server");
			this.owner.OnPeerConnectedEvent(_peer);
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0018C803 File Offset: 0x0018AA03
		public void OnPeerDisconnectedEvent(NetPeer _peer, DisconnectInfo _info)
		{
			this.owner.OnDisconnectedFromServer(_peer, _info);
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0018C814 File Offset: 0x0018AA14
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

		// Token: 0x040032C6 RID: 12998
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NetworkClientLiteNetLib owner;
	}
}
