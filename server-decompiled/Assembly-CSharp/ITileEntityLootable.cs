using System;

// Token: 0x02000C4F RID: 3151
public interface ITileEntityLootable : ITileEntity, ILockTarget, IInventory
{
	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x06005FD1 RID: 24529
	// (set) Token: 0x06005FD2 RID: 24530
	string lootListName { get; set; }

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x06005FD3 RID: 24531
	float LootStageMod { get; }

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x06005FD4 RID: 24532
	float LootStageBonus { get; }

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06005FD5 RID: 24533
	// (set) Token: 0x06005FD6 RID: 24534
	bool bPlayerStorage { get; set; }

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06005FD7 RID: 24535
	// (set) Token: 0x06005FD8 RID: 24536
	PreferenceTracker preferences { get; set; }

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x06005FD9 RID: 24537
	// (set) Token: 0x06005FDA RID: 24538
	bool bTouched { get; set; }

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x06005FDB RID: 24539
	// (set) Token: 0x06005FDC RID: 24540
	ulong worldTimeTouched { get; set; }

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x06005FDD RID: 24541
	// (set) Token: 0x06005FDE RID: 24542
	bool bWasTouched { get; set; }

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x06005FDF RID: 24543
	// (set) Token: 0x06005FE0 RID: 24544
	ItemStack[] items { get; set; }

	// Token: 0x06005FE1 RID: 24545
	Vector2i GetContainerSize();

	// Token: 0x06005FE2 RID: 24546
	void SetContainerSize(Vector2i _containerSize, bool _clearItems = true);

	// Token: 0x06005FE3 RID: 24547
	void UpdateSlot(int _idx, ItemStack _item);

	// Token: 0x06005FE4 RID: 24548
	void RemoveItem(ItemValue _itemValue);

	// Token: 0x06005FE5 RID: 24549
	int RemoveItems(ItemValue _itemValue, int _count);

	// Token: 0x06005FE6 RID: 24550
	bool IsEmpty();

	// Token: 0x06005FE7 RID: 24551
	void SetEmpty();

	// Token: 0x06005FE8 RID: 24552
	bool ShouldDestroyOnClose();

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06005FE9 RID: 24553
	bool HasSlotLocksSupport { get; }

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06005FEA RID: 24554
	// (set) Token: 0x06005FEB RID: 24555
	PackedBoolArray SlotLocks { get; set; }
}
