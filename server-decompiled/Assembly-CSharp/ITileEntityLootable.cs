using System;

// Token: 0x02000C61 RID: 3169
public interface ITileEntityLootable : ITileEntity, ILockTarget, IInventory
{
	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x0600605B RID: 24667
	// (set) Token: 0x0600605C RID: 24668
	string lootListName { get; set; }

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x0600605D RID: 24669
	float LootStageMod { get; }

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x0600605E RID: 24670
	float LootStageBonus { get; }

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x0600605F RID: 24671
	// (set) Token: 0x06006060 RID: 24672
	bool bPlayerStorage { get; set; }

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x06006061 RID: 24673
	// (set) Token: 0x06006062 RID: 24674
	PreferenceTracker preferences { get; set; }

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06006063 RID: 24675
	// (set) Token: 0x06006064 RID: 24676
	bool bTouched { get; set; }

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06006065 RID: 24677
	// (set) Token: 0x06006066 RID: 24678
	ulong worldTimeTouched { get; set; }

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x06006067 RID: 24679
	// (set) Token: 0x06006068 RID: 24680
	bool bWasTouched { get; set; }

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06006069 RID: 24681
	// (set) Token: 0x0600606A RID: 24682
	ItemStack[] items { get; set; }

	// Token: 0x0600606B RID: 24683
	Vector2i GetContainerSize();

	// Token: 0x0600606C RID: 24684
	void SetContainerSize(Vector2i _containerSize, bool _clearItems = true);

	// Token: 0x0600606D RID: 24685
	void UpdateSlot(int _idx, ItemStack _item);

	// Token: 0x0600606E RID: 24686
	void RemoveItem(ItemValue _itemValue);

	// Token: 0x0600606F RID: 24687
	int RemoveItems(ItemValue _itemValue, int _count);

	// Token: 0x06006070 RID: 24688
	bool IsEmpty();

	// Token: 0x06006071 RID: 24689
	void SetEmpty();

	// Token: 0x06006072 RID: 24690
	bool ShouldDestroyOnClose();

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x06006073 RID: 24691
	bool HasSlotLocksSupport { get; }

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x06006074 RID: 24692
	// (set) Token: 0x06006075 RID: 24693
	PackedBoolArray SlotLocks { get; set; }
}
