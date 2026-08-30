using System;

// Token: 0x02000D6C RID: 3436
public abstract class XUiC_DMBaseList<T> : XUiC_List<T> where T : XUiListEntry<T>
{
	// Token: 0x1400008E RID: 142
	// (add) Token: 0x0600684A RID: 26698 RVA: 0x00293688 File Offset: 0x00291888
	// (remove) Token: 0x0600684B RID: 26699 RVA: 0x002936C0 File Offset: 0x002918C0
	public event XUiC_DMBaseList<T>.ListEntryHoveredDelegate OnEntryHovered;

	// Token: 0x0600684C RID: 26700 RVA: 0x002936F8 File Offset: 0x002918F8
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

	// Token: 0x0600684D RID: 26701 RVA: 0x002937B4 File Offset: 0x002919B4
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

	// Token: 0x0600684E RID: 26702 RVA: 0x0029380C File Offset: 0x00291A0C
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

	// Token: 0x0600684F RID: 26703 RVA: 0x00293852 File Offset: 0x00291A52
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_DMBaseList()
	{
	}

	// Token: 0x04005001 RID: 20481
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController hoveredElement;

	// Token: 0x02000D6D RID: 3437
	// (Invoke) Token: 0x06006851 RID: 26705
	public delegate void ListEntryHoveredDelegate(T _entry, bool _isOver);
}
