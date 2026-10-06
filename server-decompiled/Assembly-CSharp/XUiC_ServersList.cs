using System;
using System.Collections.Generic;
using System.Linq;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DF6 RID: 3574
[Preserve]
public class XUiC_ServersList : XUiC_List<XUiC_ServersList.ListEntry>
{
	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06006CE6 RID: 27878 RVA: 0x002A9570 File Offset: 0x002A7770
	// (remove) Token: 0x06006CE7 RID: 27879 RVA: 0x002A95A8 File Offset: 0x002A77A8
	public event Action<int> OnFilterResultsChanged;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06006CE8 RID: 27880 RVA: 0x002A95E0 File Offset: 0x002A77E0
	// (remove) Token: 0x06006CE9 RID: 27881 RVA: 0x002A9618 File Offset: 0x002A7818
	public event Action CountsChanged;

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x06006CEA RID: 27882 RVA: 0x002A964D File Offset: 0x002A784D
	public XUiC_ServersList.EnumServerLists CurrentServerTypeList
	{
		get
		{
			return this.currentServerTypeList;
		}
	}

	// Token: 0x06006CEB RID: 27883 RVA: 0x002A9658 File Offset: 0x002A7858
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

	// Token: 0x06006CEC RID: 27884 RVA: 0x002A96B9 File Offset: 0x002A78B9
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
		this.stopListUpdateThread = false;
		ThreadManager.StartThread("ServerBrowserListUpdater", null, new ThreadManager.ThreadFunctionLoopDelegate(this.currentListUpdateThread), null, null, null, false, false);
	}

	// Token: 0x06006CED RID: 27885 RVA: 0x002A96EC File Offset: 0x002A78EC
	public override void OnClose()
	{
		base.OnClose();
		this.stopListUpdateThread = true;
	}

	// Token: 0x06006CEE RID: 27886 RVA: 0x002A96FC File Offset: 0x002A78FC
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

	// Token: 0x06006CEF RID: 27887 RVA: 0x002A9768 File Offset: 0x002A7968
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

	// Token: 0x06006CF0 RID: 27888 RVA: 0x002A97C0 File Offset: 0x002A79C0
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

	// Token: 0x06006CF1 RID: 27889 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RefreshView(bool _resetFilter = false, bool _resetPage = true)
	{
	}

	// Token: 0x06006CF2 RID: 27890 RVA: 0x002A97F8 File Offset: 0x002A79F8
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

	// Token: 0x06006CF3 RID: 27891 RVA: 0x002A986C File Offset: 0x002A7A6C
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

	// Token: 0x06006CF4 RID: 27892 RVA: 0x002A9978 File Offset: 0x002A7B78
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

	// Token: 0x06006CF5 RID: 27893 RVA: 0x002A99E8 File Offset: 0x002A7BE8
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

	// Token: 0x06006CF6 RID: 27894 RVA: 0x002A9A50 File Offset: 0x002A7C50
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

	// Token: 0x06006CF7 RID: 27895 RVA: 0x002A9A9C File Offset: 0x002A7C9C
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

	// Token: 0x06006CF8 RID: 27896 RVA: 0x002A9B3C File Offset: 0x002A7D3C
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

	// Token: 0x06006CF9 RID: 27897 RVA: 0x002A9BD0 File Offset: 0x002A7DD0
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

	// Token: 0x06006CFA RID: 27898 RVA: 0x002A9C48 File Offset: 0x002A7E48
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

	// Token: 0x06006CFB RID: 27899 RVA: 0x002A9D04 File Offset: 0x002A7F04
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

	// Token: 0x06006CFC RID: 27900 RVA: 0x002A9D74 File Offset: 0x002A7F74
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

	// Token: 0x06006CFD RID: 27901 RVA: 0x002A9DE8 File Offset: 0x002A7FE8
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

	// Token: 0x06006CFE RID: 27902 RVA: 0x002A9ED0 File Offset: 0x002A80D0
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

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x06006CFF RID: 27903 RVA: 0x002A9F0B File Offset: 0x002A810B
	// (set) Token: 0x06006D00 RID: 27904 RVA: 0x002A9F13 File Offset: 0x002A8113
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

	// Token: 0x06006D01 RID: 27905 RVA: 0x002A9F30 File Offset: 0x002A8130
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

	// Token: 0x04005344 RID: 21316
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ServerTypeFilterName = "servertype";

	// Token: 0x04005345 RID: 21317
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ServerNameFilterName = "servername";

	// Token: 0x04005348 RID: 21320
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopListUpdateThread;

	// Token: 0x04005349 RID: 21321
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateCurrentList;

	// Token: 0x0400534A RID: 21322
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageUpdateRequired;

	// Token: 0x0400534B RID: 21323
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastPageUpdate;

	// Token: 0x0400534C RID: 21324
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ServersList.ListEntry> filteredEntriesTmp = new List<XUiC_ServersList.ListEntry>();

	// Token: 0x0400534D RID: 21325
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button[] sortButtons = new XUiC_Button[5];

	// Token: 0x0400534E RID: 21326
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortAscending = true;

	// Token: 0x0400534F RID: 21327
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<XUiC_ServersList.ListEntry, string> sortFuncString;

	// Token: 0x04005350 RID: 21328
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<XUiC_ServersList.ListEntry, int> sortFuncInt;

	// Token: 0x04005351 RID: 21329
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<XUiC_ServersList.ListEntry, int> sortDefaultFavHistory = delegate(XUiC_ServersList.ListEntry _line)
	{
		if (!_line.GameServerInfo.IsFavorite)
		{
			return _line.GameServerInfo.LastPlayedLinux;
		}
		return int.MaxValue;
	};

	// Token: 0x04005352 RID: 21330
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.EnumServerLists currentServerTypeList;

	// Token: 0x04005353 RID: 21331
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ServersList.UiServerFilter> currentFilters = new Dictionary<string, XUiC_ServersList.UiServerFilter>();

	// Token: 0x04005354 RID: 21332
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly GameServerInfo.UniqueIdEqualityComparer uniqueIdComparer = GameServerInfo.UniqueIdEqualityComparer.Instance;

	// Token: 0x04005355 RID: 21333
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_ServersList.EnumServerLists, int> serverCounts = new EnumDictionary<XUiC_ServersList.EnumServerLists, int>();

	// Token: 0x04005356 RID: 21334
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.EnumColumns sortColumn = XUiC_ServersList.EnumColumns.Count;

	// Token: 0x02000DF7 RID: 3575
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ServersList.ListEntry>
	{
		// Token: 0x06006D03 RID: 27907 RVA: 0x002AA0FA File Offset: 0x002A82FA
		public ListEntry(GameServerInfo _serverInfo, XUiC_ServersList _serversList)
		{
			this.GameServerInfo = _serverInfo;
		}

		// Token: 0x06006D04 RID: 27908 RVA: 0x002AA10C File Offset: 0x002A830C
		public override int CompareTo(XUiC_ServersList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.GameServerInfo.LastPlayedLinux.CompareTo(_otherEntry.GameServerInfo.LastPlayedLinux);
		}

		// Token: 0x06006D05 RID: 27909 RVA: 0x002AA13C File Offset: 0x002A833C
		public override bool MatchesSearch(string _searchString)
		{
			return this.GameServerInfo.GetValue(GameInfoString.GameHost).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005357 RID: 21335
		public readonly GameServerInfo GameServerInfo;
	}

	// Token: 0x02000DF8 RID: 3576
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ServersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006D06 RID: 27910 RVA: 0x002AA150 File Offset: 0x002A8350
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

		// Token: 0x06006D07 RID: 27911 RVA: 0x002AA185 File Offset: 0x002A8385
		public override void Update(float _dt)
		{
			this.IsDirty = true;
			base.Update(_dt);
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06006D08 RID: 27912 RVA: 0x002AA198 File Offset: 0x002A8398
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

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06006D09 RID: 27913 RVA: 0x002AA21E File Offset: 0x002A841E
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

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06006D0A RID: 27914 RVA: 0x002AA24F File Offset: 0x002A844F
		[XuiXmlBinding("servericonatlas")]
		public string ServerIconAtlas
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "SymbolAtlas";
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06006D0B RID: 27915 RVA: 0x002AA256 File Offset: 0x002A8456
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

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06006D0C RID: 27916 RVA: 0x002AA26F File Offset: 0x002A846F
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

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06006D0D RID: 27917 RVA: 0x002AA288 File Offset: 0x002A8488
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

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06006D0E RID: 27918 RVA: 0x002AA2C8 File Offset: 0x002A84C8
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

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06006D0F RID: 27919 RVA: 0x002AA34E File Offset: 0x002A854E
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

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06006D10 RID: 27920 RVA: 0x002AA366 File Offset: 0x002A8566
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

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06006D11 RID: 27921 RVA: 0x002AA37F File Offset: 0x002A857F
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

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06006D12 RID: 27922 RVA: 0x002AA397 File Offset: 0x002A8597
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

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06006D13 RID: 27923 RVA: 0x002AA3AF File Offset: 0x002A85AF
		[XuiXmlBinding("hasping")]
		public bool HasPing
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.entryData != null && this.entryData.GameServerInfo.IsCompatibleVersion && this.entryData.GameServerInfo.GetValue(GameInfoInt.Ping) >= 0;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06006D14 RID: 27924 RVA: 0x002AA3E5 File Offset: 0x002A85E5
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

		// Token: 0x04005358 RID: 21336
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_ServersList parentList;
	}

	// Token: 0x02000DF9 RID: 3577
	public class UiServerFilter : IServerListInterface.ServerFilter
	{
		// Token: 0x06006D19 RID: 27929 RVA: 0x002AA46A File Offset: 0x002A866A
		public UiServerFilter(string _name, XUiC_ServersList.EnumServerLists _applyingTo, Func<XUiC_ServersList.ListEntry, bool> _func = null, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			: base(_name, _type, _intMinValue, _intMaxValue, _boolValue, _stringNeedle)
		{
			this.Func = _func;
			this.ApplyingLists = _applyingTo;
		}

		// Token: 0x04005359 RID: 21337
		public readonly Func<XUiC_ServersList.ListEntry, bool> Func;

		// Token: 0x0400535A RID: 21338
		public readonly XUiC_ServersList.EnumServerLists ApplyingLists;
	}

	// Token: 0x02000DFA RID: 3578
	[Flags]
	public enum EnumServerLists
	{
		// Token: 0x0400535C RID: 21340
		None = 0,
		// Token: 0x0400535D RID: 21341
		Dedicated = 1,
		// Token: 0x0400535E RID: 21342
		Peer = 2,
		// Token: 0x0400535F RID: 21343
		Friends = 4,
		// Token: 0x04005360 RID: 21344
		History = 8,
		// Token: 0x04005361 RID: 21345
		LAN = 16,
		// Token: 0x04005362 RID: 21346
		Regular = 3,
		// Token: 0x04005363 RID: 21347
		Special = 28,
		// Token: 0x04005364 RID: 21348
		All = 31
	}

	// Token: 0x02000DFB RID: 3579
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EnumColumns
	{
		// Token: 0x04005366 RID: 21350
		ServerName,
		// Token: 0x04005367 RID: 21351
		World,
		// Token: 0x04005368 RID: 21352
		Difficulty,
		// Token: 0x04005369 RID: 21353
		Players,
		// Token: 0x0400536A RID: 21354
		Ping,
		// Token: 0x0400536B RID: 21355
		Count
	}
}
