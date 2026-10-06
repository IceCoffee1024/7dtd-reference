using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CBF RID: 7359
	public class LobbyListInternet : LobbyListAbs
	{
		// Token: 0x0600DA2C RID: 55852 RVA: 0x004E1F5D File Offset: 0x004E015D
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

		// Token: 0x0600DA2D RID: 55853 RVA: 0x004E1F7D File Offset: 0x004E017D
		public override void StopSearch()
		{
			if (this.m_RequestLobbies != null && this.m_RequestLobbies.IsActive())
			{
				this.m_RequestLobbies.Cancel();
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600DA2E RID: 55854 RVA: 0x004E1FA8 File Offset: 0x004E01A8
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

		// Token: 0x0600DA2F RID: 55855 RVA: 0x004E1FF0 File Offset: 0x004E01F0
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

		// Token: 0x0400A550 RID: 42320
		[PublicizedFrom(EAccessModifier.Private)]
		public CallResult<LobbyMatchList_t> m_RequestLobbies;
	}
}
