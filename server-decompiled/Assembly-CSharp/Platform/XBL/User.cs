using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C6C RID: 7276
	public class User : IUserClient
	{
		// Token: 0x0600D7C1 RID: 55233 RVA: 0x004D5DAC File Offset: 0x004D3FAC
		[PublicizedFrom(EAccessModifier.Private)]
		static User()
		{
			Dictionary<EBlockType, XblPermission> dictionary = EnumUtils.Values<EBlockType>().ToDictionary<EBlockType, EBlockType, XblPermission>((EBlockType blockType) => blockType, delegate(EBlockType blockType)
			{
				XblPermission xblPermission3;
				switch (blockType)
				{
				case EBlockType.TextChat:
					xblPermission3 = XblPermission.CommunicateUsingText;
					break;
				case EBlockType.VoiceChat:
					xblPermission3 = XblPermission.CommunicateUsingVoice;
					break;
				case EBlockType.Play:
					xblPermission3 = XblPermission.PlayMultiplayer;
					break;
				default:
					throw new NotImplementedException(string.Format("Mapping from {0}.{1} to {2} not implemented!", "EBlockType", blockType, "XblPermission"));
				}
				return xblPermission3;
			});
			User.userBlockedPermissions = dictionary.Values.ToArray<XblPermission>();
			User.xblPermissionToBlockType = new EnumDictionary<XblPermission, EBlockType>();
			foreach (KeyValuePair<EBlockType, XblPermission> keyValuePair in dictionary)
			{
				EBlockType eblockType;
				XblPermission xblPermission;
				keyValuePair.Deconstruct(out eblockType, out xblPermission);
				EBlockType eblockType2 = eblockType;
				XblPermission xblPermission2 = xblPermission;
				User.xblPermissionToBlockType.Add(xblPermission2, eblockType2);
			}
			User.userBlockedAnonymousTypes = new XblAnonymousUserType[]
			{
				XblAnonymousUserType.CrossNetworkFriend,
				XblAnonymousUserType.CrossNetworkUser
			};
		}

		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x0600D7C2 RID: 55234 RVA: 0x004D5E90 File Offset: 0x004D4090
		public bool IsMultiplayerActivityActive
		{
			get
			{
				object activityLock = this.m_activityLock;
				bool isActivityActive;
				lock (activityLock)
				{
					isActivityActive = this.m_isActivityActive;
				}
				return isActivityActive;
			}
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600D7C3 RID: 55235 RVA: 0x004D5ED4 File Offset: 0x004D40D4
		public XUserLocalId LocalID
		{
			get
			{
				return this.m_userLocalId;
			}
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x0600D7C4 RID: 55236 RVA: 0x004D5EDC File Offset: 0x004D40DC
		public ulong Xuid
		{
			get
			{
				return this.m_userXuid;
			}
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x0600D7C5 RID: 55237 RVA: 0x004D5EE4 File Offset: 0x004D40E4
		public Unity.XGamingRuntime.XblContextHandle XblContextHandle
		{
			get
			{
				return this.m_contextHandle;
			}
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x0600D7C6 RID: 55238 RVA: 0x004D5EEC File Offset: 0x004D40EC
		public XUserHandle UserHandle
		{
			get
			{
				return this.m_userHandle;
			}
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x0600D7C7 RID: 55239 RVA: 0x004D5EF4 File Offset: 0x004D40F4
		// (set) Token: 0x0600D7C8 RID: 55240 RVA: 0x004D5EFC File Offset: 0x004D40FC
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x0600D7C9 RID: 55241 RVA: 0x004D5F08 File Offset: 0x004D4108
		// (remove) Token: 0x0600D7CA RID: 55242 RVA: 0x004D5F74 File Offset: 0x004D4174
		public event Action<XUserHandle> UserHandleReady
		{
			add
			{
				object userHandleLock = this.m_userHandleLock;
				lock (userHandleLock)
				{
					this.m_userHandleReady = (Action<XUserHandle>)Delegate.Combine(this.m_userHandleReady, value);
					if (this.m_userHandle != null)
					{
						value(this.m_userHandle);
					}
				}
			}
			remove
			{
				object userHandleLock = this.m_userHandleLock;
				lock (userHandleLock)
				{
					this.m_userHandleReady = (Action<XUserHandle>)Delegate.Remove(this.m_userHandleReady, value);
				}
			}
		}

		// Token: 0x1400012C RID: 300
		// (add) Token: 0x0600D7CB RID: 55243 RVA: 0x004D5FC8 File Offset: 0x004D41C8
		// (remove) Token: 0x0600D7CC RID: 55244 RVA: 0x004D6030 File Offset: 0x004D4230
		public event Action<IPlatform> UserLoggedIn
		{
			add
			{
				object loginLock = this.m_loginLock;
				lock (loginLock)
				{
					this.m_userLoggedIn = (Action<IPlatform>)Delegate.Combine(this.m_userLoggedIn, value);
					if (this.UserStatus == EUserStatus.LoggedIn)
					{
						value(this.m_owner);
					}
				}
			}
			remove
			{
				object loginLock = this.m_loginLock;
				lock (loginLock)
				{
					this.m_userLoggedIn = (Action<IPlatform>)Delegate.Remove(this.m_userLoggedIn, value);
				}
			}
		}

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x0600D7CD RID: 55245 RVA: 0x004D6084 File Offset: 0x004D4284
		// (remove) Token: 0x0600D7CE RID: 55246 RVA: 0x004D60BC File Offset: 0x004D42BC
		public event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x0600D7CF RID: 55247 RVA: 0x004D60F1 File Offset: 0x004D42F1
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.m_idProvider.Id;
			}
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x0600D7D0 RID: 55248 RVA: 0x004D60FE File Offset: 0x004D42FE
		// (set) Token: 0x0600D7D1 RID: 55249 RVA: 0x004D6106 File Offset: 0x004D4306
		public string GamerTag
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x0600D7D2 RID: 55250 RVA: 0x004D610F File Offset: 0x004D430F
		// (set) Token: 0x0600D7D3 RID: 55251 RVA: 0x004D6117 File Offset: 0x004D4317
		public SocialManagerXbl SocialManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x0600D7D4 RID: 55252 RVA: 0x004D6120 File Offset: 0x004D4320
		// (set) Token: 0x0600D7D5 RID: 55253 RVA: 0x004D6128 File Offset: 0x004D4328
		public MultiplayerActivityQueryManager MultiplayerActivityQueryManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x0600D7D6 RID: 55254 RVA: 0x004D6131 File Offset: 0x004D4331
		// (set) Token: 0x0600D7D7 RID: 55255 RVA: 0x004D6139 File Offset: 0x004D4339
		public XblSandboxHelper SandboxHelper
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new XblSandboxHelper();

		// Token: 0x0600D7D8 RID: 55256 RVA: 0x004D6144 File Offset: 0x004D4344
		public void Init(IPlatform _owner)
		{
			this.m_owner = _owner;
			this.m_api = _owner.Api;
			this.m_appState = _owner.ApplicationState;
			this.m_api.ClientApiInitialized += this.OnClientApiInitialized;
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			this.m_idProvider = new IdProviderGameCore(this, (crossplatformPlatform != null) ? crossplatformPlatform.User : null);
			XblXuidMapper.XuidMapped += this.OnXuidMapped;
			this.UserLoggedIn += this.SetNameOnPlayerLogin;
			this.m_appState.OnNetworkStateChanged += this.OnNetworkStateChanged;
		}

		// Token: 0x0600D7D9 RID: 55257 RVA: 0x004D61E0 File Offset: 0x004D43E0
		public void Destroy()
		{
			this.m_appState.OnNetworkStateChanged -= this.OnNetworkStateChanged;
			if (this.m_userHandle != null)
			{
				SDK.XUserCloseHandle(this.m_userHandle);
				XblHelpers.LogHR(0, "Close User Handle.", false);
				this.m_userHandle = null;
				this.m_userLocalId = null;
			}
			this.m_loginUserCallback = null;
			this.m_api.ClientApiInitialized -= this.OnClientApiInitialized;
		}

		// Token: 0x0600D7DA RID: 55258 RVA: 0x004D6258 File Offset: 0x004D4458
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetNameOnPlayerLogin(IPlatform platform)
		{
			string text;
			if (Unity.XGamingRuntime.Interop.HR.FAILED(SDK.XUserGetGamertag(this.UserHandle, XUserGamertagComponent.Classic, out text)))
			{
				Log.Error("[XBL] Failed to get player's gamertag");
				return;
			}
			GamePrefs.Set(EnumGamePrefs.PlayerName, text);
			this.GamerTag = text;
		}

		// Token: 0x0600D7DB RID: 55259 RVA: 0x004D6294 File Offset: 0x004D4494
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.SandboxHelper.RefreshSandboxId();
			SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserSilently, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0));
		}

		// Token: 0x0600D7DC RID: 55260 RVA: 0x004D62B4 File Offset: 0x004D44B4
		public void Login(LoginUserCallback _delegate)
		{
			User.<>c__DisplayClass78_0 CS$<>8__locals1 = new User.<>c__DisplayClass78_0();
			CS$<>8__locals1._delegate = _delegate;
			CS$<>8__locals1.<>4__this = this;
			object loginLock = this.m_loginLock;
			lock (loginLock)
			{
				if (this.m_loginDone)
				{
					this.UserStatus = this.m_loginUserStatus;
					XblHelpers.LogHR((this.m_loginUserStatus == EUserStatus.LoggedIn) ? 0 : (-2147467259), string.Format("Login with cached initial results. User Status: {0}, Reason: {1}, Additional: '{2}'.", this.m_loginUserStatus, this.m_loginUserCallbackReason, this.m_loginUserCallbackReasonAdditional), false);
					CS$<>8__locals1.<Login>g__Callback|0(this.m_owner, this.m_loginUserCallbackReason, this.m_loginUserCallbackReasonAdditional);
				}
				else
				{
					this.m_loginUserCallback = new LoginUserCallback(CS$<>8__locals1.<Login>g__Callback|0);
				}
			}
		}

		// Token: 0x0600D7DD RID: 55261 RVA: 0x004D6380 File Offset: 0x004D4580
		public void PlayOffline(LoginUserCallback _delegate)
		{
			if (!this.m_idProvider.LoadOfflineId())
			{
				this.UserStatus = EUserStatus.TemporaryError;
				_delegate(this.m_owner, EApiStatusReason.NoOnlineStart, null);
				return;
			}
			this.UserStatus = EUserStatus.OfflineMode;
			_delegate(this.m_owner, EApiStatusReason.Ok, null);
			Action<IPlatform> userLoggedIn = this.m_userLoggedIn;
			if (userLoggedIn == null)
			{
				return;
			}
			userLoggedIn(this.m_owner);
		}

		// Token: 0x0600D7DE RID: 55262 RVA: 0x000200DA File Offset: 0x0001E2DA
		public Action IsConnectToServerFromCommandline()
		{
			return null;
		}

		// Token: 0x0600D7DF RID: 55263 RVA: 0x004D63DC File Offset: 0x004D45DC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
			if (_serverInfo == null)
			{
				return;
			}
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				this.m_shouldActivityBeActive = true;
				if (_serverInfo != this.m_activityLastServerInfo)
				{
					if (this.m_activityLastServerInfo != null)
					{
						this.UnregisterGameServerInfoEvents(this.m_activityLastServerInfo);
					}
					this.m_activityLastServerInfo = _serverInfo;
					this.RegisterGameServerInfoEvents(_serverInfo);
				}
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D7E0 RID: 55264 RVA: 0x004D6454 File Offset: 0x004D4654
		[PublicizedFrom(EAccessModifier.Private)]
		public void RegisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedString += this.OnServerInfoChangedString;
			_serverInfo.OnChangedInt += this.OnServerInfoChangedInt;
		}

		// Token: 0x0600D7E1 RID: 55265 RVA: 0x004D647A File Offset: 0x004D467A
		[PublicizedFrom(EAccessModifier.Private)]
		public void UnregisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedInt -= this.OnServerInfoChangedInt;
			_serverInfo.OnChangedString -= this.OnServerInfoChangedString;
		}

		// Token: 0x0600D7E2 RID: 55266 RVA: 0x004D64A0 File Offset: 0x004D46A0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedString(GameServerInfo _serverInfo, GameInfoString _key)
		{
			if (_key == GameInfoString.UniqueId)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D7E3 RID: 55267 RVA: 0x004D64AE File Offset: 0x004D46AE
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedInt(GameServerInfo _serverInfo, GameInfoInt _key)
		{
			if (_key == GameInfoInt.CurrentPlayers || _key == GameInfoInt.ServerVisibility || _key == GameInfoInt.MaxPlayers)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D7E4 RID: 55268 RVA: 0x004D64C4 File Offset: 0x004D46C4
		public void StopAdvertisePlaying()
		{
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				this.m_shouldActivityBeActive = false;
				if (this.m_activityLastServerInfo != null)
				{
					this.UnregisterGameServerInfoEvents(this.m_activityLastServerInfo);
					this.m_activityLastServerInfo = null;
				}
				this.DeleteActivity();
			}
		}

		// Token: 0x0600D7E5 RID: 55269 RVA: 0x004D6528 File Offset: 0x004D4728
		[PublicizedFrom(EAccessModifier.Private)]
		public void DeleteActivity()
		{
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				if (!this.m_isActivityActive || this.m_shouldActivityBeActive)
				{
					this.m_shouldRetryDeleteActivity = false;
					return;
				}
				if (this.m_currentlyDeletingActivity)
				{
					this.m_shouldRetryDeleteActivity = true;
					return;
				}
				this.m_shouldRetryDeleteActivity = false;
				this.m_currentlyDeletingActivity = true;
				SDK.XBL.XblMultiplayerActivityDeleteActivityAsync(this.XblContextHandle, new SDK.XBL.XblMultiplayerActivityAsyncOperationCompleted(this.<DeleteActivity>g__CompletionRoutine|87_0));
			}
		}

		// Token: 0x0600D7E6 RID: 55270 RVA: 0x004D65B4 File Offset: 0x004D47B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnNetworkStateChanged(bool state)
		{
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				if (this.m_isActivityActive && !this.m_shouldActivityBeActive)
				{
					this.DeleteActivity();
				}
			}
		}

		// Token: 0x0600D7E7 RID: 55271 RVA: 0x004D6604 File Offset: 0x004D4804
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetActivity(GameServerInfo _serverInfo)
		{
			if (_serverInfo == null)
			{
				return;
			}
			string value = _serverInfo.GetValue(GameInfoString.UniqueId);
			int value2 = _serverInfo.GetValue(GameInfoInt.CurrentPlayers);
			int value3 = _serverInfo.GetValue(GameInfoInt.ServerVisibility);
			int value4 = _serverInfo.GetValue(GameInfoInt.MaxPlayers);
			if (value4 < 2)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			XblMultiplayerActivityInfo xblMultiplayerActivityInfo = new XblMultiplayerActivityInfo();
			xblMultiplayerActivityInfo.ConnectionString = value;
			xblMultiplayerActivityInfo.CurrentPlayers = (uint)value2;
			xblMultiplayerActivityInfo.GroupId = "Dummy";
			XblMultiplayerActivityInfo xblMultiplayerActivityInfo2 = xblMultiplayerActivityInfo;
			XblMultiplayerActivityJoinRestriction xblMultiplayerActivityJoinRestriction;
			if (value3 != 1)
			{
				if (value3 == 2)
				{
					xblMultiplayerActivityJoinRestriction = XblMultiplayerActivityJoinRestriction.Public;
				}
				else
				{
					xblMultiplayerActivityJoinRestriction = XblMultiplayerActivityJoinRestriction.InviteOnly;
				}
			}
			else
			{
				xblMultiplayerActivityJoinRestriction = XblMultiplayerActivityJoinRestriction.Followed;
			}
			xblMultiplayerActivityInfo2.JoinRestriction = xblMultiplayerActivityJoinRestriction;
			xblMultiplayerActivityInfo.MaxPlayers = (uint)value4;
			xblMultiplayerActivityInfo.Platform = XblMultiplayerActivityPlatform.All;
			xblMultiplayerActivityInfo.Xuid = this.m_idProvider.Id.Xuid;
			XblMultiplayerActivityInfo xblMultiplayerActivityInfo3 = xblMultiplayerActivityInfo;
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				if (this.m_shouldActivityBeActive)
				{
					SDK.XBL.XblMultiplayerActivitySetActivityAsync(this.XblContextHandle, xblMultiplayerActivityInfo3, true, new SDK.XBL.XblMultiplayerActivityAsyncOperationCompleted(this.<SetActivity>g__CompletionRoutine|89_0));
				}
			}
		}

		// Token: 0x0600D7E8 RID: 55272 RVA: 0x004D670C File Offset: 0x004D490C
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			User.<>c__DisplayClass93_0 CS$<>8__locals1 = new User.<>c__DisplayClass93_0();
			CS$<>8__locals1._callback = _callback;
			if (this.m_userHandle == null)
			{
				Log.Error("[XBL] Attempting to retrieve XSTS token before acquiring XUserHandle");
				CS$<>8__locals1._callback(false, null, null);
				return;
			}
			SDK.XUserGetTokenAndSignatureUtf16Async(this.m_userHandle, XUserGetTokenAndSignatureOptions.None, "GET", "https://eos.epicgames.com", User.eosRelyingPartyRequestHeaders, null, new XUserGetTokenAndSignatureUtf16Result(CS$<>8__locals1.<GetLoginTicket>g__CompletionRoutine|0));
		}

		// Token: 0x0600D7E9 RID: 55273 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600D7EA RID: 55274 RVA: 0x004D6778 File Offset: 0x004D4978
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			ulong xuid = XblXuidMapper.GetXuid(_playerId);
			if (xuid == 0UL)
			{
				return false;
			}
			FriendsListXbl friendsListXbl = this.friendsList;
			return friendsListXbl != null && friendsListXbl.IsFriend(xuid);
		}

		// Token: 0x0600D7EB RID: 55275 RVA: 0x004D67A3 File Offset: 0x004D49A3
		public bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return XblXuidMapper.GetXuid(_playerId) > 0UL;
		}

		// Token: 0x0600D7EC RID: 55276 RVA: 0x004D67B0 File Offset: 0x004D49B0
		public void ShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			ulong xuid = XblXuidMapper.GetXuid(_playerId);
			if (xuid != 0UL)
			{
				SDK.XGameUiShowPlayerProfileCardAsync(this.m_userHandle, xuid, delegate(int hr)
				{
					XblHelpers.LogHR(0, "XGameUiShowPlayerProfileCardAsync", false);
					if (Unity.XGamingRuntime.Interop.HR.FAILED(hr))
					{
						Log.Error("XBL-GXDK: Showing Player Profile Failed.");
						return;
					}
					Log.Out("XBL-GXDK: Showing Player Profile Succeeded.");
				});
			}
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x0600D7ED RID: 55277 RVA: 0x004D67F4 File Offset: 0x004D49F4
		public EUserPerms Permissions
		{
			get
			{
				if (this.m_privilegeHelper == null)
				{
					return (EUserPerms)0;
				}
				EUserPerms euserPerms = (EUserPerms)0;
				if (this.m_privilegeHelper.MultiplayerAllowed.Has())
				{
					euserPerms |= EUserPerms.Multiplayer | EUserPerms.HostMultiplayer;
				}
				if (this.m_privilegeHelper.CommunicationAllowed.Has())
				{
					euserPerms |= EUserPerms.Communication;
				}
				if (this.m_privilegeHelper.CrossPlayAllowed.Has())
				{
					euserPerms |= EUserPerms.Crossplay;
				}
				return euserPerms;
			}
		}

		// Token: 0x0600D7EE RID: 55278 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600D7EF RID: 55279 RVA: 0x004D6851 File Offset: 0x004D4A51
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			Log.Out(string.Format("[XBL] {0}({1}: [{2}], {3}: {4})", new object[] { "ResolvePermissions", "_perms", _perms, "_canPrompt", _canPrompt }));
			if (this.m_privilegeHelper == null)
			{
				yield break;
			}
			yield return this.m_privilegeHelper.ResolvePermissions(_perms, _canPrompt, _cancellationToken);
			yield break;
		}

		// Token: 0x0600D7F0 RID: 55280 RVA: 0x004D6875 File Offset: 0x004D4A75
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnXuidMapped(IReadOnlyCollection<PlatformUserIdentifierAbs> userIds, ulong xuid)
		{
			UserBlocksChangedCallback userBlocksChanged = this.UserBlocksChanged;
			if (userBlocksChanged == null)
			{
				return;
			}
			userBlocksChanged(userIds);
		}

		// Token: 0x0600D7F1 RID: 55281 RVA: 0x004D6888 File Offset: 0x004D4A88
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			User.<>c__DisplayClass103_0 CS$<>8__locals1 = new User.<>c__DisplayClass103_0();
			CS$<>8__locals1._results = _results;
			CS$<>8__locals1.<>4__this = this;
			this.userBlockedXuidToResultsTemp.Clear();
			this.userBlockedAnonymousResultsTemp.Clear();
			PlatformUserIdentifierAbs platformUserId = this.PlatformUserId;
			foreach (IPlatformUserBlockedResults platformUserBlockedResults in CS$<>8__locals1._results)
			{
				PlatformUserIdentifierAbs nativeId = platformUserBlockedResults.User.NativeId;
				if (!object.Equals(platformUserId, nativeId))
				{
					UserIdentifierXbl userIdentifierXbl = nativeId as UserIdentifierXbl;
					ulong xuid;
					if (userIdentifierXbl == null || (xuid = userIdentifierXbl.Xuid) == 0UL)
					{
						this.userBlockedAnonymousResultsTemp.Add(platformUserBlockedResults);
					}
					else
					{
						this.userBlockedXuidToResultsTemp[xuid] = platformUserBlockedResults;
					}
				}
			}
			CS$<>8__locals1.running = true;
			SDK.XBL.XblPrivacyBatchCheckPermissionAsync(this.XblContextHandle, User.userBlockedPermissions, this.userBlockedXuidToResultsTemp.Keys.ToArray<ulong>(), (this.userBlockedAnonymousResultsTemp.Count > 0) ? User.userBlockedAnonymousTypes : Array.Empty<XblAnonymousUserType>(), new XblPrivacyBatchCheckPermissionCompleted(CS$<>8__locals1.<ResolveUserBlocks>g__CompletionRoutine|0));
			while (CS$<>8__locals1.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600D7F2 RID: 55282 RVA: 0x004D68A0 File Offset: 0x004D4AA0
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			string sandboxId = this.SandboxHelper.SandboxId;
			if (sandboxId == null)
			{
				EMatchmakingGroup ematchmakingGroup = EMatchmakingGroup.Retail;
				Log.Warning(string.Format("[XBL] {0} no sandbox id. Defaulting to {1}", "GetMatchmakingGroup", ematchmakingGroup));
				return ematchmakingGroup;
			}
			return XblSandboxHelper.SandboxIdToMatchmakingGroup(sandboxId);
		}

		// Token: 0x0600D7F4 RID: 55284 RVA: 0x004D693C File Offset: 0x004D4B3C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0(int hr, XUserHandle userHandle)
		{
			XblHelpers.LogHR(hr, "XUserAddAsync: AddDefaultUserSilently", false);
			if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(hr))
			{
				this.<OnClientApiInitialized>g__XUserAddSucceeded|77_2(userHandle);
				return;
			}
			if (hr == -1994108666)
			{
				this.UserStatus = EUserStatus.TemporaryError;
				SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserAllowingUI, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncAllowingUICompletionRoutine|77_1));
				return;
			}
			this.UserStatus = EUserStatus.TemporaryError;
			SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserSilently, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0));
		}

		// Token: 0x0600D7F5 RID: 55285 RVA: 0x004D69A2 File Offset: 0x004D4BA2
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__XUserAddAsyncAllowingUICompletionRoutine|77_1(int hr, XUserHandle userHandle)
		{
			XblHelpers.LogHR(hr, "XUserAddAsync: AddDefaultUserAllowingUI", false);
			if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(hr))
			{
				this.<OnClientApiInitialized>g__XUserAddSucceeded|77_2(userHandle);
				return;
			}
			this.UserStatus = EUserStatus.TemporaryError;
			SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserSilently, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0));
		}

		// Token: 0x0600D7F6 RID: 55286 RVA: 0x004D69DC File Offset: 0x004D4BDC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__XUserAddSucceeded|77_2(XUserHandle userHandle)
		{
			object userHandleLock = this.m_userHandleLock;
			lock (userHandleLock)
			{
				this.m_userHandle = userHandle;
				Action<XUserHandle> userHandleReady = this.m_userHandleReady;
				if (userHandleReady != null)
				{
					userHandleReady(userHandle);
				}
			}
			this.m_privilegeHelper = new UserPrivilegeHelper(this.m_userHandle);
			this.m_privilegeHelper.AllAllowed.ResolveSilent();
			this.SocialManager = new SocialManagerXbl(this.m_userHandle);
			this.friendsList = new FriendsListXbl(this.SocialManager);
			this.<OnClientApiInitialized>g__GetXuid|77_3();
		}

		// Token: 0x0600D7F7 RID: 55287 RVA: 0x004D6A78 File Offset: 0x004D4C78
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__GetXuid|77_3()
		{
			int num = SDK.XUserGetId(this.m_userHandle, out this.m_userXuid);
			XblHelpers.LogHR(num, "XUserGetId", false);
			if (!Unity.XGamingRuntime.Interop.HR.FAILED(num))
			{
				this.<OnClientApiInitialized>g__PostLogin|77_6();
				return;
			}
			if (num == -1994108670)
			{
				this.<OnClientApiInitialized>g__ResolveXuidIssue|77_4();
				return;
			}
			this.<OnClientApiInitialized>g__DoLoginUserCallback|77_7(EUserStatus.PermanentError, EApiStatusReason.Other, "Could not obtain User's Xuid.");
		}

		// Token: 0x0600D7F8 RID: 55288 RVA: 0x004D6ACE File Offset: 0x004D4CCE
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssue|77_4()
		{
			SDK.XUserResolveIssueWithUiUtf16Async(this.m_userHandle, null, new XUserResolveIssueWithUiUtf16Result(this.<OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5));
		}

		// Token: 0x0600D7F9 RID: 55289 RVA: 0x004D6AE9 File Offset: 0x004D4CE9
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5(int hrResolve)
		{
			XblHelpers.LogHR(hrResolve, "XUserResolveIssueWithUiUtf16Async", false);
			this.<OnClientApiInitialized>g__GetXuid|77_3();
		}

		// Token: 0x0600D7FA RID: 55290 RVA: 0x004D6B00 File Offset: 0x004D4D00
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__PostLogin|77_6()
		{
			int num = SDK.XUserGetLocalId(this.m_userHandle, out this.m_userLocalId);
			XblHelpers.LogHR(num, "XUserGetLocalId", false);
			if (Unity.XGamingRuntime.Interop.HR.FAILED(num))
			{
				this.<OnClientApiInitialized>g__DoLoginUserCallback|77_7(EUserStatus.PermanentError, EApiStatusReason.Other, "Could not obtain User's Local ID.");
				return;
			}
			int num2 = SDK.XBL.XblContextCreateHandle(this.m_userHandle, out this.m_contextHandle);
			XblHelpers.LogHR(num2, "Create Xbox Live Context Handle", false);
			if (Unity.XGamingRuntime.Interop.HR.FAILED(num2))
			{
				this.<OnClientApiInitialized>g__DoLoginUserCallback|77_7(EUserStatus.PermanentError, EApiStatusReason.Other, "Could not obtain Xbox Live Context Handle.");
				return;
			}
			this.MultiplayerActivityQueryManager = new MultiplayerActivityQueryManager(this.m_contextHandle);
			this.<OnClientApiInitialized>g__DoLoginUserCallback|77_7(EUserStatus.LoggedIn, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600D7FB RID: 55291 RVA: 0x004D6B8C File Offset: 0x004D4D8C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__DoLoginUserCallback|77_7(EUserStatus userStatus, EApiStatusReason reason, string reasonAdditional)
		{
			object loginLock = this.m_loginLock;
			lock (loginLock)
			{
				this.UserStatus = userStatus;
				this.m_loginUserStatus = userStatus;
				this.m_loginUserCallbackReason = reason;
				this.m_loginUserCallbackReasonAdditional = reasonAdditional;
				this.m_loginDone = true;
				XblHelpers.LogHR((userStatus == EUserStatus.LoggedIn) ? 0 : (-2147467259), string.Format("Initial Login Callback Done. User Status: {0}, Reason: {1}, Additional: '{2}'.", userStatus, reason, reasonAdditional), false);
				LoginUserCallback loginUserCallback = this.m_loginUserCallback;
				if (loginUserCallback != null)
				{
					loginUserCallback(this.m_owner, reason, reasonAdditional);
				}
				this.m_loginUserCallback = null;
			}
		}

		// Token: 0x0600D7FC RID: 55292 RVA: 0x004D6C34 File Offset: 0x004D4E34
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <DeleteActivity>g__CompletionRoutine|87_0(int _hresult)
		{
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				this.m_currentlyDeletingActivity = false;
				if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(_hresult))
				{
					if (this.m_isActivityActive)
					{
						Log.Out("[XBL] Activity Deleted");
						this.m_isActivityActive = false;
					}
					this.m_shouldRetryDeleteActivity = false;
					if (this.m_shouldActivityBeActive)
					{
						Log.Out("[XBL] Setting activity since it should be active.");
						this.SetActivity(this.m_activityLastServerInfo);
					}
				}
				else
				{
					XblHelpers.LogHR(_hresult, "Delete Activity", true);
					if (this.m_isActivityActive && !this.m_shouldActivityBeActive)
					{
						if (this.m_shouldRetryDeleteActivity)
						{
							Log.Warning("[XBL] Failed to delete activity. Will retry now because a change (e.g. Network State) happened while deleting the activity.");
							this.m_shouldRetryDeleteActivity = false;
							SDK.XBL.XblMultiplayerActivityDeleteActivityAsync(this.XblContextHandle, new SDK.XBL.XblMultiplayerActivityAsyncOperationCompleted(this.<DeleteActivity>g__CompletionRoutine|87_0));
						}
						else
						{
							Log.Warning("[XBL] Failed to delete activity. Will retry when a change occurs (e.g. Network State).");
						}
					}
				}
			}
		}

		// Token: 0x0600D7FD RID: 55293 RVA: 0x004D6D18 File Offset: 0x004D4F18
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <SetActivity>g__CompletionRoutine|89_0(int _hresult)
		{
			object activityLock = this.m_activityLock;
			lock (activityLock)
			{
				if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(_hresult))
				{
					if (!this.m_isActivityActive)
					{
						Log.Out("[XBL] Activity Created");
						this.m_isActivityActive = true;
					}
					if (!this.m_shouldActivityBeActive)
					{
						Log.Out("[XBL] Deleting activity since it should not be active.");
						this.DeleteActivity();
					}
				}
				else
				{
					XblHelpers.LogHR(_hresult, "Set Activity", false);
				}
			}
		}

		// Token: 0x0400A3DD RID: 41949
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly EnumDictionary<XblPermission, EBlockType> xblPermissionToBlockType;

		// Token: 0x0400A3DE RID: 41950
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblPermission[] userBlockedPermissions;

		// Token: 0x0400A3DF RID: 41951
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblAnonymousUserType[] userBlockedAnonymousTypes;

		// Token: 0x0400A3E0 RID: 41952
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A3E1 RID: 41953
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformApi m_api;

		// Token: 0x0400A3E2 RID: 41954
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A3E3 RID: 41955
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserHandle m_userHandle;

		// Token: 0x0400A3E4 RID: 41956
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_userHandleLock = new object();

		// Token: 0x0400A3E5 RID: 41957
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<XUserHandle> m_userHandleReady;

		// Token: 0x0400A3E6 RID: 41958
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle m_contextHandle;

		// Token: 0x0400A3E7 RID: 41959
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserLocalId m_userLocalId;

		// Token: 0x0400A3E8 RID: 41960
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong m_userXuid;

		// Token: 0x0400A3E9 RID: 41961
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierXbl m_platformUserId;

		// Token: 0x0400A3EA RID: 41962
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_loginLock = new object();

		// Token: 0x0400A3EB RID: 41963
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_loginDone;

		// Token: 0x0400A3EC RID: 41964
		[PublicizedFrom(EAccessModifier.Private)]
		public EUserStatus m_loginUserStatus;

		// Token: 0x0400A3ED RID: 41965
		[PublicizedFrom(EAccessModifier.Private)]
		public LoginUserCallback m_loginUserCallback;

		// Token: 0x0400A3EE RID: 41966
		[PublicizedFrom(EAccessModifier.Private)]
		public EApiStatusReason m_loginUserCallbackReason;

		// Token: 0x0400A3EF RID: 41967
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_loginUserCallbackReasonAdditional;

		// Token: 0x0400A3F0 RID: 41968
		[PublicizedFrom(EAccessModifier.Private)]
		public UserPrivilegeHelper m_privilegeHelper;

		// Token: 0x0400A3F1 RID: 41969
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_activityLock = new object();

		// Token: 0x0400A3F2 RID: 41970
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo m_activityLastServerInfo;

		// Token: 0x0400A3F3 RID: 41971
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isActivityActive;

		// Token: 0x0400A3F4 RID: 41972
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldActivityBeActive;

		// Token: 0x0400A3F5 RID: 41973
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldRetryDeleteActivity;

		// Token: 0x0400A3F6 RID: 41974
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_currentlyDeletingActivity;

		// Token: 0x0400A3F7 RID: 41975
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, IPlatformUserBlockedResults> userBlockedXuidToResultsTemp = new Dictionary<ulong, IPlatformUserBlockedResults>();

		// Token: 0x0400A3F8 RID: 41976
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<IPlatformUserBlockedResults> userBlockedAnonymousResultsTemp = new List<IPlatformUserBlockedResults>();

		// Token: 0x0400A3FA RID: 41978
		public string SandboxName;

		// Token: 0x0400A3FB RID: 41979
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> m_userLoggedIn;

		// Token: 0x0400A3FD RID: 41981
		[PublicizedFrom(EAccessModifier.Private)]
		public IdProviderGameCore m_idProvider;

		// Token: 0x0400A400 RID: 41984
		[PublicizedFrom(EAccessModifier.Private)]
		public FriendsListXbl friendsList;

		// Token: 0x0400A403 RID: 41987
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyUrl = "https://eos.epicgames.com";

		// Token: 0x0400A404 RID: 41988
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyHttpMethod = "GET";

		// Token: 0x0400A405 RID: 41989
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XUserGetTokenAndSignatureUtf16HttpHeader[] eosRelyingPartyRequestHeaders = new XUserGetTokenAndSignatureUtf16HttpHeader[]
		{
			new XUserGetTokenAndSignatureUtf16HttpHeader
			{
				Name = "X-XBL-Contract-Version",
				Value = "2"
			}
		};
	}
}
