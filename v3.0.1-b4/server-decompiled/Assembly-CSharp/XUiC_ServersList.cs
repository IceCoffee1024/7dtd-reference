using System;
using System.Collections.Generic;
using System.Linq;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DD2 RID: 3538
[Preserve]
public class XUiC_ServersList : XUiC_List<XUiC_ServersList.ListEntry>
{
	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06006C4F RID: 27727 RVA: 0x002A2D4C File Offset: 0x002A0F4C
	// (remove) Token: 0x06006C50 RID: 27728 RVA: 0x002A2D84 File Offset: 0x002A0F84
	public event Action<int> OnFilterResultsChanged;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06006C51 RID: 27729 RVA: 0x002A2DBC File Offset: 0x002A0FBC
	// (remove) Token: 0x06006C52 RID: 27730 RVA: 0x002A2DF4 File Offset: 0x002A0FF4
	public event Action CountsChanged;

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x06006C53 RID: 27731 RVA: 0x002A2E29 File Offset: 0x002A1029
	public XUiC_ServersList.EnumServerLists CurrentServerTypeList
	{
		get
		{
			return this.currentServerTypeList;
		}
	}

	// Token: 0x06006C54 RID: 27732 RVA: 0x002A2E34 File Offset: 0x002A1034
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

	// Token: 0x06006C55 RID: 27733 RVA: 0x002A2E95 File Offset: 0x002A1095
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
		this.stopListUpdateThread = false;
		ThreadManager.StartThread("ServerBrowserListUpdater", null, new ThreadManager.ThreadFunctionLoopDelegate(this.currentListUpdateThread), null, null, null, false, false);
	}

	// Token: 0x06006C56 RID: 27734 RVA: 0x002A2EC8 File Offset: 0x002A10C8
	public override void OnClose()
	{
		base.OnClose();
		this.stopListUpdateThread = true;
	}

	// Token: 0x06006C57 RID: 27735 RVA: 0x002A2ED8 File Offset: 0x002A10D8
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

	// Token: 0x06006C58 RID: 27736 RVA: 0x002A2F44 File Offset: 0x002A1144
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

	// Token: 0x06006C59 RID: 27737 RVA: 0x002A2F9C File Offset: 0x002A119C
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

	// Token: 0x06006C5A RID: 27738 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RefreshView(bool _resetFilter = false, bool _resetPage = true)
	{
	}

	// Token: 0x06006C5B RID: 27739 RVA: 0x002A2FD4 File Offset: 0x002A11D4
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

	// Token: 0x06006C5C RID: 27740 RVA: 0x002A3048 File Offset: 0x002A1248
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

	// Token: 0x06006C5D RID: 27741 RVA: 0x002A3154 File Offset: 0x002A1354
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

	// Token: 0x06006C5E RID: 27742 RVA: 0x002A31C4 File Offset: 0x002A13C4
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

	// Token: 0x06006C5F RID: 27743 RVA: 0x002A322C File Offset: 0x002A142C
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

	// Token: 0x06006C60 RID: 27744 RVA: 0x002A3278 File Offset: 0x002A1478
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

	// Token: 0x06006C61 RID: 27745 RVA: 0x002A3318 File Offset: 0x002A1518
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

	// Token: 0x06006C62 RID: 27746 RVA: 0x002A33AC File Offset: 0x002A15AC
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

	// Token: 0x06006C63 RID: 27747 RVA: 0x002A3424 File Offset: 0x002A1624
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

	// Token: 0x06006C64 RID: 27748 RVA: 0x002A34E0 File Offset: 0x002A16E0
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

	// Token: 0x06006C65 RID: 27749 RVA: 0x002A3550 File Offset: 0x002A1750
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

	// Token: 0x06006C66 RID: 27750 RVA: 0x002A35C4 File Offset: 0x002A17C4
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

	// Token: 0x06006C67 RID: 27751 RVA: 0x002A36AC File Offset: 0x002A18AC
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

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x06006C68 RID: 27752 RVA: 0x002A36E7 File Offset: 0x002A18E7
	// (set) Token: 0x06006C69 RID: 27753 RVA: 0x002A36EF File Offset: 0x002A18EF
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

	// Token: 0x06006C6A RID: 27754 RVA: 0x002A370C File Offset: 0x002A190C
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

	// Token: 0x0400526A RID: 21098
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ServerTypeFilterName = "servertype";

	// Token: 0x0400526B RID: 21099
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ServerNameFilterName = "servername";

	// Token: 0x0400526E RID: 21102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopListUpdateThread;

	// Token: 0x0400526F RID: 21103
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateCurrentList;

	// Token: 0x04005270 RID: 21104
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageUpdateRequired;

	// Token: 0x04005271 RID: 21105
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastPageUpdate;

	// Token: 0x04005272 RID: 21106
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ServersList.ListEntry> filteredEntriesTmp = new List<XUiC_ServersList.ListEntry>();

	// Token: 0x04005273 RID: 21107
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button[] sortButtons = new XUiC_Button[5];

	// Token: 0x04005274 RID: 21108
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortAscending = true;

	// Token: 0x04005275 RID: 21109
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<XUiC_ServersList.ListEntry, string> sortFuncString;

	// Token: 0x04005276 RID: 21110
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<XUiC_ServersList.ListEntry, int> sortFuncInt;

	// Token: 0x04005277 RID: 21111
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<XUiC_ServersList.ListEntry, int> sortDefaultFavHistory = delegate(XUiC_ServersList.ListEntry _line)
	{
		if (!_line.GameServerInfo.IsFavorite)
		{
			return _line.GameServerInfo.LastPlayedLinux;
		}
		return int.MaxValue;
	};

	// Token: 0x04005278 RID: 21112
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.EnumServerLists currentServerTypeList;

	// Token: 0x04005279 RID: 21113
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ServersList.UiServerFilter> currentFilters = new Dictionary<string, XUiC_ServersList.UiServerFilter>();

	// Token: 0x0400527A RID: 21114
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly GameServerInfo.UniqueIdEqualityComparer uniqueIdComparer = GameServerInfo.UniqueIdEqualityComparer.Instance;

	// Token: 0x0400527B RID: 21115
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_ServersList.EnumServerLists, int> serverCounts = new EnumDictionary<XUiC_ServersList.EnumServerLists, int>();

	// Token: 0x0400527C RID: 21116
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.EnumColumns sortColumn = XUiC_ServersList.EnumColumns.Count;

	// Token: 0x02000DD3 RID: 3539
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ServersList.ListEntry>
	{
		// Token: 0x06006C6C RID: 27756 RVA: 0x002A38D6 File Offset: 0x002A1AD6
		public ListEntry(GameServerInfo _serverInfo, XUiC_ServersList _serversList)
		{
			this.GameServerInfo = _serverInfo;
		}

		// Token: 0x06006C6D RID: 27757 RVA: 0x002A38E8 File Offset: 0x002A1AE8
		public override int CompareTo(XUiC_ServersList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.GameServerInfo.LastPlayedLinux.CompareTo(_otherEntry.GameServerInfo.LastPlayedLinux);
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x002A3918 File Offset: 0x002A1B18
		public override bool MatchesSearch(string _searchString)
		{
			return this.GameServerInfo.GetValue(GameInfoString.GameHost).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x0400527D RID: 21117
		public readonly GameServerInfo GameServerInfo;
	}

	// Token: 0x02000DD4 RID: 3540
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ServersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006C6F RID: 27759 RVA: 0x002A392C File Offset: 0x002A1B2C
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

		// Token: 0x06006C70 RID: 27760 RVA: 0x002A3961 File Offset: 0x002A1B61
		public override void Update(float _dt)
		{
			this.IsDirty = true;
			base.Update(_dt);
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06006C71 RID: 27761 RVA: 0x002A3974 File Offset: 0x002A1B74
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

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06006C72 RID: 27762 RVA: 0x002A39FA File Offset: 0x002A1BFA
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

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06006C73 RID: 27763 RVA: 0x002A3A2B File Offset: 0x002A1C2B
		[XuiXmlBinding("servericonatlas")]
		public string ServerIconAtlas
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "SymbolAtlas";
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06006C74 RID: 27764 RVA: 0x002A3A32 File Offset: 0x002A1C32
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

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06006C75 RID: 27765 RVA: 0x002A3A4B File Offset: 0x002A1C4B
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

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06006C76 RID: 27766 RVA: 0x002A3A64 File Offset: 0x002A1C64
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

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06006C77 RID: 27767 RVA: 0x002A3AA4 File Offset: 0x002A1CA4
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

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06006C78 RID: 27768 RVA: 0x002A3B2A File Offset: 0x002A1D2A
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

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06006C79 RID: 27769 RVA: 0x002A3B42 File Offset: 0x002A1D42
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

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06006C7A RID: 27770 RVA: 0x002A3B5B File Offset: 0x002A1D5B
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

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06006C7B RID: 27771 RVA: 0x002A3B73 File Offset: 0x002A1D73
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

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06006C7C RID: 27772 RVA: 0x002A3B8B File Offset: 0x002A1D8B
		[XuiXmlBinding("hasping")]
		public bool HasPing
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.entryData != null && this.entryData.GameServerInfo.IsCompatibleVersion && this.entryData.GameServerInfo.GetValue(GameInfoInt.Ping) >= 0;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06006C7D RID: 27773 RVA: 0x002A3BC1 File Offset: 0x002A1DC1
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

		// Token: 0x0400527E RID: 21118
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_ServersList parentList;
	}

	// Token: 0x02000DD5 RID: 3541
	public class UiServerFilter : IServerListInterface.ServerFilter
	{
		// Token: 0x06006C82 RID: 27778 RVA: 0x002A3C46 File Offset: 0x002A1E46
		public UiServerFilter(string _name, XUiC_ServersList.EnumServerLists _applyingTo, Func<XUiC_ServersList.ListEntry, bool> _func = null, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			: base(_name, _type, _intMinValue, _intMaxValue, _boolValue, _stringNeedle)
		{
			this.Func = _func;
			this.ApplyingLists = _applyingTo;
		}

		// Token: 0x0400527F RID: 21119
		public readonly Func<XUiC_ServersList.ListEntry, bool> Func;

		// Token: 0x04005280 RID: 21120
		public readonly XUiC_ServersList.EnumServerLists ApplyingLists;
	}

	// Token: 0x02000DD6 RID: 3542
	[Flags]
	public enum EnumServerLists
	{
		// Token: 0x04005282 RID: 21122
		None = 0,
		// Token: 0x04005283 RID: 21123
		Dedicated = 1,
		// Token: 0x04005284 RID: 21124
		Peer = 2,
		// Token: 0x04005285 RID: 21125
		Friends = 4,
		// Token: 0x04005286 RID: 21126
		History = 8,
		// Token: 0x04005287 RID: 21127
		LAN = 16,
		// Token: 0x04005288 RID: 21128
		Regular = 3,
		// Token: 0x04005289 RID: 21129
		Special = 28,
		// Token: 0x0400528A RID: 21130
		All = 31
	}

	// Token: 0x02000DD7 RID: 3543
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EnumColumns
	{
		// Token: 0x0400528C RID: 21132
		ServerName,
		// Token: 0x0400528D RID: 21133
		World,
		// Token: 0x0400528E RID: 21134
		Difficulty,
		// Token: 0x0400528F RID: 21135
		Players,
		// Token: 0x04005290 RID: 21136
		Ping,
		// Token: 0x04005291 RID: 21137
		Count
	}
}
