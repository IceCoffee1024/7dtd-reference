using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C65 RID: 7269
	public class LobbyListFriends : LobbyListAbs
	{
		// Token: 0x0600D719 RID: 55065 RVA: 0x004D6DB8 File Offset: 0x004D4FB8
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

		// Token: 0x0600D71A RID: 55066 RVA: 0x004D6DD8 File Offset: 0x004D4FD8
		public override void StopSearch()
		{
			this.currentFriend = -1;
			this.isRefreshing = false;
		}

		// Token: 0x0600D71B RID: 55067 RVA: 0x004D6DE8 File Offset: 0x004D4FE8
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

		// Token: 0x0600D71C RID: 55068 RVA: 0x004D6E08 File Offset: 0x004D5008
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

		// Token: 0x0600D71D RID: 55069 RVA: 0x004D6E80 File Offset: 0x004D5080
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

		// Token: 0x0400A347 RID: 41799
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A348 RID: 41800
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentFriend;
	}
}
