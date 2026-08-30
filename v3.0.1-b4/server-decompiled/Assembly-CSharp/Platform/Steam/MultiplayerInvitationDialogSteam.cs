using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C6F RID: 7279
	public class MultiplayerInvitationDialogSteam : IMultiplayerInvitationDialog
	{
		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x0600D775 RID: 55157 RVA: 0x004D8723 File Offset: 0x004D6923
		public bool CanShow
		{
			get
			{
				return this.lobbyHost != null && this.lobbyHost.IsInLobby;
			}
		}

		// Token: 0x0600D776 RID: 55158 RVA: 0x004D873A File Offset: 0x004D693A
		public void Init(IPlatform owner)
		{
			this.lobbyHost = (LobbyHost)owner.LobbyHost;
		}

		// Token: 0x0600D777 RID: 55159 RVA: 0x004D8750 File Offset: 0x004D6950
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

		// Token: 0x0400A374 RID: 41844
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyHost lobbyHost;
	}
}
