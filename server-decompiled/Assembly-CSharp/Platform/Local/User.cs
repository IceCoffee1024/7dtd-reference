using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Local
{
	// Token: 0x02001D2D RID: 7469
	public class User : IUserClient
	{
		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x0600DD5F RID: 56671 RVA: 0x004EC420 File Offset: 0x004EA620
		// (set) Token: 0x0600DD60 RID: 56672 RVA: 0x004EC428 File Offset: 0x004EA628
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x0600DD61 RID: 56673 RVA: 0x004EC434 File Offset: 0x004EA634
		// (remove) Token: 0x0600DD62 RID: 56674 RVA: 0x000027FC File Offset: 0x000009FC
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
		// (add) Token: 0x0600DD63 RID: 56675 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DD64 RID: 56676 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x0600DD65 RID: 56677 RVA: 0x004EC478 File Offset: 0x004EA678
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

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x0600DD66 RID: 56678 RVA: 0x004EC492 File Offset: 0x004EA692
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.platformUserId;
			}
		}

		// Token: 0x0600DD67 RID: 56679 RVA: 0x004EC49A File Offset: 0x004EA69A
		public void Login(LoginUserCallback _delegate)
		{
			this.platformUserId = new UserIdentifierLocal(GamePrefs.GetString(EnumGamePrefs.PlayerName));
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600DD68 RID: 56680 RVA: 0x004EC4BD File Offset: 0x004EA6BD
		public void PlayOffline(LoginUserCallback _delegate)
		{
			this.UserStatus = EUserStatus.OfflineMode;
			_delegate(this.owner, EApiStatusReason.Ok, null);
		}

		// Token: 0x0600DD69 RID: 56681 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DD6A RID: 56682 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DD6B RID: 56683 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD6C RID: 56684 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			return null;
		}

		// Token: 0x0600DD6D RID: 56685 RVA: 0x00020119 File Offset: 0x0001E319
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return true;
		}

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x0600DD6E RID: 56686 RVA: 0x001B39BC File Offset: 0x001B1BBC
		public EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DD6F RID: 56687 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DD70 RID: 56688 RVA: 0x004E946C File Offset: 0x004E766C
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DD71 RID: 56689 RVA: 0x000027FC File Offset: 0x000009FC
		public void UserAdded(PlatformUserIdentifierAbs _userId, bool _isPrimary)
		{
		}

		// Token: 0x0600DD72 RID: 56690 RVA: 0x004E946C File Offset: 0x004E766C
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DD73 RID: 56691 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A736 RID: 42806
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A737 RID: 42807
		[PublicizedFrom(EAccessModifier.Private)]
		public UserIdentifierLocal platformUserId;
	}
}
