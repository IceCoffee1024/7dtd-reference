using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001BE2 RID: 7138
	public class User : IUserClient
	{
		// Token: 0x0600D3A7 RID: 54183 RVA: 0x004C73A8 File Offset: 0x004C55A8
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

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x0600D3A8 RID: 54184 RVA: 0x004C748C File Offset: 0x004C568C
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

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x0600D3A9 RID: 54185 RVA: 0x004C74D0 File Offset: 0x004C56D0
		public XUserLocalId LocalID
		{
			get
			{
				return this.m_userLocalId;
			}
		}

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x0600D3AA RID: 54186 RVA: 0x004C74D8 File Offset: 0x004C56D8
		public ulong Xuid
		{
			get
			{
				return this.m_userXuid;
			}
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x0600D3AB RID: 54187 RVA: 0x004C74E0 File Offset: 0x004C56E0
		public Unity.XGamingRuntime.XblContextHandle XblContextHandle
		{
			get
			{
				return this.m_contextHandle;
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x0600D3AC RID: 54188 RVA: 0x004C74E8 File Offset: 0x004C56E8
		public XUserHandle UserHandle
		{
			get
			{
				return this.m_userHandle;
			}
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x0600D3AD RID: 54189 RVA: 0x004C74F0 File Offset: 0x004C56F0
		// (set) Token: 0x0600D3AE RID: 54190 RVA: 0x004C74F8 File Offset: 0x004C56F8
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x0600D3AF RID: 54191 RVA: 0x004C7504 File Offset: 0x004C5704
		// (remove) Token: 0x0600D3B0 RID: 54192 RVA: 0x004C7570 File Offset: 0x004C5770
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
		// (add) Token: 0x0600D3B1 RID: 54193 RVA: 0x004C75C4 File Offset: 0x004C57C4
		// (remove) Token: 0x0600D3B2 RID: 54194 RVA: 0x004C762C File Offset: 0x004C582C
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
		// (add) Token: 0x0600D3B3 RID: 54195 RVA: 0x004C7680 File Offset: 0x004C5880
		// (remove) Token: 0x0600D3B4 RID: 54196 RVA: 0x004C76B8 File Offset: 0x004C58B8
		public event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x0600D3B5 RID: 54197 RVA: 0x004C76ED File Offset: 0x004C58ED
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.m_idProvider.Id;
			}
		}

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x0600D3B6 RID: 54198 RVA: 0x004C76FA File Offset: 0x004C58FA
		// (set) Token: 0x0600D3B7 RID: 54199 RVA: 0x004C7702 File Offset: 0x004C5902
		public string GamerTag
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x0600D3B8 RID: 54200 RVA: 0x004C770B File Offset: 0x004C590B
		// (set) Token: 0x0600D3B9 RID: 54201 RVA: 0x004C7713 File Offset: 0x004C5913
		public SocialManagerXbl SocialManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x0600D3BA RID: 54202 RVA: 0x004C771C File Offset: 0x004C591C
		// (set) Token: 0x0600D3BB RID: 54203 RVA: 0x004C7724 File Offset: 0x004C5924
		public MultiplayerActivityQueryManager MultiplayerActivityQueryManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x0600D3BC RID: 54204 RVA: 0x004C772D File Offset: 0x004C592D
		// (set) Token: 0x0600D3BD RID: 54205 RVA: 0x004C7735 File Offset: 0x004C5935
		public XblSandboxHelper SandboxHelper
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new XblSandboxHelper();

		// Token: 0x0600D3BE RID: 54206 RVA: 0x004C7740 File Offset: 0x004C5940
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

		// Token: 0x0600D3BF RID: 54207 RVA: 0x004C77DC File Offset: 0x004C59DC
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

		// Token: 0x0600D3C0 RID: 54208 RVA: 0x004C7854 File Offset: 0x004C5A54
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

		// Token: 0x0600D3C1 RID: 54209 RVA: 0x004C7890 File Offset: 0x004C5A90
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.SandboxHelper.RefreshSandboxId();
			SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserSilently, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0));
		}

		// Token: 0x0600D3C2 RID: 54210 RVA: 0x004C78B0 File Offset: 0x004C5AB0
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

		// Token: 0x0600D3C3 RID: 54211 RVA: 0x004C797C File Offset: 0x004C5B7C
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

		// Token: 0x0600D3C4 RID: 54212 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public Action IsConnectToServerFromCommandline()
		{
			return null;
		}

		// Token: 0x0600D3C5 RID: 54213 RVA: 0x004C79D8 File Offset: 0x004C5BD8
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

		// Token: 0x0600D3C6 RID: 54214 RVA: 0x004C7A50 File Offset: 0x004C5C50
		[PublicizedFrom(EAccessModifier.Private)]
		public void RegisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedString += this.OnServerInfoChangedString;
			_serverInfo.OnChangedInt += this.OnServerInfoChangedInt;
		}

		// Token: 0x0600D3C7 RID: 54215 RVA: 0x004C7A76 File Offset: 0x004C5C76
		[PublicizedFrom(EAccessModifier.Private)]
		public void UnregisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedInt -= this.OnServerInfoChangedInt;
			_serverInfo.OnChangedString -= this.OnServerInfoChangedString;
		}

		// Token: 0x0600D3C8 RID: 54216 RVA: 0x004C7A9C File Offset: 0x004C5C9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedString(GameServerInfo _serverInfo, GameInfoString _key)
		{
			if (_key == GameInfoString.UniqueId)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D3C9 RID: 54217 RVA: 0x004C7AAA File Offset: 0x004C5CAA
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedInt(GameServerInfo _serverInfo, GameInfoInt _key)
		{
			if (_key == GameInfoInt.CurrentPlayers || _key == GameInfoInt.ServerVisibility || _key == GameInfoInt.MaxPlayers)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D3CA RID: 54218 RVA: 0x004C7AC0 File Offset: 0x004C5CC0
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

		// Token: 0x0600D3CB RID: 54219 RVA: 0x004C7B24 File Offset: 0x004C5D24
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

		// Token: 0x0600D3CC RID: 54220 RVA: 0x004C7BB0 File Offset: 0x004C5DB0
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

		// Token: 0x0600D3CD RID: 54221 RVA: 0x004C7C00 File Offset: 0x004C5E00
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

		// Token: 0x0600D3CE RID: 54222 RVA: 0x004C7D08 File Offset: 0x004C5F08
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

		// Token: 0x0600D3CF RID: 54223 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600D3D0 RID: 54224 RVA: 0x004C7D74 File Offset: 0x004C5F74
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

		// Token: 0x0600D3D1 RID: 54225 RVA: 0x004C7D9F File Offset: 0x004C5F9F
		public bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return XblXuidMapper.GetXuid(_playerId) > 0UL;
		}

		// Token: 0x0600D3D2 RID: 54226 RVA: 0x004C7DAC File Offset: 0x004C5FAC
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

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x0600D3D3 RID: 54227 RVA: 0x004C7DF0 File Offset: 0x004C5FF0
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

		// Token: 0x0600D3D4 RID: 54228 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600D3D5 RID: 54229 RVA: 0x004C7E4D File Offset: 0x004C604D
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

		// Token: 0x0600D3D6 RID: 54230 RVA: 0x004C7E71 File Offset: 0x004C6071
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

		// Token: 0x0600D3D7 RID: 54231 RVA: 0x004C7E84 File Offset: 0x004C6084
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

		// Token: 0x0600D3D8 RID: 54232 RVA: 0x004C7E9C File Offset: 0x004C609C
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

		// Token: 0x0600D3DA RID: 54234 RVA: 0x004C7F38 File Offset: 0x004C6138
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

		// Token: 0x0600D3DB RID: 54235 RVA: 0x004C7F9E File Offset: 0x004C619E
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

		// Token: 0x0600D3DC RID: 54236 RVA: 0x004C7FD8 File Offset: 0x004C61D8
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

		// Token: 0x0600D3DD RID: 54237 RVA: 0x004C8074 File Offset: 0x004C6274
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

		// Token: 0x0600D3DE RID: 54238 RVA: 0x004C80CA File Offset: 0x004C62CA
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssue|77_4()
		{
			SDK.XUserResolveIssueWithUiUtf16Async(this.m_userHandle, null, new XUserResolveIssueWithUiUtf16Result(this.<OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5));
		}

		// Token: 0x0600D3DF RID: 54239 RVA: 0x004C80E5 File Offset: 0x004C62E5
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5(int hrResolve)
		{
			XblHelpers.LogHR(hrResolve, "XUserResolveIssueWithUiUtf16Async", false);
			this.<OnClientApiInitialized>g__GetXuid|77_3();
		}

		// Token: 0x0600D3E0 RID: 54240 RVA: 0x004C80FC File Offset: 0x004C62FC
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

		// Token: 0x0600D3E1 RID: 54241 RVA: 0x004C8188 File Offset: 0x004C6388
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

		// Token: 0x0600D3E2 RID: 54242 RVA: 0x004C8230 File Offset: 0x004C6430
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

		// Token: 0x0600D3E3 RID: 54243 RVA: 0x004C8314 File Offset: 0x004C6514
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

		// Token: 0x0400A0EE RID: 41198
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly EnumDictionary<XblPermission, EBlockType> xblPermissionToBlockType;

		// Token: 0x0400A0EF RID: 41199
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblPermission[] userBlockedPermissions;

		// Token: 0x0400A0F0 RID: 41200
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblAnonymousUserType[] userBlockedAnonymousTypes;

		// Token: 0x0400A0F1 RID: 41201
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A0F2 RID: 41202
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformApi m_api;

		// Token: 0x0400A0F3 RID: 41203
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A0F4 RID: 41204
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserHandle m_userHandle;

		// Token: 0x0400A0F5 RID: 41205
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_userHandleLock = new object();

		// Token: 0x0400A0F6 RID: 41206
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<XUserHandle> m_userHandleReady;

		// Token: 0x0400A0F7 RID: 41207
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle m_contextHandle;

		// Token: 0x0400A0F8 RID: 41208
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserLocalId m_userLocalId;

		// Token: 0x0400A0F9 RID: 41209
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong m_userXuid;

		// Token: 0x0400A0FA RID: 41210
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierXbl m_platformUserId;

		// Token: 0x0400A0FB RID: 41211
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_loginLock = new object();

		// Token: 0x0400A0FC RID: 41212
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_loginDone;

		// Token: 0x0400A0FD RID: 41213
		[PublicizedFrom(EAccessModifier.Private)]
		public EUserStatus m_loginUserStatus;

		// Token: 0x0400A0FE RID: 41214
		[PublicizedFrom(EAccessModifier.Private)]
		public LoginUserCallback m_loginUserCallback;

		// Token: 0x0400A0FF RID: 41215
		[PublicizedFrom(EAccessModifier.Private)]
		public EApiStatusReason m_loginUserCallbackReason;

		// Token: 0x0400A100 RID: 41216
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_loginUserCallbackReasonAdditional;

		// Token: 0x0400A101 RID: 41217
		[PublicizedFrom(EAccessModifier.Private)]
		public UserPrivilegeHelper m_privilegeHelper;

		// Token: 0x0400A102 RID: 41218
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_activityLock = new object();

		// Token: 0x0400A103 RID: 41219
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo m_activityLastServerInfo;

		// Token: 0x0400A104 RID: 41220
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isActivityActive;

		// Token: 0x0400A105 RID: 41221
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldActivityBeActive;

		// Token: 0x0400A106 RID: 41222
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldRetryDeleteActivity;

		// Token: 0x0400A107 RID: 41223
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_currentlyDeletingActivity;

		// Token: 0x0400A108 RID: 41224
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, IPlatformUserBlockedResults> userBlockedXuidToResultsTemp = new Dictionary<ulong, IPlatformUserBlockedResults>();

		// Token: 0x0400A109 RID: 41225
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<IPlatformUserBlockedResults> userBlockedAnonymousResultsTemp = new List<IPlatformUserBlockedResults>();

		// Token: 0x0400A10B RID: 41227
		public string SandboxName;

		// Token: 0x0400A10C RID: 41228
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> m_userLoggedIn;

		// Token: 0x0400A10E RID: 41230
		[PublicizedFrom(EAccessModifier.Private)]
		public IdProviderGameCore m_idProvider;

		// Token: 0x0400A111 RID: 41233
		[PublicizedFrom(EAccessModifier.Private)]
		public FriendsListXbl friendsList;

		// Token: 0x0400A114 RID: 41236
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyUrl = "https://eos.epicgames.com";

		// Token: 0x0400A115 RID: 41237
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyHttpMethod = "GET";

		// Token: 0x0400A116 RID: 41238
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
