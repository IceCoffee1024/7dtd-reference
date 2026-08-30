using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Local
{
	// Token: 0x02001CA3 RID: 7331
	public class User : IUserClient
	{
		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600D935 RID: 55605 RVA: 0x004DD724 File Offset: 0x004DB924
		// (set) Token: 0x0600D936 RID: 55606 RVA: 0x004DD72C File Offset: 0x004DB92C
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x0600D937 RID: 55607 RVA: 0x004DD738 File Offset: 0x004DB938
		// (remove) Token: 0x0600D938 RID: 55608 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x0600D939 RID: 55609 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600D93A RID: 55610 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x0600D93B RID: 55611 RVA: 0x004DD77C File Offset: 0x004DB97C
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

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x0600D93C RID: 55612 RVA: 0x004DD796 File Offset: 0x004DB996
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600D93D RID: 55613 RVA: 0x004DD79E File Offset: 0x004DB99E
		public void Login(LoginUserCallback _delegate)
		{
			this.platformUserId = new UserIdentifierLocal(GamePrefs.GetString(EnumGamePrefs.PlayerName));
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600D93E RID: 55614 RVA: 0x004DD7C1 File Offset: 0x004DB9C1
		public void PlayOffline(LoginUserCallback _delegate)
		{
			this.UserStatus = EUserStatus.OfflineMode;
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600D93F RID: 55615 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600D940 RID: 55616 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600D941 RID: 55617 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D942 RID: 55618 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600D943 RID: 55619 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return true;
		}

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x0600D944 RID: 55620 RVA: 0x001AFF0C File Offset: 0x001AE10C
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600D945 RID: 55621 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600D946 RID: 55622 RVA: 0x004DA868 File Offset: 0x004D8A68
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600D947 RID: 55623 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600D948 RID: 55624 RVA: 0x004DA868 File Offset: 0x004D8A68
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600D949 RID: 55625 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A445 RID: 42053
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A446 RID: 42054
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierLocal platformUserId;
	}
}
