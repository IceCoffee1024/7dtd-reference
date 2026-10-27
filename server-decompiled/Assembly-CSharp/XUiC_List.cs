using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EA7 RID: 3751
[Preserve]
public abstract class XUiC_List<T> : XUiController where T : XUiListEntry<T>
{
	// Token: 0x140000AD RID: 173
	// (add) Token: 0x060073B8 RID: 29624 RVA: 0x002C2344 File Offset: 0x002C0544
	// (remove) Token: 0x060073B9 RID: 29625 RVA: 0x002C237C File Offset: 0x002C057C
	public event XUiEvent_ListSelectionChangedEventHandler<T> SelectionChanged;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x060073BA RID: 29626 RVA: 0x002C23B4 File Offset: 0x002C05B4
	// (remove) Token: 0x060073BB RID: 29627 RVA: 0x002C23EC File Offset: 0x002C05EC
	public event XUiEvent_ListPageNumberChangedEventHandler<T> PageNumberChanged;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x060073BC RID: 29628 RVA: 0x002C2424 File Offset: 0x002C0624
	// (remove) Token: 0x060073BD RID: 29629 RVA: 0x002C245C File Offset: 0x002C065C
	public event XUiEvent_ListEntryClickedEventHandler<T> ListEntryClicked;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x060073BE RID: 29630 RVA: 0x002C2494 File Offset: 0x002C0694
	// (remove) Token: 0x060073BF RID: 29631 RVA: 0x002C24CC File Offset: 0x002C06CC
	public event XUiEvent_ListEntryDoubleClickedEventHandler<T> ListEntryDoubleClicked;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x060073C0 RID: 29632 RVA: 0x002C2504 File Offset: 0x002C0704
	// (remove) Token: 0x060073C1 RID: 29633 RVA: 0x002C253C File Offset: 0x002C073C
	public event XUiEvent_ListEntryHoveredEventHandler<T> ListEntryHovered;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x060073C2 RID: 29634 RVA: 0x002C2574 File Offset: 0x002C0774
	// (remove) Token: 0x060073C3 RID: 29635 RVA: 0x002C25AC File Offset: 0x002C07AC
	public event XUiEvent_PageContentsChangedEventHandler<T> PageContentsChanged;

	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x060073C4 RID: 29636 RVA: 0x002C25E1 File Offset: 0x002C07E1
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

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x060073C5 RID: 29637 RVA: 0x002C2601 File Offset: 0x002C0801
	// (set) Token: 0x060073C6 RID: 29638 RVA: 0x002C260C File Offset: 0x002C080C
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

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x060073C7 RID: 29639 RVA: 0x002C2675 File Offset: 0x002C0875
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x060073C8 RID: 29640 RVA: 0x002C2698 File Offset: 0x002C0898
	// (set) Token: 0x060073C9 RID: 29641 RVA: 0x002C26A0 File Offset: 0x002C08A0
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

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x060073CA RID: 29642 RVA: 0x002C2774 File Offset: 0x002C0974
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

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x060073CB RID: 29643 RVA: 0x002C279A File Offset: 0x002C099A
	// (set) Token: 0x060073CC RID: 29644 RVA: 0x002C27A4 File Offset: 0x002C09A4
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

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x060073CD RID: 29645 RVA: 0x002C2807 File Offset: 0x002C0A07
	// (set) Token: 0x060073CE RID: 29646 RVA: 0x002C280F File Offset: 0x002C0A0F
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

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x060073CF RID: 29647 RVA: 0x002C2838 File Offset: 0x002C0A38
	// (set) Token: 0x060073D0 RID: 29648 RVA: 0x002C2840 File Offset: 0x002C0A40
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

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x060073D1 RID: 29649 RVA: 0x002C285A File Offset: 0x002C0A5A
	// (set) Token: 0x060073D2 RID: 29650 RVA: 0x002C2862 File Offset: 0x002C0A62
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

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x060073D3 RID: 29651 RVA: 0x002C2881 File Offset: 0x002C0A81
	// (set) Token: 0x060073D4 RID: 29652 RVA: 0x002C288C File Offset: 0x002C0A8C
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

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x060073D5 RID: 29653 RVA: 0x002C2932 File Offset: 0x002C0B32
	[XuiXmlBinding("current_min_index")]
	public int CurrentPageMinIndex
	{
		get
		{
			return Mathf.Max(this.minIndex, 0);
		}
	}

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x060073D6 RID: 29654 RVA: 0x002C2940 File Offset: 0x002C0B40
	[XuiXmlBinding("current_max_index")]
	public int CurrentPageMaxIndex
	{
		get
		{
			return Mathf.Min(this.minIndex + this.PageLength, this.EntryCount) - 1;
		}
	}

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x060073D7 RID: 29655 RVA: 0x002C295C File Offset: 0x002C0B5C
	[XuiXmlBinding("hasselection")]
	public bool HasSelection
	{
		get
		{
			return this.SelectedEntryData != null;
		}
	}

	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x060073D8 RID: 29656 RVA: 0x002C296C File Offset: 0x002C0B6C
	[XuiXmlBinding("entrycountfiltered")]
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x060073D9 RID: 29657 RVA: 0x002C2979 File Offset: 0x002C0B79
	public T GetEntry(int _index)
	{
		return this.filteredEntries[_index];
	}

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x060073DA RID: 29658 RVA: 0x002C2987 File Offset: 0x002C0B87
	[XuiXmlBinding("entrycounttotal")]
	public int UnfilteredEntryCount
	{
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x060073DB RID: 29659 RVA: 0x002C2994 File Offset: 0x002C0B94
	// (set) Token: 0x060073DC RID: 29660 RVA: 0x002C299C File Offset: 0x002C0B9C
	[XuiXmlAttribute("clear_selection_on_open", false)]
	public bool ClearSelectionOnOpenClose { get; set; } = true;

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x060073DD RID: 29661 RVA: 0x002C29A5 File Offset: 0x002C0BA5
	// (set) Token: 0x060073DE RID: 29662 RVA: 0x002C29AD File Offset: 0x002C0BAD
	[XuiXmlAttribute("clear_searchtext_on_open", false)]
	public bool ClearSearchTextOnOpenClose { get; set; }

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x060073DF RID: 29663 RVA: 0x002C29B6 File Offset: 0x002C0BB6
	// (set) Token: 0x060073E0 RID: 29664 RVA: 0x002C29BE File Offset: 0x002C0BBE
	[XuiXmlAttribute("selectable", false)]
	public bool SelectableEntries { get; set; } = true;

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x060073E1 RID: 29665 RVA: 0x002C29C7 File Offset: 0x002C0BC7
	// (set) Token: 0x060073E2 RID: 29666 RVA: 0x002C29CF File Offset: 0x002C0BCF
	[XuiXmlAttribute("cursor_controllable", false)]
	public bool CursorControllable { get; set; }

	// Token: 0x060073E3 RID: 29667 RVA: 0x002C29D8 File Offset: 0x002C0BD8
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

	// Token: 0x060073E4 RID: 29668 RVA: 0x002C2ABB File Offset: 0x002C0CBB
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

	// Token: 0x060073E5 RID: 29669 RVA: 0x002C2ADC File Offset: 0x002C0CDC
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

	// Token: 0x060073E6 RID: 29670 RVA: 0x002C2B80 File Offset: 0x002C0D80
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

	// Token: 0x060073E7 RID: 29671 RVA: 0x002C2BC6 File Offset: 0x002C0DC6
	[XuiBindEvent("OnPageChanged", "pager")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPageChanged()
	{
		this.Page = this.pager.CurrentPageNumber;
	}

	// Token: 0x060073E8 RID: 29672 RVA: 0x002C2BD9 File Offset: 0x002C0DD9
	[XuiBindEvent("OnScroll", null)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		this.ChangePage((_delta > 0f) ? (-1) : 1);
	}

	// Token: 0x060073E9 RID: 29673 RVA: 0x002C2BF0 File Offset: 0x002C0DF0
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

	// Token: 0x060073EA RID: 29674 RVA: 0x002C2C30 File Offset: 0x002C0E30
	[XuiBindEvent("OnSubmitHandler", "searchBox")]
	[XuiBindEvent("OnSubmitHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSearchInputSubmit(XUiController _sender, string _text)
	{
		this.OnSearchInputChanged(_sender, _text, false);
	}

	// Token: 0x060073EB RID: 29675 RVA: 0x002C2C3B File Offset: 0x002C0E3B
	[XuiBindEvent("OnChangeHandler", "searchBox")]
	[XuiBindEvent("OnChangeHandler", "searchBox2")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSearchInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.FilterResults(_text);
		this.IsDirty = true;
	}

	// Token: 0x060073EC RID: 29676 RVA: 0x002C2C4B File Offset: 0x002C0E4B
	public IReadOnlyList<T> AllEntries()
	{
		return this.allEntries;
	}

	// Token: 0x060073ED RID: 29677 RVA: 0x002C2C54 File Offset: 0x002C0E54
	public T GetEntryByIndex(int _index)
	{
		if (_index < 0 || _index >= this.filteredEntries.Count)
		{
			return default(T);
		}
		return this.filteredEntries[_index];
	}

	// Token: 0x060073EE RID: 29678 RVA: 0x002C2C8C File Offset: 0x002C0E8C
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

	// Token: 0x060073EF RID: 29679 RVA: 0x002C2CE8 File Offset: 0x002C0EE8
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

	// Token: 0x060073F0 RID: 29680 RVA: 0x002C2D38 File Offset: 0x002C0F38
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

	// Token: 0x060073F1 RID: 29681 RVA: 0x002C2E14 File Offset: 0x002C1014
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

	// Token: 0x060073F2 RID: 29682 RVA: 0x002C2E84 File Offset: 0x002C1084
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

	// Token: 0x060073F3 RID: 29683 RVA: 0x002C2EFC File Offset: 0x002C10FC
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

	// Token: 0x060073F4 RID: 29684 RVA: 0x002C2F5C File Offset: 0x002C115C
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

	// Token: 0x060073F5 RID: 29685 RVA: 0x002C304C File Offset: 0x002C124C
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

	// Token: 0x060073F6 RID: 29686 RVA: 0x002C307C File Offset: 0x002C127C
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

	// Token: 0x060073F7 RID: 29687 RVA: 0x002C3210 File Offset: 0x002C1410
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

	// Token: 0x060073F8 RID: 29688 RVA: 0x002C328C File Offset: 0x002C148C
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

	// Token: 0x060073F9 RID: 29689 RVA: 0x002C32E5 File Offset: 0x002C14E5
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x060073FA RID: 29690 RVA: 0x002C32EE File Offset: 0x002C14EE
	public void SelectCursorElementForSelectedEntry()
	{
		XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.SelectedEntry;
		if (xuiC_ListEntry == null)
		{
			return;
		}
		xuiC_ListEntry.SelectCursorElement(true, false);
	}

	// Token: 0x060073FB RID: 29691 RVA: 0x002C3303 File Offset: 0x002C1503
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

	// Token: 0x060073FC RID: 29692 RVA: 0x002C3317 File Offset: 0x002C1517
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

	// Token: 0x060073FD RID: 29693 RVA: 0x002C332B File Offset: 0x002C152B
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

	// Token: 0x060073FE RID: 29694 RVA: 0x002C3340 File Offset: 0x002C1540
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List()
	{
	}

	// Token: 0x040056A2 RID: 22178
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> filteredEntries = new List<T>();

	// Token: 0x040056A3 RID: 22179
	[PublicizedFrom(EAccessModifier.Protected)]
	public T currentSelectedEntry;

	// Token: 0x040056A4 RID: 22180
	[XuiBindComponent("list.", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_List<T>.XUiC_ListEntry[] listEntryControllers;

	// Token: 0x040056A5 RID: 22181
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_List<T>.XUiC_ListEntry selectedEntry;

	// Token: 0x040056A6 RID: 22182
	[PublicizedFrom(EAccessModifier.Protected)]
	public int selectedEntryIndex = -1;

	// Token: 0x040056A7 RID: 22183
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool updateSelectedItemByIndex;

	// Token: 0x040056A8 RID: 22184
	[PublicizedFrom(EAccessModifier.Private)]
	public int pageSizeOverride = -1;

	// Token: 0x040056A9 RID: 22185
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_List<T>.EPagingStepSize pagingStepSize;

	// Token: 0x040056AA RID: 22186
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i pagingOverscroll;

	// Token: 0x040056AB RID: 22187
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex;

	// Token: 0x040056AC RID: 22188
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040056AD RID: 22189
	[XuiBindComponent("searchInput", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TextInput searchBox;

	// Token: 0x040056AE RID: 22190
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_CaptionedTextBox searchBox2;

	// Token: 0x040056AF RID: 22191
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Paging pager;

	// Token: 0x040056B0 RID: 22192
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<T> allEntries = new List<T>();

	// Token: 0x040056B5 RID: 22197
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x040056B6 RID: 22198
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x040056B7 RID: 22199
	[PublicizedFrom(EAccessModifier.Protected)]
	public string previousMatch = "";

	// Token: 0x040056B8 RID: 22200
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreSelectionChange;

	// Token: 0x02000EA8 RID: 3752
	public enum EPagingStepSize
	{
		// Token: 0x040056BA RID: 22202
		Page,
		// Token: 0x040056BB RID: 22203
		SingleEntry
	}

	// Token: 0x02000EA9 RID: 3753
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class XUiC_ListEntry : XUiController
	{
		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060073FF RID: 29695 RVA: 0x002C3390 File Offset: 0x002C1590
		[XuiXmlBinding("hasentry")]
		public bool HasEntry
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.entryData != null;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06007400 RID: 29696 RVA: 0x002C33A0 File Offset: 0x002C15A0
		[XuiXmlBinding("hovered")]
		public bool IsHovered
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.isHovered || this.forceHovered;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06007401 RID: 29697 RVA: 0x002C33B2 File Offset: 0x002C15B2
		// (set) Token: 0x06007402 RID: 29698 RVA: 0x002C33BC File Offset: 0x002C15BC
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

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06007403 RID: 29699 RVA: 0x002C342E File Offset: 0x002C162E
		// (set) Token: 0x06007404 RID: 29700 RVA: 0x002C3436 File Offset: 0x002C1636
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

		// Token: 0x06007405 RID: 29701 RVA: 0x002C3450 File Offset: 0x002C1650
		public override void Init()
		{
			base.Init();
			base.ViewComponent.Enabled = this.HasEntry;
			this.IsDirty = true;
		}

		// Token: 0x06007406 RID: 29702 RVA: 0x002C3470 File Offset: 0x002C1670
		[XuiBindEvent("OnScroll", null)]
		[PublicizedFrom(EAccessModifier.Private)]
		public void Event_OnScroll(XUiController _sender, float _delta)
		{
			this.list.HandleOnScroll(this, _delta);
		}

		// Token: 0x06007407 RID: 29703 RVA: 0x002C347F File Offset: 0x002C167F
		[XuiBindEvent("OnPress", null)]
		public void Event_OnPress(XUiController _sender, int _mouseButton)
		{
			if (!this.Selected)
			{
				this.Selected = true;
			}
			this.list.OnListEntryClicked(this.entryData);
		}

		// Token: 0x06007408 RID: 29704 RVA: 0x002C34A1 File Offset: 0x002C16A1
		[XuiBindEvent("OnDoubleClick", null)]
		public void Event_OnDoubleClick(XUiController _sender, int _mouseButton)
		{
			this.list.OnListEntryDoubleClicked(this.entryData);
		}

		// Token: 0x06007409 RID: 29705 RVA: 0x002C34B4 File Offset: 0x002C16B4
		[XuiBindEvent("OnHover", null)]
		public void Event_OnHover(XUiController _sender, bool _isOver)
		{
			this.list.OnListEntryHovered(this.entryData, _isOver);
		}

		// Token: 0x0600740A RID: 29706 RVA: 0x002C34C8 File Offset: 0x002C16C8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			this.isHovered = _isOver;
			this.IsDirty = true;
			base.OnHovered(_isOver);
		}

		// Token: 0x0600740B RID: 29707 RVA: 0x002C34E0 File Offset: 0x002C16E0
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

		// Token: 0x0600740C RID: 29708 RVA: 0x002C353A File Offset: 0x002C173A
		public virtual void SetEntry(T _data)
		{
			if (_data != this.entryData)
			{
				this.entryData = _data;
				base.ViewComponent.Enabled = this.HasEntry;
			}
			this.IsDirty = true;
		}

		// Token: 0x0600740D RID: 29709 RVA: 0x002C356E File Offset: 0x002C176E
		public T GetEntry()
		{
			return this.entryData;
		}

		// Token: 0x040056BC RID: 22204
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isHovered;

		// Token: 0x040056BD RID: 22205
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiC_List<T> list;

		// Token: 0x040056BE RID: 22206
		[PublicizedFrom(EAccessModifier.Protected)]
		public T entryData;

		// Token: 0x040056BF RID: 22207
		[PublicizedFrom(EAccessModifier.Private)]
		public bool selected;

		// Token: 0x040056C0 RID: 22208
		[PublicizedFrom(EAccessModifier.Private)]
		public bool forceHovered;
	}
}
