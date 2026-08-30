using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B4C RID: 6988
	public interface IPlatform
	{
		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x0600D0B0 RID: 53424
		// (set) Token: 0x0600D0B1 RID: 53425
		bool AsServerOnly { get; set; }

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600D0B2 RID: 53426
		// (set) Token: 0x0600D0B3 RID: 53427
		bool IsCrossplatform { get; set; }

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x0600D0B4 RID: 53428
		EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x0600D0B5 RID: 53429
		string PlatformDisplayName { get; }

		// Token: 0x0600D0B6 RID: 53430
		void CreateInstances();

		// Token: 0x0600D0B7 RID: 53431
		void Init();

		// Token: 0x0600D0B8 RID: 53432
		bool HasNetworkingEnabled(IList<string> _disabledProtocolNames);

		// Token: 0x0600D0B9 RID: 53433
		INetworkServer GetNetworkingServer(ProtocolManager _protocolManager);

		// Token: 0x0600D0BA RID: 53434
		INetworkClient GetNetworkingClient(ProtocolManager _protocolManager);

		// Token: 0x0600D0BB RID: 53435
		void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary);

		// Token: 0x0600D0BC RID: 53436
		string[] GetArgumentsForRelaunch();

		// Token: 0x0600D0BD RID: 53437
		void Update();

		// Token: 0x0600D0BE RID: 53438
		void LateUpdate();

		// Token: 0x0600D0BF RID: 53439
		void Destroy();

		// Token: 0x1700197A RID: 6522
		// (get) Token: 0x0600D0C0 RID: 53440
		IPlatformApi Api { get; }

		// Token: 0x1700197B RID: 6523
		// (get) Token: 0x0600D0C1 RID: 53441
		IUserClient User { get; }

		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x0600D0C2 RID: 53442
		IUserClient UserServer { get; }

		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x0600D0C3 RID: 53443
		IAuthenticationClient AuthenticationClient { get; }

		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x0600D0C4 RID: 53444
		IAuthenticationServer AuthenticationServer { get; }

		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x0600D0C5 RID: 53445
		IList<IServerListInterface> ServerListInterfaces { get; }

		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x0600D0C6 RID: 53446
		IServerListInterface ServerLookupInterface { get; }

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x0600D0C7 RID: 53447
		IMasterServerAnnouncer ServerListAnnouncer { get; }

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x0600D0C8 RID: 53448
		IList<IJoinSessionGameInviteListener> InviteListeners { get; }

		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x0600D0C9 RID: 53449
		IMultiplayerInvitationDialog MultiplayerInvitationDialog { get; }

		// Token: 0x17001984 RID: 6532
		// (get) Token: 0x0600D0CA RID: 53450
		ILobbyHost LobbyHost { get; }

		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x0600D0CB RID: 53451
		IPlayerInteractionsRecorder PlayerInteractionsRecorder { get; }

		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x0600D0CC RID: 53452
		IGameplayNotifier GameplayNotifier { get; }

		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x0600D0CD RID: 53453
		IPartyVoice PartyVoice { get; }

		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x0600D0CE RID: 53454
		IUtils Utils { get; }

		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x0600D0CF RID: 53455
		IPlatformMemory Memory { get; }

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x0600D0D0 RID: 53456
		IAntiCheatClient AntiCheatClient { get; }

		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x0600D0D1 RID: 53457
		IAntiCheatServer AntiCheatServer { get; }

		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x0600D0D2 RID: 53458
		IUserIdentifierMappingService IdMappingService { get; }

		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x0600D0D3 RID: 53459
		IUserDetailsService UserDetailsService { get; }

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x0600D0D4 RID: 53460
		IPlayerReporting PlayerReporting { get; }

		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x0600D0D5 RID: 53461
		ITextCensor TextCensor { get; }

		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x0600D0D6 RID: 53462
		IRemoteFileStorage RemoteFileStorage { get; }

		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x0600D0D7 RID: 53463
		IRemotePlayerFileStorage RemotePlayerFileStorage { get; }

		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x0600D0D8 RID: 53464
		IList<IEntitlementValidator> EntitlementValidators { get; }

		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x0600D0D9 RID: 53465
		PlayerInputManager Input { get; }

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x0600D0DA RID: 53466
		IVirtualKeyboard VirtualKeyboard { get; }

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x0600D0DB RID: 53467
		IPlatformSaveGameProvider SaveGameProvider { get; }

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x0600D0DC RID: 53468
		IUserDataRoaming UserDataRoaming { get; }

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x0600D0DD RID: 53469
		IAchievementManager AchievementManager { get; }

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x0600D0DE RID: 53470
		IRichPresence RichPresence { get; }

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x0600D0DF RID: 53471
		IApplicationStateController ApplicationState { get; }
	}
}
