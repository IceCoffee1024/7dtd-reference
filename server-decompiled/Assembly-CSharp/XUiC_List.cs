using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E9C RID: 3740
[Preserve]
public abstract class XUiC_List<T> : XUiController where T : XUiListEntry<T>
{
	// Token: 0x140000AD RID: 173
	// (add) Token: 0x06007393 RID: 29587 RVA: 0x002C0DD8 File Offset: 0x002BEFD8
	// (remove) Token: 0x06007394 RID: 29588 RVA: 0x002C0E10 File Offset: 0x002BF010
	public event XUiEvent_ListSelectionChangedEventHandler<T> SelectionChanged;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x06007395 RID: 29589 RVA: 0x002C0E48 File Offset: 0x002BF048
	// (remove) Token: 0x06007396 RID: 29590 RVA: 0x002C0E80 File Offset: 0x002BF080
	public event XUiEvent_ListPageNumberChangedEventHandler<T> PageNumberChanged;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x06007397 RID: 29591 RVA: 0x002C0EB8 File Offset: 0x002BF0B8
	// (remove) Token: 0x06007398 RID: 29592 RVA: 0x002C0EF0 File Offset: 0x002BF0F0
	public event XUiEvent_ListEntryClickedEventHandler<T> ListEntryClicked;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x06007399 RID: 29593 RVA: 0x002C0F28 File Offset: 0x002BF128
	// (remove) Token: 0x0600739A RID: 29594 RVA: 0x002C0F60 File Offset: 0x002BF160
	public event XUiEvent_ListEntryDoubleClickedEventHandler<T> ListEntryDoubleClicked;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x0600739B RID: 29595 RVA: 0x002C0F98 File Offset: 0x002BF198
	// (remove) Token: 0x0600739C RID: 29596 RVA: 0x002C0FD0 File Offset: 0x002BF1D0
	public event XUiEvent_ListEntryHoveredEventHandler<T> ListEntryHovered;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x0600739D RID: 29597 RVA: 0x002C1008 File Offset: 0x002BF208
	// (remove) Token: 0x0600739E RID: 29598 RVA: 0x002C1040 File Offset: 0x002BF240
	public event XUiEvent_PageContentsChangedEventHandler<T> PageContentsChanged;

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x0600739F RID: 29599 RVA: 0x002C1075 File Offset: 0x002BF275
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

	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x060073A0 RID: 29600 RVA: 0x002C1095 File Offset: 0x002BF295
	// (set) Token: 0x060073A1 RID: 29601 RVA: 0x002C10A0 File Offset: 0x002BF2A0
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

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x060073A2 RID: 29602 RVA: 0x002C1109 File Offset: 0x002BF309
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x060073A3 RID: 29603 RVA: 0x002C112C File Offset: 0x002BF32C
	// (set) Token: 0x060073A4 RID: 29604 RVA: 0x002C1134 File Offset: 0x002BF334
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

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x060073A5 RID: 29605 RVA: 0x002C1208 File Offset: 0x002BF408
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

	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x060073A6 RID: 29606 RVA: 0x002C122E File Offset: 0x002BF42E
	// (set) Token: 0x060073A7 RID: 29607 RVA: 0x002C1238 File Offset: 0x002BF438
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

	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x060073A8 RID: 29608 RVA: 0x002C129B File Offset: 0x002BF49B
	// (set) Token: 0x060073A9 RID: 29609 RVA: 0x002C12A3 File Offset: 0x002BF4A3
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

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x060073AA RID: 29610 RVA: 0x002C12CC File Offset: 0x002BF4CC
	// (set) Token: 0x060073AB RID: 29611 RVA: 0x002C12D4 File Offset: 0x002BF4D4
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

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x060073AC RID: 29612 RVA: 0x002C12EE File Offset: 0x002BF4EE
	// (set) Token: 0x060073AD RID: 29613 RVA: 0x002C12F6 File Offset: 0x002BF4F6
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

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x060073AE RID: 29614 RVA: 0x002C1315 File Offset: 0x002BF515
	// (set) Token: 0x060073AF RID: 29615 RVA: 0x002C1320 File Offset: 0x002BF520
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
					value = Mathf.Max(value, 0);
				}
			}
			this.minIndex = value;
			this.ClearSelection();
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x060073B0 RID: 29616 RVA: 0x002C13C6 File Offset: 0x002BF5C6
	[XuiXmlBinding("current_min_index")]
	public int CurrentPageMinIndex
	{
		get
		{
			return Mathf.Max(this.minIndex, 0);
		}
	}

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x060073B1 RID: 29617 RVA: 0x002C13D4 File Offset: 0x002BF5D4
	[XuiXmlBinding("current_max_index")]
	public int CurrentPageMaxIndex
	{
		get
		{
			return Mathf.Min(this.minIndex + this.PageLength, this.EntryCount) - 1;
		}
	}

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x060073B2 RID: 29618 RVA: 0x002C13F0 File Offset: 0x002BF5F0
	[XuiXmlBinding("hasselection")]
	public bool HasSelection
	{
		get
		{
			return this.SelectedEntryData != null;
		}
	}

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x060073B3 RID: 29619 RVA: 0x002C1400 File Offset: 0x002BF600
	[XuiXmlBinding("entrycountfiltered")]
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x060073B4 RID: 29620 RVA: 0x002C140D File Offset: 0x002BF60D
	public T GetEntry(int _index)
	{
		return this.filteredEntries[_index];
	}

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x060073B5 RID: 29621 RVA: 0x002C141B File Offset: 0x002BF61B
	[XuiXmlBinding("entrycounttotal")]
	public int UnfilteredEntryCount
	{
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x060073B6 RID: 29622 RVA: 0x002C1428 File Offset: 0x002BF628
	// (set) Token: 0x060073B7 RID: 29623 RVA: 0x002C1430 File Offset: 0x002BF630
	[XuiXmlAttribute("clear_selection_on_open", false)]
	public bool ClearSelectionOnOpenClose { get; set; } = true;

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x060073B8 RID: 29624 RVA: 0x002C1439 File Offset: 0x002BF639
	// (set) Token: 0x060073B9 RID: 29625 RVA: 0x002C1441 File Offset: 0x002BF641
	[XuiXmlAttribute("clear_searchtext_on_open", false)]
	public bool ClearSearchTextOnOpenClose { get; set; }

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x060073BA RID: 29626 RVA: 0x002C144A File Offset: 0x002BF64A
	// (set) Token: 0x060073BB RID: 29627 RVA: 0x002C1452 File Offset: 0x002BF652
	[XuiXmlAttribute("selectable", false)]
	public bool SelectableEntries { get; set; } = true;

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x060073BC RID: 29628 RVA: 0x002C145B File Offset: 0x002BF65B
	// (set) Token: 0x060073BD RID: 29629 RVA: 0x002C1463 File Offset: 0x002BF663
	[XuiXmlAttribute("cursor_controllable", false)]
	public bool CursorControllable { get; set; }

	// Token: 0x060073BE RID: 29630 RVA: 0x002C146C File Offset: 0x002BF66C
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

	// Token: 0x060073BF RID: 29631 RVA: 0x002C154F File Offset: 0x002BF74F
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

	// Token: 0x060073C0 RID: 29632 RVA: 0x002C1570 File Offset: 0x002BF770
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

	// Token: 0x060073C1 RID: 29633 RVA: 0x002C1614 File Offset: 0x002BF814
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

	// Token: 0x060073C2 RID: 29634 RVA: 0x002C165A File Offset: 0x002BF85A
	[XuiBindEvent("OnPageChanged", "pager")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPageChanged()
	{
		this.Page = this.pager.CurrentPageNumber;
	}

	// Token: 0x060073C3 RID: 29635 RVA: 0x002C166D File Offset: 0x002BF86D
	[XuiBindEvent("OnScroll", null)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		this.ChangePage((_delta > 0f) ? (-1) : 1);
	}

	// Token: 0x060073C4 RID: 29636 RVA: 0x002C1684 File Offset: 0x002BF884
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

	// Token: 0x060073C5 RID: 29637 RVA: 0x002C16C4 File Offset: 0x002BF8C4
	[XuiBindEvent("OnSubmitHandler", "searchBox")]
	[XuiBindEvent("OnSubmitHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSearchInputSubmit(XUiController _sender, string _text)
	{
		this.OnSearchInputChanged(_sender, _text, false);
	}

	// Token: 0x060073C6 RID: 29638 RVA: 0x002C16CF File Offset: 0x002BF8CF
	[XuiBindEvent("OnChangeHandler", "searchBox")]
	[XuiBindEvent("OnChangeHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSearchInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.FilterResults(_text);
		this.IsDirty = true;
	}

	// Token: 0x060073C7 RID: 29639 RVA: 0x002C16DF File Offset: 0x002BF8DF
	public IReadOnlyList<T> AllEntries()
	{
		return this.allEntries;
	}

	// Token: 0x060073C8 RID: 29640 RVA: 0x002C16E8 File Offset: 0x002BF8E8
	public T GetEntryByIndex(int _index)
	{
		if (_index < 0 || _index >= this.filteredEntries.Count)
		{
			return default(T);
		}
		return this.filteredEntries[_index];
	}

	// Token: 0x060073C9 RID: 29641 RVA: 0x002C1720 File Offset: 0x002BF920
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

	// Token: 0x060073CA RID: 29642 RVA: 0x002C177C File Offset: 0x002BF97C
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

	// Token: 0x060073CB RID: 29643 RVA: 0x002C17CC File Offset: 0x002BF9CC
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

	// Token: 0x060073CC RID: 29644 RVA: 0x002C18A8 File Offset: 0x002BFAA8
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

	// Token: 0x060073CD RID: 29645 RVA: 0x002C1918 File Offset: 0x002BFB18
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

	// Token: 0x060073CE RID: 29646 RVA: 0x002C1990 File Offset: 0x002BFB90
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

	// Token: 0x060073CF RID: 29647 RVA: 0x002C19F0 File Offset: 0x002BFBF0
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

	// Token: 0x060073D0 RID: 29648 RVA: 0x002C1AE0 File Offset: 0x002BFCE0
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

	// Token: 0x060073D1 RID: 29649 RVA: 0x002C1B10 File Offset: 0x002BFD10
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

	// Token: 0x060073D2 RID: 29650 RVA: 0x002C1CA4 File Offset: 0x002BFEA4
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

	// Token: 0x060073D3 RID: 29651 RVA: 0x002C1D20 File Offset: 0x002BFF20
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

	// Token: 0x060073D4 RID: 29652 RVA: 0x002C1D79 File Offset: 0x002BFF79
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x060073D5 RID: 29653 RVA: 0x002C1D82 File Offset: 0x002BFF82
	public void SelectCursorElementForSelectedEntry()
	{
		XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.SelectedEntry;
		if (xuiC_ListEntry == null)
		{
			return;
		}
		xuiC_ListEntry.SelectCursorElement(true, false);
	}

	// Token: 0x060073D6 RID: 29654 RVA: 0x002C1D97 File Offset: 0x002BFF97
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

	// Token: 0x060073D7 RID: 29655 RVA: 0x002C1DAB File Offset: 0x002BFFAB
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

	// Token: 0x060073D8 RID: 29656 RVA: 0x002C1DBF File Offset: 0x002BFFBF
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

	// Token: 0x060073D9 RID: 29657 RVA: 0x002C1DD4 File Offset: 0x002BFFD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List()
	{
	}

	// Token: 0x0400567D RID: 22141
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> filteredEntries = new List<T>();

	// Token: 0x0400567E RID: 22142
	[PublicizedFrom(EAccessModifier.Protected)]
	public T currentSelectedEntry;

	// Token: 0x0400567F RID: 22143
	[XuiBindComponent("list.", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_List<T>.XUiC_ListEntry[] listEntryControllers;

	// Token: 0x04005680 RID: 22144
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List<T>.XUiC_ListEntry selectedEntry;

	// Token: 0x04005681 RID: 22145
	[PublicizedFrom(EAccessModifier.Protected)]
	public int selectedEntryIndex = -1;

	// Token: 0x04005682 RID: 22146
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool updateSelectedItemByIndex;

	// Token: 0x04005683 RID: 22147
	[PublicizedFrom(EAccessModifier.Private)]
	public int pageSizeOverride = -1;

	// Token: 0x04005684 RID: 22148
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_List<T>.EPagingStepSize pagingStepSize;

	// Token: 0x04005685 RID: 22149
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i pagingOverscroll;

	// Token: 0x04005686 RID: 22150
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex;

	// Token: 0x04005687 RID: 22151
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005688 RID: 22152
	[XuiBindComponent("searchInput", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TextInput searchBox;

	// Token: 0x04005689 RID: 22153
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_CaptionedTextBox searchBox2;

	// Token: 0x0400568A RID: 22154
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Paging pager;

	// Token: 0x0400568B RID: 22155
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<T> allEntries = new List<T>();

	// Token: 0x04005690 RID: 22160
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x04005691 RID: 22161
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x04005692 RID: 22162
	[PublicizedFrom(EAccessModifier.Protected)]
	public string previousMatch = "";

	// Token: 0x04005693 RID: 22163
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreSelectionChange;

	// Token: 0x02000E9D RID: 3741
	public enum EPagingStepSize
	{
		// Token: 0x04005695 RID: 22165
		Page,
		// Token: 0x04005696 RID: 22166
		SingleEntry
	}

	// Token: 0x02000E9E RID: 3742
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class XUiC_ListEntry : XUiController
	{
		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x060073DA RID: 29658 RVA: 0x002C1E24 File Offset: 0x002C0024
		[XuiXmlBinding("hasentry")]
		public bool HasEntry
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.entryData != null;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x060073DB RID: 29659 RVA: 0x002C1E34 File Offset: 0x002C0034
		[XuiXmlBinding("hovered")]
		public bool IsHovered
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.isHovered || this.forceHovered;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060073DC RID: 29660 RVA: 0x002C1E46 File Offset: 0x002C0046
		// (set) Token: 0x060073DD RID: 29661 RVA: 0x002C1E50 File Offset: 0x002C0050
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

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060073DE RID: 29662 RVA: 0x002C1EC2 File Offset: 0x002C00C2
		// (set) Token: 0x060073DF RID: 29663 RVA: 0x002C1ECA File Offset: 0x002C00CA
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

		// Token: 0x060073E0 RID: 29664 RVA: 0x002C1EE4 File Offset: 0x002C00E4
		public override void Init()
		{
			base.Init();
			base.ViewComponent.Enabled = this.HasEntry;
			this.IsDirty = true;
		}

		// Token: 0x060073E1 RID: 29665 RVA: 0x002C1F04 File Offset: 0x002C0104
		[XuiBindEvent("OnScroll", null)]
		[PublicizedFrom(EAccessModifier.Private)]
		public void Event_OnScroll(XUiController _sender, float _delta)
		{
			this.list.HandleOnScroll(this, _delta);
		}

		// Token: 0x060073E2 RID: 29666 RVA: 0x002C1F13 File Offset: 0x002C0113
		[XuiBindEvent("OnPress", null)]
		public void Event_OnPress(XUiController _sender, int _mouseButton)
		{
			if (!this.Selected)
			{
				this.Selected = true;
			}
			this.list.OnListEntryClicked(this.entryData);
		}

		// Token: 0x060073E3 RID: 29667 RVA: 0x002C1F35 File Offset: 0x002C0135
		[XuiBindEvent("OnDoubleClick", null)]
		public void Event_OnDoubleClick(XUiController _sender, int _mouseButton)
		{
			this.list.OnListEntryDoubleClicked(this.entryData);
		}

		// Token: 0x060073E4 RID: 29668 RVA: 0x002C1F48 File Offset: 0x002C0148
		[XuiBindEvent("OnHover", null)]
		public void Event_OnHover(XUiController _sender, bool _isOver)
		{
			this.list.OnListEntryHovered(this.entryData, _isOver);
		}

		// Token: 0x060073E5 RID: 29669 RVA: 0x002C1F5C File Offset: 0x002C015C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			this.isHovered = _isOver;
			this.IsDirty = true;
			base.OnHovered(_isOver);
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x002C1F74 File Offset: 0x002C0174
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

		// Token: 0x060073E7 RID: 29671 RVA: 0x002C1FCE File Offset: 0x002C01CE
		public virtual void SetEntry(T _data)
		{
			if (_data != this.entryData)
			{
				this.entryData = _data;
				base.ViewComponent.Enabled = this.HasEntry;
			}
			this.IsDirty = true;
		}

		// Token: 0x060073E8 RID: 29672 RVA: 0x002C2002 File Offset: 0x002C0202
		public T GetEntry()
		{
			return this.entryData;
		}

		// Token: 0x04005697 RID: 22167
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isHovered;

		// Token: 0x04005698 RID: 22168
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiC_List<T> list;

		// Token: 0x04005699 RID: 22169
		[PublicizedFrom(EAccessModifier.Protected)]
		public T entryData;

		// Token: 0x0400569A RID: 22170
		[PublicizedFrom(EAccessModifier.Private)]
		public bool selected;

		// Token: 0x0400569B RID: 22171
		[PublicizedFrom(EAccessModifier.Private)]
		public bool forceHovered;
	}
}
