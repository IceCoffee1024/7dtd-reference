using System;

namespace Platform
{
	// Token: 0x02001B9B RID: 7067
	public interface ILobbyHost
	{
		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x0600D37D RID: 54141
		string LobbyId { get; }

		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x0600D37E RID: 54142
		bool IsInLobby { get; }

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x0600D37F RID: 54143 RVA: 0x00010E62 File Offset: 0x0000F062
		bool AllowClientLobby
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D380 RID: 54144
		void Init(IPlatform _owner);

		// Token: 0x0600D381 RID: 54145
		void UpdateLobby(GameServerInfo _gameServerInfo);

		// Token: 0x0600D382 RID: 54146
		void JoinLobby(string _lobbyId, Action<LobbyHostJoinResult> _onComplete);

		// Token: 0x0600D383 RID: 54147
		void ExitLobby();

		// Token: 0x0600D384 RID: 54148
		void UpdateGameTimePlayers(ulong _time, int _players);

		// Token: 0x0600D385 RID: 54149 RVA: 0x004CB230 File Offset: 0x004C9430
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
