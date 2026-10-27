using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BD6 RID: 7126
	public interface IPlatform
	{
		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x0600D4C7 RID: 54471
		// (set) Token: 0x0600D4C8 RID: 54472
		bool AsServerOnly { get; set; }

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x0600D4C9 RID: 54473
		// (set) Token: 0x0600D4CA RID: 54474
		bool IsCrossplatform { get; set; }

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x0600D4CB RID: 54475
		EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x0600D4CC RID: 54476
		string PlatformDisplayName { get; }

		// Token: 0x0600D4CD RID: 54477
		void CreateInstances();

		// Token: 0x0600D4CE RID: 54478
		void Init();

		// Token: 0x0600D4CF RID: 54479
		bool HasNetworkingEnabled(IList<string> _disabledProtocolNames);

		// Token: 0x0600D4D0 RID: 54480
		INetworkServer GetNetworkingServer(ProtocolManager _protocolManager);

		// Token: 0x0600D4D1 RID: 54481
		INetworkClient GetNetworkingClient(ProtocolManager _protocolManager);

		// Token: 0x0600D4D2 RID: 54482
		void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary);

		// Token: 0x0600D4D3 RID: 54483
		string[] GetArgumentsForRelaunch();

		// Token: 0x0600D4D4 RID: 54484
		void Update();

		// Token: 0x0600D4D5 RID: 54485
		void LateUpdate();

		// Token: 0x0600D4D6 RID: 54486
		void Destroy();

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x0600D4D7 RID: 54487
		IPlatformApi Api { get; }

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600D4D8 RID: 54488
		IUserClient User { get; }

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x0600D4D9 RID: 54489
		IUserClient UserServer { get; }

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x0600D4DA RID: 54490
		IAuthenticationClient AuthenticationClient { get; }

		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x0600D4DB RID: 54491
		IAuthenticationServer AuthenticationServer { get; }

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x0600D4DC RID: 54492
		IList<IServerListInterface> ServerListInterfaces { get; }

		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x0600D4DD RID: 54493
		IServerListInterface ServerLookupInterface { get; }

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x0600D4DE RID: 54494
		IMasterServerAnnouncer ServerListAnnouncer { get; }

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600D4DF RID: 54495
		IList<IJoinSessionGameInviteListener> InviteListeners { get; }

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600D4E0 RID: 54496
		IMultiplayerInvitationDialog MultiplayerInvitationDialog { get; }

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x0600D4E1 RID: 54497
		ILobbyHost LobbyHost { get; }

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x0600D4E2 RID: 54498
		IPlayerInteractionsRecorder PlayerInteractionsRecorder { get; }

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x0600D4E3 RID: 54499
		IGameplayNotifier GameplayNotifier { get; }

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x0600D4E4 RID: 54500
		IPartyVoice PartyVoice { get; }

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x0600D4E5 RID: 54501
		IUtils Utils { get; }

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x0600D4E6 RID: 54502
		IPlatformMemory Memory { get; }

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x0600D4E7 RID: 54503
		IAntiCheatClient AntiCheatClient { get; }

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x0600D4E8 RID: 54504
		IAntiCheatServer AntiCheatServer { get; }

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x0600D4E9 RID: 54505
		IUserIdentifierMappingService IdMappingService { get; }

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x0600D4EA RID: 54506
		IUserDetailsService UserDetailsService { get; }

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x0600D4EB RID: 54507
		IPlayerReporting PlayerReporting { get; }

		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x0600D4EC RID: 54508
		ITextCensor TextCensor { get; }

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x0600D4ED RID: 54509
		IRemoteFileStorage RemoteFileStorage { get; }

		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x0600D4EE RID: 54510
		IRemotePlayerFileStorage RemotePlayerFileStorage { get; }

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x0600D4EF RID: 54511
		IList<IEntitlementValidator> EntitlementValidators { get; }

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x0600D4F0 RID: 54512
		PlayerInputManager Input { get; }

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x0600D4F1 RID: 54513
		IVirtualKeyboard VirtualKeyboard { get; }

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x0600D4F2 RID: 54514
		IPlatformSaveGameProvider SaveGameProvider { get; }

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x0600D4F3 RID: 54515
		IUserDataRoaming UserDataRoaming { get; }

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x0600D4F4 RID: 54516
		IAchievementManager AchievementManager { get; }

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x0600D4F5 RID: 54517
		IRichPresence RichPresence { get; }

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x0600D4F6 RID: 54518
		IApplicationStateController ApplicationState { get; }
	}
}
