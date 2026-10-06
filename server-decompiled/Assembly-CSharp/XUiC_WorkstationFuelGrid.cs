using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010DF RID: 4319
[Preserve]
public class XUiC_WorkstationFuelGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000DF RID: 223
	// (add) Token: 0x06008943 RID: 35139 RVA: 0x00347A88 File Offset: 0x00345C88
	// (remove) Token: 0x06008944 RID: 35140 RVA: 0x00347AC0 File Offset: 0x00345CC0
	public event XuiEvent_WorkstationItemsChanged OnWorkstationFuelChanged;

	// Token: 0x06008945 RID: 35141 RVA: 0x00347AF8 File Offset: 0x00345CF8
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

	// Token: 0x06008946 RID: 35142 RVA: 0x00347BAA File Offset: 0x00345DAA
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		((XUiV_Button)this.button.ViewComponent).Enabled = this.workstationData.GetBurnTimeLeft() > 0f || this.hasAnyFuel();
		this.onFuelItemsChanged();
	}

	// Token: 0x06008947 RID: 35143 RVA: 0x00347BEA File Offset: 0x00345DEA
	[PublicizedFrom(EAccessModifier.Private)]
	public void onFuelItemsChanged()
	{
		if (this.OnWorkstationFuelChanged != null)
		{
			this.OnWorkstationFuelChanged();
		}
	}

	// Token: 0x06008948 RID: 35144 RVA: 0x00347C00 File Offset: 0x00345E00
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

	// Token: 0x06008949 RID: 35145 RVA: 0x00347D5C File Offset: 0x00345F5C
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

	// Token: 0x0600894A RID: 35146 RVA: 0x00347E1C File Offset: 0x0034601C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CurrentWorkstationFuelGrid = null;
	}

	// Token: 0x0600894B RID: 35147 RVA: 0x00347E44 File Offset: 0x00346044
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

	// Token: 0x0600894C RID: 35148 RVA: 0x00347EC8 File Offset: 0x003460C8
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

	// Token: 0x0600894D RID: 35149 RVA: 0x00347F60 File Offset: 0x00346160
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

	// Token: 0x0600894E RID: 35150 RVA: 0x00348004 File Offset: 0x00346204
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

	// Token: 0x0600894F RID: 35151 RVA: 0x00348094 File Offset: 0x00346294
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

	// Token: 0x06008950 RID: 35152 RVA: 0x00348154 File Offset: 0x00346354
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

	// Token: 0x06008951 RID: 35153 RVA: 0x003481C5 File Offset: 0x003463C5
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

	// Token: 0x06008952 RID: 35154 RVA: 0x00348203 File Offset: 0x00346403
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetFuelStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008953 RID: 35155 RVA: 0x00348228 File Offset: 0x00346428
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

	// Token: 0x06008954 RID: 35156 RVA: 0x0034826C File Offset: 0x0034646C
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

	// Token: 0x04006638 RID: 26168
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x04006639 RID: 26169
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController button;

	// Token: 0x0400663A RID: 26170
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController onOffLabel;

	// Token: 0x0400663B RID: 26171
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController flameIcon;

	// Token: 0x0400663C RID: 26172
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOnColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x0400663D RID: 26173
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOffColor = Color.white;

	// Token: 0x0400663E RID: 26174
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x0400663F RID: 26175
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x04006641 RID: 26177
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalizedDt;
}
