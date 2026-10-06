using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BA5 RID: 7077
	public interface IPlatform
	{
		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x0600D3B3 RID: 54195
		// (set) Token: 0x0600D3B4 RID: 54196
		bool AsServerOnly { get; set; }

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x0600D3B5 RID: 54197
		// (set) Token: 0x0600D3B6 RID: 54198
		bool IsCrossplatform { get; set; }

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x0600D3B7 RID: 54199
		EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x0600D3B8 RID: 54200
		string PlatformDisplayName { get; }

		// Token: 0x0600D3B9 RID: 54201
		void CreateInstances();

		// Token: 0x0600D3BA RID: 54202
		void Init();

		// Token: 0x0600D3BB RID: 54203
		bool HasNetworkingEnabled(IList<string> _disabledProtocolNames);

		// Token: 0x0600D3BC RID: 54204
		INetworkServer GetNetworkingServer(ProtocolManager _protocolManager);

		// Token: 0x0600D3BD RID: 54205
		INetworkClient GetNetworkingClient(ProtocolManager _protocolManager);

		// Token: 0x0600D3BE RID: 54206
		void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary);

		// Token: 0x0600D3BF RID: 54207
		string[] GetArgumentsForRelaunch();

		// Token: 0x0600D3C0 RID: 54208
		void Update();

		// Token: 0x0600D3C1 RID: 54209
		void LateUpdate();

		// Token: 0x0600D3C2 RID: 54210
		void Destroy();

		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x0600D3C3 RID: 54211
		IPlatformApi Api { get; }

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x0600D3C4 RID: 54212
		IUserClient User { get; }

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x0600D3C5 RID: 54213
		IUserClient UserServer { get; }

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x0600D3C6 RID: 54214
		IAuthenticationClient AuthenticationClient { get; }

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x0600D3C7 RID: 54215
		IAuthenticationServer AuthenticationServer { get; }

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x0600D3C8 RID: 54216
		IList<IServerListInterface> ServerListInterfaces { get; }

		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x0600D3C9 RID: 54217
		IServerListInterface ServerLookupInterface { get; }

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x0600D3CA RID: 54218
		IMasterServerAnnouncer ServerListAnnouncer { get; }

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x0600D3CB RID: 54219
		IList<IJoinSessionGameInviteListener> InviteListeners { get; }

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x0600D3CC RID: 54220
		IMultiplayerInvitationDialog MultiplayerInvitationDialog { get; }

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x0600D3CD RID: 54221
		ILobbyHost LobbyHost { get; }

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x0600D3CE RID: 54222
		IPlayerInteractionsRecorder PlayerInteractionsRecorder { get; }

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x0600D3CF RID: 54223
		IGameplayNotifier GameplayNotifier { get; }

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x0600D3D0 RID: 54224
		IPartyVoice PartyVoice { get; }

		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x0600D3D1 RID: 54225
		IUtils Utils { get; }

		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x0600D3D2 RID: 54226
		IPlatformMemory Memory { get; }

		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x0600D3D3 RID: 54227
		IAntiCheatClient AntiCheatClient { get; }

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x0600D3D4 RID: 54228
		IAntiCheatServer AntiCheatServer { get; }

		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x0600D3D5 RID: 54229
		IUserIdentifierMappingService IdMappingService { get; }

		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x0600D3D6 RID: 54230
		IUserDetailsService UserDetailsService { get; }

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x0600D3D7 RID: 54231
		IPlayerReporting PlayerReporting { get; }

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x0600D3D8 RID: 54232
		ITextCensor TextCensor { get; }

		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x0600D3D9 RID: 54233
		IRemoteFileStorage RemoteFileStorage { get; }

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x0600D3DA RID: 54234
		IRemotePlayerFileStorage RemotePlayerFileStorage { get; }

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x0600D3DB RID: 54235
		IList<IEntitlementValidator> EntitlementValidators { get; }

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x0600D3DC RID: 54236
		PlayerInputManager Input { get; }

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600D3DD RID: 54237
		IVirtualKeyboard VirtualKeyboard { get; }

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600D3DE RID: 54238
		IPlatformSaveGameProvider SaveGameProvider { get; }

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x0600D3DF RID: 54239
		IUserDataRoaming UserDataRoaming { get; }

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x0600D3E0 RID: 54240
		IAchievementManager AchievementManager { get; }

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x0600D3E1 RID: 54241
		IRichPresence RichPresence { get; }

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600D3E2 RID: 54242
		IApplicationStateController ApplicationState { get; }
	}
}
