using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C8A RID: 7306
	public abstract class LobbyListAbs : IServerListInterface
	{
		// Token: 0x0600D899 RID: 55449 RVA: 0x004DD17D File Offset: 0x004DB37D
		[PublicizedFrom(EAccessModifier.Protected)]
		public LobbyListAbs()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Application.wantsToQuit += this.OnApplicationQuit;
		}

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600D89A RID: 55450 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D89B RID: 55451
		public abstract void Init(IPlatform _owner);

		// Token: 0x0600D89C RID: 55452 RVA: 0x004DD19F File Offset: 0x004DB39F
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600D89D RID: 55453 RVA: 0x004DD1A8 File Offset: 0x004DB3A8
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600D89E RID: 55454
		public abstract void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600D89F RID: 55455
		public abstract void StopSearch();

		// Token: 0x0600D8A0 RID: 55456 RVA: 0x004DD1B0 File Offset: 0x004DB3B0
		public virtual void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D8A1 RID: 55457 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D8A2 RID: 55458 RVA: 0x004DD1BF File Offset: 0x004DB3BF
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnApplicationQuit()
		{
			this.Disconnect();
			return true;
		}

		// Token: 0x0600D8A3 RID: 55459 RVA: 0x004DD1C8 File Offset: 0x004DB3C8
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator restartRefreshCo(float _delay)
		{
			yield return new WaitForSeconds(_delay);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0600D8A4 RID: 55460 RVA: 0x004DD1E0 File Offset: 0x004DB3E0
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

		// Token: 0x0400A472 RID: 42098
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform owner;

		// Token: 0x0400A473 RID: 42099
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A474 RID: 42100
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isRefreshing;
	}
}
