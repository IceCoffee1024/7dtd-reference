using System;

// Token: 0x02001130 RID: 4400
public class XUiM_PlayerEquipment : XUiModel
{
	// Token: 0x140000E4 RID: 228
	// (add) Token: 0x06008B1E RID: 35614 RVA: 0x0034F9E8 File Offset: 0x0034DBE8
	// (remove) Token: 0x06008B1F RID: 35615 RVA: 0x0034FA1C File Offset: 0x0034DC1C
	public static event XUiEvent_RefreshEquipment HandleRefreshEquipment;

	// Token: 0x140000E5 RID: 229
	// (add) Token: 0x06008B20 RID: 35616 RVA: 0x0034FA50 File Offset: 0x0034DC50
	// (remove) Token: 0x06008B21 RID: 35617 RVA: 0x0034FA84 File Offset: 0x0034DC84
	public static event XUiEvent_EquipmentSlotChanged SlotChanged;

	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x06008B22 RID: 35618 RVA: 0x0034FAB7 File Offset: 0x0034DCB7
	public Equipment Equipment
	{
		get
		{
			return this.equipment;
		}
	}

	// Token: 0x06008B23 RID: 35619 RVA: 0x0034FABF File Offset: 0x0034DCBF
	public XUiM_PlayerEquipment(XUi _xui, EntityPlayerLocal _player)
	{
		if (!_player)
		{
			return;
		}
		this.xui = _xui;
		this.equipment = _player.equipment;
	}

	// Token: 0x06008B24 RID: 35620 RVA: 0x0034FAE4 File Offset: 0x0034DCE4
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

	// Token: 0x06008B25 RID: 35621 RVA: 0x0034FBBC File Offset: 0x0034DDBC
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

	// Token: 0x06008B26 RID: 35622 RVA: 0x0034FC01 File Offset: 0x0034DE01
	public bool IsEquipmentTypeWorn(EquipmentSlots _slot)
	{
		return _slot != EquipmentSlots.Count && !this.GetStackFromSlot(_slot).itemValue.IsEmpty();
	}

	// Token: 0x06008B27 RID: 35623 RVA: 0x0034FC1E File Offset: 0x0034DE1E
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

	// Token: 0x06008B28 RID: 35624 RVA: 0x0034FC48 File Offset: 0x0034DE48
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

	// Token: 0x06008B29 RID: 35625 RVA: 0x0034FC7C File Offset: 0x0034DE7C
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

	// Token: 0x04006702 RID: 26370
	public bool IsOpen;

	// Token: 0x04006705 RID: 26373
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x04006706 RID: 26374
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Equipment equipment;

	// Token: 0x04006707 RID: 26375
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStackGrid equipmentGrid;
}
