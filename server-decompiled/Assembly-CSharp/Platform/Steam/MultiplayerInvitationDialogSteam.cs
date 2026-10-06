using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CC8 RID: 7368
	public class MultiplayerInvitationDialogSteam : IMultiplayerInvitationDialog
	{
		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x0600DA81 RID: 55937 RVA: 0x004E3787 File Offset: 0x004E1987
		public bool CanShow
		{
			get
			{
				return this.lobbyHost != null && this.lobbyHost.IsInLobby;
			}
		}

		// Token: 0x0600DA82 RID: 55938 RVA: 0x004E379E File Offset: 0x004E199E
		public void Init(IPlatform owner)
		{
			this.lobbyHost = (LobbyHost)owner.LobbyHost;
		}

		// Token: 0x0600DA83 RID: 55939 RVA: 0x004E37B4 File Offset: 0x004E19B4
		public void ShowInviteDialog()
		{
			if (this.lobbyHost == null)
			{
				Log.Error("[Steam] Cannot open invite dialog, lobby host is null");
				return;
			}
			string lobbyId = this.lobbyHost.LobbyId;
			if (string.IsNullOrEmpty(lobbyId))
			{
				Log.Error("[Steam] Cannot open invite dialog, no lobby id set");
				return;
			}
			ulong num;
			if (StringParsers.TryParseUInt64(lobbyId, out num))
			{
				Log.Out(string.Format("[Steam] Opening invite dialog for lobby: {0}", num));
				SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(num));
				return;
			}
			Log.Error("[Steam] Cannot open invite dialog, could not parse Steam lobby id: " + lobbyId);
		}

		// Token: 0x0400A57B RID: 42363
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyHost lobbyHost;
	}
}
