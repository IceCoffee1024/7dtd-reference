using System;
using System.Collections;
using System.Globalization;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F1F RID: 3871
[Preserve]
public class XUiC_DewCollectorWindow : XUiController
{
	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x06007977 RID: 31095 RVA: 0x002DC8CB File Offset: 0x002DAACB
	public int ContainerSlotsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.containerSlotsSize.x * this.containerSlotsSize.y;
		}
	}

	// Token: 0x06007978 RID: 31096 RVA: 0x002DC8E4 File Offset: 0x002DAAE4
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

	// Token: 0x06007979 RID: 31097 RVA: 0x002DC978 File Offset: 0x002DAB78
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

	// Token: 0x0600797A RID: 31098 RVA: 0x002DCAF0 File Offset: 0x002DACF0
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

	// Token: 0x0600797B RID: 31099 RVA: 0x002DCBE4 File Offset: 0x002DADE4
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosing = false;
	}

	// Token: 0x0600797C RID: 31100 RVA: 0x002DCBF3 File Offset: 0x002DADF3
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.SetTileEntity(null);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		base.OnClose();
	}

	// Token: 0x0600797D RID: 31101 RVA: 0x002DCC34 File Offset: 0x002DAE34
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

	// Token: 0x0600797E RID: 31102 RVA: 0x002DCCE2 File Offset: 0x002DAEE2
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close("dewcollector");
		this.isClosing = false;
		yield break;
	}

	// Token: 0x0600797F RID: 31103 RVA: 0x002DCCF1 File Offset: 0x002DAEF1
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007980 RID: 31104 RVA: 0x002DCD1C File Offset: 0x002DAF1C
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

	// Token: 0x040059CB RID: 22987
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector te;

	// Token: 0x040059CC RID: 22988
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorContainer container;

	// Token: 0x040059CD RID: 22989
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls controls;

	// Token: 0x040059CE RID: 22990
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSlotsSize;

	// Token: 0x040059CF RID: 22991
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x040059D0 RID: 22992
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x040059D1 RID: 22993
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lootLabel;

	// Token: 0x040059D2 RID: 22994
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x040059D3 RID: 22995
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040059D4 RID: 22996
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
