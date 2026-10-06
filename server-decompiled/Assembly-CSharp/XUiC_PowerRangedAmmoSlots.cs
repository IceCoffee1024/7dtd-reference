using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC5 RID: 4037
[Preserve]
public class XUiC_PowerRangedAmmoSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x06007F17 RID: 32535 RVA: 0x00305DD2 File Offset: 0x00303FD2
	// (set) Token: 0x06007F18 RID: 32536 RVA: 0x00305DDA File Offset: 0x00303FDA
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

	// Token: 0x06007F19 RID: 32537 RVA: 0x00305DF4 File Offset: 0x00303FF4
	public override void HandleSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		base.HandleSlotChangedEvent(_slotNumber, _stack);
		this.tileEntity.SetSendSlots();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007F1A RID: 32538 RVA: 0x00305E14 File Offset: 0x00304014
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

	// Token: 0x06007F1B RID: 32539 RVA: 0x00305F00 File Offset: 0x00304100
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

	// Token: 0x06007F1C RID: 32540 RVA: 0x00305FB8 File Offset: 0x003041B8
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

	// Token: 0x17000EFB RID: 3835
	// (get) Token: 0x06007F1D RID: 32541 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000EFC RID: 3836
	// (get) Token: 0x06007F1E RID: 32542 RVA: 0x00305FFE File Offset: 0x003041FE
	// (set) Token: 0x06007F1F RID: 32543 RVA: 0x00306006 File Offset: 0x00304206
	public XUiC_PowerRangedTrapWindowGroup Owner { get; set; }

	// Token: 0x06007F20 RID: 32544 RVA: 0x0030600F File Offset: 0x0030420F
	public virtual void SetSlots(ItemStack[] _stacks)
	{
		this.items = _stacks;
		base.SetStacks(_stacks);
	}

	// Token: 0x06007F21 RID: 32545 RVA: 0x00306020 File Offset: 0x00304220
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

	// Token: 0x06007F22 RID: 32546 RVA: 0x003060C4 File Offset: 0x003042C4
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

	// Token: 0x06007F23 RID: 32547 RVA: 0x00306158 File Offset: 0x00304358
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

	// Token: 0x06007F24 RID: 32548 RVA: 0x00306210 File Offset: 0x00304410
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

	// Token: 0x06007F25 RID: 32549 RVA: 0x00306245 File Offset: 0x00304445
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
		base.UpdateBackend(_stackList);
		this.tileEntity.ItemSlots = _stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007F26 RID: 32550 RVA: 0x00306278 File Offset: 0x00304478
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

	// Token: 0x06007F27 RID: 32551 RVA: 0x00306344 File Offset: 0x00304544
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

	// Token: 0x04005E62 RID: 24162
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005E63 RID: 24163
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005E64 RID: 24164
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005E65 RID: 24165
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005E66 RID: 24166
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005E67 RID: 24167
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 offColor = Color.white;

	// Token: 0x04005E68 RID: 24168
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastLocked;

	// Token: 0x04005E69 RID: 24169
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x04005E6A RID: 24170
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04005E6B RID: 24171
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;
}
