using System;

// Token: 0x02000D6B RID: 3435
public interface IItemStackController
{
	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x060067F7 RID: 26615
	// (set) Token: 0x060067F8 RID: 26616
	ItemStack ItemStack { get; set; }

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x060067F9 RID: 26617
	// (set) Token: 0x060067FA RID: 26618
	bool IsSelected { get; set; }

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x060067FB RID: 26619
	int SlotNumber { get; }

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x060067FC RID: 26620
	XUiC_ItemStack.StackLocationTypes StackLocation { get; }

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x060067FD RID: 26621
	// (set) Token: 0x060067FE RID: 26622
	XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x060067FF RID: 26623
	XUiWindowGroup WindowGroup { get; }

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x06006800 RID: 26624
	XUi xui { get; }

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x06006801 RID: 26625
	XUiController Controller { get; }

	// Token: 0x06006802 RID: 26626
	void ForceRefreshItemStack();

	// Token: 0x06006803 RID: 26627
	void HandleMoveToPreferredLocation();
}
