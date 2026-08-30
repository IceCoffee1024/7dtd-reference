using System;

namespace Platform
{
	// Token: 0x02001B42 RID: 6978
	public interface ILobbyHost
	{
		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600D07A RID: 53370
		string LobbyId { get; }

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600D07B RID: 53371
		bool IsInLobby { get; }

		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x0600D07C RID: 53372 RVA: 0x00010E62 File Offset: 0x0000F062
		bool AllowClientLobby
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D07D RID: 53373
		void Init(IPlatform _owner);

		// Token: 0x0600D07E RID: 53374
		void UpdateLobby(GameServerInfo _gameServerInfo);

		// Token: 0x0600D07F RID: 53375
		void JoinLobby(string _lobbyId, Action<LobbyHostJoinResult> _onComplete);

		// Token: 0x0600D080 RID: 53376
		void ExitLobby();

		// Token: 0x0600D081 RID: 53377
		void UpdateGameTimePlayers(ulong _time, int _players);

		// Token: 0x0600D082 RID: 53378 RVA: 0x004C0354 File Offset: 0x004BE554
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
