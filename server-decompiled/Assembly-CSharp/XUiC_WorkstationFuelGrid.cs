using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010EF RID: 4335
[Preserve]
public class XUiC_WorkstationFuelGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000DF RID: 223
	// (add) Token: 0x06008996 RID: 35222 RVA: 0x00349B0C File Offset: 0x00347D0C
	// (remove) Token: 0x06008997 RID: 35223 RVA: 0x00349B44 File Offset: 0x00347D44
	public event XuiEvent_WorkstationItemsChanged OnWorkstationFuelChanged;

	// Token: 0x06008998 RID: 35224 RVA: 0x00349B7C File Offset: 0x00347D7C
	public override void Init()
	{
		base.Init();
		this.flameIcon = this.windowGroup.Controller.GetChildById("flameIcon");
		this.button = this.windowGroup.Controller.GetChildById("button");
		this.button.OnPress += this.Button_OnPress;
		this.onOffLabel = this.windowGroup.Controller.GetChildById("onoff");
		this.items = new ItemStack[this.itemControllers.Length];
		this.turnOff = Localization.Get("xuiTurnOff", false, null);
		this.turnOn = Localization.Get("xuiTurnOn", false, null);
	}

	// Token: 0x06008999 RID: 35225 RVA: 0x00349C2E File Offset: 0x00347E2E
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		((XUiV_Button)this.button.ViewComponent).Enabled = this.workstationData.GetBurnTimeLeft() > 0f || this.hasAnyFuel();
		this.onFuelItemsChanged();
	}

	// Token: 0x0600899A RID: 35226 RVA: 0x00349C6E File Offset: 0x00347E6E
	[PublicizedFrom(EAccessModifier.Private)]
	public void onFuelItemsChanged()
	{
		if (this.OnWorkstationFuelChanged != null)
		{
			this.OnWorkstationFuelChanged();
		}
	}

	// Token: 0x0600899B RID: 35227 RVA: 0x00349C84 File Offset: 0x00347E84
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.workstationData == null)
		{
			return;
		}
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[0];
		if (xuiC_ItemStack == null)
		{
			return;
		}
		if (!this.hasFuelStack() && this.hasAnyFuel())
		{
			for (int i = 0; i < this.itemControllers.Length - 1; i++)
			{
				this.CycleStacks();
				if (this.hasFuelStack())
				{
					this.UpdateBackend(this.getUISlots());
					break;
				}
			}
		}
		if (this.isOn && (!this.HasRequirement(null) || this.workstationData.GetIsBesideWater()))
		{
			this.TurnOff();
			this.onFuelItemsChanged();
			return;
		}
		if (this.isOn && this.workstationData != null && xuiC_ItemStack.ItemStack != null)
		{
			if (!xuiC_ItemStack.ItemStack.IsEmpty())
			{
				if (xuiC_ItemStack.IsLocked)
				{
					xuiC_ItemStack.LockTime = this.workstationData.GetBurnTimeLeft();
				}
				else
				{
					xuiC_ItemStack.LockStack(XUiC_ItemStack.LockTypes.Burning, this.workstationData.GetBurnTimeLeft(), 0, null);
				}
			}
			else
			{
				xuiC_ItemStack.UnlockStack();
			}
		}
		if (xuiC_ItemStack != null && (this.workstationData == null || xuiC_ItemStack.ItemStack == null || xuiC_ItemStack.ItemStack.IsEmpty() || !this.isOn))
		{
			xuiC_ItemStack.UnlockStack();
		}
		if (this.xui.playerUI.playerInput.GUIActions.WindowPagingRight.WasPressed && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.Button_OnPress(null, 0);
		}
	}

	// Token: 0x0600899C RID: 35228 RVA: 0x00349DE0 File Offset: 0x00347FE0
	public override void OnOpen()
	{
		base.OnOpen();
		this.isOn = this.workstationData.GetIsBurning();
		((XUiV_Label)this.onOffLabel.ViewComponent).Text = (this.isOn ? this.turnOff : this.turnOn);
		if (this.flameIcon != null)
		{
			((XUiV_Sprite)this.flameIcon.ViewComponent).Color = (this.isOn ? this.flameOnColor : this.flameOffColor);
		}
		this.xui.CurrentWorkstationFuelGrid = this;
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightBumper, "igcoWorkstationTurnOnOff", XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
	}

	// Token: 0x0600899D RID: 35229 RVA: 0x00349EA0 File Offset: 0x003480A0
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CurrentWorkstationFuelGrid = null;
	}

	// Token: 0x0600899E RID: 35230 RVA: 0x00349EC8 File Offset: 0x003480C8
	public void TurnOn()
	{
		if (!this.isOn)
		{
			Manager.PlayInsidePlayerHead("forge_burn_fuel", -1, 0f, false, false);
		}
		this.isOn = true;
		this.workstationData.SetIsBurning(this.isOn);
		((XUiV_Label)this.onOffLabel.ViewComponent).Text = this.turnOff;
		if (this.flameIcon != null)
		{
			((XUiV_Sprite)this.flameIcon.ViewComponent).Color = this.flameOnColor;
		}
	}

	// Token: 0x0600899F RID: 35231 RVA: 0x00349F4C File Offset: 0x0034814C
	public void TurnOff()
	{
		if (this.isOn)
		{
			Manager.PlayInsidePlayerHead("forge_fire_die", -1, 0f, false, false);
		}
		this.isOn = false;
		this.workstationData.SetIsBurning(this.isOn);
		((XUiV_Label)this.onOffLabel.ViewComponent).Text = this.turnOn;
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[0];
		if (xuiC_ItemStack != null)
		{
			xuiC_ItemStack.UnlockStack();
		}
		if (this.flameIcon != null)
		{
			((XUiV_Sprite)this.flameIcon.ViewComponent).Color = this.flameOffColor;
		}
	}

	// Token: 0x060089A0 RID: 35232 RVA: 0x00349FE4 File Offset: 0x003481E4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAnyFuel()
	{
		int num = 0;
		if (!XUi.IsGameRunning())
		{
			return false;
		}
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			if (xuiC_ItemStack != null && !xuiC_ItemStack.ItemStack.IsEmpty())
			{
				ItemClass itemClass = ItemClass.list[xuiC_ItemStack.ItemStack.itemValue.type];
				if (itemClass != null)
				{
					if (!itemClass.IsBlock())
					{
						if (itemClass != null && itemClass.FuelValue != null)
						{
							num += itemClass.FuelValue.Value;
						}
					}
					else
					{
						Block block = Block.list[itemClass.Id];
						if (block != null)
						{
							num += block.FuelValue;
						}
					}
				}
			}
		}
		return num > 0;
	}

	// Token: 0x060089A1 RID: 35233 RVA: 0x0034A088 File Offset: 0x00348288
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasFuelStack()
	{
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[0];
		if (!XUi.IsGameRunning() || xuiC_ItemStack == null || xuiC_ItemStack.ItemStack.IsEmpty())
		{
			return false;
		}
		int num = 0;
		ItemClass itemClass = ItemClass.list[xuiC_ItemStack.ItemStack.itemValue.type];
		if (itemClass == null)
		{
			return false;
		}
		if (!itemClass.IsBlock())
		{
			if (itemClass != null && itemClass.FuelValue != null)
			{
				num = itemClass.FuelValue.Value;
			}
		}
		else
		{
			Block block = Block.list[itemClass.Id];
			if (block == null)
			{
				return false;
			}
			num = block.FuelValue;
		}
		return num > 0;
	}

	// Token: 0x060089A2 RID: 35234 RVA: 0x0034A118 File Offset: 0x00348318
	public override bool HasRequirement(Recipe recipe)
	{
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[0];
		if (!XUi.IsGameRunning() || xuiC_ItemStack == null || xuiC_ItemStack.ItemStack.IsEmpty())
		{
			return this.workstationData.GetBurnTimeLeft() > 0f;
		}
		int num = 0;
		ItemClass itemClass = ItemClass.list[xuiC_ItemStack.ItemStack.itemValue.type];
		if (itemClass == null)
		{
			return this.workstationData.GetBurnTimeLeft() > 0f;
		}
		if (!itemClass.IsBlock())
		{
			if (itemClass != null && itemClass.FuelValue != null)
			{
				num = itemClass.FuelValue.Value;
			}
		}
		else
		{
			Block block = Block.list[itemClass.Id];
			if (block == null)
			{
				return this.workstationData.GetBurnTimeLeft() > 0f;
			}
			num = block.FuelValue;
		}
		return num > 0;
	}

	// Token: 0x060089A3 RID: 35235 RVA: 0x0034A1D8 File Offset: 0x003483D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CycleStacks()
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			if (xuiC_ItemStack != null && xuiC_ItemStack.ItemStack.count <= 0 && i + 1 < this.itemControllers.Length)
			{
				XUiC_ItemStack xuiC_ItemStack2 = this.itemControllers[i + 1];
				if (xuiC_ItemStack2 != null)
				{
					xuiC_ItemStack.ItemStack = xuiC_ItemStack2.ItemStack.Clone();
					xuiC_ItemStack2.ItemStack = ItemStack.Empty;
				}
			}
		}
	}

	// Token: 0x060089A4 RID: 35236 RVA: 0x0034A249 File Offset: 0x00348449
	[PublicizedFrom(EAccessModifier.Private)]
	public void Button_OnPress(XUiController _sender, int _mouseButton)
	{
		if (!this.isOn && (this.hasAnyFuel() || this.workstationData.GetBurnTimeLeft() > 0f) && !this.workstationData.GetIsBesideWater())
		{
			this.TurnOn();
			return;
		}
		this.TurnOff();
	}

	// Token: 0x060089A5 RID: 35237 RVA: 0x0034A287 File Offset: 0x00348487
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetFuelStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x060089A6 RID: 35238 RVA: 0x0034A2AC File Offset: 0x003484AC
	public override bool AddItem(ItemClass _itemClass, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation, -1))
		{
			return false;
		}
		int num = (this.isOn ? 1 : 0);
		base.TryStackItem(num, _itemStack);
		return _itemStack.count > 0 && base.AddItem(_itemStack);
	}

	// Token: 0x060089A7 RID: 35239 RVA: 0x0034A2F0 File Offset: 0x003484F0
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "flameoncolor"))
			{
				if (!(name == "flameoffcolor"))
				{
					return false;
				}
				this.flameOffColor = StringParsers.ParseColor32(value);
			}
			else
			{
				this.flameOnColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x04006677 RID: 26231
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x04006678 RID: 26232
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController button;

	// Token: 0x04006679 RID: 26233
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController onOffLabel;

	// Token: 0x0400667A RID: 26234
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController flameIcon;

	// Token: 0x0400667B RID: 26235
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOnColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x0400667C RID: 26236
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOffColor = Color.white;

	// Token: 0x0400667D RID: 26237
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x0400667E RID: 26238
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04006680 RID: 26240
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalizedDt;
}
