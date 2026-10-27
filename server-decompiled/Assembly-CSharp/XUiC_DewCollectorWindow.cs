using System;
using System.Collections;
using System.Globalization;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F3D RID: 3901
[Preserve]
public class XUiC_DewCollectorWindow : XUiController
{
	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x060079B5 RID: 31157 RVA: 0x002E1557 File Offset: 0x002DF757
	public int ContainerSlotsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.containerSlotsSize.x * this.containerSlotsSize.y;
		}
	}

	// Token: 0x060079B6 RID: 31158 RVA: 0x002E1570 File Offset: 0x002DF770
	public override void Init()
	{
		base.Init();
		this.container = base.GetChildByType<XUiC_DewCollectorContainer>();
		this.controls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.controls != null)
		{
			this.controls.SortPressed = delegate(PackedBoolArray _ignoredSlots)
			{
				ItemStack[] array = StackSortUtil.CombineAndSortStacks(this.te.Items, 0, _ignoredSlots);
				for (int i = 0; i < array.Length; i++)
				{
					this.te.UpdateSlot(i, array[i]);
				}
				this.te.SetModified();
			};
			this.controls.MoveAllowed = delegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _grid, out IInventory _inventory)
			{
				_parentWindow = this;
				_grid = this.container;
				_inventory = this.xui.PlayerInventory;
				return true;
			};
			this.controls.MoveAllDone = delegate(bool _allMoved, bool _anyMoved)
			{
				if (_anyMoved)
				{
					Manager.BroadcastPlayByLocalPlayer(this.te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "UseActions/takeall1");
				}
				if (_allMoved)
				{
					ThreadManager.StartCoroutine(this.closeInventoryLater());
				}
			};
		}
		this.lootLabel = base.GetChildById("lootName").ViewComponent as XUiV_Label;
	}

	// Token: 0x060079B7 RID: 31159 RVA: 0x002E1604 File Offset: 0x002DF804
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.windowGroup.isShowing)
		{
			if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
			{
				this.wasReleased = true;
			}
			if (this.wasReleased)
			{
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
				{
					this.activeKeyDown = true;
				}
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.WasReleased && this.activeKeyDown)
				{
					this.activeKeyDown = false;
					this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
				}
			}
		}
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (!this.isClosing && base.ViewComponent != null && base.ViewComponent.IsVisible && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.xui.playerUI.windowManager.IsInputActive()) && (this.xui.playerUI.playerInput.GUIActions.LeftStick.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Reload.WasPressed))
		{
			this.controls.MoveAll();
		}
	}

	// Token: 0x060079B8 RID: 31160 RVA: 0x002E177C File Offset: 0x002DF97C
	public void SetTileEntity(TileEntityCollector _te)
	{
		this.te = _te;
		if (this.te != null)
		{
			this.container.SetTileEntity(_te);
			this.lootLabel.Text = Localization.Get((this.te.blockValue.Block as BlockCollector).LootLabelKey, false, null);
			this.containerSlotsSize = this.te.GetContainerSize();
			Vector2i vector2i = new Vector2i(this.containerSlotsSize.x * this.container.GridCellSize.x, this.containerSlotsSize.y * this.container.GridCellSize.y);
			this.windowWidth = vector2i.x + this.windowGridWidthDifference;
			this.te.HandleUpdate(GameManager.Instance.World);
			this.container.SetSlots(this.te, this.te.GetItems());
			base.RefreshBindings();
		}
	}

	// Token: 0x060079B9 RID: 31161 RVA: 0x002E1870 File Offset: 0x002DFA70
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosing = false;
	}

	// Token: 0x060079BA RID: 31162 RVA: 0x002E187F File Offset: 0x002DFA7F
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.SetTileEntity(null);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		base.OnClose();
	}

	// Token: 0x060079BB RID: 31163 RVA: 0x002E18C0 File Offset: 0x002DFAC0
	public void OpenContainer()
	{
		this.container.SetSlots(this.te, this.te.GetItems());
		base.OnOpen();
		this.te.SetUserAccessing(true);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStickButton, "igcoLootAll", XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoInspect", XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot, 0f);
		base.RefreshBindings();
	}

	// Token: 0x060079BC RID: 31164 RVA: 0x002E196E File Offset: 0x002DFB6E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close("dewcollector");
		this.isClosing = false;
		yield break;
	}

	// Token: 0x060079BD RID: 31165 RVA: 0x002E197D File Offset: 0x002DFB7D
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060079BE RID: 31166 RVA: 0x002E19A8 File Offset: 0x002DFBA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "windowWidth")
		{
			_value = this.windowWidth.ToString();
			return true;
		}
		if (_bindingName == "lootcontainer_name")
		{
			_value = Localization.Get("xuiDewCollector", false, null);
			return true;
		}
		if (_bindingName == "take_all_tooltip")
		{
			_value = string.Format(Localization.Get("xuiLootTakeAllTooltip", false, null), "[action:permanent:Reload:emptystring:KeyboardWithAngleBrackets]");
			return true;
		}
		if (_bindingName == "buttons_visible")
		{
			_value = (this.windowWidth >= 450).ToString();
			return true;
		}
		if (!(_bindingName == "container_slots"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.ContainerSlotsCount.ToString();
		return true;
	}

	// Token: 0x04005A36 RID: 23094
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector te;

	// Token: 0x04005A37 RID: 23095
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorContainer container;

	// Token: 0x04005A38 RID: 23096
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls controls;

	// Token: 0x04005A39 RID: 23097
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSlotsSize;

	// Token: 0x04005A3A RID: 23098
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x04005A3B RID: 23099
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x04005A3C RID: 23100
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lootLabel;

	// Token: 0x04005A3D RID: 23101
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x04005A3E RID: 23102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005A3F RID: 23103
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
