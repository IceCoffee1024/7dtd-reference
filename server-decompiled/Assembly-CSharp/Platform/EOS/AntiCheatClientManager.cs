using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;

namespace Platform.EOS
{
	// Token: 0x02001D3D RID: 7485
	public class AntiCheatClientManager : IAntiCheatClient, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600DDC5 RID: 56773 RVA: 0x004ED604 File Offset: 0x004EB804
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
			this.antiCheatActive = !AntiCheatCommon.NoEacCmdLine;
		}

		// Token: 0x0600DDC6 RID: 56774 RVA: 0x004ED638 File Offset: 0x004EB838
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("ACC.Init");
			object obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.antiCheatInterface = ((Api)this.owner.Api).PlatformInterface.GetAntiCheatClientInterface();
			}
			if (this.antiCheatInterface == null)
			{
				this.antiCheatActive = false;
				Log.Out("[EOS-ACC] Not started with EAC, anticheat disabled");
				return;
			}
			this.clientServerClient = new AntiCheatClientCS(this.owner, this.antiCheatInterface);
			this.peerToPeerClient = new AntiCheatClientP2P(this.owner, this.antiCheatInterface);
			AddNotifyClientIntegrityViolatedOptions addNotifyClientIntegrityViolatedOptions = default(AddNotifyClientIntegrityViolatedOptions);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.antiCheatInterface.AddNotifyClientIntegrityViolated(ref addNotifyClientIntegrityViolatedOptions, null, new OnClientIntegrityViolatedCallback(this.handleClientIntegrityViolated));
			}
		}

		// Token: 0x0600DDC7 RID: 56775 RVA: 0x004ED734 File Offset: 0x004EB934
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleClientIntegrityViolated(ref OnClientIntegrityViolatedCallbackInfo data)
		{
			Log.Warning(string.Format("[EOS-ACCP2P] Client violation: {0}, message: {1}", data.ViolationType.ToStringCached<AntiCheatClientViolationType>(), data.ViolationMessage));
			this.eacViolationMessage = data.ViolationMessage;
			this.eacViolation = true;
			this.antiCheatActive = false;
		}

		// Token: 0x0600DDC8 RID: 56776 RVA: 0x004ED770 File Offset: 0x004EB970
		public bool GetUnhandledViolationMessage(out string _message)
		{
			if (this.eacViolation && !this.eacViolationHandled)
			{
				_message = this.eacViolationMessage;
				this.eacViolationHandled = true;
				return true;
			}
			_message = "";
			return false;
		}

		// Token: 0x0600DDC9 RID: 56777 RVA: 0x004ED7A0 File Offset: 0x004EB9A0
		public bool ClientAntiCheatEnabled()
		{
			return this.antiCheatActive && !this.eacViolation;
		}

		// Token: 0x0600DDCA RID: 56778 RVA: 0x004ED7B8 File Offset: 0x004EB9B8
		public void WaitForRemoteAuth(Action onRemoteAuthSkippedOrComplete)
		{
			if (!Submission.Enabled && this.clientMode == AntiCheatClientManager.AntiCheatClientMode.Unknown)
			{
				Action onRemoteAuthSkippedOrComplete2 = onRemoteAuthSkippedOrComplete;
				if (onRemoteAuthSkippedOrComplete2 == null)
				{
					return;
				}
				onRemoteAuthSkippedOrComplete2();
				return;
			}
			else
			{
				if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
				{
					AntiCheatClientP2P antiCheatClientP2P = this.peerToPeerClient;
					if (antiCheatClientP2P != null && antiCheatClientP2P.IsServerAntiCheatProtected())
					{
						if (this.connectedToServer)
						{
							this.peerToPeerClient.OnRemoteAuthComplete += delegate
							{
								Action onRemoteAuthSkippedOrComplete5 = onRemoteAuthSkippedOrComplete;
								if (onRemoteAuthSkippedOrComplete5 == null)
								{
									return;
								}
								onRemoteAuthSkippedOrComplete5();
							};
							return;
						}
						Action onRemoteAuthSkippedOrComplete3 = onRemoteAuthSkippedOrComplete;
						if (onRemoteAuthSkippedOrComplete3 == null)
						{
							return;
						}
						onRemoteAuthSkippedOrComplete3();
						return;
					}
				}
				Action onRemoteAuthSkippedOrComplete4 = onRemoteAuthSkippedOrComplete;
				if (onRemoteAuthSkippedOrComplete4 == null)
				{
					return;
				}
				onRemoteAuthSkippedOrComplete4();
				return;
			}
		}

		// Token: 0x0600DDCB RID: 56779 RVA: 0x004ED854 File Offset: 0x004EBA54
		public void ConnectToServer([TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _hostUserAndToken, Action _onNoAntiCheatOrConnectionComplete, Action<string> _onConnectionFailed)
		{
			if (!this.ClientAntiCheatEnabled())
			{
				Log.Out("[EOS-ACC] Anti cheat not loaded");
				this.connectedToServer = false;
				Action onNoAntiCheatOrConnectionComplete = _onNoAntiCheatOrConnectionComplete;
				if (onNoAntiCheatOrConnectionComplete == null)
				{
					return;
				}
				onNoAntiCheatOrConnectionComplete();
				return;
			}
			else
			{
				if (_hostUserAndToken.Item1 != null)
				{
					this.clientMode = AntiCheatClientManager.AntiCheatClientMode.PeerToPeer;
					this.peerToPeerClient.Activate();
					this.peerToPeerClient.ConnectToServer(_hostUserAndToken, delegate
					{
						Action onNoAntiCheatOrConnectionComplete3 = _onNoAntiCheatOrConnectionComplete;
						if (onNoAntiCheatOrConnectionComplete3 != null)
						{
							onNoAntiCheatOrConnectionComplete3();
						}
						this.connectedToServer = true;
					}, _onConnectionFailed);
					return;
				}
				this.clientMode = AntiCheatClientManager.AntiCheatClientMode.ClientServer;
				if (!(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
				{
					this.clientServerClient.Activate();
					this.clientServerClient.ConnectToServer(delegate
					{
						Action onNoAntiCheatOrConnectionComplete4 = _onNoAntiCheatOrConnectionComplete;
						if (onNoAntiCheatOrConnectionComplete4 != null)
						{
							onNoAntiCheatOrConnectionComplete4();
						}
						this.connectedToServer = true;
					}, _onConnectionFailed);
					return;
				}
				Action onNoAntiCheatOrConnectionComplete2 = _onNoAntiCheatOrConnectionComplete;
				if (onNoAntiCheatOrConnectionComplete2 == null)
				{
					return;
				}
				onNoAntiCheatOrConnectionComplete2();
				return;
			}
		}

		// Token: 0x0600DDCC RID: 56780 RVA: 0x004ED918 File Offset: 0x004EBB18
		public void HandleMessageFromServer(byte[] _data)
		{
			if (!this.antiCheatActive)
			{
				Log.Warning("[EOS-ACC] Received EAC package but EAC was not initialized");
				return;
			}
			AntiCheatClientManager.AntiCheatClientMode antiCheatClientMode = this.clientMode;
			if (antiCheatClientMode == AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				this.clientServerClient.HandleMessageFromServer(_data);
				return;
			}
			if (antiCheatClientMode != AntiCheatClientManager.AntiCheatClientMode.PeerToPeer)
			{
				Log.Warning("[EOS-ACC] Received EAC package but EAC client mode is unknown.");
				return;
			}
			this.peerToPeerClient.HandleMessageFromPeer(_data);
		}

		// Token: 0x0600DDCD RID: 56781 RVA: 0x004ED96C File Offset: 0x004EBB6C
		public void DisconnectFromServer()
		{
			if (!this.ClientAntiCheatEnabled())
			{
				return;
			}
			if (!this.connectedToServer)
			{
				return;
			}
			AntiCheatClientManager.AntiCheatClientMode antiCheatClientMode = this.clientMode;
			if (antiCheatClientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				if (antiCheatClientMode != AntiCheatClientManager.AntiCheatClientMode.PeerToPeer)
				{
					Log.Warning("[EOS-ACC] DisconnectFromServer called but EAC client mode is unknown.");
				}
				else
				{
					this.peerToPeerClient.DisconnectFromServer();
				}
			}
			else
			{
				this.clientServerClient.DisconnectFromServer();
			}
			Log.Out("[EOS-ACC] Disconnected from game server");
			this.connectedToServer = false;
		}

		// Token: 0x0600DDCE RID: 56782 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DDCF RID: 56783 RVA: 0x004ED9D0 File Offset: 0x004EBBD0
		public bool EncryptionAvailable()
		{
			return this.clientMode == AntiCheatClientManager.AntiCheatClientMode.ClientServer;
		}

		// Token: 0x0600DDD0 RID: 56784 RVA: 0x004ED9DB File Offset: 0x004EBBDB
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.EncryptStream(_cInfo, _stream);
		}

		// Token: 0x0600DDD1 RID: 56785 RVA: 0x004ED9FE File Offset: 0x004EBBFE
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.DecryptStream(_cInfo, _stream);
		}

		// Token: 0x0400A76F RID: 42863
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A770 RID: 42864
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A771 RID: 42865
		[PublicizedFrom(EAccessModifier.Private)]
		public bool antiCheatActive;

		// Token: 0x0400A772 RID: 42866
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolation;

		// Token: 0x0400A773 RID: 42867
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolationHandled;

		// Token: 0x0400A774 RID: 42868
		[PublicizedFrom(EAccessModifier.Private)]
		public bool connectedToServer;

		// Token: 0x0400A775 RID: 42869
		[PublicizedFrom(EAccessModifier.Private)]
		public Utf8String eacViolationMessage;

		// Token: 0x0400A776 RID: 42870
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientManager.AntiCheatClientMode clientMode = AntiCheatClientManager.AntiCheatClientMode.Unknown;

		// Token: 0x0400A777 RID: 42871
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientCS clientServerClient;

		// Token: 0x0400A778 RID: 42872
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientP2P peerToPeerClient;

		// Token: 0x02001D3E RID: 7486
		[PublicizedFrom(EAccessModifier.Private)]
		public enum AntiCheatClientMode
		{
			// Token: 0x0400A77A RID: 42874
			ClientServer,
			// Token: 0x0400A77B RID: 42875
			PeerToPeer,
			// Token: 0x0400A77C RID: 42876
			Unknown
		}
	}
}
