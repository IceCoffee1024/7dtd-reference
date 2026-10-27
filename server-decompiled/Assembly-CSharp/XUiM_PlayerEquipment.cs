using System;

// Token: 0x02001158 RID: 4440
public class XUiM_PlayerEquipment : XUiModel
{
	// Token: 0x140000E3 RID: 227
	// (add) Token: 0x06008BDD RID: 35805 RVA: 0x00355EB0 File Offset: 0x003540B0
	// (remove) Token: 0x06008BDE RID: 35806 RVA: 0x00355EE4 File Offset: 0x003540E4
	public static event XUiEvent_RefreshEquipment HandleRefreshEquipment;

	// Token: 0x140000E4 RID: 228
	// (add) Token: 0x06008BDF RID: 35807 RVA: 0x00355F18 File Offset: 0x00354118
	// (remove) Token: 0x06008BE0 RID: 35808 RVA: 0x00355F4C File Offset: 0x0035414C
	public static event XUiEvent_EquipmentSlotChanged SlotChanged;

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06008BE1 RID: 35809 RVA: 0x00355F7F File Offset: 0x0035417F
	public Equipment Equipment
	{
		get
		{
			return this.equipment;
		}
	}

	// Token: 0x06008BE2 RID: 35810 RVA: 0x00355F87 File Offset: 0x00354187
	public XUiM_PlayerEquipment(XUi _xui, EntityPlayerLocal _player)
	{
		if (!_player)
		{
			return;
		}
		this.xui = _xui;
		this.equipment = _player.equipment;
	}

	// Token: 0x06008BE3 RID: 35811 RVA: 0x00355FAC File Offset: 0x003541AC
	public ItemStack EquipItem(ItemStack _stack)
	{
		ItemClassArmor itemClassArmor = _stack.itemValue.ItemClass as ItemClassArmor;
		if (itemClassArmor == null)
		{
			this.RefreshEquipment();
			return _stack;
		}
		EquipmentSlots equipSlot = itemClassArmor.EquipSlot;
		XUiC_EquipmentStack equipmentStack = this.GetEquipmentStack(equipSlot);
		if (equipmentStack == null)
		{
			return _stack;
		}
		ItemStack stackFromSlot = this.GetStackFromSlot(equipSlot);
		bool flag = true;
		if (!stackFromSlot.IsEmpty())
		{
			ItemClassArmor itemClassArmor2 = stackFromSlot.itemValue.ItemClass as ItemClassArmor;
			if (itemClassArmor2 != null)
			{
				if (itemClassArmor2.ReplaceByTag != null && !FastTags<TagGroup.Global>.Parse(itemClassArmor2.ReplaceByTag).Test_AnySet(itemClassArmor.ItemTags))
				{
					return _stack;
				}
				flag = itemClassArmor2.AllowUnEquip;
			}
		}
		equipmentStack.ItemStack = _stack.Clone();
		QuestEventManager.Current.WoreItem(_stack.itemValue);
		this.RefreshEquipment();
		XUiEvent_EquipmentSlotChanged slotChanged = XUiM_PlayerEquipment.SlotChanged;
		if (slotChanged != null)
		{
			slotChanged(this, equipSlot, _stack);
		}
		if (!flag)
		{
			return ItemStack.Empty;
		}
		return stackFromSlot;
	}

	// Token: 0x06008BE4 RID: 35812 RVA: 0x00356084 File Offset: 0x00354284
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool IsWearing(ItemValue _itemValue)
	{
		ItemClassArmor itemClassArmor = _itemValue.ItemClass as ItemClassArmor;
		if (itemClassArmor == null)
		{
			return false;
		}
		EquipmentSlots equipSlot = itemClassArmor.EquipSlot;
		return equipSlot != EquipmentSlots.Count && this.GetStackFromSlot(equipSlot).itemValue.type == _itemValue.type;
	}

	// Token: 0x06008BE5 RID: 35813 RVA: 0x003560C9 File Offset: 0x003542C9
	public bool IsEquipmentTypeWorn(EquipmentSlots _slot)
	{
		return _slot != EquipmentSlots.Count && !this.GetStackFromSlot(_slot).itemValue.IsEmpty();
	}

	// Token: 0x06008BE6 RID: 35814 RVA: 0x003560E6 File Offset: 0x003542E6
	public void RefreshEquipment()
	{
		this.equipment.FireEventsForSetSlots();
		XUiEvent_RefreshEquipment handleRefreshEquipment = XUiM_PlayerEquipment.HandleRefreshEquipment;
		if (handleRefreshEquipment != null)
		{
			handleRefreshEquipment(this);
		}
		this.equipment.FireEventsForChangedSlots();
	}

	// Token: 0x06008BE7 RID: 35815 RVA: 0x00356110 File Offset: 0x00354310
	public ItemStack GetStackFromSlot(EquipmentSlots _slot)
	{
		ItemStack empty = ItemStack.Empty;
		ItemValue slotItem = this.Equipment.GetSlotItem((int)_slot);
		if (slotItem == null)
		{
			return empty;
		}
		empty.itemValue = slotItem;
		empty.count = 1;
		return empty;
	}

	// Token: 0x06008BE8 RID: 35816 RVA: 0x00356144 File Offset: 0x00354344
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStack GetEquipmentStack(EquipmentSlots _slot)
	{
		if (this.equipmentGrid == null)
		{
			this.equipmentGrid = this.xui.GetChildByType<XUiC_EquipmentStackGrid>();
		}
		XUiC_EquipmentStackGrid xuiC_EquipmentStackGrid = this.equipmentGrid;
		if (xuiC_EquipmentStackGrid == null)
		{
			return null;
		}
		return xuiC_EquipmentStackGrid.GetSlot(_slot);
	}

	// Token: 0x04006797 RID: 26519
	public bool IsOpen;

	// Token: 0x0400679A RID: 26522
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x0400679B RID: 26523
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Equipment equipment;

	// Token: 0x0400679C RID: 26524
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStackGrid equipmentGrid;
}
