using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E78 RID: 3704
[Preserve]
public abstract class XUiC_List<T> : XUiController where T : XUiListEntry<T>
{
	// Token: 0x140000AD RID: 173
	// (add) Token: 0x060072FB RID: 29435 RVA: 0x002BA504 File Offset: 0x002B8704
	// (remove) Token: 0x060072FC RID: 29436 RVA: 0x002BA53C File Offset: 0x002B873C
	public event XUiEvent_ListSelectionChangedEventHandler<T> SelectionChanged;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x060072FD RID: 29437 RVA: 0x002BA574 File Offset: 0x002B8774
	// (remove) Token: 0x060072FE RID: 29438 RVA: 0x002BA5AC File Offset: 0x002B87AC
	public event XUiEvent_ListPageNumberChangedEventHandler<T> PageNumberChanged;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x060072FF RID: 29439 RVA: 0x002BA5E4 File Offset: 0x002B87E4
	// (remove) Token: 0x06007300 RID: 29440 RVA: 0x002BA61C File Offset: 0x002B881C
	public event XUiEvent_ListEntryClickedEventHandler<T> ListEntryClicked;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x06007301 RID: 29441 RVA: 0x002BA654 File Offset: 0x002B8854
	// (remove) Token: 0x06007302 RID: 29442 RVA: 0x002BA68C File Offset: 0x002B888C
	public event XUiEvent_ListEntryDoubleClickedEventHandler<T> ListEntryDoubleClicked;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x06007303 RID: 29443 RVA: 0x002BA6C4 File Offset: 0x002B88C4
	// (remove) Token: 0x06007304 RID: 29444 RVA: 0x002BA6FC File Offset: 0x002B88FC
	public event XUiEvent_ListEntryHoveredEventHandler<T> ListEntryHovered;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x06007305 RID: 29445 RVA: 0x002BA734 File Offset: 0x002B8934
	// (remove) Token: 0x06007306 RID: 29446 RVA: 0x002BA76C File Offset: 0x002B896C
	public event XUiEvent_PageContentsChangedEventHandler<T> PageContentsChanged;

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x06007307 RID: 29447 RVA: 0x002BA7A1 File Offset: 0x002B89A1
	public int PageLength
	{
		get
		{
			if (this.PageSizeOverride > 0)
			{
				return this.PageSizeOverride;
			}
			XUiC_List<T>.XUiC_ListEntry[] array = this.listEntryControllers;
			if (array == null)
			{
				return 0;
			}
			return array.Length;
		}
	}

	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x06007308 RID: 29448 RVA: 0x002BA7C1 File Offset: 0x002B89C1
	// (set) Token: 0x06007309 RID: 29449 RVA: 0x002BA7CC File Offset: 0x002B89CC
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			int num = Mathf.Clamp(value, 0, this.LastPage);
			if (num == this.page)
			{
				return;
			}
			this.page = num;
			this.updatePageLabel();
			this.IsDirty = true;
			this.ClearSelection();
			this.CurrentBaseIndex = this.page * this.PageLength;
			XUiEvent_ListPageNumberChangedEventHandler<T> pageNumberChanged = this.PageNumberChanged;
			if (pageNumberChanged == null)
			{
				return;
			}
			pageNumberChanged(this, this.page);
		}
	}

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x0600730A RID: 29450 RVA: 0x002BA835 File Offset: 0x002B8A35
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x0600730B RID: 29451 RVA: 0x002BA858 File Offset: 0x002B8A58
	// (set) Token: 0x0600730C RID: 29452 RVA: 0x002BA860 File Offset: 0x002B8A60
	public XUiC_List<T>.XUiC_ListEntry SelectedEntry
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.selectedEntry;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (value == this.selectedEntry)
			{
				return;
			}
			T t = this.currentSelectedEntry;
			XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.selectedEntry;
			this.selectedEntry = null;
			if (xuiC_ListEntry != null)
			{
				xuiC_ListEntry.Selected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = true;
				this.currentSelectedEntry = this.selectedEntry.GetEntry();
				for (int i = 0; i < this.listEntryControllers.Length; i++)
				{
					if (this.selectedEntry == this.listEntryControllers[i])
					{
						this.selectedEntryIndex = this.CurrentBaseIndex + i;
						break;
					}
				}
			}
			else
			{
				this.currentSelectedEntry = default(T);
				this.selectedEntryIndex = -1;
			}
			if (t != this.currentSelectedEntry)
			{
				this.OnSelectionChanged(xuiC_ListEntry, this.selectedEntry);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x0600730D RID: 29453 RVA: 0x002BA934 File Offset: 0x002B8B34
	public T SelectedEntryData
	{
		get
		{
			XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.SelectedEntry;
			if (xuiC_ListEntry == null)
			{
				return default(T);
			}
			return xuiC_ListEntry.GetEntry();
		}
	}

	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x0600730E RID: 29454 RVA: 0x002BA95A File Offset: 0x002B8B5A
	// (set) Token: 0x0600730F RID: 29455 RVA: 0x002BA964 File Offset: 0x002B8B64
	public int SelectedEntryIndex
	{
		get
		{
			return this.selectedEntryIndex;
		}
		set
		{
			if (value < 0 || value >= this.EntryCount)
			{
				return;
			}
			if (this.selectedEntryIndex == value)
			{
				return;
			}
			this.forceEntryVisible(value);
			this.selectedEntryIndex = value;
			this.updateSelectedItemByIndex = true;
			this.updateCurrentPageContents();
			this.updateSelectedItemByIndex = false;
			this.SelectedEntry = this.listEntryControllers[value - this.CurrentBaseIndex];
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x06007310 RID: 29456 RVA: 0x002BA9C7 File Offset: 0x002B8BC7
	// (set) Token: 0x06007311 RID: 29457 RVA: 0x002BA9CF File Offset: 0x002B8BCF
	[XuiXmlAttribute("page_size_override", false)]
	public int PageSizeOverride
	{
		get
		{
			return this.pageSizeOverride;
		}
		set
		{
			if (value == this.pageSizeOverride)
			{
				return;
			}
			this.pageSizeOverride = value;
			if (this.PagingStepSize == XUiC_List<T>.EPagingStepSize.Page)
			{
				this.Page = 0;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x06007312 RID: 29458 RVA: 0x002BA9F8 File Offset: 0x002B8BF8
	// (set) Token: 0x06007313 RID: 29459 RVA: 0x002BAA00 File Offset: 0x002B8C00
	[XuiXmlAttribute("paging_step_size", false)]
	public XUiC_List<T>.EPagingStepSize PagingStepSize
	{
		get
		{
			return this.pagingStepSize;
		}
		set
		{
			if (value == this.pagingStepSize)
			{
				return;
			}
			this.pagingStepSize = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x06007314 RID: 29460 RVA: 0x002BAA1A File Offset: 0x002B8C1A
	// (set) Token: 0x06007315 RID: 29461 RVA: 0x002BAA22 File Offset: 0x002B8C22
	[XuiXmlAttribute("paging_overscroll", false)]
	public Vector2i PagingOverscroll
	{
		get
		{
			return this.pagingOverscroll;
		}
		set
		{
			if (value == this.pagingOverscroll)
			{
				return;
			}
			this.pagingOverscroll = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x06007316 RID: 29462 RVA: 0x002BAA41 File Offset: 0x002B8C41
	// (set) Token: 0x06007317 RID: 29463 RVA: 0x002BAA4C File Offset: 0x002B8C4C
	public int CurrentBaseIndex
	{
		get
		{
			return this.minIndex;
		}
		set
		{
			if (this.minIndex == value)
			{
				return;
			}
			if (value < 0)
			{
				value = Mathf.Max(value, this.PagingOverscroll.x);
			}
			if (value > this.EntryCount - this.PageLength)
			{
				XUiC_List<T>.EPagingStepSize epagingStepSize = this.PagingStepSize;
				if (epagingStepSize != XUiC_List<T>.EPagingStepSize.Page)
				{
					if (epagingStepSize != XUiC_List<T>.EPagingStepSize.SingleEntry)
					{
						throw new ArgumentOutOfRangeException();
					}
					value = Mathf.Min(value, this.EntryCount - this.PageLength + this.PagingOverscroll.y);
				}
				else
				{
					value = Mathf.Min(value, this.EntryCount - 1);
				}
			}
			this.minIndex = value;
			this.ClearSelection();
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x06007318 RID: 29464 RVA: 0x002BAAE9 File Offset: 0x002B8CE9
	[XuiXmlBinding("current_min_index")]
	public int CurrentPageMinIndex
	{
		get
		{
			return Mathf.Max(this.minIndex, 0);
		}
	}

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06007319 RID: 29465 RVA: 0x002BAAF7 File Offset: 0x002B8CF7
	[XuiXmlBinding("current_max_index")]
	public int CurrentPageMaxIndex
	{
		get
		{
			return Mathf.Min(this.minIndex + this.PageLength, this.EntryCount) - 1;
		}
	}

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x0600731A RID: 29466 RVA: 0x002BAB13 File Offset: 0x002B8D13
	[XuiXmlBinding("hasselection")]
	public bool HasSelection
	{
		get
		{
			return this.SelectedEntryData != null;
		}
	}

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x0600731B RID: 29467 RVA: 0x002BAB23 File Offset: 0x002B8D23
	[XuiXmlBinding("entrycountfiltered")]
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x0600731C RID: 29468 RVA: 0x002BAB30 File Offset: 0x002B8D30
	public T GetEntry(int _index)
	{
		return this.filteredEntries[_index];
	}

	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x0600731D RID: 29469 RVA: 0x002BAB3E File Offset: 0x002B8D3E
	[XuiXmlBinding("entrycounttotal")]
	public int UnfilteredEntryCount
	{
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x0600731E RID: 29470 RVA: 0x002BAB4B File Offset: 0x002B8D4B
	// (set) Token: 0x0600731F RID: 29471 RVA: 0x002BAB53 File Offset: 0x002B8D53
	[XuiXmlAttribute("clear_selection_on_open", false)]
	public bool ClearSelectionOnOpenClose { get; set; } = true;

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x06007320 RID: 29472 RVA: 0x002BAB5C File Offset: 0x002B8D5C
	// (set) Token: 0x06007321 RID: 29473 RVA: 0x002BAB64 File Offset: 0x002B8D64
	[XuiXmlAttribute("clear_searchtext_on_open", false)]
	public bool ClearSearchTextOnOpenClose { get; set; }

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06007322 RID: 29474 RVA: 0x002BAB6D File Offset: 0x002B8D6D
	// (set) Token: 0x06007323 RID: 29475 RVA: 0x002BAB75 File Offset: 0x002B8D75
	[XuiXmlAttribute("selectable", false)]
	public bool SelectableEntries { get; set; } = true;

	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06007324 RID: 29476 RVA: 0x002BAB7E File Offset: 0x002B8D7E
	// (set) Token: 0x06007325 RID: 29477 RVA: 0x002BAB86 File Offset: 0x002B8D86
	[XuiXmlAttribute("cursor_controllable", false)]
	public bool CursorControllable { get; set; }

	// Token: 0x06007326 RID: 29478 RVA: 0x002BAB90 File Offset: 0x002B8D90
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("list");
		if (childById == null)
		{
			Log.Warning("[XUi] List controller without a 'list' child element! (window group '" + base.WindowGroup.Id + "')");
		}
		else if (this.CursorControllable)
		{
			XUiV_Grid xuiV_Grid = childById.ViewComponent as XUiV_Grid;
			if (xuiV_Grid != null)
			{
				if (xuiV_Grid.Arrangement == UIGrid.Arrangement.Horizontal)
				{
					this.columns = xuiV_Grid.Columns;
					this.rows = this.listEntryControllers.Length / this.columns;
				}
				else
				{
					this.rows = xuiV_Grid.Rows;
					this.columns = this.listEntryControllers.Length / this.rows;
				}
			}
			XUiV_Table xuiV_Table = childById.ViewComponent as XUiV_Table;
			if (xuiV_Table != null)
			{
				this.columns = xuiV_Table.Columns;
				this.rows = this.listEntryControllers.Length / this.columns;
			}
		}
		this.RebuildList(true);
	}

	// Token: 0x06007327 RID: 29479 RVA: 0x002BAC73 File Offset: 0x002B8E73
	public virtual void RebuildList(bool _resetFilter = false)
	{
		this.ClearSelection();
		List<T> list = this.filteredEntries;
		if (list != null)
		{
			list.Clear();
		}
		this.RefreshView(_resetFilter, true);
	}

	// Token: 0x06007328 RID: 29480 RVA: 0x002BAC94 File Offset: 0x002B8E94
	public virtual void RefreshView(bool _resetFilter = false, bool _resetPage = true)
	{
		if (_resetFilter)
		{
			if (this.searchBox != null)
			{
				this.searchBox.Text = "";
			}
			if (this.searchBox2 != null)
			{
				this.searchBox2.Text = "";
			}
		}
		string text;
		if (this.searchBox == null)
		{
			XUiC_CaptionedTextBox xuiC_CaptionedTextBox = this.searchBox2;
			text = ((xuiC_CaptionedTextBox != null) ? xuiC_CaptionedTextBox.Text : null);
		}
		else
		{
			text = this.searchBox.Text;
		}
		this.OnSearchInputChanged(this, text, true);
		if (!_resetPage)
		{
			return;
		}
		XUiC_List<T>.EPagingStepSize epagingStepSize = this.PagingStepSize;
		if (epagingStepSize == XUiC_List<T>.EPagingStepSize.Page)
		{
			this.Page = 0;
			return;
		}
		if (epagingStepSize != XUiC_List<T>.EPagingStepSize.SingleEntry)
		{
			throw new ArgumentOutOfRangeException();
		}
		this.CurrentBaseIndex = this.PagingOverscroll.x;
	}

	// Token: 0x06007329 RID: 29481 RVA: 0x002BAD38 File Offset: 0x002B8F38
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List<T>.XUiC_ListEntry IsVisible(T _value)
	{
		foreach (XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry in this.listEntryControllers)
		{
			T entry = xuiC_ListEntry.GetEntry();
			if (entry != null && entry == _value)
			{
				return xuiC_ListEntry;
			}
		}
		return null;
	}

	// Token: 0x0600732A RID: 29482 RVA: 0x002BAD7E File Offset: 0x002B8F7E
	[XuiBindEvent("OnPageChanged", "pager")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPageChanged()
	{
		this.Page = this.pager.CurrentPageNumber;
	}

	// Token: 0x0600732B RID: 29483 RVA: 0x002BAD91 File Offset: 0x002B8F91
	[XuiBindEvent("OnScroll", null)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		this.ChangePage((_delta > 0f) ? (-1) : 1);
	}

	// Token: 0x0600732C RID: 29484 RVA: 0x002BADA8 File Offset: 0x002B8FA8
	public void ChangePage(int _offset)
	{
		XUiC_List<T>.EPagingStepSize epagingStepSize = this.PagingStepSize;
		if (epagingStepSize == XUiC_List<T>.EPagingStepSize.Page)
		{
			this.Page += _offset;
			return;
		}
		if (epagingStepSize != XUiC_List<T>.EPagingStepSize.SingleEntry)
		{
			throw new ArgumentOutOfRangeException();
		}
		this.CurrentBaseIndex += _offset;
	}

	// Token: 0x0600732D RID: 29485 RVA: 0x002BADE8 File Offset: 0x002B8FE8
	[XuiBindEvent("OnSubmitHandler", "searchBox")]
	[XuiBindEvent("OnSubmitHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSearchInputSubmit(XUiController _sender, string _text)
	{
		this.OnSearchInputChanged(_sender, _text, false);
	}

	// Token: 0x0600732E RID: 29486 RVA: 0x002BADF3 File Offset: 0x002B8FF3
	[XuiBindEvent("OnChangeHandler", "searchBox")]
	[XuiBindEvent("OnChangeHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSearchInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.FilterResults(_text);
		this.IsDirty = true;
	}

	// Token: 0x0600732F RID: 29487 RVA: 0x002BAE03 File Offset: 0x002B9003
	public IReadOnlyList<T> AllEntries()
	{
		return this.allEntries;
	}

	// Token: 0x06007330 RID: 29488 RVA: 0x002BAE0C File Offset: 0x002B900C
	public T GetEntryByIndex(int _index)
	{
		if (_index < 0 || _index >= this.filteredEntries.Count)
		{
			return default(T);
		}
		return this.filteredEntries[_index];
	}

	// Token: 0x06007331 RID: 29489 RVA: 0x002BAE44 File Offset: 0x002B9044
	public void GetCurrentPageEntries(List<T> _entries)
	{
		if (_entries == null)
		{
			throw new ArgumentNullException("_entries");
		}
		_entries.Clear();
		for (int i = 0; i < this.PageLength; i++)
		{
			int num = i + this.CurrentBaseIndex;
			if (num < this.filteredEntries.Count)
			{
				_entries.Add(this.filteredEntries[num]);
			}
		}
	}

	// Token: 0x06007332 RID: 29490 RVA: 0x002BAEA0 File Offset: 0x002B90A0
	public void SetFilter(string _filter)
	{
		if (this.searchBox != null)
		{
			this.searchBox.Text = _filter;
			this.IsDirty = true;
			return;
		}
		if (this.searchBox2 != null)
		{
			this.searchBox2.Text = _filter;
			this.IsDirty = true;
			return;
		}
		Log.Error("Can not set filter without search box");
	}

	// Token: 0x06007333 RID: 29491 RVA: 0x002BAEF0 File Offset: 0x002B90F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FilterResults(string _textMatch)
	{
		if (_textMatch == null)
		{
			this.filteredEntries.Clear();
			this.filteredEntries.AddRange(this.allEntries);
			return;
		}
		if (_textMatch == this.previousMatch && this.filteredEntries.Count == this.allEntries.Count)
		{
			return;
		}
		this.previousMatch = _textMatch;
		this.filteredEntries.Clear();
		if (_textMatch.Length > 0)
		{
			using (List<T>.Enumerator enumerator = this.allEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					if (t.MatchesSearch(_textMatch))
					{
						this.filteredEntries.Add(t);
					}
				}
				return;
			}
		}
		this.filteredEntries.AddRange(this.allEntries);
	}

	// Token: 0x06007334 RID: 29492 RVA: 0x002BAFCC File Offset: 0x002B91CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSelectionChanged(XUiC_List<T>.XUiC_ListEntry _previousEntry, XUiC_List<T>.XUiC_ListEntry _newEntry)
	{
		if (this.ignoreSelectionChange)
		{
			return;
		}
		XUiEvent_ListSelectionChangedEventHandler<T> selectionChanged = this.SelectionChanged;
		if (selectionChanged != null)
		{
			selectionChanged(this, (_previousEntry != null) ? _previousEntry.GetEntry() : default(T), (_newEntry != null) ? _newEntry.GetEntry() : default(T));
		}
		if (this.SelectableEntries)
		{
			return;
		}
		this.ignoreSelectionChange = true;
		this.ClearSelection();
		this.ignoreSelectionChange = false;
	}

	// Token: 0x06007335 RID: 29493 RVA: 0x002BB03C File Offset: 0x002B923C
	[PublicizedFrom(EAccessModifier.Private)]
	public void goToPageOfCurrentlySelectedEntry()
	{
		if (this.currentSelectedEntry == null)
		{
			return;
		}
		T t = this.currentSelectedEntry;
		bool flag = false;
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i] == this.currentSelectedEntry)
			{
				this.forceEntryVisible(i);
				flag = true;
				break;
			}
		}
		this.currentSelectedEntry = (flag ? t : default(T));
	}

	// Token: 0x06007336 RID: 29494 RVA: 0x002BB0B4 File Offset: 0x002B92B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void forceEntryVisible(int _index)
	{
		if (_index < 0 || _index >= this.filteredEntries.Count)
		{
			return;
		}
		if (_index >= this.CurrentBaseIndex && _index <= this.CurrentPageMaxIndex)
		{
			return;
		}
		XUiC_List<T>.EPagingStepSize epagingStepSize = this.PagingStepSize;
		if (epagingStepSize == XUiC_List<T>.EPagingStepSize.Page)
		{
			this.Page = _index / this.PageLength;
			return;
		}
		if (epagingStepSize != XUiC_List<T>.EPagingStepSize.SingleEntry)
		{
			throw new ArgumentOutOfRangeException();
		}
		this.CurrentBaseIndex = _index;
	}

	// Token: 0x06007337 RID: 29495 RVA: 0x002BB114 File Offset: 0x002B9314
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCurrentPageContents()
	{
		if (this.filteredEntries == null)
		{
			Log.Error("filteredEntries is null!");
			return;
		}
		for (int i = 0; i < this.listEntryControllers.Length; i++)
		{
			int num = i + this.CurrentBaseIndex;
			XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.listEntryControllers[i];
			if (xuiC_ListEntry == null)
			{
				Log.Error("listEntry is null! {0} items in listEntryControllers", new object[] { this.listEntryControllers.Length });
				return;
			}
			if (num >= 0 && num < this.filteredEntries.Count && i < this.PageLength)
			{
				xuiC_ListEntry.SetEntry(this.filteredEntries[num]);
			}
			else
			{
				xuiC_ListEntry.SetEntry(default(T));
				if (xuiC_ListEntry.Selected)
				{
					xuiC_ListEntry.Selected = false;
				}
			}
			if (!this.updateSelectedItemByIndex && this.currentSelectedEntry != null && this.currentSelectedEntry == xuiC_ListEntry.GetEntry())
			{
				this.SelectedEntry = xuiC_ListEntry;
			}
		}
	}

	// Token: 0x06007338 RID: 29496 RVA: 0x002BB204 File Offset: 0x002B9404
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePageLabel()
	{
		if (this.pager == null)
		{
			return;
		}
		this.pager.LastPageNumber = this.LastPage;
		this.pager.CurrentPageNumber = this.page;
	}

	// Token: 0x06007339 RID: 29497 RVA: 0x002BB234 File Offset: 0x002B9434
	public override void Update(float _dt)
	{
		if (this.SelectableEntries && this.CursorControllable && this.columns > 0 && this.rows > 0 && PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && !this.xui.playerUI.windowManager.IsInputActive())
		{
			PlayerActionsGUI guiactions = this.windowGroup.playerUI.playerInput.GUIActions;
			if (guiactions.Left.WasPressed)
			{
				this.SelectedEntryIndex = Math.Max(0, this.SelectedEntryIndex - this.rows);
			}
			if (guiactions.Right.WasPressed)
			{
				this.SelectedEntryIndex = Math.Min(this.EntryCount - 1, this.SelectedEntryIndex + this.rows);
			}
			if (guiactions.Up.WasPressed)
			{
				this.SelectedEntryIndex = Math.Max(0, this.SelectedEntryIndex - 1);
			}
			if (guiactions.Down.WasPressed)
			{
				this.SelectedEntryIndex = Math.Min(this.EntryCount - 1, this.SelectedEntryIndex + 1);
			}
		}
		if (this.IsDirty)
		{
			this.goToPageOfCurrentlySelectedEntry();
			if (this.page > this.LastPage)
			{
				this.Page = this.LastPage;
			}
			this.updateCurrentPageContents();
			if (this.SelectedEntryData != this.currentSelectedEntry)
			{
				this.ClearSelection();
			}
			this.updatePageLabel();
			XUiEvent_PageContentsChangedEventHandler<T> pageContentsChanged = this.PageContentsChanged;
			if (pageContentsChanged != null)
			{
				pageContentsChanged(this);
			}
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0600733A RID: 29498 RVA: 0x002BB3C8 File Offset: 0x002B95C8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.ClearSelectionOnOpenClose)
		{
			this.ClearSelection();
		}
		if (this.ClearSearchTextOnOpenClose)
		{
			if (this.searchBox != null)
			{
				this.searchBox.Text = string.Empty;
				this.OnSearchInputChanged(this, string.Empty, true);
			}
			if (this.searchBox2 != null)
			{
				this.searchBox2.Text = string.Empty;
				this.OnSearchInputChanged(this, string.Empty, true);
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x0600733B RID: 29499 RVA: 0x002BB444 File Offset: 0x002B9644
	public override void OnClose()
	{
		base.OnClose();
		if (this.ClearSelectionOnOpenClose)
		{
			this.ClearSelection();
		}
		if (this.ClearSearchTextOnOpenClose)
		{
			if (this.searchBox != null)
			{
				this.searchBox.Text = "";
			}
			if (this.searchBox2 != null)
			{
				this.searchBox2.Text = "";
			}
		}
	}

	// Token: 0x0600733C RID: 29500 RVA: 0x002BB49D File Offset: 0x002B969D
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x0600733D RID: 29501 RVA: 0x002BB4A6 File Offset: 0x002B96A6
	public void SelectCursorElementForSelectedEntry()
	{
		XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.SelectedEntry;
		if (xuiC_ListEntry == null)
		{
			return;
		}
		xuiC_ListEntry.SelectCursorElement(true, false);
	}

	// Token: 0x0600733E RID: 29502 RVA: 0x002BB4BB File Offset: 0x002B96BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnListEntryClicked(T _entry)
	{
		XUiEvent_ListEntryClickedEventHandler<T> listEntryClicked = this.ListEntryClicked;
		if (listEntryClicked == null)
		{
			return;
		}
		listEntryClicked(this, _entry);
	}

	// Token: 0x0600733F RID: 29503 RVA: 0x002BB4CF File Offset: 0x002B96CF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnListEntryDoubleClicked(T _entry)
	{
		XUiEvent_ListEntryDoubleClickedEventHandler<T> listEntryDoubleClicked = this.ListEntryDoubleClicked;
		if (listEntryDoubleClicked == null)
		{
			return;
		}
		listEntryDoubleClicked(this, _entry);
	}

	// Token: 0x06007340 RID: 29504 RVA: 0x002BB4E3 File Offset: 0x002B96E3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnListEntryHovered(T _entry, bool _isOver)
	{
		XUiEvent_ListEntryHoveredEventHandler<T> listEntryHovered = this.ListEntryHovered;
		if (listEntryHovered == null)
		{
			return;
		}
		listEntryHovered(this, _entry, _isOver);
	}

	// Token: 0x06007341 RID: 29505 RVA: 0x002BB4F8 File Offset: 0x002B96F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List()
	{
	}

	// Token: 0x040055A2 RID: 21922
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> filteredEntries = new List<T>();

	// Token: 0x040055A3 RID: 21923
	[PublicizedFrom(EAccessModifier.Protected)]
	public T currentSelectedEntry;

	// Token: 0x040055A4 RID: 21924
	[XuiBindComponent("list.", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_List<T>.XUiC_ListEntry[] listEntryControllers;

	// Token: 0x040055A5 RID: 21925
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List<T>.XUiC_ListEntry selectedEntry;

	// Token: 0x040055A6 RID: 21926
	[PublicizedFrom(EAccessModifier.Protected)]
	public int selectedEntryIndex = -1;

	// Token: 0x040055A7 RID: 21927
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool updateSelectedItemByIndex;

	// Token: 0x040055A8 RID: 21928
	[PublicizedFrom(EAccessModifier.Private)]
	public int pageSizeOverride = -1;

	// Token: 0x040055A9 RID: 21929
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_List<T>.EPagingStepSize pagingStepSize;

	// Token: 0x040055AA RID: 21930
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i pagingOverscroll;

	// Token: 0x040055AB RID: 21931
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex;

	// Token: 0x040055AC RID: 21932
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040055AD RID: 21933
	[XuiBindComponent("searchInput", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TextInput searchBox;

	// Token: 0x040055AE RID: 21934
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_CaptionedTextBox searchBox2;

	// Token: 0x040055AF RID: 21935
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Paging pager;

	// Token: 0x040055B0 RID: 21936
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<T> allEntries = new List<T>();

	// Token: 0x040055B5 RID: 21941
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x040055B6 RID: 21942
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x040055B7 RID: 21943
	[PublicizedFrom(EAccessModifier.Protected)]
	public string previousMatch = "";

	// Token: 0x040055B8 RID: 21944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreSelectionChange;

	// Token: 0x02000E79 RID: 3705
	public enum EPagingStepSize
	{
		// Token: 0x040055BA RID: 21946
		Page,
		// Token: 0x040055BB RID: 21947
		SingleEntry
	}

	// Token: 0x02000E7A RID: 3706
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class XUiC_ListEntry : XUiController
	{
		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06007342 RID: 29506 RVA: 0x002BB548 File Offset: 0x002B9748
		[XuiXmlBinding("hasentry")]
		public bool HasEntry
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.entryData != null;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06007343 RID: 29507 RVA: 0x002BB558 File Offset: 0x002B9758
		[XuiXmlBinding("hovered")]
		public bool IsHovered
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.isHovered || this.forceHovered;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06007344 RID: 29508 RVA: 0x002BB56A File Offset: 0x002B976A
		// (set) Token: 0x06007345 RID: 29509 RVA: 0x002BB574 File Offset: 0x002B9774
		[XuiXmlBinding("isselected")]
		public new bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				if (value)
				{
					if (this.list.SelectedEntry != null)
					{
						this.list.SelectedEntry.selected = false;
					}
				}
				else if (this.list.SelectedEntry == this)
				{
					this.selected = false;
					this.list.ClearSelection();
				}
				this.selected = value;
				if (this.selected)
				{
					this.list.SelectedEntry = this;
				}
				this.IsDirty = true;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06007346 RID: 29510 RVA: 0x002BB5E6 File Offset: 0x002B97E6
		// (set) Token: 0x06007347 RID: 29511 RVA: 0x002BB5EE File Offset: 0x002B97EE
		public bool ForceHovered
		{
			get
			{
				return this.forceHovered;
			}
			set
			{
				if (value == this.forceHovered)
				{
					return;
				}
				this.forceHovered = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x002BB608 File Offset: 0x002B9808
		public override void Init()
		{
			base.Init();
			base.ViewComponent.Enabled = this.HasEntry;
			this.IsDirty = true;
		}

		// Token: 0x06007349 RID: 29513 RVA: 0x002BB628 File Offset: 0x002B9828
		[XuiBindEvent("OnScroll", null)]
		[PublicizedFrom(EAccessModifier.Private)]
		public void Event_OnScroll(XUiController _sender, float _delta)
		{
			this.list.HandleOnScroll(this, _delta);
		}

		// Token: 0x0600734A RID: 29514 RVA: 0x002BB637 File Offset: 0x002B9837
		[XuiBindEvent("OnPress", null)]
		public void Event_OnPress(XUiController _sender, int _mouseButton)
		{
			if (!this.Selected)
			{
				this.Selected = true;
			}
			this.list.OnListEntryClicked(this.entryData);
		}

		// Token: 0x0600734B RID: 29515 RVA: 0x002BB659 File Offset: 0x002B9859
		[XuiBindEvent("OnDoubleClick", null)]
		public void Event_OnDoubleClick(XUiController _sender, int _mouseButton)
		{
			this.list.OnListEntryDoubleClicked(this.entryData);
		}

		// Token: 0x0600734C RID: 29516 RVA: 0x002BB66C File Offset: 0x002B986C
		[XuiBindEvent("OnHover", null)]
		public void Event_OnHover(XUiController _sender, bool _isOver)
		{
			this.list.OnListEntryHovered(this.entryData, _isOver);
		}

		// Token: 0x0600734D RID: 29517 RVA: 0x002BB680 File Offset: 0x002B9880
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			this.isHovered = _isOver;
			this.IsDirty = true;
			base.OnHovered(_isOver);
		}

		// Token: 0x0600734E RID: 29518 RVA: 0x002BB698 File Offset: 0x002B9898
		public override void Update(float _dt)
		{
			base.Update(_dt);
			T t = this.entryData;
			if (t != null && t.UiDirty)
			{
				this.IsDirty = true;
			}
			if (base.handleDirtyUpdateDefault() && this.entryData != null)
			{
				this.entryData.UiDirty = false;
			}
		}

		// Token: 0x0600734F RID: 29519 RVA: 0x002BB6F2 File Offset: 0x002B98F2
		public virtual void SetEntry(T _data)
		{
			if (_data != this.entryData)
			{
				this.entryData = _data;
				base.ViewComponent.Enabled = this.HasEntry;
			}
			this.IsDirty = true;
		}

		// Token: 0x06007350 RID: 29520 RVA: 0x002BB726 File Offset: 0x002B9926
		public T GetEntry()
		{
			return this.entryData;
		}

		// Token: 0x040055BC RID: 21948
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isHovered;

		// Token: 0x040055BD RID: 21949
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiC_List<T> list;

		// Token: 0x040055BE RID: 21950
		[PublicizedFrom(EAccessModifier.Protected)]
		public T entryData;

		// Token: 0x040055BF RID: 21951
		[PublicizedFrom(EAccessModifier.Private)]
		public bool selected;

		// Token: 0x040055C0 RID: 21952
		[PublicizedFrom(EAccessModifier.Private)]
		public bool forceHovered;
	}
}
