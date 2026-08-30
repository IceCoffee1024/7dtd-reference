using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E8D RID: 3725
[Preserve]
public abstract class XUiC_List<T> : XUiController where T : XUiListEntry<T>
{
	// Token: 0x140000AE RID: 174
	// (add) Token: 0x0600739B RID: 29595 RVA: 0x002BDA28 File Offset: 0x002BBC28
	// (remove) Token: 0x0600739C RID: 29596 RVA: 0x002BDA60 File Offset: 0x002BBC60
	public event XUiEvent_ListSelectionChangedEventHandler<T> SelectionChanged;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x0600739D RID: 29597 RVA: 0x002BDA98 File Offset: 0x002BBC98
	// (remove) Token: 0x0600739E RID: 29598 RVA: 0x002BDAD0 File Offset: 0x002BBCD0
	public event XUiEvent_ListPageNumberChangedEventHandler<T> PageNumberChanged;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x0600739F RID: 29599 RVA: 0x002BDB08 File Offset: 0x002BBD08
	// (remove) Token: 0x060073A0 RID: 29600 RVA: 0x002BDB40 File Offset: 0x002BBD40
	public event XUiEvent_ListEntryClickedEventHandler<T> ListEntryClicked;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x060073A1 RID: 29601 RVA: 0x002BDB78 File Offset: 0x002BBD78
	// (remove) Token: 0x060073A2 RID: 29602 RVA: 0x002BDBB0 File Offset: 0x002BBDB0
	public event XUiEvent_ListEntryDoubleClickedEventHandler<T> ListEntryDoubleClicked;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x060073A3 RID: 29603 RVA: 0x002BDBE8 File Offset: 0x002BBDE8
	// (remove) Token: 0x060073A4 RID: 29604 RVA: 0x002BDC20 File Offset: 0x002BBE20
	public event XUiEvent_ListEntryHoveredEventHandler<T> ListEntryHovered;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x060073A5 RID: 29605 RVA: 0x002BDC58 File Offset: 0x002BBE58
	// (remove) Token: 0x060073A6 RID: 29606 RVA: 0x002BDC90 File Offset: 0x002BBE90
	public event XUiEvent_PageContentsChangedEventHandler<T> PageContentsChanged;

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x060073A7 RID: 29607 RVA: 0x002BDCC5 File Offset: 0x002BBEC5
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

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x060073A8 RID: 29608 RVA: 0x002BDCE5 File Offset: 0x002BBEE5
	// (set) Token: 0x060073A9 RID: 29609 RVA: 0x002BDCF0 File Offset: 0x002BBEF0
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

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x060073AA RID: 29610 RVA: 0x002BDD59 File Offset: 0x002BBF59
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x060073AB RID: 29611 RVA: 0x002BDD7C File Offset: 0x002BBF7C
	// (set) Token: 0x060073AC RID: 29612 RVA: 0x002BDD84 File Offset: 0x002BBF84
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

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x060073AD RID: 29613 RVA: 0x002BDE58 File Offset: 0x002BC058
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

	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x060073AE RID: 29614 RVA: 0x002BDE7E File Offset: 0x002BC07E
	// (set) Token: 0x060073AF RID: 29615 RVA: 0x002BDE88 File Offset: 0x002BC088
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

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x060073B0 RID: 29616 RVA: 0x002BDEEB File Offset: 0x002BC0EB
	// (set) Token: 0x060073B1 RID: 29617 RVA: 0x002BDEF3 File Offset: 0x002BC0F3
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

	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x060073B2 RID: 29618 RVA: 0x002BDF1C File Offset: 0x002BC11C
	// (set) Token: 0x060073B3 RID: 29619 RVA: 0x002BDF24 File Offset: 0x002BC124
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

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x060073B4 RID: 29620 RVA: 0x002BDF3E File Offset: 0x002BC13E
	// (set) Token: 0x060073B5 RID: 29621 RVA: 0x002BDF46 File Offset: 0x002BC146
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

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x060073B6 RID: 29622 RVA: 0x002BDF65 File Offset: 0x002BC165
	// (set) Token: 0x060073B7 RID: 29623 RVA: 0x002BDF70 File Offset: 0x002BC170
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

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x060073B8 RID: 29624 RVA: 0x002BE016 File Offset: 0x002BC216
	[XuiXmlBinding("current_min_index")]
	public int CurrentPageMinIndex
	{
		get
		{
			return Mathf.Max(this.minIndex, 0);
		}
	}

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x060073B9 RID: 29625 RVA: 0x002BE024 File Offset: 0x002BC224
	[XuiXmlBinding("current_max_index")]
	public int CurrentPageMaxIndex
	{
		get
		{
			return Mathf.Min(this.minIndex + this.PageLength, this.EntryCount) - 1;
		}
	}

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x060073BA RID: 29626 RVA: 0x002BE040 File Offset: 0x002BC240
	[XuiXmlBinding("hasselection")]
	public bool HasSelection
	{
		get
		{
			return this.SelectedEntryData != null;
		}
	}

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x060073BB RID: 29627 RVA: 0x002BE050 File Offset: 0x002BC250
	[XuiXmlBinding("entrycountfiltered")]
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x060073BC RID: 29628 RVA: 0x002BE05D File Offset: 0x002BC25D
	public T GetEntry(int _index)
	{
		return this.filteredEntries[_index];
	}

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x060073BD RID: 29629 RVA: 0x002BE06B File Offset: 0x002BC26B
	[XuiXmlBinding("entrycounttotal")]
	public int UnfilteredEntryCount
	{
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x060073BE RID: 29630 RVA: 0x002BE078 File Offset: 0x002BC278
	// (set) Token: 0x060073BF RID: 29631 RVA: 0x002BE080 File Offset: 0x002BC280
	[XuiXmlAttribute("clear_selection_on_open", false)]
	public bool ClearSelectionOnOpenClose { get; set; } = true;

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x060073C0 RID: 29632 RVA: 0x002BE089 File Offset: 0x002BC289
	// (set) Token: 0x060073C1 RID: 29633 RVA: 0x002BE091 File Offset: 0x002BC291
	[XuiXmlAttribute("clear_searchtext_on_open", false)]
	public bool ClearSearchTextOnOpenClose { get; set; }

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x060073C2 RID: 29634 RVA: 0x002BE09A File Offset: 0x002BC29A
	// (set) Token: 0x060073C3 RID: 29635 RVA: 0x002BE0A2 File Offset: 0x002BC2A2
	[XuiXmlAttribute("selectable", false)]
	public bool SelectableEntries { get; set; } = true;

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x060073C4 RID: 29636 RVA: 0x002BE0AB File Offset: 0x002BC2AB
	// (set) Token: 0x060073C5 RID: 29637 RVA: 0x002BE0B3 File Offset: 0x002BC2B3
	[XuiXmlAttribute("cursor_controllable", false)]
	public bool CursorControllable { get; set; }

	// Token: 0x060073C6 RID: 29638 RVA: 0x002BE0BC File Offset: 0x002BC2BC
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

	// Token: 0x060073C7 RID: 29639 RVA: 0x002BE19F File Offset: 0x002BC39F
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

	// Token: 0x060073C8 RID: 29640 RVA: 0x002BE1C0 File Offset: 0x002BC3C0
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

	// Token: 0x060073C9 RID: 29641 RVA: 0x002BE264 File Offset: 0x002BC464
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

	// Token: 0x060073CA RID: 29642 RVA: 0x002BE2AA File Offset: 0x002BC4AA
	[XuiBindEvent("OnPageChanged", "pager")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPageChanged()
	{
		this.Page = this.pager.CurrentPageNumber;
	}

	// Token: 0x060073CB RID: 29643 RVA: 0x002BE2BD File Offset: 0x002BC4BD
	[XuiBindEvent("OnScroll", null)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		this.ChangePage((_delta > 0f) ? (-1) : 1);
	}

	// Token: 0x060073CC RID: 29644 RVA: 0x002BE2D4 File Offset: 0x002BC4D4
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

	// Token: 0x060073CD RID: 29645 RVA: 0x002BE314 File Offset: 0x002BC514
	[XuiBindEvent("OnSubmitHandler", "searchBox")]
	[XuiBindEvent("OnSubmitHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSearchInputSubmit(XUiController _sender, string _text)
	{
		this.OnSearchInputChanged(_sender, _text, false);
	}

	// Token: 0x060073CE RID: 29646 RVA: 0x002BE31F File Offset: 0x002BC51F
	[XuiBindEvent("OnChangeHandler", "searchBox")]
	[XuiBindEvent("OnChangeHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSearchInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.FilterResults(_text);
		this.IsDirty = true;
	}

	// Token: 0x060073CF RID: 29647 RVA: 0x002BE32F File Offset: 0x002BC52F
	public IReadOnlyList<T> AllEntries()
	{
		return this.allEntries;
	}

	// Token: 0x060073D0 RID: 29648 RVA: 0x002BE338 File Offset: 0x002BC538
	public T GetEntryByIndex(int _index)
	{
		if (_index < 0 || _index >= this.filteredEntries.Count)
		{
			return default(T);
		}
		return this.filteredEntries[_index];
	}

	// Token: 0x060073D1 RID: 29649 RVA: 0x002BE370 File Offset: 0x002BC570
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

	// Token: 0x060073D2 RID: 29650 RVA: 0x002BE3CC File Offset: 0x002BC5CC
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

	// Token: 0x060073D3 RID: 29651 RVA: 0x002BE41C File Offset: 0x002BC61C
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

	// Token: 0x060073D4 RID: 29652 RVA: 0x002BE4F8 File Offset: 0x002BC6F8
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

	// Token: 0x060073D5 RID: 29653 RVA: 0x002BE568 File Offset: 0x002BC768
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

	// Token: 0x060073D6 RID: 29654 RVA: 0x002BE5E0 File Offset: 0x002BC7E0
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

	// Token: 0x060073D7 RID: 29655 RVA: 0x002BE640 File Offset: 0x002BC840
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

	// Token: 0x060073D8 RID: 29656 RVA: 0x002BE730 File Offset: 0x002BC930
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

	// Token: 0x060073D9 RID: 29657 RVA: 0x002BE760 File Offset: 0x002BC960
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

	// Token: 0x060073DA RID: 29658 RVA: 0x002BE8F4 File Offset: 0x002BCAF4
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

	// Token: 0x060073DB RID: 29659 RVA: 0x002BE970 File Offset: 0x002BCB70
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

	// Token: 0x060073DC RID: 29660 RVA: 0x002BE9C9 File Offset: 0x002BCBC9
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x060073DD RID: 29661 RVA: 0x002BE9D2 File Offset: 0x002BCBD2
	public void SelectCursorElementForSelectedEntry()
	{
		XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.SelectedEntry;
		if (xuiC_ListEntry == null)
		{
			return;
		}
		xuiC_ListEntry.SelectCursorElement(true, false);
	}

	// Token: 0x060073DE RID: 29662 RVA: 0x002BE9E7 File Offset: 0x002BCBE7
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

	// Token: 0x060073DF RID: 29663 RVA: 0x002BE9FB File Offset: 0x002BCBFB
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

	// Token: 0x060073E0 RID: 29664 RVA: 0x002BEA0F File Offset: 0x002BCC0F
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

	// Token: 0x060073E1 RID: 29665 RVA: 0x002BEA24 File Offset: 0x002BCC24
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List()
	{
	}

	// Token: 0x04005646 RID: 22086
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> filteredEntries = new List<T>();

	// Token: 0x04005647 RID: 22087
	[PublicizedFrom(EAccessModifier.Protected)]
	public T currentSelectedEntry;

	// Token: 0x04005648 RID: 22088
	[XuiBindComponent("list.", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_List<T>.XUiC_ListEntry[] listEntryControllers;

	// Token: 0x04005649 RID: 22089
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List<T>.XUiC_ListEntry selectedEntry;

	// Token: 0x0400564A RID: 22090
	[PublicizedFrom(EAccessModifier.Protected)]
	public int selectedEntryIndex = -1;

	// Token: 0x0400564B RID: 22091
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool updateSelectedItemByIndex;

	// Token: 0x0400564C RID: 22092
	[PublicizedFrom(EAccessModifier.Private)]
	public int pageSizeOverride = -1;

	// Token: 0x0400564D RID: 22093
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_List<T>.EPagingStepSize pagingStepSize;

	// Token: 0x0400564E RID: 22094
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i pagingOverscroll;

	// Token: 0x0400564F RID: 22095
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex;

	// Token: 0x04005650 RID: 22096
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005651 RID: 22097
	[XuiBindComponent("searchInput", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TextInput searchBox;

	// Token: 0x04005652 RID: 22098
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_CaptionedTextBox searchBox2;

	// Token: 0x04005653 RID: 22099
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Paging pager;

	// Token: 0x04005654 RID: 22100
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<T> allEntries = new List<T>();

	// Token: 0x04005659 RID: 22105
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x0400565A RID: 22106
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x0400565B RID: 22107
	[PublicizedFrom(EAccessModifier.Protected)]
	public string previousMatch = "";

	// Token: 0x0400565C RID: 22108
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreSelectionChange;

	// Token: 0x02000E8E RID: 3726
	public enum EPagingStepSize
	{
		// Token: 0x0400565E RID: 22110
		Page,
		// Token: 0x0400565F RID: 22111
		SingleEntry
	}

	// Token: 0x02000E8F RID: 3727
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class XUiC_ListEntry : XUiController
	{
		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x002BEA74 File Offset: 0x002BCC74
		[XuiXmlBinding("hasentry")]
		public bool HasEntry
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.entryData != null;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x060073E3 RID: 29667 RVA: 0x002BEA84 File Offset: 0x002BCC84
		[XuiXmlBinding("hovered")]
		public bool IsHovered
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.isHovered || this.forceHovered;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x060073E4 RID: 29668 RVA: 0x002BEA96 File Offset: 0x002BCC96
		// (set) Token: 0x060073E5 RID: 29669 RVA: 0x002BEAA0 File Offset: 0x002BCCA0
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

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x060073E6 RID: 29670 RVA: 0x002BEB12 File Offset: 0x002BCD12
		// (set) Token: 0x060073E7 RID: 29671 RVA: 0x002BEB1A File Offset: 0x002BCD1A
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

		// Token: 0x060073E8 RID: 29672 RVA: 0x002BEB34 File Offset: 0x002BCD34
		public override void Init()
		{
			base.Init();
			base.ViewComponent.Enabled = this.HasEntry;
			this.IsDirty = true;
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x002BEB54 File Offset: 0x002BCD54
		[XuiBindEvent("OnScroll", null)]
		[PublicizedFrom(EAccessModifier.Private)]
		public void Event_OnScroll(XUiController _sender, float _delta)
		{
			this.list.HandleOnScroll(this, _delta);
		}

		// Token: 0x060073EA RID: 29674 RVA: 0x002BEB63 File Offset: 0x002BCD63
		[XuiBindEvent("OnPress", null)]
		public void Event_OnPress(XUiController _sender, int _mouseButton)
		{
			if (!this.Selected)
			{
				this.Selected = true;
			}
			this.list.OnListEntryClicked(this.entryData);
		}

		// Token: 0x060073EB RID: 29675 RVA: 0x002BEB85 File Offset: 0x002BCD85
		[XuiBindEvent("OnDoubleClick", null)]
		public void Event_OnDoubleClick(XUiController _sender, int _mouseButton)
		{
			this.list.OnListEntryDoubleClicked(this.entryData);
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x002BEB98 File Offset: 0x002BCD98
		[XuiBindEvent("OnHover", null)]
		public void Event_OnHover(XUiController _sender, bool _isOver)
		{
			this.list.OnListEntryHovered(this.entryData, _isOver);
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x002BEBAC File Offset: 0x002BCDAC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			this.isHovered = _isOver;
			this.IsDirty = true;
			base.OnHovered(_isOver);
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x002BEBC4 File Offset: 0x002BCDC4
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

		// Token: 0x060073EF RID: 29679 RVA: 0x002BEC1E File Offset: 0x002BCE1E
		public virtual void SetEntry(T _data)
		{
			if (_data != this.entryData)
			{
				this.entryData = _data;
				base.ViewComponent.Enabled = this.HasEntry;
			}
			this.IsDirty = true;
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x002BEC52 File Offset: 0x002BCE52
		public T GetEntry()
		{
			return this.entryData;
		}

		// Token: 0x04005660 RID: 22112
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isHovered;

		// Token: 0x04005661 RID: 22113
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiC_List<T> list;

		// Token: 0x04005662 RID: 22114
		[PublicizedFrom(EAccessModifier.Protected)]
		public T entryData;

		// Token: 0x04005663 RID: 22115
		[PublicizedFrom(EAccessModifier.Private)]
		public bool selected;

		// Token: 0x04005664 RID: 22116
		[PublicizedFrom(EAccessModifier.Private)]
		public bool forceHovered;
	}
}
