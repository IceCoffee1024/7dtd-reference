using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;

namespace Platform.EOS
{
	// Token: 0x02001CB3 RID: 7347
	public class AntiCheatClientManager : IAntiCheatClient, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D99B RID: 55707 RVA: 0x004DE908 File Offset: 0x004DCB08
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
			this.antiCheatActive = !AntiCheatCommon.NoEacCmdLine;
		}

		// Token: 0x0600D99C RID: 55708 RVA: 0x004DE93C File Offset: 0x004DCB3C
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

		// Token: 0x0600D99D RID: 55709 RVA: 0x004DEA38 File Offset: 0x004DCC38
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleClientIntegrityViolated(ref OnClientIntegrityViolatedCallbackInfo data)
		{
			Log.Warning(string.Format("[EOS-ACCP2P] Client violation: {0}, message: {1}", data.ViolationType.ToStringCached<AntiCheatClientViolationType>(), data.ViolationMessage));
			this.eacViolationMessage = data.ViolationMessage;
			this.eacViolation = true;
			this.antiCheatActive = false;
		}

		// Token: 0x0600D99E RID: 55710 RVA: 0x004DEA74 File Offset: 0x004DCC74
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

		// Token: 0x0600D99F RID: 55711 RVA: 0x004DEAA4 File Offset: 0x004DCCA4
		public bool ClientAntiCheatEnabled()
		{
			return this.antiCheatActive && !this.eacViolation;
		}

		// Token: 0x0600D9A0 RID: 55712 RVA: 0x004DEABC File Offset: 0x004DCCBC
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

		// Token: 0x0600D9A1 RID: 55713 RVA: 0x004DEB58 File Offset: 0x004DCD58
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

		// Token: 0x0600D9A2 RID: 55714 RVA: 0x004DEC1C File Offset: 0x004DCE1C
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

		// Token: 0x0600D9A3 RID: 55715 RVA: 0x004DEC70 File Offset: 0x004DCE70
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

		// Token: 0x0600D9A4 RID: 55716 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D9A5 RID: 55717 RVA: 0x004DECD4 File Offset: 0x004DCED4
		public bool EncryptionAvailable()
		{
			return this.clientMode == AntiCheatClientManager.AntiCheatClientMode.ClientServer;
		}

		// Token: 0x0600D9A6 RID: 55718 RVA: 0x004DECDF File Offset: 0x004DCEDF
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.EncryptStream(_cInfo, _stream);
		}

		// Token: 0x0600D9A7 RID: 55719 RVA: 0x004DED02 File Offset: 0x004DCF02
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.DecryptStream(_cInfo, _stream);
		}

		// Token: 0x0400A47E RID: 42110
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A47F RID: 42111
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A480 RID: 42112
		[PublicizedFrom(EAccessModifier.Private)]
		public bool antiCheatActive;

		// Token: 0x0400A481 RID: 42113
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolation;

		// Token: 0x0400A482 RID: 42114
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolationHandled;

		// Token: 0x0400A483 RID: 42115
		[PublicizedFrom(EAccessModifier.Private)]
		public bool connectedToServer;

		// Token: 0x0400A484 RID: 42116
		[PublicizedFrom(EAccessModifier.Private)]
		public Utf8String eacViolationMessage;

		// Token: 0x0400A485 RID: 42117
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientManager.AntiCheatClientMode clientMode = AntiCheatClientManager.AntiCheatClientMode.Unknown;

		// Token: 0x0400A486 RID: 42118
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientCS clientServerClient;

		// Token: 0x0400A487 RID: 42119
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientP2P peerToPeerClient;

		// Token: 0x02001CB4 RID: 7348
		[PublicizedFrom(EAccessModifier.Private)]
		public enum AntiCheatClientMode
		{
			// Token: 0x0400A489 RID: 42121
			ClientServer,
			// Token: 0x0400A48A RID: 42122
			PeerToPeer,
			// Token: 0x0400A48B RID: 42123
			Unknown
		}
	}
}
