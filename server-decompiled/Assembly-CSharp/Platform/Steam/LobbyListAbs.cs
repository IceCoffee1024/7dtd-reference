using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CBC RID: 7356
	public abstract class LobbyListAbs : IServerListInterface
	{
		// Token: 0x0600DA13 RID: 55827 RVA: 0x004E1CC5 File Offset: 0x004DFEC5
		[PublicizedFrom(EAccessModifier.Protected)]
		public LobbyListAbs()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Application.wantsToQuit += this.OnApplicationQuit;
		}

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x0600DA14 RID: 55828 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DA15 RID: 55829
		public abstract void Init(IPlatform _owner);

		// Token: 0x0600DA16 RID: 55830 RVA: 0x004E1CE7 File Offset: 0x004DFEE7
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x0600DA17 RID: 55831 RVA: 0x004E1CF0 File Offset: 0x004DFEF0
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DA18 RID: 55832
		public abstract void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600DA19 RID: 55833
		public abstract void StopSearch();

		// Token: 0x0600DA1A RID: 55834 RVA: 0x004E1CF8 File Offset: 0x004DFEF8
		public virtual void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600DA1B RID: 55835 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DA1C RID: 55836 RVA: 0x004E1D07 File Offset: 0x004DFF07
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnApplicationQuit()
		{
			this.Disconnect();
			return true;
		}

		// Token: 0x0600DA1D RID: 55837 RVA: 0x004E1D10 File Offset: 0x004DFF10
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator restartRefreshCo(float _delay)
		{
			yield return new WaitForSeconds(_delay);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0600DA1E RID: 55838 RVA: 0x004E1D28 File Offset: 0x004DFF28
		[PublicizedFrom(EAccessModifier.Protected)]
		public void ParseLobbyData(CSteamID _lobbyId, EServerRelationType _source)
		{
			if (this.gameServerFoundCallback == null)
			{
				return;
			}
			GameServerInfo gameServerInfo = new GameServerInfo
			{
				IsLobby = true
			};
			int lobbyDataCount = SteamMatchmaking.GetLobbyDataCount(_lobbyId);
			for (int i = 0; i < lobbyDataCount; i++)
			{
				string text;
				string text2;
				if (SteamMatchmaking.GetLobbyDataByIndex(_lobbyId, i, out text, 100, out text2, 200))
				{
					gameServerInfo.ParseAny(text, text2);
				}
			}
			if (PlatformManager.CrossplatformPlatform == null)
			{
				gameServerInfo.SetValue(GameInfoString.UniqueId, gameServerInfo.GetValue(GameInfoString.SteamID));
			}
			gameServerInfo.IsFriends = _source == EServerRelationType.Friends;
			this.gameServerFoundCallback(this.owner, gameServerInfo, _source);
		}

		// Token: 0x0400A547 RID: 42311
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform owner;

		// Token: 0x0400A548 RID: 42312
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A549 RID: 42313
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isRefreshing;
	}
}
