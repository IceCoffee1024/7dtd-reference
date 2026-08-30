using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B81 RID: 7041
	public interface IUserClient
	{
		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x0600D1BB RID: 53691
		EUserStatus UserStatus { get; }

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x0600D1BC RID: 53692
		// (remove) Token: 0x0600D1BD RID: 53693
		event Action<IPlatform> UserLoggedIn;

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x0600D1BE RID: 53694
		// (remove) Token: 0x0600D1BF RID: 53695
		event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x0600D1C0 RID: 53696
		PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x0600D1C1 RID: 53697
		void Init(IPlatform _owner);

		// Token: 0x0600D1C2 RID: 53698
		void Login(LoginUserCallback _delegate);

		// Token: 0x0600D1C3 RID: 53699
		void PlayOffline(LoginUserCallback _delegate);

		// Token: 0x0600D1C4 RID: 53700 RVA: 0x00010E62 File Offset: 0x0000F062
		EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Dev;
		}

		// Token: 0x0600D1C5 RID: 53701
		void StartAdvertisePlaying(GameServerInfo _serverInfo);

		// Token: 0x0600D1C6 RID: 53702
		void StopAdvertisePlaying();

		// Token: 0x0600D1C7 RID: 53703
		void GetLoginTicket(Action<bool, byte[], string> _callback);

		// Token: 0x0600D1C8 RID: 53704
		string GetFriendName(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D1C9 RID: 53705
		bool IsFriend(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D1CA RID: 53706 RVA: 0x00010E62 File Offset: 0x0000F062
		bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x0600D1CB RID: 53707 RVA: 0x000027FC File Offset: 0x000009FC
		void ShowProfile(PlatformUserIdentifierAbs _playerId)
		{
		}

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x0600D1CC RID: 53708
		EUserPerms Permissions { get; }

		// Token: 0x0600D1CD RID: 53709
		string GetPermissionDenyReason(EUserPerms _perms);

		// Token: 0x0600D1CE RID: 53710
		IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken cancellationToken = null);

		// Token: 0x0600D1CF RID: 53711
		IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results);

		// Token: 0x0600D1D0 RID: 53712
		void Destroy();
	}
}
