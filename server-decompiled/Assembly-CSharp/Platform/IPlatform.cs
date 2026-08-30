using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B73 RID: 7027
	public interface IPlatform
	{
		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x0600D23B RID: 53819
		// (set) Token: 0x0600D23C RID: 53820
		bool AsServerOnly { get; set; }

		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x0600D23D RID: 53821
		// (set) Token: 0x0600D23E RID: 53822
		bool IsCrossplatform { get; set; }

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x0600D23F RID: 53823
		EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x0600D240 RID: 53824
		string PlatformDisplayName { get; }

		// Token: 0x0600D241 RID: 53825
		void CreateInstances();

		// Token: 0x0600D242 RID: 53826
		void Init();

		// Token: 0x0600D243 RID: 53827
		bool HasNetworkingEnabled(IList<string> _disabledProtocolNames);

		// Token: 0x0600D244 RID: 53828
		INetworkServer GetNetworkingServer(ProtocolManager _protocolManager);

		// Token: 0x0600D245 RID: 53829
		INetworkClient GetNetworkingClient(ProtocolManager _protocolManager);

		// Token: 0x0600D246 RID: 53830
		void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary);

		// Token: 0x0600D247 RID: 53831
		string[] GetArgumentsForRelaunch();

		// Token: 0x0600D248 RID: 53832
		void Update();

		// Token: 0x0600D249 RID: 53833
		void LateUpdate();

		// Token: 0x0600D24A RID: 53834
		void Destroy();

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600D24B RID: 53835
		IPlatformApi Api { get; }

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x0600D24C RID: 53836
		IUserClient User { get; }

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x0600D24D RID: 53837
		IUserClient UserServer { get; }

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x0600D24E RID: 53838
		IAuthenticationClient AuthenticationClient { get; }

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x0600D24F RID: 53839
		IAuthenticationServer AuthenticationServer { get; }

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x0600D250 RID: 53840
		IList<IServerListInterface> ServerListInterfaces { get; }

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x0600D251 RID: 53841
		IServerListInterface ServerLookupInterface { get; }

		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x0600D252 RID: 53842
		IMasterServerAnnouncer ServerListAnnouncer { get; }

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x0600D253 RID: 53843
		IList<IJoinSessionGameInviteListener> InviteListeners { get; }

		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x0600D254 RID: 53844
		IMultiplayerInvitationDialog MultiplayerInvitationDialog { get; }

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x0600D255 RID: 53845
		ILobbyHost LobbyHost { get; }

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x0600D256 RID: 53846
		IPlayerInteractionsRecorder PlayerInteractionsRecorder { get; }

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x0600D257 RID: 53847
		IGameplayNotifier GameplayNotifier { get; }

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x0600D258 RID: 53848
		IPartyVoice PartyVoice { get; }

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x0600D259 RID: 53849
		IUtils Utils { get; }

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x0600D25A RID: 53850
		IPlatformMemory Memory { get; }

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x0600D25B RID: 53851
		IAntiCheatClient AntiCheatClient { get; }

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x0600D25C RID: 53852
		IAntiCheatServer AntiCheatServer { get; }

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x0600D25D RID: 53853
		IUserIdentifierMappingService IdMappingService { get; }

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x0600D25E RID: 53854
		IUserDetailsService UserDetailsService { get; }

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x0600D25F RID: 53855
		IPlayerReporting PlayerReporting { get; }

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x0600D260 RID: 53856
		ITextCensor TextCensor { get; }

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x0600D261 RID: 53857
		IRemoteFileStorage RemoteFileStorage { get; }

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x0600D262 RID: 53858
		IRemotePlayerFileStorage RemotePlayerFileStorage { get; }

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x0600D263 RID: 53859
		IList<IEntitlementValidator> EntitlementValidators { get; }

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x0600D264 RID: 53860
		PlayerInputManager Input { get; }

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x0600D265 RID: 53861
		IVirtualKeyboard VirtualKeyboard { get; }

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x0600D266 RID: 53862
		IPlatformSaveGameProvider SaveGameProvider { get; }

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x0600D267 RID: 53863
		IUserDataRoaming UserDataRoaming { get; }

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x0600D268 RID: 53864
		IAchievementManager AchievementManager { get; }

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x0600D269 RID: 53865
		IRichPresence RichPresence { get; }

		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x0600D26A RID: 53866
		IApplicationStateController ApplicationState { get; }
	}
}
