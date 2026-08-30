using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000EAE RID: 3758
[Preserve]
public class XUiC_Backpack : XUiC_ItemStackGrid
{
	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x06007576 RID: 30070 RVA: 0x00010E62 File Offset: 0x0000F062
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x06007577 RID: 30071 RVA: 0x002C1D4D File Offset: 0x002BFF4D
	public void RefreshBackpackSlots()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x06007578 RID: 30072 RVA: 0x002C1D62 File Offset: 0x002BFF62
	[PublicizedFrom(EAccessModifier.Protected)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.RefreshBackpackSlots();
	}

	// Token: 0x06007579 RID: 30073 RVA: 0x002C1D6A File Offset: 0x002BFF6A
	public override ItemStack[] GetSlots()
	{
		return this.xui.PlayerInventory.GetBackpackItemStacks();
	}

	// Token: 0x0600757A RID: 30074 RVA: 0x002C1D7C File Offset: 0x002BFF7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		this.xui.PlayerInventory.SetBackpackItemStacks(stackList);
	}

	// Token: 0x0600757B RID: 30075 RVA: 0x002C1D90 File Offset: 0x002BFF90
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

	// Token: 0x0600757C RID: 30076 RVA: 0x002C1EC7 File Offset: 0x002C00C7
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
	}

	// Token: 0x0600757D RID: 30077 RVA: 0x002C1EFC File Offset: 0x002C00FC
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

	// Token: 0x0600757E RID: 30078 RVA: 0x002C1F88 File Offset: 0x002C0188
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

	// Token: 0x0600757F RID: 30079 RVA: 0x002C1FF8 File Offset: 0x002C01F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		this.maxSlotCount = this.xui.playerUI.entityPlayer.CarryCapacity;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		int num = 0;
		int num2 = 0;
		while (num2 < stackList.Length && this.itemControllers.Length > num2 && stackList.Length > num2)
		{
			num = num2;
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[num2];
			xuiC_ItemStack.SlotChangedEvent -= this.handleSlotChangedDelegate;
			xuiC_ItemStack.ItemStack = stackList[num2].Clone();
			xuiC_ItemStack.SlotChangedEvent += this.handleSlotChangedDelegate;
			xuiC_ItemStack.SlotNumber = num2;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = this.StackLocation;
			bool flag = num2 >= this.maxSlotCount;
			if (xuiC_ItemStack.AttributeLock != flag)
			{
				xuiC_ItemStack.AttributeLock = flag;
			}
			num2++;
		}
		for (int i = num; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack2 = this.itemControllers[i];
			bool flag2 = i >= this.maxSlotCount;
			if (xuiC_ItemStack2.AttributeLock != flag2)
			{
				xuiC_ItemStack2.AttributeLock = flag2;
			}
		}
	}

	// Token: 0x040056AC RID: 22188
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wasHoldingItem;

	// Token: 0x040056AD RID: 22189
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSlotCount;
}
