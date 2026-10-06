using System;

// Token: 0x02001148 RID: 4424
public class XUiM_PlayerEquipment : XUiModel
{
	// Token: 0x140000E3 RID: 227
	// (add) Token: 0x06008B89 RID: 35721 RVA: 0x00353E00 File Offset: 0x00352000
	// (remove) Token: 0x06008B8A RID: 35722 RVA: 0x00353E34 File Offset: 0x00352034
	public static event XUiEvent_RefreshEquipment HandleRefreshEquipment;

	// Token: 0x140000E4 RID: 228
	// (add) Token: 0x06008B8B RID: 35723 RVA: 0x00353E68 File Offset: 0x00352068
	// (remove) Token: 0x06008B8C RID: 35724 RVA: 0x00353E9C File Offset: 0x0035209C
	public static event XUiEvent_EquipmentSlotChanged SlotChanged;

	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x06008B8D RID: 35725 RVA: 0x00353ECF File Offset: 0x003520CF
	public Equipment Equipment
	{
		get
		{
			return this.equipment;
		}
	}

	// Token: 0x06008B8E RID: 35726 RVA: 0x00353ED7 File Offset: 0x003520D7
	public XUiM_PlayerEquipment(XUi _xui, EntityPlayerLocal _player)
	{
		if (!_player)
		{
			return;
		}
		this.xui = _xui;
		this.equipment = _player.equipment;
	}

	// Token: 0x06008B8F RID: 35727 RVA: 0x00353EFC File Offset: 0x003520FC
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

	// Token: 0x06008B90 RID: 35728 RVA: 0x00353FD4 File Offset: 0x003521D4
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

	// Token: 0x06008B91 RID: 35729 RVA: 0x00354019 File Offset: 0x00352219
	public bool IsEquipmentTypeWorn(EquipmentSlots _slot)
	{
		return _slot != EquipmentSlots.Count && !this.GetStackFromSlot(_slot).itemValue.IsEmpty();
	}

	// Token: 0x06008B92 RID: 35730 RVA: 0x00354036 File Offset: 0x00352236
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

	// Token: 0x06008B93 RID: 35731 RVA: 0x00354060 File Offset: 0x00352260
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

	// Token: 0x06008B94 RID: 35732 RVA: 0x00354094 File Offset: 0x00352294
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

	// Token: 0x04006757 RID: 26455
	public bool IsOpen;

	// Token: 0x0400675A RID: 26458
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x0400675B RID: 26459
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Equipment equipment;

	// Token: 0x0400675C RID: 26460
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStackGrid equipmentGrid;
}
