using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C96 RID: 7318
	public class MultiplayerInvitationDialogSteam : IMultiplayerInvitationDialog
	{
		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x0600D907 RID: 55559 RVA: 0x004DEC3F File Offset: 0x004DCE3F
		public bool CanShow
		{
			get
			{
				return this.lobbyHost != null && this.lobbyHost.IsInLobby;
			}
		}

		// Token: 0x0600D908 RID: 55560 RVA: 0x004DEC56 File Offset: 0x004DCE56
		public void Init(IPlatform owner)
		{
			this.lobbyHost = (LobbyHost)owner.LobbyHost;
		}

		// Token: 0x0600D909 RID: 55561 RVA: 0x004DEC6C File Offset: 0x004DCE6C
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

		// Token: 0x0400A4A6 RID: 42150
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyHost lobbyHost;
	}
}
