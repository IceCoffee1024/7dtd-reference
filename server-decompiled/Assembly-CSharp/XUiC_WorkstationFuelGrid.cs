using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010B5 RID: 4277
[Preserve]
public class XUiC_WorkstationFuelGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000DF RID: 223
	// (add) Token: 0x06008803 RID: 34819 RVA: 0x0033F4B0 File Offset: 0x0033D6B0
	// (remove) Token: 0x06008804 RID: 34820 RVA: 0x0033F4E8 File Offset: 0x0033D6E8
	public event XuiEvent_WorkstationItemsChanged OnWorkstationFuelChanged;

	// Token: 0x06008805 RID: 34821 RVA: 0x0033F520 File Offset: 0x0033D720
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

	// Token: 0x06008806 RID: 34822 RVA: 0x0033F5D2 File Offset: 0x0033D7D2
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		((XUiV_Button)this.button.ViewComponent).Enabled = this.workstationData.GetBurnTimeLeft() > 0f || this.hasAnyFuel();
		this.onFuelItemsChanged();
	}

	// Token: 0x06008807 RID: 34823 RVA: 0x0033F612 File Offset: 0x0033D812
	[PublicizedFrom(EAccessModifier.Private)]
	public void onFuelItemsChanged()
	{
		if (this.OnWorkstationFuelChanged != null)
		{
			this.OnWorkstationFuelChanged();
		}
	}

	// Token: 0x06008808 RID: 34824 RVA: 0x0033F628 File Offset: 0x0033D828
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

	// Token: 0x06008809 RID: 34825 RVA: 0x0033F784 File Offset: 0x0033D984
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

	// Token: 0x0600880A RID: 34826 RVA: 0x0033F844 File Offset: 0x0033DA44
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CurrentWorkstationFuelGrid = null;
	}

	// Token: 0x0600880B RID: 34827 RVA: 0x0033F86C File Offset: 0x0033DA6C
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

	// Token: 0x0600880C RID: 34828 RVA: 0x0033F8F0 File Offset: 0x0033DAF0
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

	// Token: 0x0600880D RID: 34829 RVA: 0x0033F988 File Offset: 0x0033DB88
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

	// Token: 0x0600880E RID: 34830 RVA: 0x0033FA2C File Offset: 0x0033DC2C
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

	// Token: 0x0600880F RID: 34831 RVA: 0x0033FABC File Offset: 0x0033DCBC
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

	// Token: 0x06008810 RID: 34832 RVA: 0x0033FB7C File Offset: 0x0033DD7C
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

	// Token: 0x06008811 RID: 34833 RVA: 0x0033FBED File Offset: 0x0033DDED
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

	// Token: 0x06008812 RID: 34834 RVA: 0x0033FC2B File Offset: 0x0033DE2B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetFuelStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008813 RID: 34835 RVA: 0x0033FC50 File Offset: 0x0033DE50
	public override bool AddItem(ItemClass _itemClass, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation))
		{
			return false;
		}
		int num = (this.isOn ? 1 : 0);
		base.TryStackItem(num, _itemStack);
		return _itemStack.count > 0 && base.AddItem(_itemStack);
	}

	// Token: 0x06008814 RID: 34836 RVA: 0x0033FC94 File Offset: 0x0033DE94
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

	// Token: 0x0400652F RID: 25903
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x04006530 RID: 25904
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController button;

	// Token: 0x04006531 RID: 25905
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController onOffLabel;

	// Token: 0x04006532 RID: 25906
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController flameIcon;

	// Token: 0x04006533 RID: 25907
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOnColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04006534 RID: 25908
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOffColor = Color.white;

	// Token: 0x04006535 RID: 25909
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x04006536 RID: 25910
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04006538 RID: 25912
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalizedDt;
}
