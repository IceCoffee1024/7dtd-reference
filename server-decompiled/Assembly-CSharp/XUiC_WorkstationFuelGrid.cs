using System;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010CE RID: 4302
[Preserve]
public class XUiC_WorkstationFuelGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000E0 RID: 224
	// (add) Token: 0x060088F5 RID: 35061 RVA: 0x00343CD8 File Offset: 0x00341ED8
	// (remove) Token: 0x060088F6 RID: 35062 RVA: 0x00343D10 File Offset: 0x00341F10
	public event XuiEvent_WorkstationItemsChanged OnWorkstationFuelChanged;

	// Token: 0x060088F7 RID: 35063 RVA: 0x00343D48 File Offset: 0x00341F48
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

	// Token: 0x060088F8 RID: 35064 RVA: 0x00343DFA File Offset: 0x00341FFA
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		((XUiV_Button)this.button.ViewComponent).Enabled = this.workstationData.GetBurnTimeLeft() > 0f || this.hasAnyFuel();
		this.onFuelItemsChanged();
	}

	// Token: 0x060088F9 RID: 35065 RVA: 0x00343E3A File Offset: 0x0034203A
	[PublicizedFrom(EAccessModifier.Private)]
	public void onFuelItemsChanged()
	{
		if (this.OnWorkstationFuelChanged != null)
		{
			this.OnWorkstationFuelChanged();
		}
	}

	// Token: 0x060088FA RID: 35066 RVA: 0x00343E50 File Offset: 0x00342050
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

	// Token: 0x060088FB RID: 35067 RVA: 0x00343FAC File Offset: 0x003421AC
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

	// Token: 0x060088FC RID: 35068 RVA: 0x0034406C File Offset: 0x0034226C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CurrentWorkstationFuelGrid = null;
	}

	// Token: 0x060088FD RID: 35069 RVA: 0x00344094 File Offset: 0x00342294
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

	// Token: 0x060088FE RID: 35070 RVA: 0x00344118 File Offset: 0x00342318
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

	// Token: 0x060088FF RID: 35071 RVA: 0x003441B0 File Offset: 0x003423B0
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

	// Token: 0x06008900 RID: 35072 RVA: 0x00344254 File Offset: 0x00342454
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

	// Token: 0x06008901 RID: 35073 RVA: 0x003442E4 File Offset: 0x003424E4
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

	// Token: 0x06008902 RID: 35074 RVA: 0x003443A4 File Offset: 0x003425A4
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

	// Token: 0x06008903 RID: 35075 RVA: 0x00344415 File Offset: 0x00342615
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

	// Token: 0x06008904 RID: 35076 RVA: 0x00344453 File Offset: 0x00342653
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetFuelStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008905 RID: 35077 RVA: 0x00344478 File Offset: 0x00342678
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

	// Token: 0x06008906 RID: 35078 RVA: 0x003444BC File Offset: 0x003426BC
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

	// Token: 0x040065F6 RID: 26102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x040065F7 RID: 26103
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController button;

	// Token: 0x040065F8 RID: 26104
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController onOffLabel;

	// Token: 0x040065F9 RID: 26105
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController flameIcon;

	// Token: 0x040065FA RID: 26106
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOnColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x040065FB RID: 26107
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 flameOffColor = Color.white;

	// Token: 0x040065FC RID: 26108
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOff;

	// Token: 0x040065FD RID: 26109
	[PublicizedFrom(EAccessModifier.Private)]
	public string turnOn;

	// Token: 0x040065FF RID: 26111
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalizedDt;
}
