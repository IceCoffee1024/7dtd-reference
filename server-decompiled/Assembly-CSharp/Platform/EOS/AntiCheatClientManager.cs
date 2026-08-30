using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;

namespace Platform.EOS
{
	// Token: 0x02001CDA RID: 7386
	public class AntiCheatClientManager : IAntiCheatClient, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600DB36 RID: 56118 RVA: 0x004E4F1C File Offset: 0x004E311C
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
			this.antiCheatActive = !AntiCheatCommon.NoEacCmdLine;
		}

		// Token: 0x0600DB37 RID: 56119 RVA: 0x004E4F50 File Offset: 0x004E3150
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

		// Token: 0x0600DB38 RID: 56120 RVA: 0x004E504C File Offset: 0x004E324C
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleClientIntegrityViolated(ref OnClientIntegrityViolatedCallbackInfo data)
		{
			Log.Warning(string.Format("[EOS-ACCP2P] Client violation: {0}, message: {1}", data.ViolationType.ToStringCached<AntiCheatClientViolationType>(), data.ViolationMessage));
			this.eacViolationMessage = data.ViolationMessage;
			this.eacViolation = true;
			this.antiCheatActive = false;
		}

		// Token: 0x0600DB39 RID: 56121 RVA: 0x004E5088 File Offset: 0x004E3288
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

		// Token: 0x0600DB3A RID: 56122 RVA: 0x004E50B8 File Offset: 0x004E32B8
		public bool ClientAntiCheatEnabled()
		{
			return this.antiCheatActive && !this.eacViolation;
		}

		// Token: 0x0600DB3B RID: 56123 RVA: 0x004E50D0 File Offset: 0x004E32D0
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

		// Token: 0x0600DB3C RID: 56124 RVA: 0x004E516C File Offset: 0x004E336C
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

		// Token: 0x0600DB3D RID: 56125 RVA: 0x004E5230 File Offset: 0x004E3430
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

		// Token: 0x0600DB3E RID: 56126 RVA: 0x004E5284 File Offset: 0x004E3484
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

		// Token: 0x0600DB3F RID: 56127 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DB40 RID: 56128 RVA: 0x004E52E8 File Offset: 0x004E34E8
		public bool EncryptionAvailable()
		{
			return this.clientMode == AntiCheatClientManager.AntiCheatClientMode.ClientServer;
		}

		// Token: 0x0600DB41 RID: 56129 RVA: 0x004E52F3 File Offset: 0x004E34F3
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.EncryptStream(_cInfo, _stream);
		}

		// Token: 0x0600DB42 RID: 56130 RVA: 0x004E5316 File Offset: 0x004E3516
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.DecryptStream(_cInfo, _stream);
		}

		// Token: 0x0400A5B1 RID: 42417
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5B2 RID: 42418
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A5B3 RID: 42419
		[PublicizedFrom(EAccessModifier.Private)]
		public bool antiCheatActive;

		// Token: 0x0400A5B4 RID: 42420
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolation;

		// Token: 0x0400A5B5 RID: 42421
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolationHandled;

		// Token: 0x0400A5B6 RID: 42422
		[PublicizedFrom(EAccessModifier.Private)]
		public bool connectedToServer;

		// Token: 0x0400A5B7 RID: 42423
		[PublicizedFrom(EAccessModifier.Private)]
		public Utf8String eacViolationMessage;

		// Token: 0x0400A5B8 RID: 42424
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientManager.AntiCheatClientMode clientMode = AntiCheatClientManager.AntiCheatClientMode.Unknown;

		// Token: 0x0400A5B9 RID: 42425
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientCS clientServerClient;

		// Token: 0x0400A5BA RID: 42426
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientP2P peerToPeerClient;

		// Token: 0x02001CDB RID: 7387
		[PublicizedFrom(EAccessModifier.Private)]
		public enum AntiCheatClientMode
		{
			// Token: 0x0400A5BC RID: 42428
			ClientServer,
			// Token: 0x0400A5BD RID: 42429
			PeerToPeer,
			// Token: 0x0400A5BE RID: 42430
			Unknown
		}
	}
}
