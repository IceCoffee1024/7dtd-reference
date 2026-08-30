using System;

// Token: 0x02000D80 RID: 3456
public abstract class XUiC_DMBaseList<T> : XUiC_List<T> where T : XUiListEntry<T>
{
	// Token: 0x1400008F RID: 143
	// (add) Token: 0x060068E6 RID: 26854 RVA: 0x002967DC File Offset: 0x002949DC
	// (remove) Token: 0x060068E7 RID: 26855 RVA: 0x00296814 File Offset: 0x00294A14
	public event XUiC_DMBaseList<T>.ListEntryHoveredDelegate OnEntryHovered;

	// Token: 0x060068E8 RID: 26856 RVA: 0x0029684C File Offset: 0x00294A4C
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

	// Token: 0x060068E9 RID: 26857 RVA: 0x00296908 File Offset: 0x00294B08
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

	// Token: 0x060068EA RID: 26858 RVA: 0x00296960 File Offset: 0x00294B60
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

	// Token: 0x060068EB RID: 26859 RVA: 0x002969A6 File Offset: 0x00294BA6
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_DMBaseList()
	{
	}

	// Token: 0x040050A3 RID: 20643
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController hoveredElement;

	// Token: 0x02000D81 RID: 3457
	// (Invoke) Token: 0x060068ED RID: 26861
	public delegate void ListEntryHoveredDelegate(T _entry, bool _isOver);
}
