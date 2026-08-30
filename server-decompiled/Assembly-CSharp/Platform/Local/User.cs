using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Local
{
	// Token: 0x02001CCA RID: 7370
	public class User : IUserClient
	{
		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x0600DAD0 RID: 56016 RVA: 0x004E3D38 File Offset: 0x004E1F38
		// (set) Token: 0x0600DAD1 RID: 56017 RVA: 0x004E3D40 File Offset: 0x004E1F40
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x0600DAD2 RID: 56018 RVA: 0x004E3D4C File Offset: 0x004E1F4C
		// (remove) Token: 0x0600DAD3 RID: 56019 RVA: 0x000027FC File Offset: 0x000009FC
		public event Action<IPlatform> UserLoggedIn
		{
			add
			{
				lock (this)
				{
					value(this.owner);
				}
			}
			remove
			{
			}
		}

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x0600DAD4 RID: 56020 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DAD5 RID: 56021 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x0600DAD6 RID: 56022 RVA: 0x004E3D90 File Offset: 0x004E1F90
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			GamePrefs.OnGamePrefChanged += delegate(EnumGamePrefs _pref)
			{
				if (_pref == EnumGamePrefs.PlayerName)
				{
					this.platformUserId = new UserIdentifierLocal(GamePrefs.GetString(EnumGamePrefs.PlayerName));
				}
			};
		}

		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x0600DAD7 RID: 56023 RVA: 0x004E3DAA File Offset: 0x004E1FAA
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600DAD8 RID: 56024 RVA: 0x004E3DB2 File Offset: 0x004E1FB2
		public void Login(LoginUserCallback _delegate)
		{
			this.platformUserId = new UserIdentifierLocal(GamePrefs.GetString(EnumGamePrefs.PlayerName));
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600DAD9 RID: 56025 RVA: 0x004E3DD5 File Offset: 0x004E1FD5
		public void PlayOffline(LoginUserCallback _delegate)
		{
			this.UserStatus = EUserStatus.OfflineMode;
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600DADA RID: 56026 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DADB RID: 56027 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DADC RID: 56028 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DADD RID: 56029 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600DADE RID: 56030 RVA: 0x00020021 File Offset: 0x0001E221
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return true;
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x0600DADF RID: 56031 RVA: 0x001B24E4 File Offset: 0x001B06E4
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DAE0 RID: 56032 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DAE1 RID: 56033 RVA: 0x004E0D84 File Offset: 0x004DEF84
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DAE2 RID: 56034 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600DAE3 RID: 56035 RVA: 0x004E0D84 File Offset: 0x004DEF84
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DAE4 RID: 56036 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A578 RID: 42360
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A579 RID: 42361
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierLocal platformUserId;
	}
}
