using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CBE RID: 7358
	public class LobbyListFriends : LobbyListAbs
	{
		// Token: 0x0600DA25 RID: 55845 RVA: 0x004E1E1C File Offset: 0x004E001C
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

		// Token: 0x0600DA26 RID: 55846 RVA: 0x004E1E3C File Offset: 0x004E003C
		public override void StopSearch()
		{
			this.currentFriend = -1;
			this.isRefreshing = false;
		}

		// Token: 0x0600DA27 RID: 55847 RVA: 0x004E1E4C File Offset: 0x004E004C
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

		// Token: 0x0600DA28 RID: 55848 RVA: 0x004E1E6C File Offset: 0x004E006C
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

		// Token: 0x0600DA29 RID: 55849 RVA: 0x004E1EE4 File Offset: 0x004E00E4
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

		// Token: 0x0400A54E RID: 42318
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A54F RID: 42319
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentFriend;
	}
}
