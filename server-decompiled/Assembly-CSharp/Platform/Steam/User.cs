using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using InControl;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001D04 RID: 7428
	public class User : IUserClient
	{
		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x0600DBE5 RID: 56293 RVA: 0x004E902F File Offset: 0x004E722F
		// (set) Token: 0x0600DBE6 RID: 56294 RVA: 0x004E9037 File Offset: 0x004E7237
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x0600DBE7 RID: 56295 RVA: 0x004E9040 File Offset: 0x004E7240
		// (remove) Token: 0x0600DBE8 RID: 56296 RVA: 0x004E90A0 File Offset: 0x004E72A0
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
		// (add) Token: 0x0600DBE9 RID: 56297 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DBEA RID: 56298 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x0600DBEB RID: 56299 RVA: 0x004E90EC File Offset: 0x004E72EC
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600DBEC RID: 56300 RVA: 0x004E90F4 File Offset: 0x004E72F4
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

		// Token: 0x0600DBED RID: 56301 RVA: 0x004E911C File Offset: 0x004E731C
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

		// Token: 0x0600DBEE RID: 56302 RVA: 0x004E92AC File Offset: 0x004E74AC
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

		// Token: 0x0600DBEF RID: 56303 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DBF0 RID: 56304 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DBF1 RID: 56305 RVA: 0x004E9304 File Offset: 0x004E7504
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

		// Token: 0x0600DBF2 RID: 56306 RVA: 0x004E934C File Offset: 0x004E754C
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

		// Token: 0x0600DBF3 RID: 56307 RVA: 0x004E9400 File Offset: 0x004E7600
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			UserIdentifierSteam userIdentifierSteam = _playerId as UserIdentifierSteam;
			if (userIdentifierSteam == null)
			{
				return null;
			}
			return SteamFriends.GetFriendPersonaName(new CSteamID(userIdentifierSteam.SteamId));
		}

		// Token: 0x0600DBF4 RID: 56308 RVA: 0x004E942C File Offset: 0x004E762C
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			UserIdentifierSteam userIdentifierSteam = _playerId as UserIdentifierSteam;
			return userIdentifierSteam != null && this.owner.Api.ClientApiStatus == EApiStatus.Ok && SteamFriends.GetFriendRelationship(new CSteamID(userIdentifierSteam.SteamId)) == EFriendRelationship.k_EFriendRelationshipFriend;
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x0600DBF5 RID: 56309 RVA: 0x001B39BC File Offset: 0x001B1BBC
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DBF6 RID: 56310 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DBF7 RID: 56311 RVA: 0x004E946C File Offset: 0x004E766C
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DBF8 RID: 56312 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600DBF9 RID: 56313 RVA: 0x004E946C File Offset: 0x004E766C
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DBFA RID: 56314 RVA: 0x0002F260 File Offset: 0x0002D460
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Retail;
		}

		// Token: 0x0600DBFB RID: 56315 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DBFC RID: 56316 RVA: 0x004E9478 File Offset: 0x004E7678
		[PublicizedFrom(EAccessModifier.Private)]
		public void GameOverlayActivated(GameOverlayActivated_t _val)
		{
			InputManager.Enabled = _val.m_bActive == 0;
		}

		// Token: 0x0600DBFF RID: 56319 RVA: 0x004E94BF File Offset: 0x004E76BF
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

		// Token: 0x0400A6AA RID: 42666
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A6AC RID: 42668
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A6AD RID: 42669
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameOverlayActivated_t> m_gameOverlayActivated;

		// Token: 0x0400A6AE RID: 42670
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierSteam platformUserId;

		// Token: 0x0400A6AF RID: 42671
		[PublicizedFrom(EAccessModifier.Private)]
		public CallResult<EncryptedAppTicketResponse_t> requestEncryptedAppTicketCallback;

		// Token: 0x0400A6B0 RID: 42672
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<bool, byte[], string> encryptedAppTicketCallback;
	}
}
