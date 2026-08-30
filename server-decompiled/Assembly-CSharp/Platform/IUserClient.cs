using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BA8 RID: 7080
	public interface IUserClient
	{
		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x0600D347 RID: 54087
		EUserStatus UserStatus { get; }

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x0600D348 RID: 54088
		// (remove) Token: 0x0600D349 RID: 54089
		event Action<IPlatform> UserLoggedIn;

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x0600D34A RID: 54090
		// (remove) Token: 0x0600D34B RID: 54091
		event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x0600D34C RID: 54092
		PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x0600D34D RID: 54093
		void Init(IPlatform _owner);

		// Token: 0x0600D34E RID: 54094
		void Login(LoginUserCallback _delegate);

		// Token: 0x0600D34F RID: 54095
		void PlayOffline(LoginUserCallback _delegate);

		// Token: 0x0600D350 RID: 54096 RVA: 0x00010E62 File Offset: 0x0000F062
		EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Dev;
		}

		// Token: 0x0600D351 RID: 54097
		void StartAdvertisePlaying(GameServerInfo _serverInfo);

		// Token: 0x0600D352 RID: 54098
		void StopAdvertisePlaying();

		// Token: 0x0600D353 RID: 54099
		void GetLoginTicket(Action<bool, byte[], string> _callback);

		// Token: 0x0600D354 RID: 54100
		string GetFriendName(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D355 RID: 54101
		bool IsFriend(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D356 RID: 54102 RVA: 0x00010E62 File Offset: 0x0000F062
		bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x0600D357 RID: 54103 RVA: 0x000027FC File Offset: 0x000009FC
		void ShowProfile(PlatformUserIdentifierAbs _playerId)
		{
		}

		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x0600D358 RID: 54104
		EUserPerms Permissions { get; }

		// Token: 0x0600D359 RID: 54105
		string GetPermissionDenyReason(EUserPerms _perms);

		// Token: 0x0600D35A RID: 54106
		IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken cancellationToken = null);

		// Token: 0x0600D35B RID: 54107
		IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results);

		// Token: 0x0600D35C RID: 54108
		void Destroy();
	}
}
