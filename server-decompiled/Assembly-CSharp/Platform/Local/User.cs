using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Local
{
	// Token: 0x02001CFC RID: 7420
	public class User : IUserClient
	{
		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x0600DC4B RID: 56395 RVA: 0x004E8880 File Offset: 0x004E6A80
		// (set) Token: 0x0600DC4C RID: 56396 RVA: 0x004E8888 File Offset: 0x004E6A88
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x0600DC4D RID: 56397 RVA: 0x004E8894 File Offset: 0x004E6A94
		// (remove) Token: 0x0600DC4E RID: 56398 RVA: 0x000027FC File Offset: 0x000009FC
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
		// (add) Token: 0x0600DC4F RID: 56399 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DC50 RID: 56400 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x0600DC51 RID: 56401 RVA: 0x004E88D8 File Offset: 0x004E6AD8
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

		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x0600DC52 RID: 56402 RVA: 0x004E88F2 File Offset: 0x004E6AF2
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600DC53 RID: 56403 RVA: 0x004E88FA File Offset: 0x004E6AFA
		public void Login(LoginUserCallback _delegate)
		{
			this.platformUserId = new UserIdentifierLocal(GamePrefs.GetString(EnumGamePrefs.PlayerName));
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600DC54 RID: 56404 RVA: 0x004E891D File Offset: 0x004E6B1D
		public void PlayOffline(LoginUserCallback _delegate)
		{
			this.UserStatus = EUserStatus.OfflineMode;
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600DC55 RID: 56405 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DC56 RID: 56406 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DC57 RID: 56407 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DC58 RID: 56408 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600DC59 RID: 56409 RVA: 0x00020119 File Offset: 0x0001E319
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return true;
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x0600DC5A RID: 56410 RVA: 0x001B2938 File Offset: 0x001B0B38
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DC5B RID: 56411 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DC5C RID: 56412 RVA: 0x004E58CC File Offset: 0x004E3ACC
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DC5D RID: 56413 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600DC5E RID: 56414 RVA: 0x004E58CC File Offset: 0x004E3ACC
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DC5F RID: 56415 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A64D RID: 42573
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A64E RID: 42574
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierLocal platformUserId;
	}
}
