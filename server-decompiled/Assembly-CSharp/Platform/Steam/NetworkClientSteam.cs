using System;
using System.Collections;
using System.Text;
using System.Threading;
using Steamworks;
using UnityEngine.Networking;

namespace Platform.Steam
{
	// Token: 0x02001C97 RID: 7319
	public class NetworkClientSteam : IPlatformNetworkClient, INetworkClient
	{
		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x0600D90B RID: 55563 RVA: 0x004DECE8 File Offset: 0x004DCEE8
		public bool IsConnected
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.serverId != CSteamID.Nil && this.handlerThread != null && (this.protoManager.IsClient || this.connecting) && this.owner.User.UserStatus == EUserStatus.LoggedIn;
			}
		}

		// Token: 0x0600D90C RID: 55564 RVA: 0x004DED3C File Offset: 0x004DCF3C
		public NetworkClientSteam(IPlatform _owner, IProtocolManagerProtocolInterface _protoManager)
		{
			this.owner = _owner;
			this.protoManager = _protoManager;
			this.owner.Api.ClientApiInitialized += delegate
			{
				if (!GameManager.IsDedicatedServer)
				{
					this.m_P2PSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.P2PSessionConnectFail));
				}
			};
		}

		// Token: 0x0600D90D RID: 55565 RVA: 0x004DEDAC File Offset: 0x004DCFAC
		public void Connect(GameServerInfo _gsi)
		{
			this.disconnectEventReceived = false;
			this.anyPacketsSent = false;
			Log.Out("NET: Steam NW trying to connect to: " + _gsi.GetValue(GameInfoString.IP) + ":" + _gsi.GetValue(GameInfoInt.Port).ToString());
			if (string.IsNullOrEmpty(_gsi.GetValue(GameInfoString.SteamID)))
			{
				Log.Out("[Steamworks.NET] NET: Resolving SteamID for IP " + _gsi.GetValue(GameInfoString.IP) + ":" + _gsi.GetValue(GameInfoInt.Port).ToString());
				ServerInformationTcpClient.RequestRules(_gsi, false, new ServerInformationTcpClient.RulesRequestDone(this.RulesRequestTcpDone));
				this.connecting = true;
				return;
			}
			this.ConnectInternal(_gsi);
		}

		// Token: 0x0600D90E RID: 55566 RVA: 0x004DEE4B File Offset: 0x004DD04B
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

		// Token: 0x0600D90F RID: 55567 RVA: 0x004DEE80 File Offset: 0x004DD080
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConnectInternal(GameServerInfo _gsi)
		{
			if (string.IsNullOrEmpty(_gsi.GetValue(GameInfoString.SteamID)))
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
				Encoding.UTF8.GetBytes(password, 0, password.Length, arrayListMP.Items, 0);
			}
			else
			{
				arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, 1)
				{
					Count = 1
				};
			}
			this.serverId = new CSteamID(ulong.Parse(_gsi.GetValue(GameInfoString.SteamID)));
			string text = "[Steamworks.NET] NET: Connecting to SteamID ";
			CSteamID csteamID = this.serverId;
			Log.Out(text + csteamID.ToString());
			if (this.handlerThread == null)
			{
				this.handlerThread = ThreadManager.StartThread("SteamNetworkingClient", new ThreadManager.ThreadFunctionDelegate(this.threadHandlerMethod), null, null, true, false);
			}
			this.connecting = true;
			this.SendData(50, arrayListMP);
		}

		// Token: 0x0600D910 RID: 55568 RVA: 0x004DEF94 File Offset: 0x004DD194
		public void Disconnect()
		{
			this.connecting = false;
			if (this.serverId != CSteamID.Nil)
			{
				this.sendBufs.Clear();
			}
			if (this.handlerThread == null)
			{
				return;
			}
			this.signalThread.Set();
			this.handlerThread.WaitForEnd(30);
			this.handlerThread = null;
			this.serverId = CSteamID.Nil;
		}

		// Token: 0x0600D911 RID: 55569 RVA: 0x004DEFFC File Offset: 0x004DD1FC
		public NetworkError SendData(int _channel, ArrayListMP<byte> _data)
		{
			if (this.IsConnected)
			{
				CSteamID csteamID = this.serverId;
				_data[_data.Count - 1] = (byte)_channel;
				this.sendBufs.Enqueue(new NetworkCommonSteam.SendInfo(csteamID, _data));
				this.signalThread.Set();
			}
			else
			{
				Log.Warning("[Steamworks.NET] NET: Tried to send a package while not connected to a server");
			}
			return NetworkError.Ok;
		}

		// Token: 0x0600D912 RID: 55570 RVA: 0x004DF053 File Offset: 0x004DD253
		public void Update()
		{
			if (!this.IsConnected)
			{
				return;
			}
			if (this.disconnectEventReceived)
			{
				this.OnDisconnectedFromServer();
			}
		}

		// Token: 0x0600D913 RID: 55571 RVA: 0x004DF06C File Offset: 0x004DD26C
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator connectionFailedLater(string _message)
		{
			yield return null;
			yield return null;
			this.protoManager.ConnectionFailedEv(_message);
			yield break;
		}

		// Token: 0x0600D914 RID: 55572 RVA: 0x004DF082 File Offset: 0x004DD282
		public void LateUpdate()
		{
			this.flushBuffers = true;
			this.signalThread.Set();
		}

		// Token: 0x0600D915 RID: 55573 RVA: 0x004DF099 File Offset: 0x004DD299
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisconnectedFromServer()
		{
			this.Disconnect();
			this.protoManager.DisconnectedFromServerEv(Localization.Get("netSteamNetworking_ConnectionClosedByServer", false, null));
		}

		// Token: 0x0600D916 RID: 55574 RVA: 0x004DF0B8 File Offset: 0x004DD2B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void P2PSessionConnectFail(P2PSessionConnectFail_t _par)
		{
			if (!this.connecting)
			{
				return;
			}
			this.Disconnect();
			Log.Out("[Steamworks.NET] NET: P2PSessionConnectFail to: " + _par.m_steamIDRemote.m_SteamID.ToString() + " - Error: " + ((EP2PSessionError)_par.m_eP2PSessionError).ToStringCached<EP2PSessionError>());
			string text = Localization.Get("netSteamNetworkingSessionError_" + ((EP2PSessionError)_par.m_eP2PSessionError).ToStringCached<EP2PSessionError>(), false, null);
			this.protoManager.ConnectionFailedEv(text);
		}

		// Token: 0x0600D917 RID: 55575 RVA: 0x004DF130 File Offset: 0x004DD330
		[PublicizedFrom(EAccessModifier.Private)]
		public void threadHandlerMethod(ThreadManager.ThreadInfo _threadinfo)
		{
			while (!_threadinfo.TerminationRequested())
			{
				if (!this.IsConnected)
				{
					this.signalThread.WaitOne(100);
				}
				else
				{
					this.signalThread.WaitOne(6);
					if (this.anyPacketsSent)
					{
						this.CheckConnection();
					}
					this.ReceivePackets();
					while (this.sendBufs.HasData())
					{
						NetworkCommonSteam.SendInfo sendInfo = this.sendBufs.Dequeue();
						if (!SteamNetworking.SendP2PPacket(sendInfo.Recipient, sendInfo.Data.Items, (uint)sendInfo.Data.Count, EP2PSend.k_EP2PSendReliableWithBuffering, 0))
						{
							Log.Error("[Steamworks.NET] NET: Could not send package to server");
						}
						else
						{
							this.packetsPendingSend = true;
							this.anyPacketsSent = true;
						}
					}
					if (this.flushBuffers && this.packetsPendingSend)
					{
						this.packetsPendingSend = false;
						this.flushBuffers = false;
						this.FlushBuffer();
					}
				}
			}
			if (this.serverId != CSteamID.Nil)
			{
				SteamNetworking.CloseP2PSessionWithUser(this.serverId);
			}
		}

		// Token: 0x0600D918 RID: 55576 RVA: 0x004DF22C File Offset: 0x004DD42C
		[PublicizedFrom(EAccessModifier.Private)]
		public void CheckConnection()
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamNetworking.GetP2PSessionState(this.serverId, out p2PSessionState_t))
			{
				if (p2PSessionState_t.m_bConnectionActive != 0 && this.connecting)
				{
					this.connecting = false;
					Log.Out("[Steamworks.NET] NET: Connection established");
					return;
				}
				if (p2PSessionState_t.m_bConnecting == 0 && p2PSessionState_t.m_bConnectionActive == 0 && this.protoManager.IsClient)
				{
					Log.Out("[Steamworks.NET] NET: Connection closed");
					this.disconnectEventReceived = true;
					return;
				}
			}
			else if (this.protoManager.IsClient)
			{
				Log.Out("[Steamworks.NET] NET: Connection closed");
				this.disconnectEventReceived = true;
			}
		}

		// Token: 0x0600D919 RID: 55577 RVA: 0x004DF2B8 File Offset: 0x004DD4B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReceivePackets()
		{
			uint num;
			CSteamID csteamID;
			bool flag = SteamNetworking.ReadP2PPacket(this.recvBuf, (uint)this.recvBuf.Length, out num, out csteamID, 0);
			while (flag)
			{
				if (num > 0U)
				{
					num -= 1U;
					NetworkCommonSteam.ESteamNetChannels esteamNetChannels = (NetworkCommonSteam.ESteamNetChannels)this.recvBuf[(int)num];
					if (esteamNetChannels > NetworkCommonSteam.ESteamNetChannels.NetpackageChannel1)
					{
						if (esteamNetChannels != NetworkCommonSteam.ESteamNetChannels.Authentication)
						{
							if (esteamNetChannels == NetworkCommonSteam.ESteamNetChannels.Ping)
							{
								ArrayListMP<byte> arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, (int)(num + 1U));
								Array.Copy(this.recvBuf, arrayListMP.Items, (long)((ulong)num));
								arrayListMP.Count = (int)(num + 1U);
								this.SendData(60, arrayListMP);
							}
						}
						else
						{
							if (this.connecting)
							{
								this.connecting = false;
								Log.Out("[Steamworks.NET] NET: Connection established");
							}
							if (this.recvBuf[0] == 0)
							{
								Log.Out("[Steamworks.NET] NET: Received invalid password package");
								ThreadManager.AddSingleTaskMainThread("SteamNetInvalidPassword", delegate(object _info)
								{
									this.protoManager.InvalidPasswordEv();
								}, null);
							}
							else
							{
								Log.Out("[Steamworks.NET] NET: Password accepted");
								this.OnConnectedToServer();
							}
						}
					}
					else if (num > 0U)
					{
						byte[] array = MemoryPools.poolByte.Alloc((int)num);
						Array.Copy(this.recvBuf, array, (long)((ulong)num));
						SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DataReceivedClient((int)esteamNetChannels, array, (int)num);
					}
				}
				flag = SteamNetworking.ReadP2PPacket(this.recvBuf, (uint)this.recvBuf.Length, out num, out csteamID, 0);
			}
		}

		// Token: 0x0600D91A RID: 55578 RVA: 0x004DF3EC File Offset: 0x004DD5EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void FlushBuffer()
		{
			if (!SteamNetworking.SendP2PPacket(this.serverId, NetworkClientSteam.emptyData, 0U, EP2PSend.k_EP2PSendReliable, 0))
			{
				Log.Error("[Steamworks.NET] NET: Could not flush the data buffer");
			}
		}

		// Token: 0x0600D91B RID: 55579 RVA: 0x004DF410 File Offset: 0x004DD610
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnConnectedToServer()
		{
			INetConnection[] array = new INetConnection[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = new NetConnectionSteam(i, null, this, null);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SetConnectionToServer(array);
		}

		// Token: 0x0600D91C RID: 55580 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetLatencySimulation(bool _enable, int _minLatency, int _maxLatency)
		{
		}

		// Token: 0x0600D91D RID: 55581 RVA: 0x000027FC File Offset: 0x000009FC
		public void SetPacketLossSimulation(bool _enable, int _chance)
		{
		}

		// Token: 0x0600D91E RID: 55582 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableStatistics()
		{
		}

		// Token: 0x0600D91F RID: 55583 RVA: 0x000027FC File Offset: 0x000009FC
		public void DisableStatistics()
		{
		}

		// Token: 0x0600D920 RID: 55584 RVA: 0x00032147 File Offset: 0x00030347
		public string PrintNetworkStatistics()
		{
			return "";
		}

		// Token: 0x0600D921 RID: 55585 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetNetworkStatistics()
		{
		}

		// Token: 0x0400A4A7 RID: 42151
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A4A8 RID: 42152
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IProtocolManagerProtocolInterface protoManager;

		// Token: 0x0400A4A9 RID: 42153
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<P2PSessionConnectFail_t> m_P2PSessionConnectFail;

		// Token: 0x0400A4AA RID: 42154
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo handlerThread;

		// Token: 0x0400A4AB RID: 42155
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent signalThread = new AutoResetEvent(false);

		// Token: 0x0400A4AC RID: 42156
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<NetworkCommonSteam.SendInfo> sendBufs = new BlockingQueue<NetworkCommonSteam.SendInfo>();

		// Token: 0x0400A4AD RID: 42157
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool flushBuffers;

		// Token: 0x0400A4AE RID: 42158
		[PublicizedFrom(EAccessModifier.Private)]
		public bool packetsPendingSend;

		// Token: 0x0400A4AF RID: 42159
		[PublicizedFrom(EAccessModifier.Private)]
		public bool anyPacketsSent;

		// Token: 0x0400A4B0 RID: 42160
		[PublicizedFrom(EAccessModifier.Private)]
		public CSteamID serverId = CSteamID.Nil;

		// Token: 0x0400A4B1 RID: 42161
		[PublicizedFrom(EAccessModifier.Private)]
		public bool connecting;

		// Token: 0x0400A4B2 RID: 42162
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disconnectEventReceived;

		// Token: 0x0400A4B3 RID: 42163
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] recvBuf = new byte[1048576];

		// Token: 0x0400A4B4 RID: 42164
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] emptyData = new byte[0];
	}
}
