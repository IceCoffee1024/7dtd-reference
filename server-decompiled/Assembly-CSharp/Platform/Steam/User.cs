using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using InControl;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CD3 RID: 7379
	public class User : IUserClient
	{
		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x0600DAD1 RID: 56017 RVA: 0x004E548F File Offset: 0x004E368F
		// (set) Token: 0x0600DAD2 RID: 56018 RVA: 0x004E5497 File Offset: 0x004E3697
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x0600DAD3 RID: 56019 RVA: 0x004E54A0 File Offset: 0x004E36A0
		// (remove) Token: 0x0600DAD4 RID: 56020 RVA: 0x004E5500 File Offset: 0x004E3700
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

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x0600DAD5 RID: 56021 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DAD6 RID: 56022 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x0600DAD7 RID: 56023 RVA: 0x004E554C File Offset: 0x004E374C
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600DAD8 RID: 56024 RVA: 0x004E5554 File Offset: 0x004E3754
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

		// Token: 0x0600DAD9 RID: 56025 RVA: 0x004E557C File Offset: 0x004E377C
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

		// Token: 0x0600DADA RID: 56026 RVA: 0x004E570C File Offset: 0x004E390C
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

		// Token: 0x0600DADB RID: 56027 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DADC RID: 56028 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DADD RID: 56029 RVA: 0x004E5764 File Offset: 0x004E3964
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

		// Token: 0x0600DADE RID: 56030 RVA: 0x004E57AC File Offset: 0x004E39AC
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

		// Token: 0x0600DADF RID: 56031 RVA: 0x004E5860 File Offset: 0x004E3A60
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			UserIdentifierSteam userIdentifierSteam = _playerId as UserIdentifierSteam;
			if (userIdentifierSteam == null)
			{
				return null;
			}
			return SteamFriends.GetFriendPersonaName(new CSteamID(userIdentifierSteam.SteamId));
		}

		// Token: 0x0600DAE0 RID: 56032 RVA: 0x004E588C File Offset: 0x004E3A8C
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			UserIdentifierSteam userIdentifierSteam = _playerId as UserIdentifierSteam;
			return userIdentifierSteam != null && this.owner.Api.ClientApiStatus == EApiStatus.Ok && SteamFriends.GetFriendRelationship(new CSteamID(userIdentifierSteam.SteamId)) == EFriendRelationship.k_EFriendRelationshipFriend;
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x0600DAE1 RID: 56033 RVA: 0x001B2938 File Offset: 0x001B0B38
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DAE2 RID: 56034 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DAE3 RID: 56035 RVA: 0x004E58CC File Offset: 0x004E3ACC
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DAE4 RID: 56036 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600DAE5 RID: 56037 RVA: 0x004E58CC File Offset: 0x004E3ACC
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DAE6 RID: 56038 RVA: 0x0002F260 File Offset: 0x0002D460
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Retail;
		}

		// Token: 0x0600DAE7 RID: 56039 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DAE8 RID: 56040 RVA: 0x004E58D8 File Offset: 0x004E3AD8
		[PublicizedFrom(EAccessModifier.Private)]
		public void GameOverlayActivated(GameOverlayActivated_t _val)
		{
			InputManager.Enabled = _val.m_bActive == 0;
		}

		// Token: 0x0600DAEB RID: 56043 RVA: 0x004E591F File Offset: 0x004E3B1F
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

		// Token: 0x0400A5C1 RID: 42433
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5C3 RID: 42435
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A5C4 RID: 42436
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameOverlayActivated_t> m_gameOverlayActivated;

		// Token: 0x0400A5C5 RID: 42437
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierSteam platformUserId;

		// Token: 0x0400A5C6 RID: 42438
		[PublicizedFrom(EAccessModifier.Private)]
		public CallResult<EncryptedAppTicketResponse_t> requestEncryptedAppTicketCallback;

		// Token: 0x0400A5C7 RID: 42439
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<bool, byte[], string> encryptedAppTicketCallback;
	}
}
