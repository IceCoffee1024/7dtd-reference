using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x020008A4 RID: 2212
public class ServerListManager
{
	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x0600403E RID: 16446 RVA: 0x00191D24 File Offset: 0x0018FF24
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

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x0600403F RID: 16447 RVA: 0x00191D3A File Offset: 0x0018FF3A
	// (set) Token: 0x06004040 RID: 16448 RVA: 0x00191D42 File Offset: 0x0018FF42
	public bool IsRefreshing
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06004041 RID: 16449 RVA: 0x00191D4B File Offset: 0x0018FF4B
	// (set) Token: 0x06004042 RID: 16450 RVA: 0x00191D53 File Offset: 0x0018FF53
	public bool IsPrefilteredSearch
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x00191D5C File Offset: 0x0018FF5C
	public void StartSearch(List<IServerListInterface.ServerFilter> _activeFilters)
	{
		this.IsRefreshing = true;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StartSearch(_activeFilters);
		}
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x00191DB4 File Offset: 0x0018FFB4
	public void StopSearch()
	{
		this.IsRefreshing = false;
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.StopSearch();
		}
	}

	// Token: 0x06004045 RID: 16453 RVA: 0x00191E0C File Offset: 0x0019000C
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

	// Token: 0x06004046 RID: 16454 RVA: 0x00191EAC File Offset: 0x001900AC
	public void RegisterAdditionalServerList(IServerListInterface _serverList)
	{
		this.serverLists.Add(_serverList);
		if (_serverList.IsPrefiltered)
		{
			this.IsPrefilteredSearch = true;
		}
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00191ECC File Offset: 0x001900CC
	public void Disconnect()
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.Disconnect();
		}
		this.IsRefreshing = false;
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x00191F24 File Offset: 0x00190124
	public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
	{
		foreach (IServerListInterface serverListInterface in this.serverLists)
		{
			serverListInterface.RegisterGameServerFoundCallback(_serverFound, _maxResultsCallback, _errorCallback);
		}
	}

	// Token: 0x040033DA RID: 13274
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerListManager instance;

	// Token: 0x040033DD RID: 13277
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerListInterface> serverLists = new List<IServerListInterface>();
}
