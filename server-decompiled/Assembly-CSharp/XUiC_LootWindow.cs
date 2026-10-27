using System;
using System.Collections;
using System.Globalization;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F9C RID: 3996
[Preserve]
public class XUiC_LootWindow : XUiController
{
	// Token: 0x17000EDE RID: 3806
	// (get) Token: 0x06007D80 RID: 32128 RVA: 0x002F97E7 File Offset: 0x002F79E7
	public int ContainerSlotsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.containerSlotsSize.x * this.containerSlotsSize.y;
		}
	}

	// Token: 0x06007D81 RID: 32129 RVA: 0x002F9800 File Offset: 0x002F7A00
	public override void Init()
	{
		base.Init();
		this.lootContainer = base.GetChildByType<XUiC_LootContainer>();
		this.standardControls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.standardControls != null)
		{
			this.standardControls.GetLockedSlotsFromStorage = () => this.te.ItemGrid.SlotLocks;
			this.standardControls.SetLockedSlotsToStorage = delegate(PackedBoolArray _slots)
			{
				if (this.te.ItemGrid.SlotLocks != null)
				{
					this.te.ItemGrid.SlotLocks.CopyFrom(_slots);
				}
			};
			this.standardControls.ApplyLockedSlotStates = new Action<PackedBoolArray>(this.ApplyLockedSlotStates);
			this.standardControls.LockModeToggled = delegate(bool _)
			{
				this.IsDirty = true;
			};
			this.standardControls.SearchFiltersChanged = delegate(XUiC_ContainerStandardControls.MatchesFiltersDelegate filter)
			{
				this.lootContainer.MatchesFiltersFunc = filter;
			};
			this.standardControls.SortPressed = delegate(PackedBoolArray _)
			{
				this.te.ItemGrid.Sort();
				this.te.Parent.NotifyListeners();
			};
			this.standardControls.MoveAllowed = delegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _grid, out IInventory _inventory)
			{
				_parentWindow = this;
				_grid = this.lootContainer;
				_inventory = this.xui.PlayerInventory;
				return true;
			};
			this.standardControls.MoveAllDone = delegate(bool _allMoved, bool _anyMoved)
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
		base.registerForInputStyleChanges();
	}

	// Token: 0x06007D82 RID: 32130 RVA: 0x002F98F4 File Offset: 0x002F7AF4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
		{
			this.wasReleased = true;
		}
		if (this.wasReleased && !this.xui.playerUI.windowManager.IsInputActive())
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
		if (this.te != null)
		{
			Vector3 vector = this.te.ToWorldCenterPos();
			if (vector != Vector3.zero)
			{
				float num = Constants.cCollectItemDistance + 30f;
				float sqrMagnitude = (this.xui.playerUI.entityPlayer.position - vector).sqrMagnitude;
				if (sqrMagnitude > num * num)
				{
					Log.Out("Loot Window closed at distance {0}", new object[] { Mathf.Sqrt(sqrMagnitude) });
					this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
				}
			}
		}
		if (!this.isClosing && base.ViewComponent != null && base.ViewComponent.IsVisible && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.xui.playerUI.windowManager.IsInputActive()) && (this.xui.playerUI.playerInput.GUIActions.LeftStick.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Reload.WasPressed))
		{
			this.standardControls.MoveAll();
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007D83 RID: 32131 RVA: 0x002F9AF4 File Offset: 0x002F7CF4
	public void SetTileEntityChest(string _lootContainerName, TEFeatureStorage _te, bool _playOpenSound = true)
	{
		this.te = _te;
		this.lootContainerName = _lootContainerName;
		this.playOpenSound = _playOpenSound;
		if (this.te == null)
		{
			return;
		}
		this.containerSlotsSize = this.te.ItemGrid.ContainerSize;
		Vector2i vector2i = new Vector2i(this.containerSlotsSize.x * this.lootContainer.GridCellSize.x, this.containerSlotsSize.y * this.lootContainer.GridCellSize.y);
		this.windowWidth = vector2i.x + this.windowGridWidthDifference;
		this.lootContainer.SetSlots(this.te);
		ITileEntitySignable selfOrFeature = _te.GetSelfOrFeature<ITileEntitySignable>();
		if (selfOrFeature != null)
		{
			GeneratedTextManager.GetDisplayText(selfOrFeature.GetAuthoredText(), delegate(string _containerName)
			{
				if (!string.IsNullOrEmpty(_containerName))
				{
					this.lootContainerName = _containerName;
				}
			}, true, true, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
		}
	}

	// Token: 0x06007D84 RID: 32132 RVA: 0x002F9BC0 File Offset: 0x002F7DC0
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosing = false;
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStickButton, "igcoLootAll", XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoInspect", XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot, 0f);
		this.lootContainer.SelectCursorElement(true, false);
		if (this.playOpenSound)
		{
			LootContainer lootContainer = LootContainer.GetLootContainer(this.te.lootListName, true);
			this.playSound((lootContainer != null) ? lootContainer.soundOpen : null);
		}
	}

	// Token: 0x06007D85 RID: 32133 RVA: 0x002F9C80 File Offset: 0x002F7E80
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		entityPlayer.MinEventContext.TileEntity = this.te;
		entityPlayer.MinEventContext.BlockValue = this.te.blockValue;
		entityPlayer.FireEvent(MinEventTypes.onSelfCloseLootContainer, true);
		if (this.te == null)
		{
			return;
		}
		LootContainer lootContainer = LootContainer.GetLootContainer(this.te.lootListName, true);
		this.playSound((lootContainer != null) ? lootContainer.soundClose : null);
		this.SetTileEntityChest("", null, true);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuLoot);
		base.OnClose();
	}

	// Token: 0x06007D86 RID: 32134 RVA: 0x002F9D3C File Offset: 0x002F7F3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _soundName)
	{
		if (_soundName == null)
		{
			return;
		}
		Manager.BroadcastPlayByLocalPlayer(this.te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, _soundName);
	}

	// Token: 0x06007D87 RID: 32135 RVA: 0x002F9D7A File Offset: 0x002F7F7A
	public PreferenceTracker GetPreferenceTrackerFromTileEntity()
	{
		TEFeatureStorage tefeatureStorage = this.te;
		if (tefeatureStorage == null)
		{
			return null;
		}
		return tefeatureStorage.ItemGrid.Preferences;
	}

	// Token: 0x06007D88 RID: 32136 RVA: 0x002F9D92 File Offset: 0x002F7F92
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.isClosing = false;
		yield break;
	}

	// Token: 0x06007D89 RID: 32137 RVA: 0x002F9DA1 File Offset: 0x002F7FA1
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007D8A RID: 32138 RVA: 0x002F9DCC File Offset: 0x002F7FCC
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
			_value = ((!string.IsNullOrEmpty(this.lootContainerName)) ? this.lootContainerName : Localization.Get("xuiLoot", false, null));
			return true;
		}
		if (_bindingName == "take_all_tooltip")
		{
			if (base.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				_value = string.Format(Localization.Get("xuiLootTakeAllTooltip", false, null), "[action:permanent:Reload:emptystring:KeyboardWithAngleBrackets]");
			}
			else
			{
				_value = string.Format(Localization.Get("xuiLootTakeAllTooltip", false, null), this.xui.playerUI.playerInput.GUIActions.LeftStick.GetBindingString(true, PlatformManager.NativePlatform.Input.CurrentControllerInputStyle, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null));
			}
			return true;
		}
		if (_bindingName == "buttons_visible")
		{
			_value = (this.windowWidth >= 450).ToString();
			return true;
		}
		if (_bindingName == "container_slots")
		{
			_value = this.ContainerSlotsCount.ToString();
			return true;
		}
		if (!(_bindingName == "hasslotlocks"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		TEFeatureStorage tefeatureStorage = this.te;
		_value = (((tefeatureStorage != null) ? tefeatureStorage.ItemGrid.SlotLocks : null) != null).ToString();
		return true;
	}

	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x06007D8B RID: 32139 RVA: 0x002F9F30 File Offset: 0x002F8130
	[XuiXmlBinding("gridDimensions")]
	public Vector2i GridDimensions
	{
		get
		{
			return this.containerSlotsSize;
		}
	}

	// Token: 0x06007D8C RID: 32140 RVA: 0x002F9F38 File Offset: 0x002F8138
	[XuiXmlBinding("userlockmode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingUserLockMode()
	{
		XUiC_ContainerStandardControls xuiC_ContainerStandardControls = this.standardControls;
		return xuiC_ContainerStandardControls != null && xuiC_ContainerStandardControls.UserLockMode;
	}

	// Token: 0x06007D8D RID: 32141 RVA: 0x002F9F4B File Offset: 0x002F814B
	[XuiXmlBinding("container_slots")]
	[PublicizedFrom(EAccessModifier.Private)]
	public int bindingContainerSlots()
	{
		return this.ContainerSlotsCount;
	}

	// Token: 0x06007D8E RID: 32142 RVA: 0x002F9F54 File Offset: 0x002F8154
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.lootContainer.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007D8F RID: 32143 RVA: 0x002F9F9C File Offset: 0x002F819C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLockedSlots(XUiC_ContainerStandardControls _csc)
	{
		if (_csc == null)
		{
			return;
		}
		int containerSlotsCount = this.ContainerSlotsCount;
		PackedBoolArray packedBoolArray = _csc.LockedSlots ?? new PackedBoolArray(containerSlotsCount);
		if (packedBoolArray.Length != containerSlotsCount)
		{
			packedBoolArray.Length = containerSlotsCount;
		}
		XUiC_ItemStack[] itemStackControllers = this.lootContainer.GetItemStackControllers();
		int num = 0;
		while (num < itemStackControllers.Length && num < packedBoolArray.Length)
		{
			packedBoolArray[num] = itemStackControllers[num].UserLockedSlot;
			num++;
		}
		_csc.LockedSlots = packedBoolArray;
	}

	// Token: 0x04005D13 RID: 23827
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureStorage te;

	// Token: 0x04005D14 RID: 23828
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LootContainer lootContainer;

	// Token: 0x04005D15 RID: 23829
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x04005D16 RID: 23830
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x04005D17 RID: 23831
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootContainerName;

	// Token: 0x04005D18 RID: 23832
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSlotsSize;

	// Token: 0x04005D19 RID: 23833
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x04005D1A RID: 23834
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x04005D1B RID: 23835
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOpenSound;

	// Token: 0x04005D1C RID: 23836
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x04005D1D RID: 23837
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005D1E RID: 23838
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
