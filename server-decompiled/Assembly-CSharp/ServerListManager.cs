using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
public class ServerListManager
{
	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06003FEF RID: 16367 RVA: 0x00192BBC File Offset: 0x00190DBC
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

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x00192BD2 File Offset: 0x00190DD2
	// (set) Token: 0x06003FF1 RID: 16369 RVA: 0x00192BDA File Offset: 0x00190DDA
	public bool IsRefreshing
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x00192BE3 File Offset: 0x00190DE3
	// (set) Token: 0x06003FF3 RID: 16371 RVA: 0x00192BEB File Offset: 0x00190DEB
	public bool IsPrefilteredSearch
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x00192BF4 File Offset: 0x00190DF4
	public void StartSearch(List<IServerListInterface.ServerFilter> _activeFilters)
	{
		this.IsRefreshing = true;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StartSearch(_activeFilters);
		}
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x00192C4C File Offset: 0x00190E4C
	public void StopSearch()
	{
		this.IsRefreshing = false;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StopSearch();
		}
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x00192CA4 File Offset: 0x00190EA4
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

	// Token: 0x06003FF7 RID: 16375 RVA: 0x00192D44 File Offset: 0x00190F44
	public void RegisterAdditionalServerList(IServerListInterface _serverList)
	{
		this.serverLists.Add(_serverList);
		if (_serverList.IsPrefiltered)
		{
			this.IsPrefilteredSearch = true;
		}
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x00192D64 File Offset: 0x00190F64
	public void Disconnect()
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.Disconnect();
		}
		this.IsRefreshing = false;
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x00192DBC File Offset: 0x00190FBC
	public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.RegisterGameServerFoundCallback(_serverFound, _maxResultsCallback, _errorCallback);
		}
	}

	// Token: 0x040033E5 RID: 13285
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerListManager instance;

	// Token: 0x040033E8 RID: 13288
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerListInterface> serverLists = new List<IServerListInterface>();
}
