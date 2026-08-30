using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB6 RID: 4022
[Preserve]
public class XUiC_PowerRangedAmmoSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x06007F08 RID: 32520 RVA: 0x00302DFA File Offset: 0x00300FFA
	// (set) Token: 0x06007F09 RID: 32521 RVA: 0x00302E02 File Offset: 0x00301002
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

	// Token: 0x06007F0A RID: 32522 RVA: 0x00302E1C File Offset: 0x0030101C
	public override void HandleSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		base.HandleSlotChangedEvent(_slotNumber, _stack);
		this.tileEntity.SetSendSlots();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007F0B RID: 32523 RVA: 0x00302E3C File Offset: 0x0030103C
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

	// Token: 0x06007F0C RID: 32524 RVA: 0x00302F28 File Offset: 0x00301128
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

	// Token: 0x06007F0D RID: 32525 RVA: 0x00302FE0 File Offset: 0x003011E0
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

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x06007F0E RID: 32526 RVA: 0x000817FC File Offset: 0x0007F9FC
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x06007F0F RID: 32527 RVA: 0x00303026 File Offset: 0x00301226
	// (set) Token: 0x06007F10 RID: 32528 RVA: 0x0030302E File Offset: 0x0030122E
	public XUiC_PowerRangedTrapWindowGroup Owner { get; set; }

	// Token: 0x06007F11 RID: 32529 RVA: 0x00303037 File Offset: 0x00301237
	public virtual void SetSlots(ItemStack[] _stacks)
	{
		this.items = _stacks;
		base.SetStacks(_stacks);
	}

	// Token: 0x06007F12 RID: 32530 RVA: 0x00303048 File Offset: 0x00301248
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

	// Token: 0x06007F13 RID: 32531 RVA: 0x003030EC File Offset: 0x003012EC
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

	// Token: 0x06007F14 RID: 32532 RVA: 0x00303180 File Offset: 0x00301380
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

	// Token: 0x06007F15 RID: 32533 RVA: 0x00303238 File Offset: 0x00301438
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

	// Token: 0x06007F16 RID: 32534 RVA: 0x0030326D File Offset: 0x0030146D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
		base.UpdateBackend(_stackList);
		this.tileEntity.ItemSlots = _stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007F17 RID: 32535 RVA: 0x003032A0 File Offset: 0x003014A0
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

	// Token: 0x06007F18 RID: 32536 RVA: 0x0030336C File Offset: 0x0030156C
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

	// Token: 0x04005E39 RID: 24121
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005E3A RID: 24122
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005E3B RID: 24123
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005E3C RID: 24124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005E3D RID: 24125
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005E3E RID: 24126
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 offColor = Color.white;

	// Token: 0x04005E3F RID: 24127
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastLocked;

	// Token: 0x04005E40 RID: 24128
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x04005E41 RID: 24129
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04005E42 RID: 24130
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;
}
