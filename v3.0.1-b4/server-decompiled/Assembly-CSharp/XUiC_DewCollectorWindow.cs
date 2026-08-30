using System;
using System.Collections;
using System.Globalization;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F08 RID: 3848
[Preserve]
public class XUiC_DewCollectorWindow : XUiController
{
	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x060078BF RID: 30911 RVA: 0x002D893F File Offset: 0x002D6B3F
	public int ContainerSlotsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.containerSlotsSize.x * this.containerSlotsSize.y;
		}
	}

	// Token: 0x060078C0 RID: 30912 RVA: 0x002D8958 File Offset: 0x002D6B58
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

	// Token: 0x060078C1 RID: 30913 RVA: 0x002D89EC File Offset: 0x002D6BEC
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

	// Token: 0x060078C2 RID: 30914 RVA: 0x002D8B64 File Offset: 0x002D6D64
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

	// Token: 0x060078C3 RID: 30915 RVA: 0x002D8C58 File Offset: 0x002D6E58
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosing = false;
	}

	// Token: 0x060078C4 RID: 30916 RVA: 0x002D8C67 File Offset: 0x002D6E67
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.SetTileEntity(null);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		base.OnClose();
	}

	// Token: 0x060078C5 RID: 30917 RVA: 0x002D8CA8 File Offset: 0x002D6EA8
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

	// Token: 0x060078C6 RID: 30918 RVA: 0x002D8D56 File Offset: 0x002D6F56
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close("dewcollector");
		this.isClosing = false;
		yield break;
	}

	// Token: 0x060078C7 RID: 30919 RVA: 0x002D8D65 File Offset: 0x002D6F65
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060078C8 RID: 30920 RVA: 0x002D8D90 File Offset: 0x002D6F90
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

	// Token: 0x04005919 RID: 22809
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector te;

	// Token: 0x0400591A RID: 22810
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorContainer container;

	// Token: 0x0400591B RID: 22811
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls controls;

	// Token: 0x0400591C RID: 22812
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSlotsSize;

	// Token: 0x0400591D RID: 22813
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x0400591E RID: 22814
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x0400591F RID: 22815
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lootLabel;

	// Token: 0x04005920 RID: 22816
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x04005921 RID: 22817
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005922 RID: 22818
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
