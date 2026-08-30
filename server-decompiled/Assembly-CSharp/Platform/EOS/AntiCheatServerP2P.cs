using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;

namespace Platform.EOS
{
	// Token: 0x02001CE3 RID: 7395
	public class AntiCheatServerP2P : IAntiCheatServer, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600DB7A RID: 56186 RVA: 0x004E69C4 File Offset: 0x004E4BC4
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x0600DB7B RID: 56187 RVA: 0x004E69EC File Offset: 0x004E4BEC
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

		// Token: 0x0600DB7C RID: 56188 RVA: 0x004E6A68 File Offset: 0x004E4C68
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

		// Token: 0x0600DB7D RID: 56189 RVA: 0x004E6B70 File Offset: 0x004E4D70
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

		// Token: 0x0600DB7E RID: 56190 RVA: 0x004E6BDC File Offset: 0x004E4DDC
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

		// Token: 0x0600DB7F RID: 56191 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DB80 RID: 56192 RVA: 0x004E6C30 File Offset: 0x004E4E30
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

		// Token: 0x0600DB81 RID: 56193 RVA: 0x004E6D04 File Offset: 0x004E4F04
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

		// Token: 0x0600DB82 RID: 56194 RVA: 0x004E6E14 File Offset: 0x004E5014
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

		// Token: 0x0600DB83 RID: 56195 RVA: 0x004E6EB0 File Offset: 0x004E50B0
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

		// Token: 0x0600DB84 RID: 56196 RVA: 0x004E6F74 File Offset: 0x004E5174
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

		// Token: 0x0600DB85 RID: 56197 RVA: 0x004E7000 File Offset: 0x004E5200
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

		// Token: 0x0600DB86 RID: 56198 RVA: 0x004E70E4 File Offset: 0x004E52E4
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

		// Token: 0x0600DB87 RID: 56199 RVA: 0x004E719C File Offset: 0x004E539C
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

		// Token: 0x0600DB88 RID: 56200 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DB89 RID: 56201 RVA: 0x004E71F7 File Offset: 0x004E53F7
		public bool ServerEacEnabled()
		{
			return this.antiCheatInterface != null && GamePrefs.GetBool(EnumGamePrefs.ServerEACPeerToPeer);
		}

		// Token: 0x0600DB8A RID: 56202 RVA: 0x004E7210 File Offset: 0x004E5410
		public bool ServerEacAvailable()
		{
			return this.antiCheatInterface != null;
		}

		// Token: 0x0600DB8B RID: 56203 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool EncryptionAvailable()
		{
			return false;
		}

		// Token: 0x0600DB8C RID: 56204 RVA: 0x004E721E File Offset: 0x004E541E
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			throw new NotImplementedException("Encryption is not supported for a Peer to Peer AntiCheatServer.");
		}

		// Token: 0x0600DB8D RID: 56205 RVA: 0x004E721E File Offset: 0x004E541E
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			throw new NotImplementedException("Encryption is not supported for a Peer to Peer AntiCheatServer.");
		}

		// Token: 0x0400A5E1 RID: 42465
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5E2 RID: 42466
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A5E3 RID: 42467
		[PublicizedFrom(EAccessModifier.Private)]
		public bool serverRunning;

		// Token: 0x0400A5E4 RID: 42468
		[PublicizedFrom(EAccessModifier.Private)]
		public AuthenticationSuccessfulCallbackDelegate authSuccessfulDelegate;

		// Token: 0x0400A5E5 RID: 42469
		[PublicizedFrom(EAccessModifier.Private)]
		public KickPlayerDelegate kickPlayerDelegate;

		// Token: 0x0400A5E6 RID: 42470
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToPeerID;

		// Token: 0x0400A5E7 RID: 42471
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerAuthStateChangeID;

		// Token: 0x0400A5E8 RID: 42472
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerActionRequiredID;
	}
}
