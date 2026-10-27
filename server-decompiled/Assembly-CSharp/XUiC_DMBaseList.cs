using System;

// Token: 0x02000D9A RID: 3482
public abstract class XUiC_DMBaseList<T> : XUiC_List<T> where T : XUiListEntry<T>
{
	// Token: 0x1400008E RID: 142
	// (add) Token: 0x06006903 RID: 26883 RVA: 0x0029B090 File Offset: 0x00299290
	// (remove) Token: 0x06006904 RID: 26884 RVA: 0x0029B0C8 File Offset: 0x002992C8
	public event XUiC_DMBaseList<T>.ListEntryHoveredDelegate OnEntryHovered;

	// Token: 0x06006905 RID: 26885 RVA: 0x0029B100 File Offset: 0x00299300
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

	// Token: 0x06006906 RID: 26886 RVA: 0x0029B1BC File Offset: 0x002993BC
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

	// Token: 0x06006907 RID: 26887 RVA: 0x0029B214 File Offset: 0x00299414
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

	// Token: 0x06006908 RID: 26888 RVA: 0x0029B25A File Offset: 0x0029945A
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_DMBaseList()
	{
	}

	// Token: 0x040050FF RID: 20735
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController hoveredElement;

	// Token: 0x02000D9B RID: 3483
	// (Invoke) Token: 0x0600690A RID: 26890
	public delegate void ListEntryHoveredDelegate(T _entry, bool _isOver);
}
