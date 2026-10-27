using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000F51 RID: 3921
[Preserve]
public class XUiC_EquipmentStackGrid : XUiController
{
	// Token: 0x06007A6F RID: 31343 RVA: 0x002E5DB0 File Offset: 0x002E3FB0
	public override void Init()
	{
		base.Init();
		XUiC_EquipmentStack[] childrenByType = base.GetChildrenByType<XUiC_EquipmentStack>(null);
		this.RegisterExtraSlots(childrenByType);
		this.IsDirty = false;
		XUiM_PlayerEquipment.HandleRefreshEquipment += this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.OnShutdown += this.handleShutdown;
	}

	// Token: 0x06007A70 RID: 31344 RVA: 0x002E5E01 File Offset: 0x002E4001
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment _playerEquipment)
	{
		if (this.xui.PlayerEquipment == _playerEquipment)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06007A71 RID: 31345 RVA: 0x002E5E18 File Offset: 0x002E4018
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleShutdown()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.OnShutdown -= this.handleShutdown;
	}

	// Token: 0x06007A72 RID: 31346 RVA: 0x002E5E44 File Offset: 0x002E4044
	public void RegisterExtraSlots(IEnumerable<XUiC_EquipmentStack> _slots)
	{
		foreach (XUiC_EquipmentStack xuiC_EquipmentStack in _slots)
		{
			if (xuiC_EquipmentStack.EquipSlot == EquipmentSlots.Count)
			{
				Log.Error("[XUi] EquipmentStack slot does not have a proper 'slot' value");
			}
			this.itemControllers[xuiC_EquipmentStack.EquipSlot] = xuiC_EquipmentStack;
			xuiC_EquipmentStack.SlotChangedEvent += this.HandleSlotChangedEvent;
		}
	}

	// Token: 0x06007A73 RID: 31347 RVA: 0x002E5EC0 File Offset: 0x002E40C0
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.IsDirty)
		{
			this.items = this.GetSlots();
			this.SetStacks(this.items);
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06007A74 RID: 31348 RVA: 0x002E5F18 File Offset: 0x002E4118
	public virtual ItemValue[] GetSlots()
	{
		Equipment equipment = this.xui.PlayerEquipment.Equipment;
		ItemValue[] array = new ItemValue[13];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = equipment.GetSlotItem(i) ?? ItemValue.None;
		}
		return array;
	}

	// Token: 0x06007A75 RID: 31349 RVA: 0x002E5F60 File Offset: 0x002E4160
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetStacks(ItemValue[] _stackList)
	{
		if (_stackList == null)
		{
			return;
		}
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		XUiC_CharacterFrameWindow childByType2 = this.xui.GetChildByType<XUiC_CharacterFrameWindow>();
		for (int i = 0; i < _stackList.Length; i++)
		{
			XUiC_EquipmentStack slot = this.GetSlot((EquipmentSlots)i);
			if (slot != null)
			{
				slot.SlotChangedEvent -= this.HandleSlotChangedEvent;
				slot.ItemValue = _stackList[i];
				slot.SlotChangedEvent += this.HandleSlotChangedEvent;
				slot.InfoWindow = childByType;
				slot.FrameWindow = childByType2;
			}
		}
	}

	// Token: 0x06007A76 RID: 31350 RVA: 0x002E5FE0 File Offset: 0x002E41E0
	public void HandleSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		if (this.items == null)
		{
			this.items = this.GetSlots();
		}
		if (!_stack.IsEmpty())
		{
			string soundPlace = _stack.itemValue.ItemClass.SoundPlace;
			if (!string.IsNullOrEmpty(soundPlace))
			{
				Manager.PlayInsidePlayerHead(soundPlace, -1, 0f, false, false);
			}
		}
		if (_stack.IsEmpty())
		{
			this.xui.PlayerEquipment.Equipment.SetSlotItem(_slotNumber, null);
			this.xui.PlayerEquipment.RefreshEquipment();
			return;
		}
		this.items[_slotNumber] = _stack.itemValue.Clone();
		this.xui.PlayerEquipment.Equipment.SetSlotItem(_slotNumber, _stack.itemValue);
		this.xui.PlayerEquipment.RefreshEquipment();
		QuestEventManager.Current.WoreItem(_stack.itemValue);
	}

	// Token: 0x06007A77 RID: 31351 RVA: 0x002E60B0 File Offset: 0x002E42B0
	public XUiC_EquipmentStack GetSlot(EquipmentSlots _slotType)
	{
		XUiC_EquipmentStack xuiC_EquipmentStack;
		this.itemControllers.TryGetValue(_slotType, out xuiC_EquipmentStack);
		return xuiC_EquipmentStack;
	}

	// Token: 0x06007A78 RID: 31352 RVA: 0x002E60CD File Offset: 0x002E42CD
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007A79 RID: 31353 RVA: 0x002E60F8 File Offset: 0x002E42F8
	public override void OnClose()
	{
		foreach (KeyValuePair<EquipmentSlots, XUiC_EquipmentStack> keyValuePair in this.itemControllers)
		{
			EquipmentSlots equipmentSlots;
			XUiC_EquipmentStack xuiC_EquipmentStack;
			keyValuePair.Deconstruct(out equipmentSlots, out xuiC_EquipmentStack);
			xuiC_EquipmentStack.Hovered(false);
		}
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005AD6 RID: 23254
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EnumDictionary<EquipmentSlots, XUiC_EquipmentStack> itemControllers = new EnumDictionary<EquipmentSlots, XUiC_EquipmentStack>();

	// Token: 0x04005AD7 RID: 23255
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue[] items;
}
