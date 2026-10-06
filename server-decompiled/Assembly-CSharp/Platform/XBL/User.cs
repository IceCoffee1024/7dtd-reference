using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C3B RID: 7227
	public class User : IUserClient
	{
		// Token: 0x0600D6AD RID: 54957 RVA: 0x004D2284 File Offset: 0x004D0484
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

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x0600D6AE RID: 54958 RVA: 0x004D2368 File Offset: 0x004D0568
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

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x0600D6AF RID: 54959 RVA: 0x004D23AC File Offset: 0x004D05AC
		public XUserLocalId LocalID
		{
			get
			{
				return this.m_userLocalId;
			}
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x0600D6B0 RID: 54960 RVA: 0x004D23B4 File Offset: 0x004D05B4
		public ulong Xuid
		{
			get
			{
				return this.m_userXuid;
			}
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x0600D6B1 RID: 54961 RVA: 0x004D23BC File Offset: 0x004D05BC
		public Unity.XGamingRuntime.XblContextHandle XblContextHandle
		{
			get
			{
				return this.m_contextHandle;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x0600D6B2 RID: 54962 RVA: 0x004D23C4 File Offset: 0x004D05C4
		public XUserHandle UserHandle
		{
			get
			{
				return this.m_userHandle;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x0600D6B3 RID: 54963 RVA: 0x004D23CC File Offset: 0x004D05CC
		// (set) Token: 0x0600D6B4 RID: 54964 RVA: 0x004D23D4 File Offset: 0x004D05D4
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x0600D6B5 RID: 54965 RVA: 0x004D23E0 File Offset: 0x004D05E0
		// (remove) Token: 0x0600D6B6 RID: 54966 RVA: 0x004D244C File Offset: 0x004D064C
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
		// (add) Token: 0x0600D6B7 RID: 54967 RVA: 0x004D24A0 File Offset: 0x004D06A0
		// (remove) Token: 0x0600D6B8 RID: 54968 RVA: 0x004D2508 File Offset: 0x004D0708
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
		// (add) Token: 0x0600D6B9 RID: 54969 RVA: 0x004D255C File Offset: 0x004D075C
		// (remove) Token: 0x0600D6BA RID: 54970 RVA: 0x004D2594 File Offset: 0x004D0794
		public event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x0600D6BB RID: 54971 RVA: 0x004D25C9 File Offset: 0x004D07C9
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.m_idProvider.Id;
			}
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x0600D6BC RID: 54972 RVA: 0x004D25D6 File Offset: 0x004D07D6
		// (set) Token: 0x0600D6BD RID: 54973 RVA: 0x004D25DE File Offset: 0x004D07DE
		public string GamerTag
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x0600D6BE RID: 54974 RVA: 0x004D25E7 File Offset: 0x004D07E7
		// (set) Token: 0x0600D6BF RID: 54975 RVA: 0x004D25EF File Offset: 0x004D07EF
		public SocialManagerXbl SocialManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x0600D6C0 RID: 54976 RVA: 0x004D25F8 File Offset: 0x004D07F8
		// (set) Token: 0x0600D6C1 RID: 54977 RVA: 0x004D2600 File Offset: 0x004D0800
		public MultiplayerActivityQueryManager MultiplayerActivityQueryManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x0600D6C2 RID: 54978 RVA: 0x004D2609 File Offset: 0x004D0809
		// (set) Token: 0x0600D6C3 RID: 54979 RVA: 0x004D2611 File Offset: 0x004D0811
		public XblSandboxHelper SandboxHelper
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new XblSandboxHelper();

		// Token: 0x0600D6C4 RID: 54980 RVA: 0x004D261C File Offset: 0x004D081C
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

		// Token: 0x0600D6C5 RID: 54981 RVA: 0x004D26B8 File Offset: 0x004D08B8
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

		// Token: 0x0600D6C6 RID: 54982 RVA: 0x004D2730 File Offset: 0x004D0930
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

		// Token: 0x0600D6C7 RID: 54983 RVA: 0x004D276C File Offset: 0x004D096C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.SandboxHelper.RefreshSandboxId();
			SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserSilently, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0));
		}

		// Token: 0x0600D6C8 RID: 54984 RVA: 0x004D278C File Offset: 0x004D098C
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

		// Token: 0x0600D6C9 RID: 54985 RVA: 0x004D2858 File Offset: 0x004D0A58
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

		// Token: 0x0600D6CA RID: 54986 RVA: 0x000200DA File Offset: 0x0001E2DA
		public Action IsConnectToServerFromCommandline()
		{
			return null;
		}

		// Token: 0x0600D6CB RID: 54987 RVA: 0x004D28B4 File Offset: 0x004D0AB4
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

		// Token: 0x0600D6CC RID: 54988 RVA: 0x004D292C File Offset: 0x004D0B2C
		[PublicizedFrom(EAccessModifier.Private)]
		public void RegisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedString += this.OnServerInfoChangedString;
			_serverInfo.OnChangedInt += this.OnServerInfoChangedInt;
		}

		// Token: 0x0600D6CD RID: 54989 RVA: 0x004D2952 File Offset: 0x004D0B52
		[PublicizedFrom(EAccessModifier.Private)]
		public void UnregisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedInt -= this.OnServerInfoChangedInt;
			_serverInfo.OnChangedString -= this.OnServerInfoChangedString;
		}

		// Token: 0x0600D6CE RID: 54990 RVA: 0x004D2978 File Offset: 0x004D0B78
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedString(GameServerInfo _serverInfo, GameInfoString _key)
		{
			if (_key == GameInfoString.UniqueId)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D6CF RID: 54991 RVA: 0x004D2986 File Offset: 0x004D0B86
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedInt(GameServerInfo _serverInfo, GameInfoInt _key)
		{
			if (_key == GameInfoInt.CurrentPlayers || _key == GameInfoInt.ServerVisibility || _key == GameInfoInt.MaxPlayers)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D6D0 RID: 54992 RVA: 0x004D299C File Offset: 0x004D0B9C
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

		// Token: 0x0600D6D1 RID: 54993 RVA: 0x004D2A00 File Offset: 0x004D0C00
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

		// Token: 0x0600D6D2 RID: 54994 RVA: 0x004D2A8C File Offset: 0x004D0C8C
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

		// Token: 0x0600D6D3 RID: 54995 RVA: 0x004D2ADC File Offset: 0x004D0CDC
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

		// Token: 0x0600D6D4 RID: 54996 RVA: 0x004D2BE4 File Offset: 0x004D0DE4
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

		// Token: 0x0600D6D5 RID: 54997 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600D6D6 RID: 54998 RVA: 0x004D2C50 File Offset: 0x004D0E50
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

		// Token: 0x0600D6D7 RID: 54999 RVA: 0x004D2C7B File Offset: 0x004D0E7B
		public bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return XblXuidMapper.GetXuid(_playerId) > 0UL;
		}

		// Token: 0x0600D6D8 RID: 55000 RVA: 0x004D2C88 File Offset: 0x004D0E88
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

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x0600D6D9 RID: 55001 RVA: 0x004D2CCC File Offset: 0x004D0ECC
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

		// Token: 0x0600D6DA RID: 55002 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600D6DB RID: 55003 RVA: 0x004D2D29 File Offset: 0x004D0F29
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

		// Token: 0x0600D6DC RID: 55004 RVA: 0x004D2D4D File Offset: 0x004D0F4D
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

		// Token: 0x0600D6DD RID: 55005 RVA: 0x004D2D60 File Offset: 0x004D0F60
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

		// Token: 0x0600D6DE RID: 55006 RVA: 0x004D2D78 File Offset: 0x004D0F78
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

		// Token: 0x0600D6E0 RID: 55008 RVA: 0x004D2E14 File Offset: 0x004D1014
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

		// Token: 0x0600D6E1 RID: 55009 RVA: 0x004D2E7A File Offset: 0x004D107A
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

		// Token: 0x0600D6E2 RID: 55010 RVA: 0x004D2EB4 File Offset: 0x004D10B4
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

		// Token: 0x0600D6E3 RID: 55011 RVA: 0x004D2F50 File Offset: 0x004D1150
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

		// Token: 0x0600D6E4 RID: 55012 RVA: 0x004D2FA6 File Offset: 0x004D11A6
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssue|77_4()
		{
			SDK.XUserResolveIssueWithUiUtf16Async(this.m_userHandle, null, new XUserResolveIssueWithUiUtf16Result(this.<OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5));
		}

		// Token: 0x0600D6E5 RID: 55013 RVA: 0x004D2FC1 File Offset: 0x004D11C1
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5(int hrResolve)
		{
			XblHelpers.LogHR(hrResolve, "XUserResolveIssueWithUiUtf16Async", false);
			this.<OnClientApiInitialized>g__GetXuid|77_3();
		}

		// Token: 0x0600D6E6 RID: 55014 RVA: 0x004D2FD8 File Offset: 0x004D11D8
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

		// Token: 0x0600D6E7 RID: 55015 RVA: 0x004D3064 File Offset: 0x004D1264
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

		// Token: 0x0600D6E8 RID: 55016 RVA: 0x004D310C File Offset: 0x004D130C
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

		// Token: 0x0600D6E9 RID: 55017 RVA: 0x004D31F0 File Offset: 0x004D13F0
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

		// Token: 0x0400A2F4 RID: 41716
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly EnumDictionary<XblPermission, EBlockType> xblPermissionToBlockType;

		// Token: 0x0400A2F5 RID: 41717
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblPermission[] userBlockedPermissions;

		// Token: 0x0400A2F6 RID: 41718
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblAnonymousUserType[] userBlockedAnonymousTypes;

		// Token: 0x0400A2F7 RID: 41719
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A2F8 RID: 41720
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformApi m_api;

		// Token: 0x0400A2F9 RID: 41721
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A2FA RID: 41722
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserHandle m_userHandle;

		// Token: 0x0400A2FB RID: 41723
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_userHandleLock = new object();

		// Token: 0x0400A2FC RID: 41724
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<XUserHandle> m_userHandleReady;

		// Token: 0x0400A2FD RID: 41725
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle m_contextHandle;

		// Token: 0x0400A2FE RID: 41726
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserLocalId m_userLocalId;

		// Token: 0x0400A2FF RID: 41727
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong m_userXuid;

		// Token: 0x0400A300 RID: 41728
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierXbl m_platformUserId;

		// Token: 0x0400A301 RID: 41729
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_loginLock = new object();

		// Token: 0x0400A302 RID: 41730
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_loginDone;

		// Token: 0x0400A303 RID: 41731
		[PublicizedFrom(EAccessModifier.Private)]
		public EUserStatus m_loginUserStatus;

		// Token: 0x0400A304 RID: 41732
		[PublicizedFrom(EAccessModifier.Private)]
		public LoginUserCallback m_loginUserCallback;

		// Token: 0x0400A305 RID: 41733
		[PublicizedFrom(EAccessModifier.Private)]
		public EApiStatusReason m_loginUserCallbackReason;

		// Token: 0x0400A306 RID: 41734
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_loginUserCallbackReasonAdditional;

		// Token: 0x0400A307 RID: 41735
		[PublicizedFrom(EAccessModifier.Private)]
		public UserPrivilegeHelper m_privilegeHelper;

		// Token: 0x0400A308 RID: 41736
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_activityLock = new object();

		// Token: 0x0400A309 RID: 41737
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo m_activityLastServerInfo;

		// Token: 0x0400A30A RID: 41738
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isActivityActive;

		// Token: 0x0400A30B RID: 41739
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldActivityBeActive;

		// Token: 0x0400A30C RID: 41740
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldRetryDeleteActivity;

		// Token: 0x0400A30D RID: 41741
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_currentlyDeletingActivity;

		// Token: 0x0400A30E RID: 41742
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, IPlatformUserBlockedResults> userBlockedXuidToResultsTemp = new Dictionary<ulong, IPlatformUserBlockedResults>();

		// Token: 0x0400A30F RID: 41743
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<IPlatformUserBlockedResults> userBlockedAnonymousResultsTemp = new List<IPlatformUserBlockedResults>();

		// Token: 0x0400A311 RID: 41745
		public string SandboxName;

		// Token: 0x0400A312 RID: 41746
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> m_userLoggedIn;

		// Token: 0x0400A314 RID: 41748
		[PublicizedFrom(EAccessModifier.Private)]
		public IdProviderGameCore m_idProvider;

		// Token: 0x0400A317 RID: 41751
		[PublicizedFrom(EAccessModifier.Private)]
		public FriendsListXbl friendsList;

		// Token: 0x0400A31A RID: 41754
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyUrl = "https://eos.epicgames.com";

		// Token: 0x0400A31B RID: 41755
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyHttpMethod = "GET";

		// Token: 0x0400A31C RID: 41756
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
