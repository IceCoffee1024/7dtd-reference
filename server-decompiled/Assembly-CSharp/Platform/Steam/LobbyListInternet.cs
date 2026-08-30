using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C8D RID: 7309
	public class LobbyListInternet : LobbyListAbs
	{
		// Token: 0x0600D8B2 RID: 55474 RVA: 0x004DD415 File Offset: 0x004DB615
		public override void Init(IPlatform _owner)
		{
			this.owner = _owner;
			_owner.Api.ClientApiInitialized += delegate
			{
				if (this.m_RequestLobbies == null && !GameManager.IsDedicatedServer)
				{
					this.m_RequestLobbies = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(this.RequestLobbies_CallResult));
				}
			};
		}

		// Token: 0x0600D8B3 RID: 55475 RVA: 0x004DD435 File Offset: 0x004DB635
		public override void StopSearch()
		{
			if (this.m_RequestLobbies != null && this.m_RequestLobbies.IsActive())
			{
				this.m_RequestLobbies.Cancel();
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600D8B4 RID: 55476 RVA: 0x004DD460 File Offset: 0x004DB660
		public override void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			if (this.gameServerFoundCallback == null)
			{
				return;
			}
			SteamMatchmaking.AddRequestLobbyListStringFilter("CompatibilityVersion", global::Constants.cVersionInformation.LongStringNoBuild, ELobbyComparison.k_ELobbyComparisonEqual);
			SteamAPICall_t steamAPICall_t = SteamMatchmaking.RequestLobbyList();
			this.m_RequestLobbies.Set(steamAPICall_t, null);
			this.isRefreshing = true;
		}

		// Token: 0x0600D8B5 RID: 55477 RVA: 0x004DD4A8 File Offset: 0x004DB6A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void RequestLobbies_CallResult(LobbyMatchList_t _val, bool _ioFailure)
		{
			if (_ioFailure)
			{
				Log.Out("[Steamworks.NET] RequestLobbies failed");
			}
			else
			{
				int num = 0;
				while ((long)num < (long)((ulong)_val.m_nLobbiesMatching))
				{
					base.ParseLobbyData(SteamMatchmaking.GetLobbyByIndex(num), EServerRelationType.Internet);
					num++;
				}
			}
			ThreadManager.StartCoroutine(base.restartRefreshCo(3f));
		}

		// Token: 0x0400A47B RID: 42107
		[PublicizedFrom(EAccessModifier.Private)]
		public CallResult<LobbyMatchList_t> m_RequestLobbies;
	}
}
