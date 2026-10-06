using System;
using System.Collections;
using System.Globalization;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F8C RID: 3980
[Preserve]
public class XUiC_LootWindow : XUiController
{
	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x06007D30 RID: 32048 RVA: 0x002F7B6D File Offset: 0x002F5D6D
	public int ContainerSlotsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.containerSlotsSize.x * this.containerSlotsSize.y;
		}
	}

	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x06007D31 RID: 32049 RVA: 0x002F7B86 File Offset: 0x002F5D86
	// (set) Token: 0x06007D32 RID: 32050 RVA: 0x002F7B90 File Offset: 0x002F5D90
	public bool UserLockMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.userLockMode;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == this.userLockMode)
			{
				return;
			}
			if (this.userLockMode)
			{
				this.UpdateLockedSlots(this.standardControls);
			}
			XUiC_ContainerStandardControls xuiC_ContainerStandardControls = this.standardControls;
			if (xuiC_ContainerStandardControls != null)
			{
				xuiC_ContainerStandardControls.LockModeChanged(value);
			}
			this.userLockMode = value;
			base.WindowGroup.isEscClosable = !this.userLockMode;
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			base.RefreshBindings();
			microStopwatch.Stop();
			Log.Warning(string.Format("ZZZ LootWindow.RefreshBindings took {0} us", microStopwatch.ElapsedMicroseconds));
		}
	}

	// Token: 0x06007D33 RID: 32051 RVA: 0x002F7C18 File Offset: 0x002F5E18
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
			this.standardControls.UpdateLockedSlotStates = new Action<XUiC_ContainerStandardControls>(this.UpdateLockedSlots);
			this.standardControls.LockModeToggled = delegate
			{
				this.UserLockMode = !this.UserLockMode;
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

	// Token: 0x06007D34 RID: 32052 RVA: 0x002F7D0C File Offset: 0x002F5F0C
	public override void Update(float _dt)
	{
		base.Update(_dt);
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
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (this.UserLockMode && (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.UserLockMode = false;
		}
	}

	// Token: 0x06007D35 RID: 32053 RVA: 0x002F7F30 File Offset: 0x002F6130
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

	// Token: 0x06007D36 RID: 32054 RVA: 0x002F7FFC File Offset: 0x002F61FC
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

	// Token: 0x06007D37 RID: 32055 RVA: 0x002F80BC File Offset: 0x002F62BC
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.UserLockMode = false;
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

	// Token: 0x06007D38 RID: 32056 RVA: 0x002F8180 File Offset: 0x002F6380
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _soundName)
	{
		if (_soundName == null)
		{
			return;
		}
		Manager.BroadcastPlayByLocalPlayer(this.te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, _soundName);
	}

	// Token: 0x06007D39 RID: 32057 RVA: 0x002F81BE File Offset: 0x002F63BE
	public PreferenceTracker GetPreferenceTrackerFromTileEntity()
	{
		TEFeatureStorage tefeatureStorage = this.te;
		if (tefeatureStorage == null)
		{
			return null;
		}
		return tefeatureStorage.ItemGrid.Preferences;
	}

	// Token: 0x06007D3A RID: 32058 RVA: 0x002F81D6 File Offset: 0x002F63D6
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.isClosing = false;
		yield break;
	}

	// Token: 0x06007D3B RID: 32059 RVA: 0x002F81E5 File Offset: 0x002F63E5
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007D3C RID: 32060 RVA: 0x002F8210 File Offset: 0x002F6410
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 775821220U)
		{
			if (num != 58930246U)
			{
				if (num != 610671175U)
				{
					if (num == 775821220U)
					{
						if (_bindingName == "lootcontainer_name")
						{
							_value = ((!string.IsNullOrEmpty(this.lootContainerName)) ? this.lootContainerName : Localization.Get("xuiLoot", false, null));
							return true;
						}
					}
				}
				else if (_bindingName == "windowWidth")
				{
					_value = this.windowWidth.ToString();
					return true;
				}
			}
			else if (_bindingName == "container_slots")
			{
				_value = this.ContainerSlotsCount.ToString();
				return true;
			}
		}
		else if (num <= 1899514017U)
		{
			if (num != 1249315359U)
			{
				if (num == 1899514017U)
				{
					if (_bindingName == "hasslotlocks")
					{
						TEFeatureStorage tefeatureStorage = this.te;
						_value = (((tefeatureStorage != null) ? tefeatureStorage.ItemGrid.SlotLocks : null) != null).ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "buttons_visible")
			{
				_value = (this.windowWidth >= 450).ToString();
				return true;
			}
		}
		else if (num != 2305669482U)
		{
			if (num == 4041168338U)
			{
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
			}
		}
		else if (_bindingName == "userlockmode")
		{
			_value = this.UserLockMode.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x06007D3D RID: 32061 RVA: 0x002F841E File Offset: 0x002F661E
	[XuiXmlBinding("gridDimensions")]
	public Vector2i GridDimensions
	{
		get
		{
			return this.containerSlotsSize;
		}
	}

	// Token: 0x06007D3E RID: 32062 RVA: 0x002F8426 File Offset: 0x002F6626
	[XuiXmlBinding("userlockmode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingUserLockMode()
	{
		return this.UserLockMode;
	}

	// Token: 0x06007D3F RID: 32063 RVA: 0x002F842E File Offset: 0x002F662E
	[XuiXmlBinding("container_slots")]
	[PublicizedFrom(EAccessModifier.Private)]
	public int bindingContainerSlots()
	{
		return this.ContainerSlotsCount;
	}

	// Token: 0x06007D40 RID: 32064 RVA: 0x002F8438 File Offset: 0x002F6638
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.lootContainer.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007D41 RID: 32065 RVA: 0x002F8480 File Offset: 0x002F6680
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

	// Token: 0x04005CD8 RID: 23768
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureStorage te;

	// Token: 0x04005CD9 RID: 23769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LootContainer lootContainer;

	// Token: 0x04005CDA RID: 23770
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x04005CDB RID: 23771
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x04005CDC RID: 23772
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootContainerName;

	// Token: 0x04005CDD RID: 23773
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSlotsSize;

	// Token: 0x04005CDE RID: 23774
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x04005CDF RID: 23775
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x04005CE0 RID: 23776
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOpenSound;

	// Token: 0x04005CE1 RID: 23777
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x04005CE2 RID: 23778
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005CE3 RID: 23779
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
