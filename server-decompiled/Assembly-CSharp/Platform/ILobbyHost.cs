using System;

namespace Platform
{
	// Token: 0x02001B69 RID: 7017
	public interface ILobbyHost
	{
		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x0600D205 RID: 53765
		string LobbyId { get; }

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x0600D206 RID: 53766
		bool IsInLobby { get; }

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x0600D207 RID: 53767 RVA: 0x00010E62 File Offset: 0x0000F062
		bool AllowClientLobby
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D208 RID: 53768
		void Init(IPlatform _owner);

		// Token: 0x0600D209 RID: 53769
		void UpdateLobby(GameServerInfo _gameServerInfo);

		// Token: 0x0600D20A RID: 53770
		void JoinLobby(string _lobbyId, Action<LobbyHostJoinResult> _onComplete);

		// Token: 0x0600D20B RID: 53771
		void ExitLobby();

		// Token: 0x0600D20C RID: 53772
		void UpdateGameTimePlayers(ulong _time, int _players);

		// Token: 0x0600D20D RID: 53773 RVA: 0x004C6748 File Offset: 0x004C4948
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void NotifyJoinedSession(string sessionId, bool overwriteHostLobby)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				PlatformLobbyId platformLobbyId = new PlatformLobbyId(PlatformManager.NativePlatform.PlatformIdentifier, sessionId);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageLobbyRegisterClient>().Setup(platformLobbyId, overwriteHostLobby), false);
			}
		}
	}
}
