using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CED RID: 7405
	public abstract class LobbyListAbs : IServerListInterface
	{
		// Token: 0x0600DB27 RID: 56103 RVA: 0x004E5865 File Offset: 0x004E3A65
		[PublicizedFrom(EAccessModifier.Protected)]
		public LobbyListAbs()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Application.wantsToQuit += this.OnApplicationQuit;
		}

		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x0600DB28 RID: 56104 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DB29 RID: 56105
		public abstract void Init(IPlatform _owner);

		// Token: 0x0600DB2A RID: 56106 RVA: 0x004E5887 File Offset: 0x004E3A87
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x0600DB2B RID: 56107 RVA: 0x004E5890 File Offset: 0x004E3A90
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DB2C RID: 56108
		public abstract void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600DB2D RID: 56109
		public abstract void StopSearch();

		// Token: 0x0600DB2E RID: 56110 RVA: 0x004E5898 File Offset: 0x004E3A98
		public virtual void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600DB2F RID: 56111 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB30 RID: 56112 RVA: 0x004E58A7 File Offset: 0x004E3AA7
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnApplicationQuit()
		{
			this.Disconnect();
			return true;
		}

		// Token: 0x0600DB31 RID: 56113 RVA: 0x004E58B0 File Offset: 0x004E3AB0
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator restartRefreshCo(float _delay)
		{
			yield return new WaitForSeconds(_delay);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0600DB32 RID: 56114 RVA: 0x004E58C8 File Offset: 0x004E3AC8
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

		// Token: 0x0400A630 RID: 42544
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform owner;

		// Token: 0x0400A631 RID: 42545
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A632 RID: 42546
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isRefreshing;
	}
}
