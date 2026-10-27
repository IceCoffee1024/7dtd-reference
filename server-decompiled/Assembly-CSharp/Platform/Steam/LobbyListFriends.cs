using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CEF RID: 7407
	public class LobbyListFriends : LobbyListAbs
	{
		// Token: 0x0600DB39 RID: 56121 RVA: 0x004E59BC File Offset: 0x004E3BBC
		public override void Init(IPlatform _owner)
		{
			this.owner = _owner;
			_owner.Api.ClientApiInitialized += delegate
			{
				if (this.m_lobbyDataUpdate == null && !GameManager.IsDedicatedServer)
				{
					this.m_lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(new Callback<LobbyDataUpdate_t>.DispatchDelegate(this.Lobby_DataUpdate));
				}
			};
		}

		// Token: 0x0600DB3A RID: 56122 RVA: 0x004E59DC File Offset: 0x004E3BDC
		public override void StopSearch()
		{
			this.currentFriend = -1;
			this.isRefreshing = false;
		}

		// Token: 0x0600DB3B RID: 56123 RVA: 0x004E59EC File Offset: 0x004E3BEC
		public override void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			if (this.gameServerFoundCallback == null)
			{
				return;
			}
			this.isRefreshing = true;
			this.currentFriend = 0;
			this.queryNextFriend();
		}

		// Token: 0x0600DB3C RID: 56124 RVA: 0x004E5A0C File Offset: 0x004E3C0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void queryNextFriend()
		{
			while (this.currentFriend < SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll))
			{
				FriendGameInfo_t friendGameInfo_t;
				if (SteamFriends.GetFriendGamePlayed(SteamFriends.GetFriendByIndex(this.currentFriend, EFriendFlags.k_EFriendFlagAll), out friendGameInfo_t) && friendGameInfo_t.m_steamIDLobby != CSteamID.Nil)
				{
					SteamMatchmaking.RequestLobbyData(friendGameInfo_t.m_steamIDLobby);
					return;
				}
				this.currentFriend++;
			}
			ThreadManager.StartCoroutine(base.restartRefreshCo(2f));
		}

		// Token: 0x0600DB3D RID: 56125 RVA: 0x004E5A84 File Offset: 0x004E3C84
		[PublicizedFrom(EAccessModifier.Private)]
		public void Lobby_DataUpdate(LobbyDataUpdate_t _val)
		{
			CSteamID csteamID = new CSteamID(_val.m_ulSteamIDLobby);
			if (_val.m_bSuccess == 0)
			{
				return;
			}
			base.ParseLobbyData(csteamID, EServerRelationType.Friends);
			if (this.currentFriend < 0)
			{
				return;
			}
			this.currentFriend++;
			this.queryNextFriend();
		}

		// Token: 0x0400A637 RID: 42551
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A638 RID: 42552
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentFriend;
	}
}
