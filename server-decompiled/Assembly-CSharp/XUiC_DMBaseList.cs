using System;

// Token: 0x02000D8F RID: 3471
public abstract class XUiC_DMBaseList<T> : XUiC_List<T> where T : XUiListEntry<T>
{
	// Token: 0x1400008E RID: 142
	// (add) Token: 0x060068DE RID: 26846 RVA: 0x00299B68 File Offset: 0x00297D68
	// (remove) Token: 0x060068DF RID: 26847 RVA: 0x00299BA0 File Offset: 0x00297DA0
	public event XUiC_DMBaseList<T>.ListEntryHoveredDelegate OnEntryHovered;

	// Token: 0x060068E0 RID: 26848 RVA: 0x00299BD8 File Offset: 0x00297DD8
	public override void Init()
	{
		base.Init();
		XUiC_List<T>.XUiC_ListEntry[] listEntryControllers = this.listEntryControllers;
		for (int i = 0; i < listEntryControllers.Length; i++)
		{
			listEntryControllers[i].OnHover += this.childElementHovered;
		}
		foreach (XUiController xuiController in this.pager.Children)
		{
			xuiController.OnHover += this.childElementHovered;
		}
		base.PageContentsChanged += this.pageContentsChangedHandler;
		this.searchBox.OnHover += this.childElementHovered;
	}

	// Token: 0x060068E1 RID: 26849 RVA: 0x00299C94 File Offset: 0x00297E94
	[PublicizedFrom(EAccessModifier.Protected)]
	public void pageContentsChangedHandler(XUiC_List<T> _list)
	{
		if (this.hoveredElement == null || this.OnEntryHovered == null)
		{
			return;
		}
		T t = default(T);
		XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = this.hoveredElement as XUiC_List<T>.XUiC_ListEntry;
		if (xuiC_ListEntry != null)
		{
			t = xuiC_ListEntry.GetEntry();
		}
		this.OnEntryHovered(t, false);
		this.OnEntryHovered(t, true);
	}

	// Token: 0x060068E2 RID: 26850 RVA: 0x00299CEC File Offset: 0x00297EEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void childElementHovered(XUiController _sender, bool _isOver)
	{
		this.hoveredElement = (_isOver ? _sender : null);
		XUiC_List<T>.XUiC_ListEntry xuiC_ListEntry = _sender as XUiC_List<T>.XUiC_ListEntry;
		XUiC_DMBaseList<T>.ListEntryHoveredDelegate onEntryHovered = this.OnEntryHovered;
		if (onEntryHovered == null)
		{
			return;
		}
		onEntryHovered((xuiC_ListEntry != null) ? xuiC_ListEntry.GetEntry() : default(T), _isOver);
	}

	// Token: 0x060068E3 RID: 26851 RVA: 0x00299D32 File Offset: 0x00297F32
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_DMBaseList()
	{
	}

	// Token: 0x040050DA RID: 20698
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController hoveredElement;

	// Token: 0x02000D90 RID: 3472
	// (Invoke) Token: 0x060068E5 RID: 26853
	public delegate void ListEntryHoveredDelegate(T _entry, bool _isOver);
}
