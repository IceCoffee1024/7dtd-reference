using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public class ServerListManager
{
	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06003FCC RID: 16332 RVA: 0x00191B14 File Offset: 0x0018FD14
	public static ServerListManager Instance
	{
		get
		{
			ServerListManager serverListManager;
			if ((serverListManager = ServerListManager.instance) == null)
			{
				serverListManager = (ServerListManager.instance = new ServerListManager());
			}
			return serverListManager;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06003FCD RID: 16333 RVA: 0x00191B2A File Offset: 0x0018FD2A
	// (set) Token: 0x06003FCE RID: 16334 RVA: 0x00191B32 File Offset: 0x0018FD32
	public bool IsRefreshing
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06003FCF RID: 16335 RVA: 0x00191B3B File Offset: 0x0018FD3B
	// (set) Token: 0x06003FD0 RID: 16336 RVA: 0x00191B43 File Offset: 0x0018FD43
	public bool IsPrefilteredSearch
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003FD1 RID: 16337 RVA: 0x00191B4C File Offset: 0x0018FD4C
	public void StartSearch(List<IServerListInterface.ServerFilter> _activeFilters)
	{
		this.IsRefreshing = true;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StartSearch(_activeFilters);
		}
	}

	// Token: 0x06003FD2 RID: 16338 RVA: 0x00191BA4 File Offset: 0x0018FDA4
	public void StopSearch()
	{
		this.IsRefreshing = false;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StopSearch();
		}
	}

	// Token: 0x06003FD3 RID: 16339 RVA: 0x00191BFC File Offset: 0x0018FDFC
	[PublicizedFrom(EAccessModifier.Private)]
	public ServerListManager()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		Application.wantsToQuit += delegate
		{
			this.Disconnect();
			return true;
		};
		IList<IServerListInterface> serverListInterfaces = PlatformManager.MultiPlatform.ServerListInterfaces;
		if (serverListInterfaces != null)
		{
			this.serverLists.AddRange(serverListInterfaces);
		}
		using (List<IServerListInterface>.Enumerator enumerator = this.serverLists.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsPrefiltered)
				{
					this.IsPrefilteredSearch = true;
				}
			}
		}
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x00191C9C File Offset: 0x0018FE9C
	public void RegisterAdditionalServerList(IServerListInterface _serverList)
	{
		this.serverLists.Add(_serverList);
		if (_serverList.IsPrefiltered)
		{
			this.IsPrefilteredSearch = true;
		}
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x00191CBC File Offset: 0x0018FEBC
	public void Disconnect()
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.Disconnect();
		}
		this.IsRefreshing = false;
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x00191D14 File Offset: 0x0018FF14
	public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.RegisterGameServerFoundCallback(_serverFound, _maxResultsCallback, _errorCallback);
		}
	}

	// Token: 0x040033C5 RID: 13253
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerListManager instance;

	// Token: 0x040033C8 RID: 13256
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerListInterface> serverLists = new List<IServerListInterface>();
}
