using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000EC3 RID: 3779
[Preserve]
public class XUiC_Backpack : XUiC_ItemStackGrid
{
	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x06007617 RID: 30231 RVA: 0x00010E62 File Offset: 0x0000F062
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x06007618 RID: 30232 RVA: 0x002C5271 File Offset: 0x002C3471
	public void RefreshBackpackSlots()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x06007619 RID: 30233 RVA: 0x002C5286 File Offset: 0x002C3486
	[PublicizedFrom(EAccessModifier.Protected)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.RefreshBackpackSlots();
	}

	// Token: 0x0600761A RID: 30234 RVA: 0x002C528E File Offset: 0x002C348E
	public override ItemStack[] GetSlots()
	{
		return this.xui.PlayerInventory.GetBackpackItemStacks();
	}

	// Token: 0x0600761B RID: 30235 RVA: 0x002C52A0 File Offset: 0x002C34A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		this.xui.PlayerInventory.SetBackpackItemStacks(stackList);
	}

	// Token: 0x0600761C RID: 30236 RVA: 0x002C52B4 File Offset: 0x002C34B4
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

	// Token: 0x0600761D RID: 30237 RVA: 0x002C53EB File Offset: 0x002C35EB
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
	}

	// Token: 0x0600761E RID: 30238 RVA: 0x002C5420 File Offset: 0x002C3620
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

	// Token: 0x0600761F RID: 30239 RVA: 0x002C54AC File Offset: 0x002C36AC
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

	// Token: 0x06007620 RID: 30240 RVA: 0x002C551C File Offset: 0x002C371C
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

	// Token: 0x04005751 RID: 22353
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wasHoldingItem;

	// Token: 0x04005752 RID: 22354
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSlotCount;
}
