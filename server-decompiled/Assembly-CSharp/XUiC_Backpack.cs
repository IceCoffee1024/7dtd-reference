using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000EDD RID: 3805
[Preserve]
public class XUiC_Backpack : XUiC_ItemStackGrid
{
	// Token: 0x17000E09 RID: 3593
	// (get) Token: 0x06007634 RID: 30260 RVA: 0x00020119 File Offset: 0x0001E319
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E0A RID: 3594
	// (get) Token: 0x06007635 RID: 30261 RVA: 0x002C9B61 File Offset: 0x002C7D61
	// (set) Token: 0x06007636 RID: 30262 RVA: 0x002C9B69 File Offset: 0x002C7D69
	[XuiXmlBinding("container_slots")]
	public int BackpackSlots
	{
		get
		{
			return this.backpackSlots;
		}
		set
		{
			if (this.backpackSlots == value)
			{
				return;
			}
			this.backpackSlots = value;
			this.RefreshBackpackSlots();
		}
	}

	// Token: 0x06007637 RID: 30263 RVA: 0x002C9B82 File Offset: 0x002C7D82
	public void RefreshBackpackSlots()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x06007638 RID: 30264 RVA: 0x002C9B97 File Offset: 0x002C7D97
	[PublicizedFrom(EAccessModifier.Protected)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.RefreshBackpackSlots();
		base.updateSearch();
	}

	// Token: 0x06007639 RID: 30265 RVA: 0x002C9BA5 File Offset: 0x002C7DA5
	public override ItemStack[] GetSlots()
	{
		return this.xui.PlayerInventory.GetBackpackItemStacks();
	}

	// Token: 0x0600763A RID: 30266 RVA: 0x002C9BB8 File Offset: 0x002C7DB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemStack[] getUISlots()
	{
		ItemStack[] array = new ItemStack[this.BackpackSlots];
		for (int i = 0; i < this.BackpackSlots; i++)
		{
			array[i] = ((i < this.itemControllers.Length) ? this.itemControllers[i].ItemStack.Clone() : ItemStack.Empty);
		}
		return array;
	}

	// Token: 0x0600763B RID: 30267 RVA: 0x002C9C0A File Offset: 0x002C7E0A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		this.xui.PlayerInventory.SetBackpackItemStacks(stackList);
	}

	// Token: 0x0600763C RID: 30268 RVA: 0x002C9C20 File Offset: 0x002C7E20
	public override void OnOpen()
	{
		base.OnOpen();
		this.wasHoldingItem = !this.xui.DragAndDropWindow.CurrentStack.IsEmpty();
		this.UpdateCallouts(this.wasHoldingItem);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		if (EffectManager.GetValue(PassiveEffects.ShuffledBackpack, null, 0f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0f)
		{
			ItemStack[] slots = this.GetSlots();
			GameRandom rand = this.xui.playerUI.entityPlayer.rand;
			for (int i = 0; i < slots.Length * 2; i++)
			{
				int num = rand.RandomRange(slots.Length);
				int num2 = rand.RandomRange(slots.Length);
				if (!this.itemControllers[num].StackLock && !this.itemControllers[num2].StackLock)
				{
					ItemStack itemStack = slots[num];
					slots[num] = slots[num2];
					slots[num2] = itemStack;
				}
			}
			this.SetStacks(slots);
		}
		else
		{
			this.SetStacks(this.GetSlots());
		}
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
	}

	// Token: 0x0600763D RID: 30269 RVA: 0x002C9D57 File Offset: 0x002C7F57
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
	}

	// Token: 0x0600763E RID: 30270 RVA: 0x002C9D8C File Offset: 0x002C7F8C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.xui.playerUI.entityPlayer.AimingGun = false;
		if (base.ViewComponent.IsVisible)
		{
			bool flag = !this.xui.DragAndDropWindow.CurrentStack.IsEmpty();
			if (flag != this.wasHoldingItem)
			{
				this.wasHoldingItem = flag;
				this.UpdateCallouts(flag);
			}
		}
		if (this.maxSlotCount != this.xui.playerUI.entityPlayer.CarryCapacity)
		{
			this.RefreshBackpackSlots();
		}
	}

	// Token: 0x0600763F RID: 30271 RVA: 0x002C9E18 File Offset: 0x002C8018
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateCallouts(bool _holdingItem)
	{
		string text = string.Format(Localization.Get("igcoItemActions", false, null), InControlExtensions.GetBlankDPadSourceString());
		XUiC_GamepadCalloutWindow calloutWindow = this.xui.CalloutWindow;
		if (calloutWindow != null)
		{
			calloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
			if (!_holdingItem)
			{
				calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoInspect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
				calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, text, XUiC_GamepadCalloutWindow.CalloutType.Menu);
				calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
			}
			calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		}
	}

	// Token: 0x06007640 RID: 30272 RVA: 0x002C9E88 File Offset: 0x002C8088
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		this.maxSlotCount = this.xui.playerUI.entityPlayer.CarryCapacity;
		this.BackpackSlots = this.xui.PlayerInventory.Backpack.SlotCount;
		if (this.BackpackSlots > this.itemControllers.Length)
		{
			Log.Warning(string.Format("[XUi] Current backpack size ({0}) exceeds backpack window's stack slot count ({1})", this.BackpackSlots, this.itemControllers.Length));
		}
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.SlotChangedEvent -= this.handleSlotChangedDelegate;
			xuiC_ItemStack.ItemStack = ((i < stackList.Length) ? stackList[i].Clone() : ItemStack.Empty);
			xuiC_ItemStack.SlotChangedEvent += this.handleSlotChangedDelegate;
			xuiC_ItemStack.SlotNumber = i;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = this.StackLocation;
			xuiC_ItemStack.AttributeLock = i >= this.maxSlotCount;
		}
	}

	// Token: 0x06007641 RID: 30273 RVA: 0x002C9F8B File Offset: 0x002C818B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void dropItemToGrid(ItemStack _dropStack)
	{
		base.dropItemToGrid(_dropStack);
		if (this.xui.PlayerInventory.AddItemToBackpack(_dropStack))
		{
			_dropStack.count = 0;
		}
	}

	// Token: 0x040057AD RID: 22445
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wasHoldingItem;

	// Token: 0x040057AE RID: 22446
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSlotCount;

	// Token: 0x040057AF RID: 22447
	[PublicizedFrom(EAccessModifier.Private)]
	public int backpackSlots;
}
