using System;
using System.Collections;
using System.Globalization;
using Audio;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F7D RID: 3965
[Preserve]
public class XUiC_LootWindow : XUiController
{
	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x06007D21 RID: 32033 RVA: 0x002F4A75 File Offset: 0x002F2C75
	public int ContainerSlotsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.containerSlotsSize.x * this.containerSlotsSize.y;
		}
	}

	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x06007D22 RID: 32034 RVA: 0x002F4A8E File Offset: 0x002F2C8E
	// (set) Token: 0x06007D23 RID: 32035 RVA: 0x002F4A98 File Offset: 0x002F2C98
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

	// Token: 0x06007D24 RID: 32036 RVA: 0x002F4B20 File Offset: 0x002F2D20
	public override void Init()
	{
		base.Init();
		this.lootContainer = base.GetChildByType<XUiC_LootContainer>();
		this.standardControls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.standardControls != null)
		{
			this.standardControls.GetLockedSlotsFromStorage = delegate
			{
				if (!this.te.HasSlotLocksSupport)
				{
					return null;
				}
				return this.te.SlotLocks;
			};
			this.standardControls.SetLockedSlotsToStorage = delegate(PackedBoolArray _slots)
			{
				if (this.te.HasSlotLocksSupport)
				{
					this.te.SlotLocks = _slots;
				}
			};
			this.standardControls.ApplyLockedSlotStates = new Action<PackedBoolArray>(this.ApplyLockedSlotStates);
			this.standardControls.UpdateLockedSlotStates = new Action<XUiC_ContainerStandardControls>(this.UpdateLockedSlots);
			this.standardControls.LockModeToggled = delegate
			{
				this.UserLockMode = !this.UserLockMode;
			};
			this.standardControls.SortPressed = delegate(PackedBoolArray _ignoredSlots)
			{
				ItemStack[] array = StackSortUtil.CombineAndSortStacks(this.te.items, 0, _ignoredSlots);
				for (int i = 0; i < array.Length; i++)
				{
					this.te.UpdateSlot(i, array[i]);
				}
				this.te.SetModified();
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

	// Token: 0x06007D25 RID: 32037 RVA: 0x002F4C14 File Offset: 0x002F2E14
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

	// Token: 0x06007D26 RID: 32038 RVA: 0x002F4E38 File Offset: 0x002F3038
	public void SetTileEntityChest(string _lootContainerName, ITileEntityLootable _te, bool _playOpenSound = true)
	{
		this.te = _te;
		this.lootContainerName = _lootContainerName;
		this.playOpenSound = _playOpenSound;
		if (this.te == null)
		{
			return;
		}
		this.containerSlotsSize = this.te.GetContainerSize();
		Vector2i vector2i = new Vector2i(this.containerSlotsSize.x * this.lootContainer.GridCellSize.x, this.containerSlotsSize.y * this.lootContainer.GridCellSize.y);
		this.windowWidth = vector2i.x + this.windowGridWidthDifference;
		this.lootContainer.SetSlots(this.te, this.te.items);
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

	// Token: 0x06007D27 RID: 32039 RVA: 0x002F4F08 File Offset: 0x002F3108
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

	// Token: 0x06007D28 RID: 32040 RVA: 0x002F4FC8 File Offset: 0x002F31C8
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

	// Token: 0x06007D29 RID: 32041 RVA: 0x002F508C File Offset: 0x002F328C
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _soundName)
	{
		if (_soundName == null)
		{
			return;
		}
		Manager.BroadcastPlayByLocalPlayer(this.te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, _soundName);
	}

	// Token: 0x06007D2A RID: 32042 RVA: 0x002F50CA File Offset: 0x002F32CA
	public PreferenceTracker GetPreferenceTrackerFromTileEntity()
	{
		ITileEntityLootable tileEntityLootable = this.te;
		if (tileEntityLootable == null)
		{
			return null;
		}
		return tileEntityLootable.preferences;
	}

	// Token: 0x06007D2B RID: 32043 RVA: 0x002F50DD File Offset: 0x002F32DD
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.isClosing = false;
		yield break;
	}

	// Token: 0x06007D2C RID: 32044 RVA: 0x002F50EC File Offset: 0x002F32EC
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007D2D RID: 32045 RVA: 0x002F5114 File Offset: 0x002F3314
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
						ITileEntityLootable tileEntityLootable = this.te;
						_value = (tileEntityLootable != null && tileEntityLootable.HasSlotLocksSupport).ToString();
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

	// Token: 0x06007D2E RID: 32046 RVA: 0x002F531A File Offset: 0x002F351A
	[XuiXmlBinding("userlockmode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingUserLockMode()
	{
		return this.UserLockMode;
	}

	// Token: 0x06007D2F RID: 32047 RVA: 0x002F5322 File Offset: 0x002F3522
	[XuiXmlBinding("container_slots")]
	[PublicizedFrom(EAccessModifier.Private)]
	public int bindingContainerSlots()
	{
		return this.ContainerSlotsCount;
	}

	// Token: 0x06007D30 RID: 32048 RVA: 0x002F532C File Offset: 0x002F352C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.lootContainer.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007D31 RID: 32049 RVA: 0x002F5374 File Offset: 0x002F3574
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

	// Token: 0x04005CAF RID: 23727
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityLootable te;

	// Token: 0x04005CB0 RID: 23728
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LootContainer lootContainer;

	// Token: 0x04005CB1 RID: 23729
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x04005CB2 RID: 23730
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x04005CB3 RID: 23731
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootContainerName;

	// Token: 0x04005CB4 RID: 23732
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSlotsSize;

	// Token: 0x04005CB5 RID: 23733
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x04005CB6 RID: 23734
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x04005CB7 RID: 23735
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOpenSound;

	// Token: 0x04005CB8 RID: 23736
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x04005CB9 RID: 23737
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005CBA RID: 23738
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
