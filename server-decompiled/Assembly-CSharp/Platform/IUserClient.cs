using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001C0B RID: 7179
	public interface IUserClient
	{
		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x0600D5D3 RID: 54739
		EUserStatus UserStatus { get; }

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x0600D5D4 RID: 54740
		// (remove) Token: 0x0600D5D5 RID: 54741
		event Action<IPlatform> UserLoggedIn;

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x0600D5D6 RID: 54742
		// (remove) Token: 0x0600D5D7 RID: 54743
		event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x0600D5D8 RID: 54744
		PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x0600D5D9 RID: 54745
		void Init(IPlatform _owner);

		// Token: 0x0600D5DA RID: 54746
		void Login(LoginUserCallback _delegate);

		// Token: 0x0600D5DB RID: 54747
		void PlayOffline(LoginUserCallback _delegate);

		// Token: 0x0600D5DC RID: 54748 RVA: 0x00010E62 File Offset: 0x0000F062
		EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Dev;
		}

		// Token: 0x0600D5DD RID: 54749
		void StartAdvertisePlaying(GameServerInfo _serverInfo);

		// Token: 0x0600D5DE RID: 54750
		void StopAdvertisePlaying();

		// Token: 0x0600D5DF RID: 54751
		void GetLoginTicket(Action<bool, byte[], string> _callback);

		// Token: 0x0600D5E0 RID: 54752
		string GetFriendName(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D5E1 RID: 54753
		bool IsFriend(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D5E2 RID: 54754 RVA: 0x00010E62 File Offset: 0x0000F062
		bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x0600D5E3 RID: 54755 RVA: 0x000027FC File Offset: 0x000009FC
		void ShowProfile(PlatformUserIdentifierAbs _playerId)
		{
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x0600D5E4 RID: 54756
		EUserPerms Permissions { get; }

		// Token: 0x0600D5E5 RID: 54757
		string GetPermissionDenyReason(EUserPerms _perms);

		// Token: 0x0600D5E6 RID: 54758
		IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken cancellationToken = null);

		// Token: 0x0600D5E7 RID: 54759
		IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results);

		// Token: 0x0600D5E8 RID: 54760
		void Destroy();
	}
}
