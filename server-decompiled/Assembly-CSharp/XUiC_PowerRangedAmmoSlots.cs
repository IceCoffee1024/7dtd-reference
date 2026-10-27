using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FD5 RID: 4053
[Preserve]
public class XUiC_PowerRangedAmmoSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x06007F66 RID: 32614 RVA: 0x003078EE File Offset: 0x00305AEE
	// (set) Token: 0x06007F67 RID: 32615 RVA: 0x003078F6 File Offset: 0x00305AF6
	public TileEntityPoweredRangedTrap TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.SetSlots(this.tileEntity.ItemSlots);
		}
	}

	// Token: 0x06007F68 RID: 32616 RVA: 0x00307910 File Offset: 0x00305B10
	public override void HandleSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		base.HandleSlotChangedEvent(_slotNumber, _stack);
		this.tileEntity.SetSendSlots();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007F69 RID: 32617 RVA: 0x00307930 File Offset: 0x00305B30
	public override void Init()
	{
		base.Init();
		this.btnOn = this.windowGroup.Controller.GetChildById("windowPowerTrapSlots").GetChildById("btnOn");
		this.btnOn_Background = (XUiV_Button)this.btnOn.GetChildById("clickable").ViewComponent;
		this.btnOn_Background.Controller.OnPress += this.btnOn_OnPress;
		XUiController xuiController = this.btnOn.GetChildById("lblOnOff");
		if (xuiController != null)
		{
			this.lblOnOff = (XUiV_Label)xuiController.ViewComponent;
		}
		xuiController = this.btnOn.GetChildById("sprOnOff");
		if (xuiController != null)
		{
			this.sprOnOff = (XUiV_Sprite)xuiController.ViewComponent;
		}
		this.IsDirty = true;
		this.turnOff = Localization.Get("xuiUnlockAmmo", false, null);
		this.turnOn = Localization.Get("xuiLockAmmo", false, null);
	}

	// Token: 0x06007F6A RID: 32618 RVA: 0x00307A1C File Offset: 0x00305C1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOn_OnPress(XUiController _sender, int _mouseButton)
	{
		bool flag = false;
		for (int i = 0; i < this.TileEntity.ItemSlots.Length; i++)
		{
			if (!this.TileEntity.ItemSlots[i].IsEmpty())
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
			GameManager.ShowTooltip(this.xui.playerUI.localPlayer.entityPlayerLocal, Localization.Get("ttRequiresAmmo", false, null), false, false, 0f);
			return;
		}
		this.tileEntity.IsLocked = !this.tileEntity.IsLocked;
		this.tileEntity.SetModified();
		this.RefreshIsLocked(this.tileEntity.IsLocked);
	}

	// Token: 0x06007F6B RID: 32619 RVA: 0x00307AD4 File Offset: 0x00305CD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setRequirements()
	{
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		for (int i = 0; i < itemControllers.Length; i++)
		{
			XUiC_RequiredItemStack xuiC_RequiredItemStack = itemControllers[i] as XUiC_RequiredItemStack;
			if (xuiC_RequiredItemStack != null)
			{
				xuiC_RequiredItemStack.RequiredType = XUiC_RequiredItemStack.RequiredTypes.ItemClass;
				xuiC_RequiredItemStack.SetAllowedItemClasses(this.tileEntity.AmmoItems);
			}
		}
	}

	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x06007F6C RID: 32620 RVA: 0x00060491 File Offset: 0x0005E691
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x06007F6D RID: 32621 RVA: 0x00307B1A File Offset: 0x00305D1A
	// (set) Token: 0x06007F6E RID: 32622 RVA: 0x00307B22 File Offset: 0x00305D22
	public XUiC_PowerRangedTrapWindowGroup Owner { get; set; }

	// Token: 0x06007F6F RID: 32623 RVA: 0x00307B2B File Offset: 0x00305D2B
	public virtual void SetSlots(ItemStack[] _stacks)
	{
		this.items = _stacks;
		base.SetStacks(_stacks);
	}

	// Token: 0x06007F70 RID: 32624 RVA: 0x00307B3C File Offset: 0x00305D3C
	public override void OnOpen()
	{
		base.OnOpen();
		this.tileEntity.SetUserAccessing(true);
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
		this.setRequirements();
		this.RefreshIsLocked(this.tileEntity.IsLocked);
		this.xui.CurrentPowerAmmoSlots = this;
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightBumper, "igcoPoweredTrapLockAmmo", XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
	}

	// Token: 0x06007F71 RID: 32625 RVA: 0x00307BE0 File Offset: 0x00305DE0
	public override void OnClose()
	{
		base.OnClose();
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		if (!XUiC_CameraWindow.hackyIsOpeningMaximizedWindow)
		{
			this.tileEntity.SetUserAccessing(false);
			LockManager.Instance.UnlockRequestLocal();
			this.tileEntity.SetModified();
		}
		this.IsDirty = true;
		this.tileEntity = null;
		this.xui.CurrentPowerAmmoSlots = null;
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
	}

	// Token: 0x06007F72 RID: 32626 RVA: 0x00307C74 File Offset: 0x00305E74
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshIsLocked(bool _isOn)
	{
		if (_isOn)
		{
			this.lblOnOff.Text = this.turnOff;
			if (this.sprOnOff != null)
			{
				this.sprOnOff.Color = this.onColor;
				this.sprOnOff.SpriteName = "ui_game_symbol_lock";
			}
		}
		else
		{
			this.lblOnOff.Text = this.turnOn;
			if (this.sprOnOff != null)
			{
				this.sprOnOff.Color = this.offColor;
				this.sprOnOff.SpriteName = "ui_game_symbol_unlock";
			}
		}
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		for (int i = 0; i < itemControllers.Length; i++)
		{
			XUiC_RequiredItemStack xuiC_RequiredItemStack = itemControllers[i] as XUiC_RequiredItemStack;
			if (xuiC_RequiredItemStack != null)
			{
				xuiC_RequiredItemStack.ToolLock = _isOn;
			}
		}
	}

	// Token: 0x06007F73 RID: 32627 RVA: 0x00307D2C File Offset: 0x00305F2C
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetLocked(bool _isOn)
	{
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		for (int i = 0; i < itemControllers.Length; i++)
		{
			XUiC_RequiredItemStack xuiC_RequiredItemStack = itemControllers[i] as XUiC_RequiredItemStack;
			if (xuiC_RequiredItemStack != null)
			{
				xuiC_RequiredItemStack.ToolLock = _isOn;
			}
		}
	}

	// Token: 0x06007F74 RID: 32628 RVA: 0x00307D61 File Offset: 0x00305F61
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
		base.UpdateBackend(_stackList);
		this.tileEntity.ItemSlots = _stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007F75 RID: 32629 RVA: 0x00307D94 File Offset: 0x00305F94
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.tileEntity == null)
		{
			return;
		}
		base.Update(_dt);
		if (this.lastLocked != this.tileEntity.IsLocked)
		{
			this.lastLocked = this.tileEntity.IsLocked;
			this.RefreshIsLocked(this.tileEntity.IsLocked);
		}
		if (this.tileEntity.IsLocked)
		{
			this.SetSlots(this.tileEntity.ItemSlots);
		}
		if (this.xui.playerUI.playerInput.GUIActions.WindowPagingRight.WasPressed && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.btnOn_OnPress(null, 0);
		}
		base.RefreshBindings();
	}

	// Token: 0x06007F76 RID: 32630 RVA: 0x00307E60 File Offset: 0x00306060
	public bool TryAddItemToSlot(ItemClass _itemClass, ItemStack _itemStack)
	{
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		for (int i = 0; i < itemControllers.Length; i++)
		{
			((XUiC_RequiredItemStack)itemControllers[i]).TryStack(_itemStack, -1);
			if (_itemStack.count == 0)
			{
				return true;
			}
		}
		return _itemStack.count == 0;
	}

	// Token: 0x04005E9D RID: 24221
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005E9E RID: 24222
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005E9F RID: 24223
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005EA0 RID: 24224
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005EA1 RID: 24225
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005EA2 RID: 24226
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 offColor = Color.white;

	// Token: 0x04005EA3 RID: 24227
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastLocked;

	// Token: 0x04005EA4 RID: 24228
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x04005EA5 RID: 24229
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04005EA6 RID: 24230
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;
}
