using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BDA RID: 7130
	public interface IUserClient
	{
		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x0600D4BF RID: 54463
		EUserStatus UserStatus { get; }

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x0600D4C0 RID: 54464
		// (remove) Token: 0x0600D4C1 RID: 54465
		event Action<IPlatform> UserLoggedIn;

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x0600D4C2 RID: 54466
		// (remove) Token: 0x0600D4C3 RID: 54467
		event UserBlocksChangedCallback UserBlocksChanged;

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x0600D4C4 RID: 54468
		PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x0600D4C5 RID: 54469
		void Init(IPlatform _owner);

		// Token: 0x0600D4C6 RID: 54470
		void Login(LoginUserCallback _delegate);

		// Token: 0x0600D4C7 RID: 54471
		void PlayOffline(LoginUserCallback _delegate);

		// Token: 0x0600D4C8 RID: 54472 RVA: 0x00010E62 File Offset: 0x0000F062
		EMatchmakingGroup GetMatchmakingGroup()
		{
			return EMatchmakingGroup.Dev;
		}

		// Token: 0x0600D4C9 RID: 54473
		void StartAdvertisePlaying(GameServerInfo _serverInfo);

		// Token: 0x0600D4CA RID: 54474
		void StopAdvertisePlaying();

		// Token: 0x0600D4CB RID: 54475
		void GetLoginTicket(Action<bool, byte[], string> _callback);

		// Token: 0x0600D4CC RID: 54476
		string GetFriendName(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D4CD RID: 54477
		bool IsFriend(PlatformUserIdentifierAbs _playerId);

		// Token: 0x0600D4CE RID: 54478 RVA: 0x00010E62 File Offset: 0x0000F062
		bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x0600D4CF RID: 54479 RVA: 0x000027FC File Offset: 0x000009FC
		void ShowProfile(PlatformUserIdentifierAbs _playerId)
		{
		}

		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x0600D4D0 RID: 54480
		EUserPerms Permissions { get; }

		// Token: 0x0600D4D1 RID: 54481
		string GetPermissionDenyReason(EUserPerms _perms);

		// Token: 0x0600D4D2 RID: 54482
		IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken cancellationToken = null);

		// Token: 0x0600D4D3 RID: 54483
		IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results);

		// Token: 0x0600D4D4 RID: 54484
		void Destroy();
	}
}
