using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F9D RID: 3997
[Preserve]
public class XUiC_PowerRangedAmmoSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x06007E16 RID: 32278 RVA: 0x002FE626 File Offset: 0x002FC826
	// (set) Token: 0x06007E17 RID: 32279 RVA: 0x002FE62E File Offset: 0x002FC82E
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

	// Token: 0x06007E18 RID: 32280 RVA: 0x002FE648 File Offset: 0x002FC848
	public override void HandleSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		base.HandleSlotChangedEvent(_slotNumber, _stack);
		this.tileEntity.SetSendSlots();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007E19 RID: 32281 RVA: 0x002FE668 File Offset: 0x002FC868
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

	// Token: 0x06007E1A RID: 32282 RVA: 0x002FE754 File Offset: 0x002FC954
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

	// Token: 0x06007E1B RID: 32283 RVA: 0x002FE80C File Offset: 0x002FCA0C
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

	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x06007E1C RID: 32284 RVA: 0x00081148 File Offset: 0x0007F348
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x06007E1D RID: 32285 RVA: 0x002FE852 File Offset: 0x002FCA52
	// (set) Token: 0x06007E1E RID: 32286 RVA: 0x002FE85A File Offset: 0x002FCA5A
	public XUiC_PowerRangedTrapWindowGroup Owner { get; set; }

	// Token: 0x06007E1F RID: 32287 RVA: 0x002FE863 File Offset: 0x002FCA63
	public virtual void SetSlots(ItemStack[] _stacks)
	{
		this.items = _stacks;
		base.SetStacks(_stacks);
	}

	// Token: 0x06007E20 RID: 32288 RVA: 0x002FE874 File Offset: 0x002FCA74
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

	// Token: 0x06007E21 RID: 32289 RVA: 0x002FE918 File Offset: 0x002FCB18
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

	// Token: 0x06007E22 RID: 32290 RVA: 0x002FE9AC File Offset: 0x002FCBAC
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

	// Token: 0x06007E23 RID: 32291 RVA: 0x002FEA64 File Offset: 0x002FCC64
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

	// Token: 0x06007E24 RID: 32292 RVA: 0x002FEA99 File Offset: 0x002FCC99
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
		base.UpdateBackend(_stackList);
		this.tileEntity.ItemSlots = _stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007E25 RID: 32293 RVA: 0x002FEACC File Offset: 0x002FCCCC
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

	// Token: 0x06007E26 RID: 32294 RVA: 0x002FEB98 File Offset: 0x002FCD98
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

	// Token: 0x04005D72 RID: 23922
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005D73 RID: 23923
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005D74 RID: 23924
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005D75 RID: 23925
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005D76 RID: 23926
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005D77 RID: 23927
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 offColor = Color.white;

	// Token: 0x04005D78 RID: 23928
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastLocked;

	// Token: 0x04005D79 RID: 23929
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x04005D7A RID: 23930
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04005D7B RID: 23931
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;
}
