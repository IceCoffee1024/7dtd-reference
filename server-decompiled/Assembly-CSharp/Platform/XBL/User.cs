using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C09 RID: 7177
	public class User : IUserClient
	{
		// Token: 0x0600D535 RID: 54581 RVA: 0x004CD79C File Offset: 0x004CB99C
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

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x0600D536 RID: 54582 RVA: 0x004CD880 File Offset: 0x004CBA80
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

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600D537 RID: 54583 RVA: 0x004CD8C4 File Offset: 0x004CBAC4
		public XUserLocalId LocalID
		{
			get
			{
				return this.m_userLocalId;
			}
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x0600D538 RID: 54584 RVA: 0x004CD8CC File Offset: 0x004CBACC
		public ulong Xuid
		{
			get
			{
				return this.m_userXuid;
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x0600D539 RID: 54585 RVA: 0x004CD8D4 File Offset: 0x004CBAD4
		public Unity.XGamingRuntime.XblContextHandle XblContextHandle
		{
			get
			{
				return this.m_contextHandle;
			}
		}

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x0600D53A RID: 54586 RVA: 0x004CD8DC File Offset: 0x004CBADC
		public XUserHandle UserHandle
		{
			get
			{
				return this.m_userHandle;
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x0600D53B RID: 54587 RVA: 0x004CD8E4 File Offset: 0x004CBAE4
		// (set) Token: 0x0600D53C RID: 54588 RVA: 0x004CD8EC File Offset: 0x004CBAEC
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x1400012C RID: 300
		// (add) Token: 0x0600D53D RID: 54589 RVA: 0x004CD8F8 File Offset: 0x004CBAF8
		// (remove) Token: 0x0600D53E RID: 54590 RVA: 0x004CD964 File Offset: 0x004CBB64
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

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x0600D53F RID: 54591 RVA: 0x004CD9B8 File Offset: 0x004CBBB8
		// (remove) Token: 0x0600D540 RID: 54592 RVA: 0x004CDA20 File Offset: 0x004CBC20
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

		// Token: 0x1400012E RID: 302
		// (add) Token: 0x0600D541 RID: 54593 RVA: 0x004CDA74 File Offset: 0x004CBC74
		// (remove) Token: 0x0600D542 RID: 54594 RVA: 0x004CDAAC File Offset: 0x004CBCAC
		public event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x0600D543 RID: 54595 RVA: 0x004CDAE1 File Offset: 0x004CBCE1
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.m_idProvider.Id;
			}
		}

		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x0600D544 RID: 54596 RVA: 0x004CDAEE File Offset: 0x004CBCEE
		// (set) Token: 0x0600D545 RID: 54597 RVA: 0x004CDAF6 File Offset: 0x004CBCF6
		public string GamerTag
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x0600D546 RID: 54598 RVA: 0x004CDAFF File Offset: 0x004CBCFF
		// (set) Token: 0x0600D547 RID: 54599 RVA: 0x004CDB07 File Offset: 0x004CBD07
		public SocialManagerXbl SocialManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x0600D548 RID: 54600 RVA: 0x004CDB10 File Offset: 0x004CBD10
		// (set) Token: 0x0600D549 RID: 54601 RVA: 0x004CDB18 File Offset: 0x004CBD18
		public MultiplayerActivityQueryManager MultiplayerActivityQueryManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x0600D54A RID: 54602 RVA: 0x004CDB21 File Offset: 0x004CBD21
		// (set) Token: 0x0600D54B RID: 54603 RVA: 0x004CDB29 File Offset: 0x004CBD29
		public XblSandboxHelper SandboxHelper
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new XblSandboxHelper();

		// Token: 0x0600D54C RID: 54604 RVA: 0x004CDB34 File Offset: 0x004CBD34
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

		// Token: 0x0600D54D RID: 54605 RVA: 0x004CDBD0 File Offset: 0x004CBDD0
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

		// Token: 0x0600D54E RID: 54606 RVA: 0x004CDC48 File Offset: 0x004CBE48
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

		// Token: 0x0600D54F RID: 54607 RVA: 0x004CDC84 File Offset: 0x004CBE84
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.SandboxHelper.RefreshSandboxId();
			SDK.XUserAddAsync(XUserAddOptions.AddDefaultUserSilently, new XUserAddCompleted(this.<OnClientApiInitialized>g__XUserAddAsyncSilentCompletionRoutine|77_0));
		}

		// Token: 0x0600D550 RID: 54608 RVA: 0x004CDCA4 File Offset: 0x004CBEA4
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

		// Token: 0x0600D551 RID: 54609 RVA: 0x004CDD70 File Offset: 0x004CBF70
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

		// Token: 0x0600D552 RID: 54610 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public Action IsConnectToServerFromCommandline()
		{
			return null;
		}

		// Token: 0x0600D553 RID: 54611 RVA: 0x004CDDCC File Offset: 0x004CBFCC
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

		// Token: 0x0600D554 RID: 54612 RVA: 0x004CDE44 File Offset: 0x004CC044
		[PublicizedFrom(EAccessModifier.Private)]
		public void RegisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedString += this.OnServerInfoChangedString;
			_serverInfo.OnChangedInt += this.OnServerInfoChangedInt;
		}

		// Token: 0x0600D555 RID: 54613 RVA: 0x004CDE6A File Offset: 0x004CC06A
		[PublicizedFrom(EAccessModifier.Private)]
		public void UnregisterGameServerInfoEvents(GameServerInfo _serverInfo)
		{
			_serverInfo.OnChangedInt -= this.OnServerInfoChangedInt;
			_serverInfo.OnChangedString -= this.OnServerInfoChangedString;
		}

		// Token: 0x0600D556 RID: 54614 RVA: 0x004CDE90 File Offset: 0x004CC090
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedString(GameServerInfo _serverInfo, GameInfoString _key)
		{
			if (_key == GameInfoString.UniqueId)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D557 RID: 54615 RVA: 0x004CDE9E File Offset: 0x004CC09E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerInfoChangedInt(GameServerInfo _serverInfo, GameInfoInt _key)
		{
			if (_key == GameInfoInt.CurrentPlayers || _key == GameInfoInt.ServerVisibility || _key == GameInfoInt.MaxPlayers)
			{
				this.SetActivity(_serverInfo);
			}
		}

		// Token: 0x0600D558 RID: 54616 RVA: 0x004CDEB4 File Offset: 0x004CC0B4
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

		// Token: 0x0600D559 RID: 54617 RVA: 0x004CDF18 File Offset: 0x004CC118
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

		// Token: 0x0600D55A RID: 54618 RVA: 0x004CDFA4 File Offset: 0x004CC1A4
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

		// Token: 0x0600D55B RID: 54619 RVA: 0x004CDFF4 File Offset: 0x004CC1F4
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

		// Token: 0x0600D55C RID: 54620 RVA: 0x004CE0FC File Offset: 0x004CC2FC
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

		// Token: 0x0600D55D RID: 54621 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600D55E RID: 54622 RVA: 0x004CE168 File Offset: 0x004CC368
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

		// Token: 0x0600D55F RID: 54623 RVA: 0x004CE193 File Offset: 0x004CC393
		public bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return XblXuidMapper.GetXuid(_playerId) > 0UL;
		}

		// Token: 0x0600D560 RID: 54624 RVA: 0x004CE1A0 File Offset: 0x004CC3A0
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

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x0600D561 RID: 54625 RVA: 0x004CE1E4 File Offset: 0x004CC3E4
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

		// Token: 0x0600D562 RID: 54626 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600D563 RID: 54627 RVA: 0x004CE241 File Offset: 0x004CC441
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

		// Token: 0x0600D564 RID: 54628 RVA: 0x004CE265 File Offset: 0x004CC465
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

		// Token: 0x0600D565 RID: 54629 RVA: 0x004CE278 File Offset: 0x004CC478
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

		// Token: 0x0600D566 RID: 54630 RVA: 0x004CE290 File Offset: 0x004CC490
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

		// Token: 0x0600D568 RID: 54632 RVA: 0x004CE32C File Offset: 0x004CC52C
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

		// Token: 0x0600D569 RID: 54633 RVA: 0x004CE392 File Offset: 0x004CC592
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

		// Token: 0x0600D56A RID: 54634 RVA: 0x004CE3CC File Offset: 0x004CC5CC
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

		// Token: 0x0600D56B RID: 54635 RVA: 0x004CE468 File Offset: 0x004CC668
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

		// Token: 0x0600D56C RID: 54636 RVA: 0x004CE4BE File Offset: 0x004CC6BE
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssue|77_4()
		{
			SDK.XUserResolveIssueWithUiUtf16Async(this.m_userHandle, null, new XUserResolveIssueWithUiUtf16Result(this.<OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5));
		}

		// Token: 0x0600D56D RID: 54637 RVA: 0x004CE4D9 File Offset: 0x004CC6D9
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <OnClientApiInitialized>g__ResolveXuidIssueCompletionRoutine|77_5(int hrResolve)
		{
			XblHelpers.LogHR(hrResolve, "XUserResolveIssueWithUiUtf16Async", false);
			this.<OnClientApiInitialized>g__GetXuid|77_3();
		}

		// Token: 0x0600D56E RID: 54638 RVA: 0x004CE4F0 File Offset: 0x004CC6F0
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

		// Token: 0x0600D56F RID: 54639 RVA: 0x004CE57C File Offset: 0x004CC77C
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

		// Token: 0x0600D570 RID: 54640 RVA: 0x004CE624 File Offset: 0x004CC824
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

		// Token: 0x0600D571 RID: 54641 RVA: 0x004CE708 File Offset: 0x004CC908
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

		// Token: 0x0400A21F RID: 41503
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly EnumDictionary<XblPermission, EBlockType> xblPermissionToBlockType;

		// Token: 0x0400A220 RID: 41504
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblPermission[] userBlockedPermissions;

		// Token: 0x0400A221 RID: 41505
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblAnonymousUserType[] userBlockedAnonymousTypes;

		// Token: 0x0400A222 RID: 41506
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A223 RID: 41507
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformApi m_api;

		// Token: 0x0400A224 RID: 41508
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A225 RID: 41509
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserHandle m_userHandle;

		// Token: 0x0400A226 RID: 41510
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_userHandleLock = new object();

		// Token: 0x0400A227 RID: 41511
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<XUserHandle> m_userHandleReady;

		// Token: 0x0400A228 RID: 41512
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle m_contextHandle;

		// Token: 0x0400A229 RID: 41513
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserLocalId m_userLocalId;

		// Token: 0x0400A22A RID: 41514
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong m_userXuid;

		// Token: 0x0400A22B RID: 41515
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierXbl m_platformUserId;

		// Token: 0x0400A22C RID: 41516
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_loginLock = new object();

		// Token: 0x0400A22D RID: 41517
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_loginDone;

		// Token: 0x0400A22E RID: 41518
		[PublicizedFrom(EAccessModifier.Private)]
		public EUserStatus m_loginUserStatus;

		// Token: 0x0400A22F RID: 41519
		[PublicizedFrom(EAccessModifier.Private)]
		public LoginUserCallback m_loginUserCallback;

		// Token: 0x0400A230 RID: 41520
		[PublicizedFrom(EAccessModifier.Private)]
		public EApiStatusReason m_loginUserCallbackReason;

		// Token: 0x0400A231 RID: 41521
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_loginUserCallbackReasonAdditional;

		// Token: 0x0400A232 RID: 41522
		[PublicizedFrom(EAccessModifier.Private)]
		public UserPrivilegeHelper m_privilegeHelper;

		// Token: 0x0400A233 RID: 41523
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_activityLock = new object();

		// Token: 0x0400A234 RID: 41524
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo m_activityLastServerInfo;

		// Token: 0x0400A235 RID: 41525
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isActivityActive;

		// Token: 0x0400A236 RID: 41526
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldActivityBeActive;

		// Token: 0x0400A237 RID: 41527
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_shouldRetryDeleteActivity;

		// Token: 0x0400A238 RID: 41528
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_currentlyDeletingActivity;

		// Token: 0x0400A239 RID: 41529
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, IPlatformUserBlockedResults> userBlockedXuidToResultsTemp = new Dictionary<ulong, IPlatformUserBlockedResults>();

		// Token: 0x0400A23A RID: 41530
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<IPlatformUserBlockedResults> userBlockedAnonymousResultsTemp = new List<IPlatformUserBlockedResults>();

		// Token: 0x0400A23C RID: 41532
		public string SandboxName;

		// Token: 0x0400A23D RID: 41533
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> m_userLoggedIn;

		// Token: 0x0400A23F RID: 41535
		[PublicizedFrom(EAccessModifier.Private)]
		public IdProviderGameCore m_idProvider;

		// Token: 0x0400A242 RID: 41538
		[PublicizedFrom(EAccessModifier.Private)]
		public FriendsListXbl friendsList;

		// Token: 0x0400A245 RID: 41541
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyUrl = "https://eos.epicgames.com";

		// Token: 0x0400A246 RID: 41542
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosRelyingPartyHttpMethod = "GET";

		// Token: 0x0400A247 RID: 41543
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
