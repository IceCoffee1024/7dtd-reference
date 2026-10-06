using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;

namespace Platform.EOS
{
	// Token: 0x02001D0C RID: 7436
	public class AntiCheatClientManager : IAntiCheatClient, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600DCB1 RID: 56497 RVA: 0x004E9A64 File Offset: 0x004E7C64
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
			this.antiCheatActive = !AntiCheatCommon.NoEacCmdLine;
		}

		// Token: 0x0600DCB2 RID: 56498 RVA: 0x004E9A98 File Offset: 0x004E7C98
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

		// Token: 0x0600DCB3 RID: 56499 RVA: 0x004E9B94 File Offset: 0x004E7D94
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleClientIntegrityViolated(ref OnClientIntegrityViolatedCallbackInfo data)
		{
			Log.Warning(string.Format("[EOS-ACCP2P] Client violation: {0}, message: {1}", data.ViolationType.ToStringCached<AntiCheatClientViolationType>(), data.ViolationMessage));
			this.eacViolationMessage = data.ViolationMessage;
			this.eacViolation = true;
			this.antiCheatActive = false;
		}

		// Token: 0x0600DCB4 RID: 56500 RVA: 0x004E9BD0 File Offset: 0x004E7DD0
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

		// Token: 0x0600DCB5 RID: 56501 RVA: 0x004E9C00 File Offset: 0x004E7E00
		public bool ClientAntiCheatEnabled()
		{
			return this.antiCheatActive && !this.eacViolation;
		}

		// Token: 0x0600DCB6 RID: 56502 RVA: 0x004E9C18 File Offset: 0x004E7E18
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

		// Token: 0x0600DCB7 RID: 56503 RVA: 0x004E9CB4 File Offset: 0x004E7EB4
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

		// Token: 0x0600DCB8 RID: 56504 RVA: 0x004E9D78 File Offset: 0x004E7F78
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

		// Token: 0x0600DCB9 RID: 56505 RVA: 0x004E9DCC File Offset: 0x004E7FCC
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

		// Token: 0x0600DCBA RID: 56506 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DCBB RID: 56507 RVA: 0x004E9E30 File Offset: 0x004E8030
		public bool EncryptionAvailable()
		{
			return this.clientMode == AntiCheatClientManager.AntiCheatClientMode.ClientServer;
		}

		// Token: 0x0600DCBC RID: 56508 RVA: 0x004E9E3B File Offset: 0x004E803B
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.EncryptStream(_cInfo, _stream);
		}

		// Token: 0x0600DCBD RID: 56509 RVA: 0x004E9E5E File Offset: 0x004E805E
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			if (this.clientMode != AntiCheatClientManager.AntiCheatClientMode.ClientServer)
			{
				Log.Error("[EOS-ACC] Encryption is not supported in AntiCheatClientMode.PeerToPeer");
				return false;
			}
			return this.clientServerClient.DecryptStream(_cInfo, _stream);
		}

		// Token: 0x0400A686 RID: 42630
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A687 RID: 42631
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A688 RID: 42632
		[PublicizedFrom(EAccessModifier.Private)]
		public bool antiCheatActive;

		// Token: 0x0400A689 RID: 42633
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolation;

		// Token: 0x0400A68A RID: 42634
		[PublicizedFrom(EAccessModifier.Private)]
		public bool eacViolationHandled;

		// Token: 0x0400A68B RID: 42635
		[PublicizedFrom(EAccessModifier.Private)]
		public bool connectedToServer;

		// Token: 0x0400A68C RID: 42636
		[PublicizedFrom(EAccessModifier.Private)]
		public Utf8String eacViolationMessage;

		// Token: 0x0400A68D RID: 42637
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientManager.AntiCheatClientMode clientMode = AntiCheatClientManager.AntiCheatClientMode.Unknown;

		// Token: 0x0400A68E RID: 42638
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientCS clientServerClient;

		// Token: 0x0400A68F RID: 42639
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientP2P peerToPeerClient;

		// Token: 0x02001D0D RID: 7437
		[PublicizedFrom(EAccessModifier.Private)]
		public enum AntiCheatClientMode
		{
			// Token: 0x0400A691 RID: 42641
			ClientServer,
			// Token: 0x0400A692 RID: 42642
			PeerToPeer,
			// Token: 0x0400A693 RID: 42643
			Unknown
		}
	}
}
