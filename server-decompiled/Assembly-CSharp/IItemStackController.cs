using System;

// Token: 0x02000D60 RID: 3424
public interface IItemStackController
{
	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x060067D2 RID: 26578
	// (set) Token: 0x060067D3 RID: 26579
	ItemStack ItemStack { get; set; }

	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x060067D4 RID: 26580
	// (set) Token: 0x060067D5 RID: 26581
	bool IsSelected { get; set; }

	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x060067D6 RID: 26582
	int SlotNumber { get; }

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x060067D7 RID: 26583
	XUiC_ItemStack.StackLocationTypes StackLocation { get; }

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x060067D8 RID: 26584
	// (set) Token: 0x060067D9 RID: 26585
	XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x060067DA RID: 26586
	XUiWindowGroup WindowGroup { get; }

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x060067DB RID: 26587
	XUi xui { get; }

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x060067DC RID: 26588
	XUiController Controller { get; }

	// Token: 0x060067DD RID: 26589
	void ForceRefreshItemStack();

	// Token: 0x060067DE RID: 26590
	void HandleMoveToPreferredLocation();
}
