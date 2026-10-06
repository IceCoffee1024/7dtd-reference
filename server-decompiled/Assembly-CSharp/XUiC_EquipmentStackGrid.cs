using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000F42 RID: 3906
[Preserve]
public class XUiC_EquipmentStackGrid : XUiController
{
	// Token: 0x06007A2C RID: 31276 RVA: 0x002E44C0 File Offset: 0x002E26C0
	public override void Init()
	{
		base.Init();
		XUiC_EquipmentStack[] childrenByType = base.GetChildrenByType<XUiC_EquipmentStack>(null);
		this.RegisterExtraSlots(childrenByType);
		this.IsDirty = false;
		XUiM_PlayerEquipment.HandleRefreshEquipment += this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.OnShutdown += this.handleShutdown;
	}

	// Token: 0x06007A2D RID: 31277 RVA: 0x002E4511 File Offset: 0x002E2711
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment _playerEquipment)
	{
		if (this.xui.PlayerEquipment == _playerEquipment)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06007A2E RID: 31278 RVA: 0x002E4528 File Offset: 0x002E2728
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleShutdown()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.OnShutdown -= this.handleShutdown;
	}

	// Token: 0x06007A2F RID: 31279 RVA: 0x002E4554 File Offset: 0x002E2754
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

	// Token: 0x06007A30 RID: 31280 RVA: 0x002E45D0 File Offset: 0x002E27D0
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

	// Token: 0x06007A31 RID: 31281 RVA: 0x002E4628 File Offset: 0x002E2828
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

	// Token: 0x06007A32 RID: 31282 RVA: 0x002E4670 File Offset: 0x002E2870
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

	// Token: 0x06007A33 RID: 31283 RVA: 0x002E46F0 File Offset: 0x002E28F0
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

	// Token: 0x06007A34 RID: 31284 RVA: 0x002E47C0 File Offset: 0x002E29C0
	public XUiC_EquipmentStack GetSlot(EquipmentSlots _slotType)
	{
		XUiC_EquipmentStack xuiC_EquipmentStack;
		this.itemControllers.TryGetValue(_slotType, out xuiC_EquipmentStack);
		return xuiC_EquipmentStack;
	}

	// Token: 0x06007A35 RID: 31285 RVA: 0x002E47DD File Offset: 0x002E29DD
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007A36 RID: 31286 RVA: 0x002E4808 File Offset: 0x002E2A08
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

	// Token: 0x04005AA3 RID: 23203
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EnumDictionary<EquipmentSlots, XUiC_EquipmentStack> itemControllers = new EnumDictionary<EquipmentSlots, XUiC_EquipmentStack>();

	// Token: 0x04005AA4 RID: 23204
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue[] items;
}
