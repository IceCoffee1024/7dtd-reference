using System;
using System.Collections.Generic;
using System.Linq;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E01 RID: 3585
[Preserve]
public class XUiC_ServersList : XUiC_List<XUiC_ServersList.ListEntry>
{
	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06006D0B RID: 27915 RVA: 0x002AAA98 File Offset: 0x002A8C98
	// (remove) Token: 0x06006D0C RID: 27916 RVA: 0x002AAAD0 File Offset: 0x002A8CD0
	public event Action<int> OnFilterResultsChanged;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06006D0D RID: 27917 RVA: 0x002AAB08 File Offset: 0x002A8D08
	// (remove) Token: 0x06006D0E RID: 27918 RVA: 0x002AAB40 File Offset: 0x002A8D40
	public event Action CountsChanged;

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x06006D0F RID: 27919 RVA: 0x002AAB75 File Offset: 0x002A8D75
	public XUiC_ServersList.EnumServerLists CurrentServerTypeList
	{
		get
		{
			return this.currentServerTypeList;
		}
	}

	// Token: 0x06006D10 RID: 27920 RVA: 0x002AAB80 File Offset: 0x002A8D80
	public override void Init()
	{
		base.Init();
		XUiController xuiController;
		if (base.TryGetChildController<XUiController>("serverlistheader", out xuiController))
		{
			for (int i = 0; i < 5; i++)
			{
				if (xuiController.TryGetChildController<XUiC_Button>(((XUiC_ServersList.EnumColumns)i).ToStringCached<XUiC_ServersList.EnumColumns>(), out this.sortButtons[i]))
				{
					this.sortButtons[i].OnPress += this.SortButton_OnPress;
				}
			}
		}
	}

