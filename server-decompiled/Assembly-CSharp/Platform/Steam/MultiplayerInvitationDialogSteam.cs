using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CF9 RID: 7417
	public class MultiplayerInvitationDialogSteam : IMultiplayerInvitationDialog
	{
		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x0600DB95 RID: 56213 RVA: 0x004E7327 File Offset: 0x004E5527
		public bool CanShow
		{
			get
			{
				return this.lobbyHost != null && this.lobbyHost.IsInLobby;
			}
		}

		// Token: 0x0600DB96 RID: 56214 RVA: 0x004E733E File Offset: 0x004E553E
		public void Init(IPlatform owner)
		{
			this.lobbyHost = (LobbyHost)owner.LobbyHost;
		}

		// Token: 0x0600DB97 RID: 56215 RVA: 0x004E7354 File Offset: 0x004E5554
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

		// Token: 0x0400A664 RID: 42596
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyHost lobbyHost;
	}
}
