using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using InControl;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CA1 RID: 7329
	public class User : IUserClient
	{
		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x0600D957 RID: 55639 RVA: 0x004E0947 File Offset: 0x004DEB47
		// (set) Token: 0x0600D958 RID: 55640 RVA: 0x004E094F File Offset: 0x004DEB4F
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x0600D959 RID: 55641 RVA: 0x004E0958 File Offset: 0x004DEB58
		// (remove) Token: 0x0600D95A RID: 55642 RVA: 0x004E09B8 File Offset: 0x004DEBB8
		public event Action<IPlatform> UserLoggedIn
		{
			add
			{
				lock (this)
				{
					this.userLoggedIn = (Action<IPlatform>)Delegate.Combine(this.userLoggedIn, value);
					if (this.UserStatus == EUserStatus.LoggedIn)
					{
						value(this.owner);
					}
				}
			}
			remove
			{
				lock (this)
				{
					this.userLoggedIn = (Action<IPlatform>)Delegate.Remove(this.userLoggedIn, value);
				}
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x0600D95B RID: 55643 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600D95C RID: 55644 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x0600D95D RID: 55645 RVA: 0x004E0A04 File Offset: 0x004DEC04
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600D95E RID: 55646 RVA: 0x004E0A0C File Offset: 0x004DEC0C
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += delegate
			{
				if (!GameManager.IsDedicatedServer && this.m_gameOverlayActivated == null)
				{
					this.m_gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.GameOverlayActivated));
				}
			};
		}

		// Token: 0x0600D95F RID: 55647 RVA: 0x004E0A34 File Offset: 0x004DEC34
		public void Login(LoginUserCallback _delegate)
		{
			if (this.UserStatus == EUserStatus.LoggedIn)
			{
				Log.Out("[Steamworks.NET] Already logged in");
				_delegate(this.owner, EApiStatusReason.Ok, null);
				return;
			}
			if (this.owner.Api.ClientApiStatus == EApiStatus.PermanentError)
			{
				Log.Out("[Steamworks.NET] API could not be loaded.");
				this.UserStatus = EUserStatus.PermanentError;
				_delegate(this.owner, EApiStatusReason.ApiNotLoadable, null);
				return;
			}
			if (this.owner.Api.ClientApiStatus == EApiStatus.TemporaryError)
			{
				this.owner.Api.InitClientApis();
				if (this.owner.Api.ClientApiStatus == EApiStatus.TemporaryError)
				{
					Log.Out("[Steamworks.NET] API could not be initialized - probably Steam not running.");
					this.UserStatus = EUserStatus.TemporaryError;
					_delegate(this.owner, EApiStatusReason.SteamNotRunning, null);
					return;
				}
			}
			if (!SteamApps.BIsSubscribedApp((AppId_t)251570U))
			{
				Log.Out("[Steamworks.NET] User not licensed for app.");
				this.UserStatus = EUserStatus.PermanentError;
				_delegate(this.owner, EApiStatusReason.NoLicense, null);
				return;
			}
			string personaName = SteamFriends.GetPersonaName();
			if (string.IsNullOrEmpty(personaName))
			{
				Log.Out("[Steamworks.NET] Username not found.");
				this.UserStatus = EUserStatus.TemporaryError;
				_delegate(this.owner, EApiStatusReason.NoFriendsName, null);
				return;
			}
			GamePrefs.Set(EnumGamePrefs.PlayerName, personaName);
			this.platformUserId = new UserIdentifierSteam(SteamUser.GetSteamID());
			if (!SteamUser.BLoggedOn())
			{
				this.UserStatus = EUserStatus.OfflineMode;
				Log.Out("[Steamworks.NET] User not logged in.");
				_delegate(this.owner, EApiStatusReason.NotLoggedOn, null);
				return;
			}
			Log.Out("[Steamworks.NET] Login ok.");
			this.UserStatus = EUserStatus.LoggedIn;
			Action<IPlatform> action = this.userLoggedIn;
			if (action != null)
			{
				action(this.owner);
			}
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600D960 RID: 55648 RVA: 0x004E0BC4 File Offset: 0x004DEDC4
		public void PlayOffline(LoginUserCallback _delegate)
		{
			if (this.UserStatus != EUserStatus.OfflineMode && this.UserStatus != EUserStatus.LoggedIn)
			{
				throw new Exception("Can not explicitly set Steam to offline mode");
			}
			this.UserStatus = EUserStatus.OfflineMode;
			Action<IPlatform> action = this.userLoggedIn;
			if (action != null)
			{
				action(this.owner);
			}
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600D961 RID: 55649 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600D962 RID: 55650 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600D963 RID: 55651 RVA: 0x004E0C1C File Offset: 0x004DEE1C
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			if (this.requestEncryptedAppTicketCallback == null)
			{
				this.requestEncryptedAppTicketCallback = CallResult<EncryptedAppTicketResponse_t>.Create(new CallResult<EncryptedAppTicketResponse_t>.APIDispatchDelegate(this.EncryptedAppTicketCallback));
			}
			this.encryptedAppTicketCallback = _callback;
			SteamAPICall_t steamAPICall_t = SteamUser.RequestEncryptedAppTicket(null, 0);
			this.requestEncryptedAppTicketCallback.Set(steamAPICall_t, null);
		}

		// Token: 0x0600D964 RID: 55652 RVA: 0x004E0C64 File Offset: 0x004DEE64
		[PublicizedFrom(EAccessModifier.Private)]
		public void EncryptedAppTicketCallback(EncryptedAppTicketResponse_t _result, bool _ioFailure)
		{
			if (_ioFailure || _result.m_eResult != EResult.k_EResultOK)
			{
				this.<EncryptedAppTicketCallback>g__Callback|24_0(null, "[Steamworks.NET] RequestEncryptedAppTicket failed (result=" + _result.m_eResult.ToStringCached<EResult>() + ")");
				return;
			}
			uint num;
			SteamUser.GetEncryptedAppTicket(null, 0, out num);
			if (num == 0U || num > 1024U)
			{
				this.<EncryptedAppTicketCallback>g__Callback|24_0(null, string.Format("[Steamworks.NET] Fetching encrypted app ticket size: {0}", num));
				return;
			}
			byte[] array = new byte[num];
			uint num2;
			if (!SteamUser.GetEncryptedAppTicket(array, (int)num, out num2))
			{
				this.<EncryptedAppTicketCallback>g__Callback|24_0(null, "[Steamworks.NET] Failed fetching encrypted app ticket");
				return;
			}
			if (num2 != num)
			{
				this.<EncryptedAppTicketCallback>g__Callback|24_0(null, string.Format("[Steamworks.NET] Ticket size expected {0} does not match ticket size received {1}", num, num2));
				return;
			}
			this.<EncryptedAppTicketCallback>g__Callback|24_0(array, null);
		}

		// Token: 0x0600D965 RID: 55653 RVA: 0x004E0D18 File Offset: 0x004DEF18
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			UserIdentifierSteam userIdentifierSteam = _playerId as UserIdentifierSteam;
			if (userIdentifierSteam == null)
			{
				return null;
			}
			return SteamFriends.GetFriendPersonaName(new CSteamID(userIdentifierSteam.SteamId));
		}

		// Token: 0x0600D966 RID: 55654 RVA: 0x004E0D44 File Offset: 0x004DEF44
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			UserIdentifierSteam userIdentifierSteam = _playerId as UserIdentifierSteam;
			return userIdentifierSteam != null && this.owner.Api.ClientApiStatus == EApiStatus.Ok && SteamFriends.GetFriendRelationship(new CSteamID(userIdentifierSteam.SteamId)) == EFriendRelationship.k_EFriendRelationshipFriend;
		}

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x0600D967 RID: 55655 RVA: 0x001B24E4 File Offset: 0x001B06E4
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600D968 RID: 55656 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600D969 RID: 55657 RVA: 0x004E0D84 File Offset: 0x004DEF84
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600D96A RID: 55658 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600D96B RID: 55659 RVA: 0x004E0D84 File Offset: 0x004DEF84
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600D96C RID: 55660 RVA: 0x0002F168 File Offset: 0x0002D368
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Retail;
		}

		// Token: 0x0600D96D RID: 55661 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D96E RID: 55662 RVA: 0x004E0D90 File Offset: 0x004DEF90
		[PublicizedFrom(EAccessModifier.Private)]
		public void GameOverlayActivated(GameOverlayActivated_t _val)
		{
			InputManager.Enabled = _val.m_bActive == 0;
		}

		// Token: 0x0600D971 RID: 55665 RVA: 0x004E0DD7 File Offset: 0x004DEFD7
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <EncryptedAppTicketCallback>g__Callback|24_0(byte[] _ticket, string _message)
		{
			if (_message != null)
			{
				Log.Error(_message);
			}
			Action<bool, byte[], string> action = this.encryptedAppTicketCallback;
			if (action != null)
			{
				action(_message == null, _ticket, null);
			}
			this.encryptedAppTicketCallback = null;
		}

		// Token: 0x0400A4EC RID: 42220
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A4EE RID: 42222
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A4EF RID: 42223
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameOverlayActivated_t> m_gameOverlayActivated;

		// Token: 0x0400A4F0 RID: 42224
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierSteam platformUserId;

		// Token: 0x0400A4F1 RID: 42225
		[PublicizedFrom(EAccessModifier.Private)]
		public CallResult<EncryptedAppTicketResponse_t> requestEncryptedAppTicketCallback;

		// Token: 0x0400A4F2 RID: 42226
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<bool, byte[], string> encryptedAppTicketCallback;
	}
}
