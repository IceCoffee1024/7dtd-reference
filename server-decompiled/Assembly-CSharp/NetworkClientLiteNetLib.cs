using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LiteNetLib;
using UnityEngine.Networking;

// Token: 0x02000892 RID: 2194
public class NetworkClientLiteNetLib : INetworkClient
{
	// Token: 0x06003F41 RID: 16193 RVA: 0x0018F285 File Offset: 0x0018D485
	public NetworkClientLiteNetLib(IProtocolManagerProtocolInterface _protoManager)
	{
		this.protoManager = _protoManager;
	}

	// Token: 0x06003F42 RID: 16194 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06003F43 RID: 16195 RVA: 0x000027FC File Offset: 0x000009FC
	public void LateUpdate()
	{
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x0018F294 File Offset: 0x0018D494
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

	// Token: 0x06003F45 RID: 16197 RVA: 0x0018F3FA File Offset: 0x0018D5FA
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

	// Token: 0x06003F46 RID: 16198 RVA: 0x0018F438 File Offset: 0x0018D638
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

	// Token: 0x06003F47 RID: 16199 RVA: 0x0018F4AC File Offset: 0x0018D6AC
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

	// Token: 0x06003F48 RID: 16200 RVA: 0x0018F524 File Offset: 0x0018D724
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

	// Token: 0x06003F49 RID: 16201 RVA: 0x0018F578 File Offset: 0x0018D778
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

	// Token: 0x06003F4A RID: 16202 RVA: 0x0018F68E File Offset: 0x0018D88E
	public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
	{
		if (this.client != null)
		{
			this.client.SimulateLatency = _enable;
			this.client.SimulationMinLatency = _minLatency;
			this.client.SimulationMaxLatency = _maxLatency;
		}
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0018F6BC File Offset: 0x0018D8BC
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		if (this.client != null)
		{
			this.client.SimulatePacketLoss = _enable;
			this.client.SimulationPacketLossChance = _chance;
		}
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0018F6DE File Offset: 0x0018D8DE
	public void EnableStatistics()
	{
		if (this.client != null)
		{
			this.client.EnableStatistics = true;
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x0018F6F4 File Offset: 0x0018D8F4
	public void DisableStatistics()
	{
		if (this.client != null)
		{
			this.client.EnableStatistics = false;
		}
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x0018F70A File Offset: 0x0018D90A
	public string PrintNetworkStatistics()
	{
		if (this.client != null)
		{
			return this.client.Statistics.ToString();
		}
		return "No client!";
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x0018F72A File Offset: 0x0018D92A
	public void ResetNetworkStatistics()
	{
		if (this.client != null)
		{
			this.client.Statistics.Reset();
		}
	}

	// Token: 0x0400334D RID: 13133
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IProtocolManagerProtocolInterface protoManager;

	// Token: 0x0400334E RID: 13134
	[PublicizedFrom(EAccessModifier.Private)]
	public bool connected;

	// Token: 0x0400334F RID: 13135
	[PublicizedFrom(EAccessModifier.Private)]
	public NetManager client;

	// Token: 0x04003350 RID: 13136
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkClientLiteNetLib.LiteNetLibAuthWrapperClient authWrapper;

	// Token: 0x04003351 RID: 13137
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPeer serverPeer;

	// Token: 0x02000893 RID: 2195
	[PublicizedFrom(EAccessModifier.Private)]
	public class LiteNetLibAuthWrapperClient
	{
		// Token: 0x06003F50 RID: 16208 RVA: 0x0018F744 File Offset: 0x0018D944
		public LiteNetLibAuthWrapperClient(NetworkClientLiteNetLib _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x0018F753 File Offset: 0x0018D953
		public void OnPeerConnectedEvent(NetPeer _peer)
		{
			Log.Out("NET: LiteNetLib: Connected to server");
			this.owner.OnPeerConnectedEvent(_peer);
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0018F76B File Offset: 0x0018D96B
		public void OnPeerDisconnectedEvent(NetPeer _peer, DisconnectInfo _info)
		{
			this.owner.OnDisconnectedFromServer(_peer, _info);
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0018F77C File Offset: 0x0018D97C
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

		// Token: 0x04003352 RID: 13138
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NetworkClientLiteNetLib owner;
	}
}
