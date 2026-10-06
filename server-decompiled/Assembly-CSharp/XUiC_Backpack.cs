using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000ED2 RID: 3794
[Preserve]
public class XUiC_Backpack : XUiC_ItemStackGrid
{
	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x0600760F RID: 30223 RVA: 0x00020119 File Offset: 0x0001E319
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x06007610 RID: 30224 RVA: 0x002C85F5 File Offset: 0x002C67F5
	// (set) Token: 0x06007611 RID: 30225 RVA: 0x002C85FD File Offset: 0x002C67FD
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

	// Token: 0x06007612 RID: 30226 RVA: 0x002C8616 File Offset: 0x002C6816
	public void RefreshBackpackSlots()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x06007613 RID: 30227 RVA: 0x002C862B File Offset: 0x002C682B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.RefreshBackpackSlots();
	}

	// Token: 0x06007614 RID: 30228 RVA: 0x002C8633 File Offset: 0x002C6833
	public override ItemStack[] GetSlots()
	{
		return this.xui.PlayerInventory.GetBackpackItemStacks();
	}

	// Token: 0x06007615 RID: 30229 RVA: 0x002C8648 File Offset: 0x002C6848
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

	// Token: 0x06007616 RID: 30230 RVA: 0x002C869A File Offset: 0x002C689A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		this.xui.PlayerInventory.SetBackpackItemStacks(stackList);
	}

	// Token: 0x06007617 RID: 30231 RVA: 0x002C86B0 File Offset: 0x002C68B0
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

	// Token: 0x06007618 RID: 30232 RVA: 0x002C87E7 File Offset: 0x002C69E7
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
	}

	// Token: 0x06007619 RID: 30233 RVA: 0x002C881C File Offset: 0x002C6A1C
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

	// Token: 0x0600761A RID: 30234 RVA: 0x002C88A8 File Offset: 0x002C6AA8
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

	// Token: 0x0600761B RID: 30235 RVA: 0x002C8918 File Offset: 0x002C6B18
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

	// Token: 0x04005788 RID: 22408
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wasHoldingItem;

	// Token: 0x04005789 RID: 22409
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSlotCount;

	// Token: 0x0400578A RID: 22410
	[PublicizedFrom(EAccessModifier.Private)]
	public int backpackSlots;
}
