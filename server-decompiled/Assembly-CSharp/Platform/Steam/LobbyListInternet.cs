using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CF0 RID: 7408
	public class LobbyListInternet : LobbyListAbs
	{
		// Token: 0x0600DB40 RID: 56128 RVA: 0x004E5AFD File Offset: 0x004E3CFD
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

		// Token: 0x0600DB41 RID: 56129 RVA: 0x004E5B1D File Offset: 0x004E3D1D
		public override void StopSearch()
		{
			if (this.m_RequestLobbies != null && this.m_RequestLobbies.IsActive())
			{
				this.m_RequestLobbies.Cancel();
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600DB42 RID: 56130 RVA: 0x004E5B48 File Offset: 0x004E3D48
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

		// Token: 0x0600DB43 RID: 56131 RVA: 0x004E5B90 File Offset: 0x004E3D90
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

		// Token: 0x0400A639 RID: 42553
		[PublicizedFrom(EAccessModifier.Private)]
		public CallResult<LobbyMatchList_t> m_RequestLobbies;
	}
}
