using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C8C RID: 7308
	public class LobbyListFriends : LobbyListAbs
	{
		// Token: 0x0600D8AB RID: 55467 RVA: 0x004DD2D4 File Offset: 0x004DB4D4
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

		// Token: 0x0600D8AC RID: 55468 RVA: 0x004DD2F4 File Offset: 0x004DB4F4
		public override void StopSearch()
		{
			this.currentFriend = -1;
			this.isRefreshing = false;
		}

		// Token: 0x0600D8AD RID: 55469 RVA: 0x004DD304 File Offset: 0x004DB504
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

		// Token: 0x0600D8AE RID: 55470 RVA: 0x004DD324 File Offset: 0x004DB524
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

		// Token: 0x0600D8AF RID: 55471 RVA: 0x004DD39C File Offset: 0x004DB59C
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

		// Token: 0x0400A479 RID: 42105
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A47A RID: 42106
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentFriend;
	}
}