	// Token: 0x06006D11 RID: 27921 RVA: 0x002AABE1 File Offset: 0x002A8DE1
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
		this.stopListUpdateThread = false;
		ThreadManager.StartThread("ServerBrowserListUpdater", null, new ThreadManager.ThreadFunctionLoopDelegate(this.currentListUpdateThread), null, null, null, false, false);
	}

	// Token: 0x06006D12 RID: 27922 RVA: 0x002AAC14 File Offset: 0x002A8E14
	public override void OnClose()
	{
		base.OnClose();
		this.stopListUpdateThread = true;
	}

	// Token: 0x06006D13 RID: 27923 RVA: 0x002AAC24 File Offset: 0x002A8E24
	public override void Update(float _dt)
	{
		bool flag = false;
		if (this.pageUpdateRequired && (double)(Time.realtimeSinceStartup - this.lastPageUpdate) > 0.1)
		{
			flag = true;
			this.pageUpdateRequired = false;
			this.lastPageUpdate = Time.realtimeSinceStartup;
			this.IsDirty = true;
		}
		base.Update(_dt);
		if (flag)
		{
			Action<int> onFilterResultsChanged = this.OnFilterResultsChanged;
			if (onFilterResultsChanged == null)
			{
				return;
			}
			onFilterResultsChanged(base.EntryCount);
		}
	}

	// Token: 0x06006D14 RID: 27924 RVA: 0x002AAC90 File Offset: 0x002A8E90
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		Dictionary<XUiC_ServersList.EnumServerLists, int> dictionary = this.serverCounts;
		lock (dictionary)
		{
			this.serverCounts.Clear();
		}
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06006D15 RID: 27925 RVA: 0x002AACE8 File Offset: 0x002A8EE8
	public void RefreshBindingListEntry(XUiC_ServersList.ListEntry _entry)
	{
		foreach (XUiC_List<XUiC_ServersList.ListEntry>.XUiC_ListEntry xuiC_ListEntry in this.listEntryControllers)
		{
			if (xuiC_ListEntry.GetEntry() == _entry)
			{
				xuiC_ListEntry.RefreshBindings();
				return;
			}
		}
	}

	// Token: 0x06006D16 RID: 27926 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RefreshView(bool _resetFilter = false, bool _resetPage = true)
	{
	}

	// Token: 0x06006D17 RID: 27927 RVA: 0x002AAD20 File Offset: 0x002A8F20
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnSearchInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (!string.IsNullOrEmpty(_text))
		{
			this.SetFilter(new XUiC_ServersList.UiServerFilter("servername", XUiC_ServersList.EnumServerLists.All, (XUiC_ServersList.ListEntry _entry) => _entry.MatchesSearch(_text), IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null));
		}
		else
		{
			this.SetFilter(new XUiC_ServersList.UiServerFilter("servername", XUiC_ServersList.EnumServerLists.All, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null));
		}
		this.updateCurrentList = true;
		this.IsDirty = true;
	}

	// Token: 0x06006D18 RID: 27928 RVA: 0x002AAD94 File Offset: 0x002A8F94
	public void SetServerTypeFilter(XUiC_ServersList.EnumServerLists _typelist)
	{
		Func<XUiC_ServersList.ListEntry, bool> func;
		switch (_typelist)
		{
		case XUiC_ServersList.EnumServerLists.Dedicated:
			func = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.IsDedicated;
			goto IL_00D8;
		case XUiC_ServersList.EnumServerLists.Peer:
			func = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.IsPeerToPeer;
			goto IL_00D8;
		case XUiC_ServersList.EnumServerLists.Regular:
			break;
		case XUiC_ServersList.EnumServerLists.Friends:
			func = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.IsFriends;
			goto IL_00D8;
		default:
			if (_typelist == XUiC_ServersList.EnumServerLists.History)
			{
				func = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.IsFavoriteHistory;
				goto IL_00D8;
			}
			if (_typelist == XUiC_ServersList.EnumServerLists.LAN)
			{
				func = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.IsLAN;
				goto IL_00D8;
			}
			break;
		}
		func = null;
		IL_00D8:
		Func<XUiC_ServersList.ListEntry, bool> func2 = func;
		this.currentServerTypeList = _typelist;
		this.SetFilter(new XUiC_ServersList.UiServerFilter("servertype", XUiC_ServersList.EnumServerLists.All, func2, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null));
		base.ClearSelection();
	}

	// Token: 0x06006D19 RID: 27929 RVA: 0x002AAEA0 File Offset: 0x002A90A0
	public void SetFilter(XUiC_ServersList.UiServerFilter _filter)
	{
		Dictionary<string, XUiC_ServersList.UiServerFilter> dictionary = this.currentFilters;
		lock (dictionary)
		{
			if (_filter.Func == null)
			{
				this.currentFilters.Remove(_filter.Name);
			}
			else
			{
				this.currentFilters[_filter.Name] = _filter;
			}
		}
		this.updateCurrentList = true;
	}

	// Token: 0x06006D1A RID: 27930 RVA: 0x002AAF10 File Offset: 0x002A9110
	public int GetActiveFilterCount()
	{
		Dictionary<string, XUiC_ServersList.UiServerFilter> dictionary = this.currentFilters;
		int num;
		lock (dictionary)
		{
			num = (this.currentFilters.ContainsKey("servertype") ? (this.currentFilters.Count - 1) : this.currentFilters.Count);
		}
		return num;
	}

	// Token: 0x06006D1B RID: 27931 RVA: 0x002AAF78 File Offset: 0x002A9178
	public int GetServerCount(XUiC_ServersList.EnumServerLists _list)
	{
		Dictionary<XUiC_ServersList.EnumServerLists, int> dictionary = this.serverCounts;
		int num;
		lock (dictionary)
		{
			this.serverCounts.TryGetValue(_list, out num);
		}
		return num;
	}

	// Token: 0x06006D1C RID: 27932 RVA: 0x002AAFC4 File Offset: 0x002A91C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateListFiltering(ref IEnumerable<XUiC_ServersList.ListEntry> _list)
	{
		Dictionary<string, XUiC_ServersList.UiServerFilter> dictionary = this.currentFilters;
		lock (dictionary)
		{
			foreach (KeyValuePair<string, XUiC_ServersList.UiServerFilter> keyValuePair in this.currentFilters)
			{
				if ((this.currentServerTypeList & keyValuePair.Value.ApplyingLists) != XUiC_ServersList.EnumServerLists.None)
				{
					_list = _list.Where<XUiC_ServersList.ListEntry>(keyValuePair.Value.Func);
				}
			}
		}
	}

	// Token: 0x06006D1D RID: 27933 RVA: 0x002AB064 File Offset: 0x002A9264
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateListSorting(ref IEnumerable<XUiC_ServersList.ListEntry> _list)
	{
		if (this.currentServerTypeList == XUiC_ServersList.EnumServerLists.History && this.sortFuncInt == null && this.sortFuncString == null)
		{
			_list = _list.OrderByDescending<XUiC_ServersList.ListEntry, int>(this.sortDefaultFavHistory);
		}
		if (this.sortFuncInt != null)
		{
			_list = (this.sortAscending ? _list.OrderBy<XUiC_ServersList.ListEntry, int>(this.sortFuncInt) : _list.OrderByDescending<XUiC_ServersList.ListEntry, int>(this.sortFuncInt));
		}
		if (this.sortFuncString != null)
		{
			_list = (this.sortAscending ? _list.OrderBy<XUiC_ServersList.ListEntry, string>(this.sortFuncString) : _list.OrderByDescending<XUiC_ServersList.ListEntry, string>(this.sortFuncString));
		}
	}

	// Token: 0x06006D1E RID: 27934 RVA: 0x002AB0F8 File Offset: 0x002A92F8
	[PublicizedFrom(EAccessModifier.Private)]
	public int indexOfGameServerInfo(GameServerInfo _gameServerInfo)
	{
		if (_gameServerInfo == null)
		{
			return -1;
		}
		int hashCode = this.uniqueIdComparer.GetHashCode(_gameServerInfo);
		int count = this.allEntries.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.uniqueIdComparer.GetHashCode(this.allEntries[i].GameServerInfo) == hashCode && this.uniqueIdComparer.Equals(_gameServerInfo, this.allEntries[i].GameServerInfo))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06006D1F RID: 27935 RVA: 0x002AB170 File Offset: 0x002A9370
	public bool AddGameServer(GameServerInfo _gameServerInfo, EServerRelationType _source)
	{
		int num = this.indexOfGameServerInfo(_gameServerInfo);
		bool flag;
		if (num >= 0)
		{
			GameServerInfo gameServerInfo = this.allEntries[num].GameServerInfo;
			this.addRemoveServerCount(gameServerInfo, false);
			gameServerInfo.Merge(_gameServerInfo, _source);
			this.addRemoveServerCount(gameServerInfo, true);
			XUiC_List<XUiC_ServersList.ListEntry>.XUiC_ListEntry xuiC_ListEntry = base.IsVisible(this.allEntries[num]);
			if (xuiC_ListEntry != null)
			{
				xuiC_ListEntry.IsDirty = true;
			}
			flag = false;
		}
		else
		{
			List<XUiC_ServersList.ListEntry> allEntries = this.allEntries;
			lock (allEntries)
			{
				this.allEntries.Add(new XUiC_ServersList.ListEntry(_gameServerInfo, this));
			}
			this.addRemoveServerCount(_gameServerInfo, true);
			flag = true;
		}
		this.updateCurrentList = true;
		return flag;
	}

	// Token: 0x06006D20 RID: 27936 RVA: 0x002AB22C File Offset: 0x002A942C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addRemoveServerCount(GameServerInfo _gsi, bool _add)
	{
		if (_gsi.IsDedicated)
		{
			this.addRemoveCountSingleListType(_add, XUiC_ServersList.EnumServerLists.Dedicated);
		}
		if (_gsi.IsPeerToPeer)
		{
			this.addRemoveCountSingleListType(_add, XUiC_ServersList.EnumServerLists.Peer);
		}
		if (_gsi.IsFriends)
		{
			this.addRemoveCountSingleListType(_add, XUiC_ServersList.EnumServerLists.Friends);
		}
		if (_gsi.IsFavoriteHistory)
		{
			this.addRemoveCountSingleListType(_add, XUiC_ServersList.EnumServerLists.History);
		}
		if (_gsi.IsLAN)
		{
			this.addRemoveCountSingleListType(_add, XUiC_ServersList.EnumServerLists.LAN);
		}
		Action countsChanged = this.CountsChanged;
		if (countsChanged == null)
		{
			return;
		}
		countsChanged();
	}

	// Token: 0x06006D21 RID: 27937 RVA: 0x002AB29C File Offset: 0x002A949C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addRemoveCountSingleListType(bool _add, XUiC_ServersList.EnumServerLists _list)
	{
		Dictionary<XUiC_ServersList.EnumServerLists, int> dictionary = this.serverCounts;
		lock (dictionary)
		{
			int num;
			if (this.serverCounts.TryGetValue(_list, out num))
			{
				this.serverCounts[_list] = num + (_add ? 1 : (-1));
			}
			else if (_add)
			{
				this.serverCounts[_list] = 1;
			}
		}
	}

	// Token: 0x06006D22 RID: 27938 RVA: 0x002AB310 File Offset: 0x002A9510
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentListUpdateThread(ThreadManager.ThreadInfo _tInfo)
	{
		if (this.stopListUpdateThread || _tInfo.TerminationRequested())
		{
			return -1;
		}
		if (!this.updateCurrentList)
		{
			return 50;
		}
		this.updateCurrentList = false;
		List<XUiC_ServersList.ListEntry> allEntries = this.allEntries;
		lock (allEntries)
		{
			this.filteredEntriesTmp.AddRange(this.allEntries);
		}
		IEnumerable<XUiC_ServersList.ListEntry> enumerable = this.filteredEntriesTmp;
		this.updateListFiltering(ref enumerable);
		this.updateListSorting(ref enumerable);
		this.filteredEntriesTmp = enumerable.ToList<XUiC_ServersList.ListEntry>();
		lock (this)
		{
			allEntries = this.filteredEntriesTmp;
			List<XUiC_ServersList.ListEntry> filteredEntries = this.filteredEntries;
			this.filteredEntries = allEntries;
			this.filteredEntriesTmp = filteredEntries;
		}
		this.pageUpdateRequired = true;
		this.filteredEntriesTmp.Clear();
		return 50;
	}

	// Token: 0x06006D23 RID: 27939 RVA: 0x002AB3F8 File Offset: 0x002A95F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortButton_OnPress(XUiController _sender, int _mouseButton)
	{
		for (int i = 0; i < this.sortButtons.Length; i++)
		{
			if (_sender == this.sortButtons[i])
			{
				this.updateSortType((XUiC_ServersList.EnumColumns)i);
				break;
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x06006D24 RID: 27940 RVA: 0x002AB433 File Offset: 0x002A9633
	// (set) Token: 0x06006D25 RID: 27941 RVA: 0x002AB43B File Offset: 0x002A963B
	[XuiXmlBinding("sortcolumn")]
	public XUiC_ServersList.EnumColumns SortColumn
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sortColumn;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == this.sortColumn)
			{
				return;
			}
			this.sortColumn = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006D26 RID: 27942 RVA: 0x002AB458 File Offset: 0x002A9658
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateSortType(XUiC_ServersList.EnumColumns _sortType)
	{
		switch (_sortType)
		{
		case XUiC_ServersList.EnumColumns.ServerName:
			this.sortFuncString = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.GetValue(GameInfoString.GameHost);
			this.sortFuncInt = null;
			break;
		case XUiC_ServersList.EnumColumns.World:
			this.sortFuncString = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.GetValue(GameInfoString.LevelName);
			this.sortFuncInt = null;
			break;
		case XUiC_ServersList.EnumColumns.Difficulty:
			this.sortFuncString = null;
			this.sortFuncInt = null;
			break;
		case XUiC_ServersList.EnumColumns.Players:
			this.sortFuncString = null;
			this.sortFuncInt = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.GetValue(GameInfoInt.CurrentPlayers);
			break;
		case XUiC_ServersList.EnumColumns.Ping:
			this.sortFuncString = null;
			this.sortFuncInt = (XUiC_ServersList.ListEntry _line) => _line.GameServerInfo.GetValue(GameInfoInt.Ping);
			break;
		default:
			this.sortFuncString = null;
			this.sortFuncInt = null;
			break;
		}
		if (this.sortColumn == _sortType)
		{
			if (!this.sortAscending)
			{
				this.sortColumn = XUiC_ServersList.EnumColumns.Count;
				this.sortFuncString = null;
				this.sortFuncInt = null;
				return;
			}
			this.sortAscending = false;
		}
		else
		{
			this.sortAscending = true;
		}
		this.sortColumn = _sortType;
		this.updateCurrentList = true;
	}

	// Token: 0x04005369 RID: 21353
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ServerTypeFilterName = "servertype";

	// Token: 0x0400536A RID: 21354
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ServerNameFilterName = "servername";

	// Token: 0x0400536D RID: 21357
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopListUpdateThread;

	// Token: 0x0400536E RID: 21358
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateCurrentList;

	// Token: 0x0400536F RID: 21359
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageUpdateRequired;

	// Token: 0x04005370 RID: 21360
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastPageUpdate;

	// Token: 0x04005371 RID: 21361
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ServersList.ListEntry> filteredEntriesTmp = new List<XUiC_ServersList.ListEntry>();

	// Token: 0x04005372 RID: 21362
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button[] sortButtons = new XUiC_Button[5];

	// Token: 0x04005373 RID: 21363
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortAscending = true;

	// Token: 0x04005374 RID: 21364
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<XUiC_ServersList.ListEntry, string> sortFuncString;

	// Token: 0x04005375 RID: 21365
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<XUiC_ServersList.ListEntry, int> sortFuncInt;

	// Token: 0x04005376 RID: 21366
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<XUiC_ServersList.ListEntry, int> sortDefaultFavHistory = delegate(XUiC_ServersList.ListEntry _line)
	{
		if (!_line.GameServerInfo.IsFavorite)
		{
			return _line.GameServerInfo.LastPlayedLinux;
		}
		return int.MaxValue;
	};

	// Token: 0x04005377 RID: 21367
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.EnumServerLists currentServerTypeList;

	// Token: 0x04005378 RID: 21368
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ServersList.UiServerFilter> currentFilters = new Dictionary<string, XUiC_ServersList.UiServerFilter>();

	// Token: 0x04005379 RID: 21369
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly GameServerInfo.UniqueIdEqualityComparer uniqueIdComparer = GameServerInfo.UniqueIdEqualityComparer.Instance;

	// Token: 0x0400537A RID: 21370
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_ServersList.EnumServerLists, int> serverCounts = new EnumDictionary<XUiC_ServersList.EnumServerLists, int>();

	// Token: 0x0400537B RID: 21371
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.EnumColumns sortColumn = XUiC_ServersList.EnumColumns.Count;

	// Token: 0x02000E02 RID: 3586
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ServersList.ListEntry>
	{
		// Token: 0x06006D28 RID: 27944 RVA: 0x002AB622 File Offset: 0x002A9822
		public ListEntry(GameServerInfo _serverInfo, XUiC_ServersList _serversList)
		{
			this.GameServerInfo = _serverInfo;
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x002AB634 File Offset: 0x002A9834
		public override int CompareTo(XUiC_ServersList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.GameServerInfo.LastPlayedLinux.CompareTo(_otherEntry.GameServerInfo.LastPlayedLinux);
		}

		// Token: 0x06006D2A RID: 27946 RVA: 0x002AB664 File Offset: 0x002A9864
		public override bool MatchesSearch(string _searchString)
		{
			return this.GameServerInfo.GetValue(GameInfoString.GameHost).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x0400537C RID: 21372
		public readonly GameServerInfo GameServerInfo;
	}

	// Token: 0x02000E03 RID: 3587
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ServersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006D2B RID: 27947 RVA: 0x002AB678 File Offset: 0x002A9878
		public override void Init()
		{
			base.Init();
			this.parentList = (XUiC_ServersList)this.list;
			base.GetChildById("favorite").OnPress += delegate(XUiController _, int _)
			{
				GameServerInfo gameServerInfo = this.entryData.GameServerInfo;
				((XUiC_ServersList)this.list).addRemoveServerCount(gameServerInfo, false);
				ServerInfoCache.Instance.ToggleFavorite(gameServerInfo);
				((XUiC_ServersList)this.list).addRemoveServerCount(gameServerInfo, true);
				this.IsDirty = true;
			};
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x002AB6AD File Offset: 0x002A98AD
		public override void Update(float _dt)
		{
			this.IsDirty = true;
			base.Update(_dt);
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06006D2D RID: 27949 RVA: 0x002AB6C0 File Offset: 0x002A98C0
		[XuiXmlBinding("servername")]
		public string ServerName
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this.entryData == null)
				{
					return "";
				}
				string displayTextImmediately = GeneratedTextManager.GetDisplayTextImmediately(this.entryData.GameServerInfo.ServerDisplayName, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.NotSupported);
				if (!GeneratedTextManager.IsFiltering(this.entryData.GameServerInfo.ServerDisplayName) && !GeneratedTextManager.IsFiltered(this.entryData.GameServerInfo.ServerDisplayName))
				{
					GeneratedTextManager.GetDisplayText(this.entryData.GameServerInfo.ServerDisplayName, delegate(string _)
					{
						this.parentList.RefreshBindingListEntry(this.entryData);
					}, false, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.NotSupported);
				}
				return displayTextImmediately;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06006D2E RID: 27950 RVA: 0x002AB746 File Offset: 0x002A9946
		[XuiXmlBinding("servericon")]
		public string ServerIcon
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this.entryData != null)
				{
					return PlatformManager.NativePlatform.Utils.GetCrossplayPlayerIcon(this.entryData.GameServerInfo.PlayGroup, true, EPlatformIdentifier.None);
				}
				return "";
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06006D2F RID: 27951 RVA: 0x002AB777 File Offset: 0x002A9977
		[XuiXmlBinding("servericonatlas")]
		public string ServerIconAtlas
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "SymbolAtlas";
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06006D30 RID: 27952 RVA: 0x002AB77E File Offset: 0x002A997E
		[XuiXmlBinding("playersonline")]
		public int PlayersOnline
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				if (entryData == null)
				{
					return 0;
				}
				return entryData.GameServerInfo.GetValue(GameInfoInt.CurrentPlayers);
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x002AB797 File Offset: 0x002A9997
		[XuiXmlBinding("playersmax")]
		public int PlayersMax
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				if (entryData == null)
				{
					return 0;
				}
				return entryData.GameServerInfo.GetValue(GameInfoInt.MaxPlayers);
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06006D32 RID: 27954 RVA: 0x002AB7B0 File Offset: 0x002A99B0
		[XuiXmlBinding("ping")]
		public string Ping
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this.entryData == null)
				{
					return "";
				}
				int value = this.entryData.GameServerInfo.GetValue(GameInfoInt.Ping);
				if (value < 0)
				{
					return "N/A";
				}
				return value.ToString();
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06006D33 RID: 27955 RVA: 0x002AB7F0 File Offset: 0x002A99F0
		[XuiXmlBinding("world")]
		public string World
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this.entryData == null)
				{
					return "";
				}
				string displayTextImmediately = GeneratedTextManager.GetDisplayTextImmediately(this.entryData.GameServerInfo.ServerWorldName, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.NotSupported);
				if (!GeneratedTextManager.IsFiltering(this.entryData.GameServerInfo.ServerWorldName) && !GeneratedTextManager.IsFiltered(this.entryData.GameServerInfo.ServerWorldName))
				{
					GeneratedTextManager.GetDisplayText(this.entryData.GameServerInfo.ServerWorldName, delegate(string _)
					{
						this.parentList.RefreshBindingListEntry(this.entryData);
					}, false, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.NotSupported);
				}
				return displayTextImmediately;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06006D34 RID: 27956 RVA: 0x002AB876 File Offset: 0x002A9A76
		[XuiXmlBinding("anticheatprotected")]
		public bool AntiCheatProtected
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				return entryData != null && entryData.GameServerInfo.EACEnabled;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06006D35 RID: 27957 RVA: 0x002AB88E File Offset: 0x002A9A8E
		[XuiXmlBinding("passwordprotected")]
		public bool PasswordColorProtected
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				return entryData != null && entryData.GameServerInfo.GetValue(GameInfoBool.IsPasswordProtected);
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06006D36 RID: 27958 RVA: 0x002AB8A7 File Offset: 0x002A9AA7
		[XuiXmlBinding("isfavorite")]
		public bool IsFavorite
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				return entryData != null && entryData.GameServerInfo.IsFavorite;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06006D37 RID: 27959 RVA: 0x002AB8BF File Offset: 0x002A9ABF
		[XuiXmlBinding("isdedicated")]
		public bool IsDedicated
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				return entryData != null && entryData.GameServerInfo.IsDedicated;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06006D38 RID: 27960 RVA: 0x002AB8D7 File Offset: 0x002A9AD7
		[XuiXmlBinding("hasping")]
		public bool HasPing
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.entryData != null && this.entryData.GameServerInfo.IsCompatibleVersion && this.entryData.GameServerInfo.GetValue(GameInfoInt.Ping) >= 0;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06006D39 RID: 27961 RVA: 0x002AB90D File Offset: 0x002A9B0D
		[XuiXmlBinding("sameversion")]
		public bool SameVersion
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				XUiC_ServersList.ListEntry entryData = this.entryData;
				return entryData == null || entryData.GameServerInfo.IsCompatibleVersion;
			}
		}

		// Token: 0x0400537D RID: 21373
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_ServersList parentList;
	}

	// Token: 0x02000E04 RID: 3588
	public class UiServerFilter : IServerListInterface.ServerFilter
	{
		// Token: 0x06006D3E RID: 27966 RVA: 0x002AB992 File Offset: 0x002A9B92
		public UiServerFilter(string _name, XUiC_ServersList.EnumServerLists _applyingTo, Func<XUiC_ServersList.ListEntry, bool> _func = null, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			: base(_name, _type, _intMinValue, _intMaxValue, _boolValue, _stringNeedle)
		{
			this.Func = _func;
			this.ApplyingLists = _applyingTo;
		}

		// Token: 0x0400537E RID: 21374
		public readonly Func<XUiC_ServersList.ListEntry, bool> Func;

		// Token: 0x0400537F RID: 21375
		public readonly XUiC_ServersList.EnumServerLists ApplyingLists;
	}

	// Token: 0x02000E05 RID: 3589
	[Flags]
	public enum EnumServerLists
	{
		// Token: 0x04005381 RID: 21377
		None = 0,
		// Token: 0x04005382 RID: 21378
		Dedicated = 1,
		// Token: 0x04005383 RID: 21379
		Peer = 2,
		// Token: 0x04005384 RID: 21380
		Friends = 4,
		// Token: 0x04005385 RID: 21381
		History = 8,
		// Token: 0x04005386 RID: 21382
		LAN = 16,
		// Token: 0x04005387 RID: 21383
		Regular = 3,
		// Token: 0x04005388 RID: 21384
		Special = 28,
		// Token: 0x04005389 RID: 21385
		All = 31
	}

	// Token: 0x02000E06 RID: 3590
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EnumColumns
	{
		// Token: 0x0400538B RID: 21387
		ServerName,
		// Token: 0x0400538C RID: 21388
		World,
		// Token: 0x0400538D RID: 21389
		Difficulty,
		// Token: 0x0400538E RID: 21390
		Players,
		// Token: 0x0400538F RID: 21391
		Ping,
		// Token: 0x04005390 RID: 21392
		Count
	}
}
