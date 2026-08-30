using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000F1C RID: 3868
[Preserve]
public class XUiC_EquipmentStackGrid : XUiController
{
	// Token: 0x06007976 RID: 31094 RVA: 0x002DD198 File Offset: 0x002DB398
	public override void Init()
	{
		base.Init();
		XUiC_EquipmentStack[] childrenByType = base.GetChildrenByType<XUiC_EquipmentStack>(null);
		this.RegisterExtraSlots(childrenByType);
		this.IsDirty = false;
		XUiM_PlayerEquipment.HandleRefreshEquipment += this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.OnShutdown += this.handleShutdown;
	}

	// Token: 0x06007977 RID: 31095 RVA: 0x002DD1E9 File Offset: 0x002DB3E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment _playerEquipment)
	{
		if (this.xui.PlayerEquipment == _playerEquipment)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06007978 RID: 31096 RVA: 0x002DD200 File Offset: 0x002DB400
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleShutdown()
	{
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.OnShutdown -= this.handleShutdown;
	}

	// Token: 0x06007979 RID: 31097 RVA: 0x002DD22C File Offset: 0x002DB42C
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

	// Token: 0x0600797A RID: 31098 RVA: 0x002DD2A8 File Offset: 0x002DB4A8
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

	// Token: 0x0600797B RID: 31099 RVA: 0x002DD300 File Offset: 0x002DB500
	public virtual ItemValue[] GetSlots()
	{
		Equipment equipment = this.xui.PlayerEquipment.Equipment;
		ItemValue[] array = new ItemValue[12];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = equipment.GetSlotItem(i) ?? ItemValue.None;
		}
		return array;
	}

	// Token: 0x0600797C RID: 31100 RVA: 0x002DD348 File Offset: 0x002DB548
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

	// Token: 0x0600797D RID: 31101 RVA: 0x002DD3C8 File Offset: 0x002DB5C8
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
			this.xui.PlayerEquipment.Equipment.SetSlotItem(_slotNumber, null, true);
			this.xui.PlayerEquipment.RefreshEquipment();
			return;
		}
		this.items[_slotNumber] = _stack.itemValue.Clone();
		this.xui.PlayerEquipment.Equipment.SetSlotItem(_slotNumber, _stack.itemValue, true);
		this.xui.PlayerEquipment.RefreshEquipment();
		QuestEventManager.Current.WoreItem(_stack.itemValue);
	}

	// Token: 0x0600797E RID: 31102 RVA: 0x002DD49C File Offset: 0x002DB69C
	public XUiC_EquipmentStack GetSlot(EquipmentSlots _slotType)
	{
		XUiC_EquipmentStack xuiC_EquipmentStack;
		this.itemControllers.TryGetValue(_slotType, out xuiC_EquipmentStack);
		return xuiC_EquipmentStack;
	}

	// Token: 0x0600797F RID: 31103 RVA: 0x002DD4B9 File Offset: 0x002DB6B9
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007980 RID: 31104 RVA: 0x002DD4E4 File Offset: 0x002DB6E4
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

	// Token: 0x040059B8 RID: 22968
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EnumDictionary<EquipmentSlots, XUiC_EquipmentStack> itemControllers = new EnumDictionary<EquipmentSlots, XUiC_EquipmentStack>();

	// Token: 0x040059B9 RID: 22969
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue[] items;
}
