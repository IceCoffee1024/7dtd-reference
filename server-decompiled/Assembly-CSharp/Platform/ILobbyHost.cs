using System;

namespace Platform
{
	// Token: 0x02001BCC RID: 7116
	public interface ILobbyHost
	{
		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x0600D491 RID: 54417
		string LobbyId { get; }

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600D492 RID: 54418
		bool IsInLobby { get; }

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x0600D493 RID: 54419 RVA: 0x00010E62 File Offset: 0x0000F062
		bool AllowClientLobby
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D494 RID: 54420
		void Init(IPlatform _owner);

		// Token: 0x0600D495 RID: 54421
		void UpdateLobby(GameServerInfo _gameServerInfo);

		// Token: 0x0600D496 RID: 54422
		void JoinLobby(string _lobbyId, Action<LobbyHostJoinResult> _onComplete);

		// Token: 0x0600D497 RID: 54423
		void ExitLobby();

		// Token: 0x0600D498 RID: 54424
		void UpdateGameTimePlayers(ulong _time, int _players);

		// Token: 0x0600D499 RID: 54425 RVA: 0x004CED58 File Offset: 0x004CCF58
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
