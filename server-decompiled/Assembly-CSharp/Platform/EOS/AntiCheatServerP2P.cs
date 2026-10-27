using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;

namespace Platform.EOS
{
	// Token: 0x02001D46 RID: 7494
	public class AntiCheatServerP2P : IAntiCheatServer, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600DE09 RID: 56841 RVA: 0x004EF0AC File Offset: 0x004ED2AC
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x0600DE0A RID: 56842 RVA: 0x004EF0D4 File Offset: 0x004ED2D4
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("ACSP2P.Init");
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.antiCheatInterface = ((Api)this.owner.Api).PlatformInterface.GetAntiCheatClientInterface();
			}
			if (this.antiCheatInterface == null)
			{
				Log.Out("[EAC] AntiCheatServerP2P initialized with null interface");
				return;
			}
		}

		// Token: 0x0600DE0B RID: 56843 RVA: 0x004EF150 File Offset: 0x004ED350
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddCallbacks()
		{
			AddNotifyMessageToPeerOptions addNotifyMessageToPeerOptions = default(AddNotifyMessageToPeerOptions);
			object obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.handleMessageToPeerID = this.antiCheatInterface.AddNotifyMessageToPeer(ref addNotifyMessageToPeerOptions, null, new OnMessageToPeerCallback(this.handleMessageToPeer));
			}
			AddNotifyPeerAuthStatusChangedOptions addNotifyPeerAuthStatusChangedOptions = default(AddNotifyPeerAuthStatusChangedOptions);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.handlePeerAuthStateChangeID = this.antiCheatInterface.AddNotifyPeerAuthStatusChanged(ref addNotifyPeerAuthStatusChangedOptions, null, new OnPeerAuthStatusChangedCallback(this.handlePeerAuthStateChange));
			}
			AddNotifyPeerActionRequiredOptions addNotifyPeerActionRequiredOptions = default(AddNotifyPeerActionRequiredOptions);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.handlePeerActionRequiredID = this.antiCheatInterface.AddNotifyPeerActionRequired(ref addNotifyPeerActionRequiredOptions, null, new OnPeerActionRequiredCallback(this.handlePeerActionRequired));
			}
		}

		// Token: 0x0600DE0C RID: 56844 RVA: 0x004EF258 File Offset: 0x004ED458
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveCallbacks()
		{
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.antiCheatInterface.RemoveNotifyMessageToPeer(this.handleMessageToPeerID);
				this.antiCheatInterface.RemoveNotifyPeerAuthStatusChanged(this.handlePeerAuthStateChangeID);
				this.antiCheatInterface.RemoveNotifyPeerActionRequired(this.handlePeerActionRequiredID);
			}
		}

		// Token: 0x0600DE0D RID: 56845 RVA: 0x004EF2C4 File Offset: 0x004ED4C4
		public bool GetHostUserIdAndToken([TupleElementNames(new string[] { "userId", "token" })] out ValueTuple<PlatformUserIdentifierAbs, string> _hostUserIdAndToken)
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if (crossplatformPlatform == null)
			{
				platformUserIdentifierAbs = null;
			}
			else
			{
				IUserClient user = crossplatformPlatform.User;
				platformUserIdentifierAbs = ((user != null) ? user.PlatformUserId : null);
			}
			IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
			string text;
			if (crossplatformPlatform2 == null)
			{
				text = null;
			}
			else
			{
				IAuthenticationClient authenticationClient = crossplatformPlatform2.AuthenticationClient;
				text = ((authenticationClient != null) ? authenticationClient.GetAuthTicket() : null);
			}
			_hostUserIdAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(platformUserIdentifierAbs, text);
			return true;
		}

		// Token: 0x0600DE0E RID: 56846 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DE0F RID: 56847 RVA: 0x004EF318 File Offset: 0x004ED518
		public bool StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate)
		{
			if (this.ServerEacEnabled())
			{
				this.AddCallbacks();
				Log.Out("[EAC] Starting EAC peer to peer server");
				this.authSuccessfulDelegate = _authSuccessfulDelegate;
				this.kickPlayerDelegate = _kickPlayerDelegate;
				ProductUserId productUserId = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
				BeginSessionOptions beginSessionOptions = new BeginSessionOptions
				{
					LocalUserId = productUserId,
					Mode = AntiCheatClientMode.PeerToPeer
				};
				object lockObject = AntiCheatCommon.LockObject;
				Result result;
				lock (lockObject)
				{
					result = this.antiCheatInterface.BeginSession(ref beginSessionOptions);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-ACSP2P] Starting module failed: " + result.ToStringCached<Result>());
				}
				else
				{
					this.serverRunning = true;
				}
				return result == Result.Success;
			}
			return true;
		}

		// Token: 0x0600DE10 RID: 56848 RVA: 0x004EF3EC File Offset: 0x004ED5EC
		public bool RegisterUser(ClientInfo _client)
		{
			if (!this.serverRunning)
			{
				return false;
			}
			Log.Out(string.Format("[EOS-ACSP2P] Registering user: {0}", _client));
			EosHelpers.AssertMainThread("ACSP2P.Reg");
			RegisterPeerOptions registerPeerOptions = new RegisterPeerOptions
			{
				PeerHandle = AntiCheatCommon.ClientInfoToIntPtr(_client),
				ClientPlatform = EosHelpers.DeviceTypeToAntiCheatPlatformMappings[_client.device],
				PeerProductUserId = ((UserIdentifierEos)_client.CrossplatformId).ProductUserId,
				ClientType = (_client.requiresAntiCheat ? AntiCheatCommonClientType.ProtectedClient : AntiCheatCommonClientType.UnprotectedClient),
				IpAddress = _client.ip,
				AuthenticationTimeout = 60U
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.RegisterPeer(ref registerPeerOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACSP2P] Failed registering user: " + result.ToStringCached<Result>());
				return false;
			}
			if (!_client.requiresAntiCheat)
			{
				this.authSuccessfulDelegate(_client);
			}
			return true;
		}

		// Token: 0x0600DE11 RID: 56849 RVA: 0x004EF4FC File Offset: 0x004ED6FC
		public void FreeUser(ClientInfo _client)
		{
			if (!this.serverRunning)
			{
				return;
			}
			EosHelpers.AssertMainThread("ACS.Free");
			Log.Out(string.Format("[EOS-ACSP2P] FreeUser: {0}", _client));
			UnregisterPeerOptions unregisterPeerOptions = new UnregisterPeerOptions
			{
				PeerHandle = AntiCheatCommon.ClientInfoToIntPtr(_client)
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.UnregisterPeer(ref unregisterPeerOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACSP2P] Failed unregistering user: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DE12 RID: 56850 RVA: 0x004EF598 File Offset: 0x004ED798
		public void HandleMessageFromClient(ClientInfo _cInfo, byte[] _data)
		{
			if (!this.serverRunning)
			{
				Log.Warning("[EOS-ACSP2P] Server: Received EAC package but EAC was not initialized");
				return;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACSP2P] PushNetworkMessage (len={0}, from={1})", _data.Length, _cInfo.InternalId));
			}
			ReceiveMessageFromPeerOptions receiveMessageFromPeerOptions = new ReceiveMessageFromPeerOptions
			{
				Data = new ArraySegment<byte>(_data),
				PeerHandle = AntiCheatCommon.ClientInfoToIntPtr(_cInfo)
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.ReceiveMessageFromPeer(ref receiveMessageFromPeerOptions);
			}
			if (result != Result.AntiCheatPeerNotFound && result != Result.Success)
			{
				Log.Error("[EOS-ACSP2P] Failed handling message: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DE13 RID: 56851 RVA: 0x004EF65C File Offset: 0x004ED85C
		public void StopServer()
		{
			if (!this.serverRunning)
			{
				return;
			}
			this.RemoveCallbacks();
			EndSessionOptions endSessionOptions = default(EndSessionOptions);
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.EndSession(ref endSessionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACSP2P] Stopping module failed: " + result.ToStringCached<Result>());
			}
			this.serverRunning = false;
			this.authSuccessfulDelegate = null;
			this.kickPlayerDelegate = null;
		}

		// Token: 0x0600DE14 RID: 56852 RVA: 0x004EF6E8 File Offset: 0x004ED8E8
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToPeer(ref OnMessageToClientCallbackInfo _data)
		{
			if (!this.serverRunning)
			{
				return;
			}
			ClientInfo clientInfo = AntiCheatCommon.IntPtrToClientInfo(_data.ClientHandle, "[EOS-ACSP2P] Got message for unknown client number: {0}");
			if (clientInfo == null)
			{
				Log.Out(string.Format("[EOS-ACSP2P] FreeUser: {0}", _data.ClientHandle));
				UnregisterPeerOptions unregisterPeerOptions = new UnregisterPeerOptions
				{
					PeerHandle = _data.ClientHandle
				};
				object lockObject = AntiCheatCommon.LockObject;
				Result result;
				lock (lockObject)
				{
					result = this.antiCheatInterface.UnregisterPeer(ref unregisterPeerOptions);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-ACSP2P] Failed unregistering user: " + result.ToStringCached<Result>());
				}
			}
			if (clientInfo != null)
			{
				clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array));
			}
		}

		// Token: 0x0600DE15 RID: 56853 RVA: 0x004EF7CC File Offset: 0x004ED9CC
		[PublicizedFrom(EAccessModifier.Private)]
		public void handlePeerActionRequired(ref OnClientActionRequiredCallbackInfo _data)
		{
			if (!this.serverRunning)
			{
				return;
			}
			ClientInfo clientInfo = AntiCheatCommon.IntPtrToClientInfo(_data.ClientHandle, "[EOS-ACSP2P] Got action for unknown client number: {0}");
			if (clientInfo == null)
			{
				return;
			}
			AntiCheatCommonClientAction clientAction = _data.ClientAction;
			AntiCheatCommonClientActionReason actionReasonCode = _data.ActionReasonCode;
			string text = _data.ActionReasonDetailsString;
			if (clientAction != AntiCheatCommonClientAction.RemovePlayer)
			{
				Log.Warning(string.Format("[EOS-ACSP2P] Got invalid action ({0}), reason='{1}', details={2}, client={3}", new object[]
				{
					clientAction.ToStringCached<AntiCheatCommonClientAction>(),
					actionReasonCode.ToStringCached<AntiCheatCommonClientActionReason>(),
					text,
					clientInfo
				}));
				return;
			}
			Log.Out(string.Format("[EOS-ACSP2P] Kicking player. Reason={0}, details='{1}', client={2}", actionReasonCode.ToStringCached<AntiCheatCommonClientActionReason>(), text, clientInfo));
			KickPlayerDelegate kickPlayerDelegate = this.kickPlayerDelegate;
			if (kickPlayerDelegate == null)
			{
				return;
			}
			ClientInfo clientInfo2 = clientInfo;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.EosEacViolation;
			int num = (int)actionReasonCode;
			string text2 = text;
			kickPlayerDelegate(clientInfo2, new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text2));
		}

		// Token: 0x0600DE16 RID: 56854 RVA: 0x004EF884 File Offset: 0x004EDA84
		[PublicizedFrom(EAccessModifier.Private)]
		public void handlePeerAuthStateChange(ref OnClientAuthStatusChangedCallbackInfo _data)
		{
			if (!this.serverRunning)
			{
				return;
			}
			ClientInfo clientInfo = AntiCheatCommon.IntPtrToClientInfo(_data.ClientHandle, "[EOS-ACSP2P] Got auth state change for unknown client number: {0}");
			if (_data.ClientAuthStatus == AntiCheatCommonClientAuthStatus.RemoteAuthComplete)
			{
				Log.Out(string.Format("[EOS-ACSP2P] Remote Auth complete for client number {0}", _data.ClientHandle));
				AuthenticationSuccessfulCallbackDelegate authenticationSuccessfulCallbackDelegate = this.authSuccessfulDelegate;
				if (authenticationSuccessfulCallbackDelegate == null)
				{
					return;
				}
				authenticationSuccessfulCallbackDelegate(clientInfo);
			}
		}

		// Token: 0x0600DE17 RID: 56855 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DE18 RID: 56856 RVA: 0x004EF8DF File Offset: 0x004EDADF
		public bool ServerEacEnabled()
		{
			return this.antiCheatInterface != null && GamePrefs.GetBool(EnumGamePrefs.ServerEACPeerToPeer);
		}

		// Token: 0x0600DE19 RID: 56857 RVA: 0x004EF8F8 File Offset: 0x004EDAF8
		public bool ServerEacAvailable()
		{
			return this.antiCheatInterface != null;
		}

		// Token: 0x0600DE1A RID: 56858 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool EncryptionAvailable()
		{
			return false;
		}

		// Token: 0x0600DE1B RID: 56859 RVA: 0x004EF906 File Offset: 0x004EDB06
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			throw new NotImplementedException("Encryption is not supported for a Peer to Peer AntiCheatServer.");
		}

		// Token: 0x0600DE1C RID: 56860 RVA: 0x004EF906 File Offset: 0x004EDB06
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			throw new NotImplementedException("Encryption is not supported for a Peer to Peer AntiCheatServer.");
		}

		// Token: 0x0400A79F RID: 42911
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A7A0 RID: 42912
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A7A1 RID: 42913
		[PublicizedFrom(EAccessModifier.Private)]
		public bool serverRunning;

		// Token: 0x0400A7A2 RID: 42914
		[PublicizedFrom(EAccessModifier.Private)]
		public AuthenticationSuccessfulCallbackDelegate authSuccessfulDelegate;

		// Token: 0x0400A7A3 RID: 42915
		[PublicizedFrom(EAccessModifier.Private)]
		public KickPlayerDelegate kickPlayerDelegate;

		// Token: 0x0400A7A4 RID: 42916
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToPeerID;

		// Token: 0x0400A7A5 RID: 42917
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerAuthStateChangeID;

		// Token: 0x0400A7A6 RID: 42918
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerActionRequiredID;
	}
}
