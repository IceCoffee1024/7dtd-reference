using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000898 RID: 2200
public class ServerListManager
{
	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0018FC54 File Offset: 0x0018DE54
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

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06003FDA RID: 16346 RVA: 0x0018FC6A File Offset: 0x0018DE6A
	// (set) Token: 0x06003FDB RID: 16347 RVA: 0x0018FC72 File Offset: 0x0018DE72
	public bool IsRefreshing
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06003FDC RID: 16348 RVA: 0x0018FC7B File Offset: 0x0018DE7B
	// (set) Token: 0x06003FDD RID: 16349 RVA: 0x0018FC83 File Offset: 0x0018DE83
	public bool IsPrefilteredSearch
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x0018FC8C File Offset: 0x0018DE8C
	public void StartSearch(List<IServerListInterface.ServerFilter> _activeFilters)
	{
		this.IsRefreshing = true;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StartSearch(_activeFilters);
		}
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x0018FCE4 File Offset: 0x0018DEE4
	public void StopSearch()
	{
		this.IsRefreshing = false;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StopSearch();
		}
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x0018FD3C File Offset: 0x0018DF3C
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

	// Token: 0x06003FE1 RID: 16353 RVA: 0x0018FDDC File Offset: 0x0018DFDC
	public void RegisterAdditionalServerList(IServerListInterface _serverList)
	{
		this.serverLists.Add(_serverList);
		if (_serverList.IsPrefiltered)
		{
			this.IsPrefilteredSearch = true;
		}
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x0018FDFC File Offset: 0x0018DFFC
	public void Disconnect()
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.Disconnect();
		}
		this.IsRefreshing = false;
	}

	// Token: 0x06003FE3 RID: 16355 RVA: 0x0018FE54 File Offset: 0x0018E054
	public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.RegisterGameServerFoundCallback(_serverFound, _maxResultsCallback, _errorCallback);
		}
	}

	// Token: 0x04003359 RID: 13145
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerListManager instance;

	// Token: 0x0400335C RID: 13148
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerListInterface> serverLists = new List<IServerListInterface>();
}
