using System;

// Token: 0x02001117 RID: 4375
public class XUiM_PlayerEquipment : XUiModel
{
	// Token: 0x140000E3 RID: 227
	// (add) Token: 0x06008A2C RID: 35372 RVA: 0x0034B1C4 File Offset: 0x003493C4
	// (remove) Token: 0x06008A2D RID: 35373 RVA: 0x0034B1F8 File Offset: 0x003493F8
	public static event XUiEvent_RefreshEquipment HandleRefreshEquipment;

	// Token: 0x140000E4 RID: 228
	// (add) Token: 0x06008A2E RID: 35374 RVA: 0x0034B22C File Offset: 0x0034942C
	// (remove) Token: 0x06008A2F RID: 35375 RVA: 0x0034B260 File Offset: 0x00349460
	public static event XUiEvent_EquipmentSlotChanged SlotChanged;

	// Token: 0x17000FDD RID: 4061
	// (get) Token: 0x06008A30 RID: 35376 RVA: 0x0034B293 File Offset: 0x00349493
	public Equipment Equipment
	{
		get
		{
			return this.equipment;
		}
	}

	// Token: 0x06008A31 RID: 35377 RVA: 0x0034B29B File Offset: 0x0034949B
	public XUiM_PlayerEquipment(XUi _xui, EntityPlayerLocal _player)
	{
		if (!_player)
		{
			return;
		}
		this.xui = _xui;
		this.equipment = _player.equipment;
	}

	// Token: 0x06008A32 RID: 35378 RVA: 0x0034B2C0 File Offset: 0x003494C0
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

	// Token: 0x06008A33 RID: 35379 RVA: 0x0034B398 File Offset: 0x00349598
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

	// Token: 0x06008A34 RID: 35380 RVA: 0x0034B3DD File Offset: 0x003495DD
	public bool IsEquipmentTypeWorn(EquipmentSlots _slot)
	{
		return _slot != EquipmentSlots.Count && !this.GetStackFromSlot(_slot).itemValue.IsEmpty();
	}

	// Token: 0x06008A35 RID: 35381 RVA: 0x0034B3FA File Offset: 0x003495FA
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

	// Token: 0x06008A36 RID: 35382 RVA: 0x0034B424 File Offset: 0x00349624
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

	// Token: 0x06008A37 RID: 35383 RVA: 0x0034B458 File Offset: 0x00349658
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

	// Token: 0x0400663B RID: 26171
	public bool IsOpen;

	// Token: 0x0400663E RID: 26174
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x0400663F RID: 26175
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Equipment equipment;

	// Token: 0x04006640 RID: 26176
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStackGrid equipmentGrid;
}
