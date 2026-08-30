using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C63 RID: 7267
	public abstract class LobbyListAbs : IServerListInterface
	{
		// Token: 0x0600D707 RID: 55047 RVA: 0x004D6C61 File Offset: 0x004D4E61
		[PublicizedFrom(EAccessModifier.Protected)]
		public LobbyListAbs()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Application.wantsToQuit += this.OnApplicationQuit;
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x0600D708 RID: 55048 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D709 RID: 55049
		public abstract void Init(IPlatform _owner);

		// Token: 0x0600D70A RID: 55050 RVA: 0x004D6C83 File Offset: 0x004D4E83
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600D70B RID: 55051 RVA: 0x004D6C8C File Offset: 0x004D4E8C
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600D70C RID: 55052
		public abstract void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600D70D RID: 55053
		public abstract void StopSearch();

		// Token: 0x0600D70E RID: 55054 RVA: 0x004D6C94 File Offset: 0x004D4E94
		public virtual void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D70F RID: 55055 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D710 RID: 55056 RVA: 0x004D6CA3 File Offset: 0x004D4EA3
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnApplicationQuit()
		{
			this.Disconnect();
			return true;
		}

		// Token: 0x0600D711 RID: 55057 RVA: 0x004D6CAC File Offset: 0x004D4EAC
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator restartRefreshCo(float _delay)
		{
			yield return new WaitForSeconds(_delay);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0600D712 RID: 55058 RVA: 0x004D6CC4 File Offset: 0x004D4EC4
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

		// Token: 0x0400A340 RID: 41792
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform owner;

		// Token: 0x0400A341 RID: 41793
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A342 RID: 41794
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isRefreshing;
	}
}
