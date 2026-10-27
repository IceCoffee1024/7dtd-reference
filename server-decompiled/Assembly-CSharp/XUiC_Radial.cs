using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101F RID: 4127
[Preserve]
public class XUiC_Radial : XUiController
{
	// Token: 0x17000F47 RID: 3911
	// (get) Token: 0x060081AE RID: 33198 RVA: 0x003154C8 File Offset: 0x003136C8
	public XUiC_RadialEntry mCurrentlySelectedEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.selectedIndex >= 0 && this.selectedIndex < this.menuItem.Length)
			{
				return this.menuItem[this.selectedIndex];
			}
			return null;
		}
	}

	// Token: 0x060081AF RID: 33199 RVA: 0x003154F4 File Offset: 0x003136F4
	public override void Init()
	{
		base.Init();
		this.menuItem = base.GetChildrenByType<XUiC_RadialEntry>(null);
		this.menuItemState = new bool[this.menuItem.Length];
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			this.menuItem[i].OnHover += this.XUiC_Radial_OnHover;
			this.menuItem[i].ViewComponent.IsVisible = false;
			this.menuItem[i].MenuItemIndex = i;
			this.menuItemState[i] = false;
		}
		this.selectionText = base.GetChildById("selection").ViewComponent as XUiV_Label;
		this.xui.RadialWindow = this;
	}

	// Token: 0x060081B0 RID: 33200 RVA: 0x003155A4 File Offset: 0x003137A4
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.RadialWindow = null;
	}

	// Token: 0x060081B1 RID: 33201 RVA: 0x003155B8 File Offset: 0x003137B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_Radial_OnPress(XUiC_RadialEntry _sender)
	{
		this.selectedIndex = _sender.MenuItemIndex;
		this.CallContextAction();
	}

	// Token: 0x060081B2 RID: 33202 RVA: 0x003155CC File Offset: 0x003137CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_Radial_OnHover(XUiController _sender, bool _isOver)
	{
		XUiC_RadialEntry xuiC_RadialEntry = (XUiC_RadialEntry)_sender;
		if (_isOver)
		{
			this.SelectionEffect(xuiC_RadialEntry, true);
			this.selectedIndex = xuiC_RadialEntry.MenuItemIndex;
			return;
		}
		this.SelectionEffect(xuiC_RadialEntry, false);
		this.selectedIndex = int.MinValue;
	}

	// Token: 0x060081B3 RID: 33203 RVA: 0x0031560B File Offset: 0x0031380B
	public void Open()
	{
		this.openTime = Time.time;
		this.isOpenRequested = true;
	}

	// Token: 0x060081B4 RID: 33204 RVA: 0x00315620 File Offset: 0x00313820
	public override void OnOpen()
	{
		base.OnOpen();
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			this.menuItem[i].ViewComponent.IsVisible = this.menuItemState[i];
		}
		if (!this.showingCallouts)
		{
			this.showingCallouts = true;
			UIUtils.ButtonIcon buttonIcon = (this.radialButtonIsLeftSide(this.xui.playerUI.playerInput) ? UIUtils.ButtonIcon.RightStick : UIUtils.ButtonIcon.LeftStick);
			this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
			this.xui.CalloutWindow.AddCallout(buttonIcon, "igcoRadialHighlight", XUiC_GamepadCalloutWindow.CalloutType.Menu);
			this.xui.CalloutWindow.AddCallout(buttonIcon, "igcoRadialSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
			this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		}
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.xui.playerUI.entityPlayer.SetControllable(false);
			this.xui.playerUI.CursorController.SetCursorHidden(true);
		}
		this.xui.playerUI.entityPlayer.ClearMovementInputs();
	}

	// Token: 0x060081B5 RID: 33205 RVA: 0x00315738 File Offset: 0x00313938
	public override void OnClose()
	{
		base.OnClose();
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			this.SelectionEffect(this.menuItem[i], false);
		}
		this.context = null;
		this.commandHandler = null;
		this.validCheckDelegate = null;
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.showingCallouts = false;
		this.isOpenRequested = false;
		this.xui.playerUI.entityPlayer.SetControllable(true);
		this.xui.playerUI.CursorController.SetCursorHidden(false);
	}

	// Token: 0x060081B6 RID: 33206 RVA: 0x003157D0 File Offset: 0x003139D0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.isOpenRequested)
		{
			return;
		}
		bool flag = Time.time - this.openTime >= this.displayDelay;
		if (flag && !base.IsOpen)
		{
			if (this.validCheckDelegate != null && !this.validCheckDelegate(this, this.context))
			{
				this.Close();
				return;
			}
			this.xui.playerUI.windowManager.Open(this.windowGroup, true, true);
		}
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (!this.radialButtonPressed(playerInput))
		{
			if (this.mCurrentlySelectedEntry != null)
			{
				this.XUiC_Radial_OnPress(this.mCurrentlySelectedEntry);
			}
			else
			{
				if (!flag)
				{
					if (this.hasSpecialActionPriorToRadialVisibility)
					{
						if (InputUtils.ShiftKeyPressed)
						{
							this.selectedIndex = -2;
						}
						else
						{
							this.selectedIndex = -1;
						}
					}
					else if (this.selectedIndex == -2147483648)
					{
						for (int i = 0; i < this.menuItemState.Length; i++)
						{
							if (this.menuItemState[i])
							{
								this.selectedIndex = i;
								break;
							}
						}
					}
				}
				if (this.selectedIndex != -2147483648 && !GameManager.Instance.IsPaused())
				{
					this.CallContextAction();
				}
				else
				{
					this.Close();
				}
			}
		}
		if (base.IsOpen && this.radialButtonPressed(playerInput))
		{
			this.CalculateSelectionFromController(playerInput);
		}
	}

	// Token: 0x060081B7 RID: 33207 RVA: 0x0031591C File Offset: 0x00313B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool radialButtonPressed(PlayerActionsLocal _actionSet)
	{
		return _actionSet.Activate.IsPressed || _actionSet.PermanentActions.Activate.IsPressed || _actionSet.Reload.IsPressed || _actionSet.PermanentActions.Reload.IsPressed || _actionSet.ToggleFlashlight.IsPressed || _actionSet.PermanentActions.ToggleFlashlight.IsPressed || _actionSet.Inventory.IsPressed || _actionSet.VehicleActions.Inventory.IsPressed || _actionSet.PermanentActions.Inventory.IsPressed || _actionSet.Swap.IsPressed || _actionSet.PermanentActions.Swap.IsPressed || _actionSet.InventorySlotLeft.IsPressed || _actionSet.InventorySlotRight.IsPressed || _actionSet.QuickMenu.IsPressed || _actionSet.PermanentActions.QuickMenu.IsPressed;
	}

	// Token: 0x060081B8 RID: 33208 RVA: 0x00315A23 File Offset: 0x00313C23
	[PublicizedFrom(EAccessModifier.Private)]
	public bool radialButtonIsLeftSide(PlayerActionsLocal _actionSet)
	{
		return _actionSet.ToggleFlashlight.IsPressed || _actionSet.PermanentActions.ToggleFlashlight.IsPressed;
	}

	// Token: 0x060081B9 RID: 33209 RVA: 0x00315A44 File Offset: 0x00313C44
	[PublicizedFrom(EAccessModifier.Private)]
	public void CallContextAction()
	{
		if (this.selectedIndex == -2147483648)
		{
			this.Close();
			return;
		}
		if (GameManager.Instance == null || GameManager.Instance.World == null)
		{
			this.Close();
			return;
		}
		if (this.commandHandler == null)
		{
			this.Close();
			return;
		}
		if (this.validCheckDelegate != null && !this.validCheckDelegate(this, this.context))
		{
			this.Close();
			return;
		}
		int commandIndex;
		if (this.selectedIndex < 0)
		{
			commandIndex = this.selectedIndex;
		}
		else
		{
			if (!this.menuItemState[this.selectedIndex])
			{
				this.Close();
				return;
			}
			commandIndex = this.menuItem[this.selectedIndex].CommandIndex;
		}
		XUiC_Radial.RadialContextAbs radialContextAbs = this.context;
		XUiC_Radial.CommandHandlerDelegate commandHandlerDelegate = this.commandHandler;
		this.Close();
		commandHandlerDelegate(this, commandIndex, radialContextAbs);
	}

	// Token: 0x060081BA RID: 33210 RVA: 0x00315B10 File Offset: 0x00313D10
	[PublicizedFrom(EAccessModifier.Private)]
	public void Close()
	{
		if (GameManager.Instance == null)
		{
			Log.Out("GetGameManager is null");
		}
		if (this.xui.playerUI.windowManager == null)
		{
			Log.Out("GetWindowManager is null");
		}
		this.isOpenRequested = false;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x060081BB RID: 33211 RVA: 0x00315B7C File Offset: 0x00313D7C
	public void ResetRadialEntries()
	{
		for (int i = 0; i < this.menuItemState.Length; i++)
		{
			this.menuItemState[i] = false;
			this.menuItem[i].SetHighlighted(false);
		}
	}

	// Token: 0x060081BC RID: 33212 RVA: 0x00315BB4 File Offset: 0x00313DB4
	public void SetCommonData(UIUtils.ButtonIcon _controllerButtonForSelect, XUiC_Radial.CommandHandlerDelegate _commandHandlerFunc, XUiC_Radial.RadialContextAbs _context = null, int _preSelectedCommandIndex = -1, bool _hasSpecialActionPriorToRadialVisibility = false, XUiC_Radial.RadialStillValidDelegate _radialValidityCallback = null)
	{
		this.updateRadialButtonPositions();
		int num = this.currentEnabledEntriesCount();
		this.context = _context;
		this.selectionControllerButton = _controllerButtonForSelect;
		this.commandHandler = _commandHandlerFunc;
		this.validCheckDelegate = _radialValidityCallback;
		this.hasSpecialActionPriorToRadialVisibility = _hasSpecialActionPriorToRadialVisibility;
		this.DefaultSelect();
		this.selectedIndex = int.MinValue;
		if (_preSelectedCommandIndex >= 0)
		{
			for (int i = 0; i < this.menuItem.Length; i++)
			{
				if (this.menuItemState[i] && this.menuItem[i].CommandIndex == _preSelectedCommandIndex)
				{
					this.selectedIndex = i;
				}
			}
		}
		if (num == 0 || num == 1)
		{
			this.selectedIndex = 0;
			if (this.hasSpecialActionPriorToRadialVisibility)
			{
				if (InputUtils.ShiftKeyPressed)
				{
					this.selectedIndex = -2;
				}
				else
				{
					this.selectedIndex = -1;
				}
			}
			if (num == 1 || this.selectedIndex < 0)
			{
				this.CallContextAction();
				return;
			}
			this.Close();
		}
		for (int j = 0; j < this.menuItem.Length; j++)
		{
			this.menuItem[j].ViewComponent.IsVisible = j < num;
			this.menuItem[j].ViewComponent.Enabled = j < num;
		}
	}

	// Token: 0x060081BD RID: 33213 RVA: 0x00315CC5 File Offset: 0x00313EC5
	[PublicizedFrom(EAccessModifier.Private)]
	public void DefaultSelect()
	{
		this.SetHovered(null);
		this.ResetIconsScale();
	}

	// Token: 0x060081BE RID: 33214 RVA: 0x00315CD4 File Offset: 0x00313ED4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalculateSelectionFromController(PlayerActionsLocal _actionSet)
	{
		bool flag = this.xui.playerUI.entityPlayer != null && this.xui.playerUI.entityPlayer.AttachedToEntity is EntityVehicle;
		Vector2 vector = (flag ? _actionSet.VehicleActions.Look.Value : _actionSet.Look.Value);
		if (vector == Vector2.zero)
		{
			vector = (flag ? _actionSet.VehicleActions.LeftStick.Value : _actionSet.Move.Value);
		}
		if (vector.magnitude < 0.75f)
		{
			return;
		}
		float num = 361f;
		XUiC_RadialEntry xuiC_RadialEntry = null;
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			if (this.menuItemState[i])
			{
				XUiC_RadialEntry xuiC_RadialEntry2 = this.menuItem[i];
				Vector3 localPosition = xuiC_RadialEntry2.ViewComponent.UiTransform.localPosition;
				Vector2 vector2 = new Vector2(localPosition.x, localPosition.y);
				float num2 = Vector2.Angle(vector, vector2);
				if (num2 < num)
				{
					num = num2;
					xuiC_RadialEntry = xuiC_RadialEntry2;
				}
			}
		}
		if (xuiC_RadialEntry != null)
		{
			this.SetHovered(xuiC_RadialEntry);
		}
	}

	// Token: 0x060081BF RID: 33215 RVA: 0x00315DF4 File Offset: 0x00313FF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetIconsScale()
	{
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			this.menuItem[i].ResetScale();
		}
	}

	// Token: 0x060081C0 RID: 33216 RVA: 0x00315E24 File Offset: 0x00314024
	[PublicizedFrom(EAccessModifier.Private)]
	public void SelectionEffect(XUiC_RadialEntry _entry, bool _selected)
	{
		if (!_selected)
		{
			if (_entry != null)
			{
				_entry.SetScale(1f, false);
			}
			this.selectionText.Text = "";
			return;
		}
		if (_entry != null)
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
			{
				_entry.ViewComponent.PlayHoverSound();
			}
			_entry.SetScale(1.5f, false);
			this.selectionText.Text = _entry.SelectionText;
			return;
		}
		this.selectionText.Text = "";
	}

	// Token: 0x060081C1 RID: 33217 RVA: 0x00315EA2 File Offset: 0x003140A2
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetHovered(XUiC_RadialEntry _newSelected)
	{
		if (this.mCurrentlySelectedEntry != _newSelected)
		{
			this.SelectionEffect(this.mCurrentlySelectedEntry, false);
			this.selectedIndex = Array.IndexOf<XUiC_RadialEntry>(this.menuItem, _newSelected);
			this.SelectionEffect(this.mCurrentlySelectedEntry, true);
		}
	}

	// Token: 0x060081C2 RID: 33218 RVA: 0x00315EDC File Offset: 0x003140DC
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentEnabledEntriesCount()
	{
		int num = 0;
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			if (this.menuItemState[i])
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060081C3 RID: 33219 RVA: 0x00315F0D File Offset: 0x0031410D
	public void CreateRadialEntry(int _commandIdx, string _icon, string _atlas = "UIAtlas", string _text = "", string _selectionText = "", bool _highlighted = false)
	{
		this.CreateRadialEntry(_commandIdx, _icon, Color.white, _atlas, _text, _selectionText, _highlighted);
	}

	// Token: 0x060081C4 RID: 33220 RVA: 0x00315F24 File Offset: 0x00314124
	public void CreateRadialEntry(int _commandIdx, string _icon, Color _iconColor, string _atlas = "UIAtlas", string _text = "", string _selectionText = "", bool _highlighted = false)
	{
		int num = this.currentEnabledEntriesCount();
		if (num >= this.menuItem.Length)
		{
			return;
		}
		this.menuItemState[num] = true;
		XUiC_RadialEntry xuiC_RadialEntry = this.menuItem[num];
		xuiC_RadialEntry.CommandIndex = _commandIdx;
		xuiC_RadialEntry.SetAtlas(_atlas);
		xuiC_RadialEntry.SetSprite(_icon, _iconColor);
		xuiC_RadialEntry.SetText(_text);
		xuiC_RadialEntry.SetHighlighted(_highlighted);
		xuiC_RadialEntry.SelectionText = _selectionText;
	}

	// Token: 0x060081C5 RID: 33221 RVA: 0x00315F84 File Offset: 0x00314184
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateRadialButtonPositions()
	{
		int num = this.currentEnabledEntriesCount();
		int num2 = ((num > 1) ? 50 : 0);
		float num3 = (float)Utils.FastMax(1, num);
		int num4 = 0;
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			float num5 = -1.5707964f - 2f / num3 * (float)num4 * 3.1415927f;
			float num6 = ((float)(-(float)num) * 12.5f - (float)num2) * Mathf.Cos(num5);
			float num7 = ((float)(-(float)num) * 12.5f - (float)num2) * Mathf.Sin(num5);
			this.menuItem[i].ViewComponent.UiTransform.localPosition = new Vector3(num6, num7, 0f);
			num4++;
		}
	}

	// Token: 0x060081C6 RID: 33222 RVA: 0x00316034 File Offset: 0x00314234
	public static bool RadialValidSameHoldingSlotIndex(XUiC_Radial _sender, XUiC_Radial.RadialContextAbs _context)
	{
		XUiC_Radial.RadialContextHoldingSlotIndex radialContextHoldingSlotIndex = _context as XUiC_Radial.RadialContextHoldingSlotIndex;
		return radialContextHoldingSlotIndex != null && _sender.xui.playerUI.entityPlayer.inventory.holdingItemIdx == radialContextHoldingSlotIndex.ItemSlotIndex;
	}

	// Token: 0x060081C7 RID: 33223 RVA: 0x00316070 File Offset: 0x00314270
	public void SetCurrentBlockData(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _entityFocusing)
	{
		this.ResetRadialEntries();
		BlockActivationCommand[] blockActivationCommands = _blockValue.Block.GetBlockActivationCommands(_world, _blockValue, _blockPos, _entityFocusing);
		BlockActivationCommand[] customCmds = _blockValue.Block.CustomCmds;
		int num = blockActivationCommands.Length + customCmds.Length;
		for (int i = 0; i < num; i++)
		{
			if (i < blockActivationCommands.Length)
			{
				if (blockActivationCommands[i].enabled)
				{
					this.CreateRadialEntry(i, string.Format("ui_game_symbol_{0}", blockActivationCommands[i].icon), "UIAtlas", "", Localization.Get("blockcommand_" + blockActivationCommands[i].text, false, null), blockActivationCommands[i].highlighted);
				}
			}
			else
			{
				int num2 = i - blockActivationCommands.Length;
				if (customCmds[num2].enabled)
				{
					this.CreateRadialEntry(i, customCmds[num2].icon, customCmds[num2].iconColor, "UIAtlas", "", Localization.Get(customCmds[num2].text, false, null), customCmds[num2].highlighted);
				}
			}
		}
		this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.Activate), new XUiC_Radial.CommandHandlerDelegate(this.handleBlockCommand), new XUiC_Radial.RadialContextBlock(_blockPos, _blockValue, _entityFocusing, blockActivationCommands, customCmds), -1, false, null);
	}

	// Token: 0x060081C8 RID: 33224 RVA: 0x003161C4 File Offset: 0x003143C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleBlockCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		XUiC_Radial.RadialContextBlock radialContextBlock = _context as XUiC_Radial.RadialContextBlock;
		if (radialContextBlock == null)
		{
			return;
		}
		if (_commandIndex < radialContextBlock.Commands.Length)
		{
			radialContextBlock.BlockValue.Block.OnBlockActivated(radialContextBlock.Commands[_commandIndex].text, GameManager.Instance.World, radialContextBlock.BlockPos, radialContextBlock.BlockValue, radialContextBlock.EntityFocusing);
			return;
		}
		BlockActivationCommand blockActivationCommand = radialContextBlock.CustomCommands[_commandIndex - radialContextBlock.Commands.Length];
		if (blockActivationCommand.activateTime > 0f)
		{
			LocalPlayerUI playerUI = radialContextBlock.EntityFocusing.PlayerUI;
			TimerEventData timerEventData = new TimerEventData();
			timerEventData.Data = new object[] { blockActivationCommand.eventName, radialContextBlock.BlockPos, radialContextBlock.EntityFocusing };
			timerEventData.CloseOnHit = true;
			timerEventData.FullTimeFinishEvent += this.EventData_Event;
			XUiC_Timer.OpenTimer(playerUI.xui, blockActivationCommand.activateTime, timerEventData, -1f, "", true);
			return;
		}
		GameEventManager.Current.HandleAction(blockActivationCommand.eventName, null, radialContextBlock.EntityFocusing, false, radialContextBlock.BlockPos, "", "", false, true, "", null, null);
	}

	// Token: 0x060081C9 RID: 33225 RVA: 0x00316300 File Offset: 0x00314500
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_Event(TimerEventData timerData)
	{
		object[] array = (object[])timerData.Data;
		string text = (string)array[0];
		Vector3i vector3i = (Vector3i)array[1];
		EntityPlayerLocal entityPlayerLocal = array[2] as EntityPlayerLocal;
		GameEventManager.Current.HandleAction(text, null, entityPlayerLocal, false, vector3i, "", "", false, true, "", null, null);
	}

	// Token: 0x060081CA RID: 33226 RVA: 0x0031635C File Offset: 0x0031455C
	public void SetCurrentEntityData(Entity _entity, EntityAlive _entityFocusing)
	{
		this.ResetRadialEntries();
		EntityPlayerLocal entityPlayerLocal = _entityFocusing as EntityPlayerLocal;
		_entity.UpdateActivationCommands(entityPlayerLocal);
		EntityActivationCommand[] activationCommands = _entity.GetActivationCommands();
		List<EntityActivationCommand> list = new List<EntityActivationCommand>(activationCommands.Length);
		for (int i = 0; i < activationCommands.Length; i++)
		{
			if (activationCommands[i].enabled)
			{
				int count = list.Count;
				list.Add(activationCommands[i]);
				this.CreateRadialEntry(count, string.Format("ui_game_symbol_{0}", activationCommands[i].icon), "UIAtlas", "", activationCommands[i].commandText, false);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.Activate), new XUiC_Radial.CommandHandlerDelegate(this.handleEntityCommand), new XUiC_Radial.RadialContextEntity(default(Vector3i), _entityFocusing, _entity, list.ToArray()), -1, false, null);
	}

	// Token: 0x060081CB RID: 33227 RVA: 0x00316444 File Offset: 0x00314644
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleEntityCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		XUiC_Radial.RadialContextEntity radialContextEntity = _context as XUiC_Radial.RadialContextEntity;
		if (radialContextEntity == null)
		{
			return;
		}
		if (_commandIndex < 0 || _commandIndex >= radialContextEntity.Commands.Length)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = radialContextEntity.EntityFocusing as EntityPlayerLocal;
		EntityActivationCommand entityActivationCommand = radialContextEntity.Commands[_commandIndex];
		if (!radialContextEntity.EntityFocused.AllowActivationCommand(entityActivationCommand.commandId, entityPlayerLocal))
		{
			return;
		}
		radialContextEntity.EntityFocused.ActivateEntityCommand(entityActivationCommand, entityPlayerLocal);
	}

	// Token: 0x060081CC RID: 33228 RVA: 0x003164AC File Offset: 0x003146AC
	public void SetActivatableItemData(EntityPlayerLocal _epl)
	{
		this.ResetRadialEntries();
		this.activatableItemPool.Clear();
		_epl.CollectActivatableItems(this.activatableItemPool);
		for (int i = 0; i < this.activatableItemPool.Count; i++)
		{
			this.CreateRadialEntry(i, this.activatableItemPool[i].ItemClass.GetIconName(), this.activatableItemPool[i].Activated ? "ItemIconAtlas" : "ItemIconAtlasGreyscale", "", this.activatableItemPool[i].ItemClass.GetLocalizedItemName(), false);
		}
		this.SetCommonData(UIUtils.GetButtonIconForAction(_epl.playerInput.Activate), new XUiC_Radial.CommandHandlerDelegate(this.handleActivatableItemCommand), new XUiC_Radial.RadialContextHoldingSlotIndex(_epl.inventory.holdingItemIdx), -1, false, new XUiC_Radial.RadialStillValidDelegate(XUiC_Radial.RadialValidSameHoldingSlotIndex));
	}

	// Token: 0x060081CD RID: 33229 RVA: 0x00316584 File Offset: 0x00314784
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleActivatableItemCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		MinEventParams.CopyTo(entityPlayer.MinEventContext, MinEventParams.CachedEventParam);
		this.activatableItemPool.Clear();
		entityPlayer.CollectActivatableItems(this.activatableItemPool);
		if (!this.activatableItemPool[_commandIndex].Activated)
		{
			MinEventParams.CachedEventParam.ItemValue = this.activatableItemPool[_commandIndex];
			this.activatableItemPool[_commandIndex].FireEvent(MinEventTypes.onSelfItemActivate, MinEventParams.CachedEventParam);
			this.activatableItemPool[_commandIndex].Activated = true;
			entityPlayer.bPlayerStatsChanged = true;
		}
		else
		{
			MinEventParams.CachedEventParam.ItemValue = this.activatableItemPool[_commandIndex];
			this.activatableItemPool[_commandIndex].FireEvent(MinEventTypes.onSelfItemDeactivate, MinEventParams.CachedEventParam);
			this.activatableItemPool[_commandIndex].Activated = false;
			entityPlayer.bPlayerStatsChanged = true;
		}
		entityPlayer.inventory.CallOnToolbeltChangedInternal();
	}

	// Token: 0x060081CE RID: 33230 RVA: 0x00316678 File Offset: 0x00314878
	public void SetupMenuData()
	{
		this.ResetRadialEntries();
		bool flag = GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled);
		this.CreateRadialEntry(0, "ui_game_symbol_hammer", "UIAtlas", "", Localization.Get("xuiWPcrafting", false, null), false);
		this.CreateRadialEntry(1, "ui_game_symbol_character", "UIAtlas", "", Localization.Get("xuiWPcharacter", false, null), false);
		if (World.MapEnabled)
		{
			this.CreateRadialEntry(2, "ui_game_symbol_map", "UIAtlas", "", Localization.Get("xuiWPmap", false, null), false);
		}
		this.CreateRadialEntry(3, "ui_game_symbol_skills", "UIAtlas", "", Localization.Get("xuiWPskills", false, null), false);
		this.CreateRadialEntry(4, "ui_game_symbol_quest", "UIAtlas", "", Localization.Get("xuiWPquests", false, null), false);
		if (ChallengeJournal.AllowChallenges || World.BiomeProgressionEnabled)
		{
			this.CreateRadialEntry(5, "ui_game_symbol_challenge", "UIAtlas", "", Localization.Get("xuiChallenges", false, null), false);
		}
		this.CreateRadialEntry(6, "ui_game_symbol_players", "UIAtlas", "", Localization.Get("xuiWPplayers", false, null), false);
		if (flag)
		{
			this.CreateRadialEntry(7, "ui_game_symbol_lightbulb", "UIAtlas", "", Localization.Get("xuiWPcreative", false, null), false);
		}
		if (EntityDrone.DebugModeEnabled)
		{
			this.CreateRadialEntry(8, "ui_game_symbol_drone", Localization.Get("entityJunkDrone", false, null), "", "", false);
		}
		this.CreateRadialEntry(9, "ui_game_symbol_chat", "UIAtlas", "", Localization.Get("inpActChatName", false, null), false);
		this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.Inventory), new XUiC_Radial.CommandHandlerDelegate(this.handleMenuCommand), null, -1, false, null);
	}

	// Token: 0x060081CF RID: 33231 RVA: 0x0031684C File Offset: 0x00314A4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleMenuCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		switch (_commandIndex)
		{
		case 0:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "crafting");
			return;
		case 1:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "character");
			return;
		case 2:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "map");
			return;
		case 3:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "skills");
			return;
		case 4:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "quests");
			return;
		case 5:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "challenges");
			return;
		case 6:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "players");
			return;
		case 7:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "creative");
			return;
		case 8:
			if (entityPlayer.ownedEntities.Count > 0)
			{
				throw new NotImplementedException();
			}
			break;
		case 9:
			this.xui.playerUI.windowManager.Open(XUiC_Chat.ID, true);
			return;
		default:
			XUiC_WindowSelector.OpenSelectorAndWindow(entityPlayer, "crafting");
			break;
		}
	}

	// Token: 0x060081D0 RID: 33232 RVA: 0x00316938 File Offset: 0x00314B38
	public void SetupToolbeltMenu(int _direction)
	{
		this.toolbeltSwapDirection = _direction;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		this.ResetRadialEntries();
		for (int i = 0; i < Mathf.Min(entityPlayer.inventory.Length, this.menuItem.Length); i++)
		{
			ItemStack item = entityPlayer.inventory.GetItem(i);
			if (item.IsEmpty())
			{
				this.CreateRadialEntry(i, "", "UIAtlas", "", "", false);
			}
			else
			{
				this.CreateRadialEntry(i, item.itemValue.ItemClass.GetIconName(), item.itemValue.ItemClass.GetIconTint(item.itemValue), "ItemIconAtlas", item.itemValue.ItemClass.CanStack() ? item.count.ToString() : "", item.itemValue.ItemClass.GetLocalizedItemName(), false);
			}
		}
		this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.Swap), new XUiC_Radial.CommandHandlerDelegate(this.HandleToolbeltCommand), null, -1, false, null);
	}

	// Token: 0x060081D1 RID: 33233 RVA: 0x00316A5C File Offset: 0x00314C5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleToolbeltCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		if (_commandIndex < 0 || Time.time - this.openTime < 0.4f)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			if (this.toolbeltSwapDirection == 0)
			{
				int bestQuickSwapSlot = entityPlayer.inventory.GetBestQuickSwapSlot();
				entityPlayer.MoveController.SetInventoryIdxFromScroll(bestQuickSwapSlot);
				return;
			}
			if (!InputUtils.AltKeyPressed)
			{
				int num = entityPlayer.inventory.GetFocusedItemIdx();
				XUiController xuiController = this.xui.FindWindowGroupByName("toolbelt");
				XUiC_Toolbelt xuiC_Toolbelt = ((xuiController != null) ? xuiController.GetChildByType<XUiC_Toolbelt>() : null);
				if (xuiC_Toolbelt != null)
				{
					num = xuiC_Toolbelt.WrapFocusWithinPage(num, (this.toolbeltSwapDirection < 0) ? (-1) : 1);
				}
				entityPlayer.MoveController.SetInventoryIdxFromScroll(num);
				return;
			}
			XUiController xuiController2 = this.xui.FindWindowGroupByName("toolbelt");
			XUiC_Toolbelt xuiC_Toolbelt2 = ((xuiController2 != null) ? xuiController2.GetChildByType<XUiC_Toolbelt>() : null);
			if (xuiC_Toolbelt2 != null)
			{
				if (this.toolbeltSwapDirection < 0)
				{
					xuiC_Toolbelt2.PrevPage();
					return;
				}
				xuiC_Toolbelt2.NextPage();
				return;
			}
		}
		else if (_commandIndex >= 0 && this.xui.playerUI.entityPlayer.inventory.holdingItemIdx != _commandIndex)
		{
			this.xui.playerUI.entityPlayer.inventory.SetHoldingItemIdx(_commandIndex);
		}
	}

	// Token: 0x060081D2 RID: 33234 RVA: 0x00316B84 File Offset: 0x00314D84
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getBasicBlockInfo(out EntityPlayerLocal _epl, out ItemInventoryData _ibid, out Block _blockHolding, out Block _blockSelectedShape, out bool _hasAutoRotation, out bool _onlySimpleRotations, out bool _hasCopyRotation, out bool _allowShapes, out bool _hasCopyAutoShape, out bool _hasCopyShapeLegacy, out bool _allowPainting, out bool _allowProp, out BlockPlacement.EnumPlacement _blockPlacement)
	{
		_hasAutoRotation = false;
		_onlySimpleRotations = false;
		_hasCopyRotation = false;
		_hasCopyAutoShape = false;
		_hasCopyShapeLegacy = false;
		_allowPainting = false;
		_epl = this.xui.playerUI.entityPlayer;
		Inventory inventory = _epl.inventory;
		_blockHolding = inventory.GetHoldingBlock().GetBlock();
		_ibid = (inventory.holdingItemData.IsBlock ? inventory.holdingItemData : null);
		_allowShapes = _blockHolding.SelectAlternates;
		_blockSelectedShape = null;
		_allowProp = false;
		_blockPlacement = BlockPlacement.EnumPlacement.Voxel;
		if (_blockHolding == null || _ibid == null)
		{
			return false;
		}
		_blockSelectedShape = (_allowShapes ? _blockHolding.GetAltBlock(_ibid.itemValue.Meta) : _blockHolding);
		_hasAutoRotation = _blockSelectedShape.BlockPlacementHelper != BlockPlacement.None;
		_onlySimpleRotations = (_blockSelectedShape.AllowedRotations & EBlockRotationClasses.Advanced) == EBlockRotationClasses.None;
		_hasCopyRotation = _epl.HitInfo.bHitValid && !_epl.HitInfo.hit.blockValue.isair && _blockSelectedShape.SupportsRotation(_epl.HitInfo.hit.blockValue.rotation);
		if (_allowShapes && _epl.HitInfo.bHitValid)
		{
			Block block = _epl.HitInfo.hit.blockValue.Block;
			if (block.GetAutoShapeType() != EAutoShapeType.None && _blockHolding.AutoShapeSupportsShapeName(block.GetAutoShapeShapeName()))
			{
				string text;
				_hasCopyAutoShape = !block.Properties.TryGetValue("ShapeMenu", out text) || !text.EqualsCaseInsensitive("false") || GameManager.Instance.IsEditMode() || _epl.IsGodMode.Value;
			}
			else if (_blockHolding.ContainsAlternateBlock(block.GetBlockName()))
			{
				_hasCopyShapeLegacy = true;
			}
		}
		_allowPainting = _blockSelectedShape.shape is BlockShapeNew && _blockSelectedShape.MeshIndex == 0;
		_allowProp = LaunchPrefs.EnableProps.Value && _ibid.item.IsProp();
		_blockPlacement = _ibid.Placement;
		return true;
	}

	// Token: 0x060081D3 RID: 33235 RVA: 0x00316D88 File Offset: 0x00314F88
	public void SetupBlockShapeData()
	{
		this.ResetRadialEntries();
		EntityPlayerLocal entityPlayerLocal;
		ItemInventoryData itemInventoryData;
		Block block;
		Block block2;
		bool flag;
		bool flag2;
		bool flag3;
		bool flag4;
		bool flag5;
		bool flag6;
		bool flag7;
		bool flag8;
		BlockPlacement.EnumPlacement enumPlacement;
		if (!this.getBasicBlockInfo(out entityPlayerLocal, out itemInventoryData, out block, out block2, out flag, out flag2, out flag3, out flag4, out flag5, out flag6, out flag7, out flag8, out enumPlacement))
		{
			this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.Activate), new XUiC_Radial.CommandHandlerDelegate(this.handleBlockShapeCommand), null, -1, false, null);
			return;
		}
		if (flag4)
		{
			this.CreateRadialEntry(0, "ui_game_symbol_all_blocks", "UIAtlas", "", Localization.Get("xuiShape", false, null), false);
			if (flag5 || flag6)
			{
				this.CreateRadialEntry(1, "ui_game_symbol_copy_shape", "UIAtlas", "", Localization.Get("xuiCopyShape", false, null), false);
				if (!flag2 && flag3)
				{
					this.CreateRadialEntry(8, "ui_game_symbol_copy_shape_and_rotation", "UIAtlas", "", Localization.Get("xuiCopyShapeAndRotation", false, null), false);
				}
			}
		}
		this.CreateRadialEntry(2, "ui_game_symbol_rotate_simple", "UIAtlas", "", Localization.Get("xuiSimpleRotation", false, null), false);
		if (!flag2)
		{
			this.CreateRadialEntry(3, "ui_game_symbol_rotate_advanced", "UIAtlas", "", Localization.Get("xuiAdvancedRotation", false, null), false);
			this.CreateRadialEntry(4, "ui_game_symbol_rotate_on_face", "UIAtlas", "", Localization.Get("xuiOnFaceRotation", false, null), false);
			if (flag)
			{
				this.CreateRadialEntry(5, "ui_game_symbol_rotate_auto", "UIAtlas", "", Localization.Get("xuiAutoRotation", false, null), false);
			}
			if (flag3)
			{
				this.CreateRadialEntry(6, "ui_game_symbol_paint_copy_block", "UIAtlas", "", Localization.Get("xuiCopyRotation", false, null), false);
			}
		}
		if (flag7 && (GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled)))
		{
			this.CreateRadialEntry(7, "ui_game_symbol_paint_bucket", "UIAtlas", "", Localization.Get("xuiMaterials", false, null), false);
		}
		if (flag8)
		{
			this.CreateRadialEntry(9, "ui_game_symbol_placement_voxel", "UIAtlas", "", Localization.Get("xuiPlacementVoxel", false, null), false);
			this.CreateRadialEntry(10, "ui_game_symbol_placement_free", "UIAtlas", "", Localization.Get("xuiPlacementFree", false, null), false);
		}
		this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.Activate), new XUiC_Radial.CommandHandlerDelegate(this.handleBlockShapeCommand), new XUiC_Radial.RadialContextHoldingSlotIndex(entityPlayerLocal.inventory.holdingItemIdx), -1, true, new XUiC_Radial.RadialStillValidDelegate(XUiC_Radial.RadialValidSameHoldingSlotIndex));
	}

	// Token: 0x060081D4 RID: 33236 RVA: 0x00316FF4 File Offset: 0x003151F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleBlockShapeCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		EntityPlayerLocal entityPlayerLocal;
		ItemInventoryData itemInventoryData;
		Block block;
		Block block2;
		bool flag;
		bool flag2;
		bool flag3;
		bool flag4;
		bool flag5;
		bool flag6;
		bool flag7;
		bool flag8;
		BlockPlacement.EnumPlacement enumPlacement;
		if (!this.getBasicBlockInfo(out entityPlayerLocal, out itemInventoryData, out block, out block2, out flag, out flag2, out flag3, out flag4, out flag5, out flag6, out flag7, out flag8, out enumPlacement))
		{
			return;
		}
		switch (_commandIndex)
		{
		case -2:
		case 0:
			if (flag4)
			{
				this.xui.GetChildByType<XUiC_ShapesWindow>().ItemValue = entityPlayerLocal.inventory.holdingItemItemValue.Clone();
				this.xui.playerUI.windowManager.Open("shapes", true);
				return;
			}
			break;
		case -1:
			block2.RotateHoldingBlock(itemInventoryData, false, true);
			return;
		case 1:
			if (flag5 || flag6)
			{
				Block block3 = entityPlayerLocal.HitInfo.hit.blockValue.Block;
				this.<handleBlockShapeCommand>g__copyShape|72_0(entityPlayerLocal, flag5, block, block3, itemInventoryData);
				return;
			}
			break;
		case 2:
			itemInventoryData.mode = BlockPlacement.EnumRotationMode.Simple;
			return;
		case 3:
			if (!flag2)
			{
				itemInventoryData.mode = BlockPlacement.EnumRotationMode.Advanced;
				return;
			}
			break;
		case 4:
			if (!flag2)
			{
				itemInventoryData.mode = BlockPlacement.EnumRotationMode.ToFace;
				return;
			}
			break;
		case 5:
			if (!flag2 && flag)
			{
				itemInventoryData.mode = BlockPlacement.EnumRotationMode.Auto;
				return;
			}
			break;
		case 6:
			if (!flag2 && flag3)
			{
				BlockValue blockValue = entityPlayerLocal.HitInfo.hit.blockValue;
				if (blockValue.ischild)
				{
					blockValue = entityPlayerLocal.world.GetBlock(blockValue.Block.multiBlockPos.GetParentPos(entityPlayerLocal.HitInfo.hit.blockPos, blockValue));
				}
				XUiC_Radial.<handleBlockShapeCommand>g__copyRotation|72_1(itemInventoryData, blockValue);
				return;
			}
			break;
		case 7:
			this.xui.playerUI.windowManager.Open("materials", true);
			return;
		case 8:
			if (!flag2 && flag3 && (flag5 || flag6))
			{
				BlockValue blockValue2 = entityPlayerLocal.HitInfo.hit.blockValue;
				if (blockValue2.ischild)
				{
					blockValue2 = entityPlayerLocal.world.GetBlock(blockValue2.Block.multiBlockPos.GetParentPos(entityPlayerLocal.HitInfo.hit.blockPos, blockValue2));
				}
				this.<handleBlockShapeCommand>g__copyShape|72_0(entityPlayerLocal, flag5, block, blockValue2.Block, itemInventoryData);
				XUiC_Radial.<handleBlockShapeCommand>g__copyRotation|72_1(itemInventoryData, blockValue2);
				return;
			}
			break;
		case 9:
			itemInventoryData.Placement = BlockPlacement.EnumPlacement.Voxel;
			return;
		case 10:
			itemInventoryData.Placement = BlockPlacement.EnumPlacement.Free;
			break;
		default:
			return;
		}
	}

	// Token: 0x060081D5 RID: 33237 RVA: 0x00317220 File Offset: 0x00315420
	public void SetupQuickActionsMenu(EntityPlayerLocal _epl)
	{
		this.ResetRadialEntries();
		this.activatableItemPool.Clear();
		_epl.CollectActivatableItems(this.activatableItemPool);
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsControlsDefaultQuickAction);
		if (@int == 0)
		{
			this.CreateRadialEntry(0, "ui_game_symbol_camera", "UIAtlas", "", Localization.Get("inpActCameraChangeName", false, null), false);
			this.CreateRadialEntry(1, "ui_game_symbol_drop_item", "UIAtlas", "", Localization.Get("inpActPlayerDropName", false, null), false);
		}
		else
		{
			if (this.activatableItemPool.Count > 0)
			{
				this.CreateRadialEntry(0, this.activatableItemPool[0].ItemClass.GetIconName(), this.activatableItemPool[0].Activated ? "ItemIconAtlas" : "ItemIconAtlasGreyscale", "", this.activatableItemPool[0].ItemClass.GetLocalizedItemName(), false);
			}
			else
			{
				this.CreateRadialEntry(0, "ui_game_symbol_x", "UIAtlas", "", Localization.Get("radialNoActivatableItems", false, null), false);
			}
			this.CreateRadialEntry(1, "ui_game_symbol_camera", "UIAtlas", "", Localization.Get("inpActCameraChangeName", false, null), false);
			this.CreateRadialEntry(2, "ui_game_symbol_drop_item", "UIAtlas", "", Localization.Get("inpActPlayerDropName", false, null), false);
		}
		for (int i = ((@int == 1) ? 1 : 0); i < this.activatableItemPool.Count; i++)
		{
			int num = i + 2;
			this.CreateRadialEntry(num, this.activatableItemPool[i].ItemClass.GetIconName(), this.activatableItemPool[i].Activated ? "ItemIconAtlas" : "ItemIconAtlasGreyscale", "", this.activatableItemPool[i].ItemClass.GetLocalizedItemName(), false);
		}
		this.SetCommonData(UIUtils.GetButtonIconForAction(this.xui.playerUI.playerInput.PermanentActions.QuickMenu), new XUiC_Radial.CommandHandlerDelegate(this.HandleQuickActionsCommand), null, -1, false, null);
	}

	// Token: 0x060081D6 RID: 33238 RVA: 0x00317420 File Offset: 0x00315620
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleQuickActionsCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		XUiC_Radial.eDefaultQuickAction @int = (XUiC_Radial.eDefaultQuickAction)GamePrefs.GetInt(EnumGamePrefs.OptionsControlsDefaultQuickAction);
		switch (_commandIndex)
		{
		case 0:
			if (@int == XUiC_Radial.eDefaultQuickAction.CameraChange)
			{
				entityPlayer.MoveController.cameraChangeRequested = true;
				return;
			}
			if (this.activatableItemPool.Count > 0)
			{
				this.handleActivatableItemCommand(this, 0, _context);
				return;
			}
			break;
		case 1:
			if (@int == XUiC_Radial.eDefaultQuickAction.CameraChange)
			{
				entityPlayer.MoveController.DropHeldItem();
				return;
			}
			entityPlayer.MoveController.cameraChangeRequested = true;
			return;
		case 2:
			if (@int == XUiC_Radial.eDefaultQuickAction.CameraChange)
			{
				this.handleActivatableItemCommand(this, _commandIndex - 2, _context);
				return;
			}
			entityPlayer.MoveController.DropHeldItem();
			return;
		default:
			if (_commandIndex > 1)
			{
				this.handleActivatableItemCommand(this, _commandIndex - 2, _context);
			}
			break;
		}
	}

	// Token: 0x060081D8 RID: 33240 RVA: 0x003174EC File Offset: 0x003156EC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <handleBlockShapeCommand>g__copyShape|72_0(EntityPlayerLocal _epl, bool _hasCopyAutoShape, Block _block, Block _targetedBlock, ItemInventoryData _ibid)
	{
		int num = (_hasCopyAutoShape ? _block.AutoShapeAlternateShapeNameIndex(_targetedBlock.GetAutoShapeShapeName()) : _block.GetAlternateBlockIndex(_targetedBlock.GetBlockName()));
		if (num >= 0)
		{
			_ibid.itemValue.Meta = num;
			int holdingItemIdx = _epl.inventory.holdingItemIdx;
			XUiC_Toolbelt childByType = ((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>();
			XUiC_ItemStack xuiC_ItemStack;
			if (childByType != null && childByType.TryGetSlotControl(holdingItemIdx, out xuiC_ItemStack))
			{
				xuiC_ItemStack.ItemStack = new ItemStack(_ibid.itemValue, _ibid.itemStack.count);
				xuiC_ItemStack.ForceRefreshItemStack();
			}
		}
	}

	// Token: 0x060081D9 RID: 33241 RVA: 0x00317592 File Offset: 0x00315792
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <handleBlockShapeCommand>g__copyRotation|72_1(ItemInventoryData _ibid, BlockValue _bvLookingAt)
	{
		_ibid.rotation = _bvLookingAt.rotation;
		_ibid.mode = BlockPlacement.EnumRotationMode.Advanced;
	}

	// Token: 0x04006040 RID: 24640
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RadialEntry[] menuItem;

	// Token: 0x04006041 RID: 24641
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] menuItemState;

	// Token: 0x04006042 RID: 24642
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex;

	// Token: 0x04006043 RID: 24643
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSpecialActionPriorToRadialVisibility;

	// Token: 0x04006044 RID: 24644
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Radial.RadialContextAbs context;

	// Token: 0x04006045 RID: 24645
	[PublicizedFrom(EAccessModifier.Private)]
	public UIUtils.ButtonIcon selectionControllerButton;

	// Token: 0x04006046 RID: 24646
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Radial.CommandHandlerDelegate commandHandler;

	// Token: 0x04006047 RID: 24647
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Radial.RadialStillValidDelegate validCheckDelegate;

	// Token: 0x04006048 RID: 24648
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpenRequested;

	// Token: 0x04006049 RID: 24649
	[PublicizedFrom(EAccessModifier.Private)]
	public float displayDelay = 0.25f;

	// Token: 0x0400604A RID: 24650
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTime;

	// Token: 0x0400604B RID: 24651
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadialSelectedScale = 1.5f;

	// Token: 0x0400604C RID: 24652
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingCallouts;

	// Token: 0x0400604D RID: 24653
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label selectionText;

	// Token: 0x0400604E RID: 24654
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemValue> activatableItemPool = new List<ItemValue>();

	// Token: 0x0400604F RID: 24655
	[PublicizedFrom(EAccessModifier.Private)]
	public int toolbeltSwapDirection;

	// Token: 0x04006050 RID: 24656
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ALL_BLOCKS = 0;

	// Token: 0x04006051 RID: 24657
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_COPY_SHAPE = 1;

	// Token: 0x04006052 RID: 24658
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_SIMPLE = 2;

	// Token: 0x04006053 RID: 24659
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_ADVANCED = 3;

	// Token: 0x04006054 RID: 24660
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_ON_FACE = 4;

	// Token: 0x04006055 RID: 24661
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_AUTO = 5;

	// Token: 0x04006056 RID: 24662
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PAINT_COPY_BLOCK = 6;

	// Token: 0x04006057 RID: 24663
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PAINT_MATERIALS = 7;

	// Token: 0x04006058 RID: 24664
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_COPY_SHAPE_AND_ROTATION = 8;

	// Token: 0x04006059 RID: 24665
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PLACEMENT_VOXEL = 9;

	// Token: 0x0400605A RID: 24666
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PLACEMENT_FREE = 10;

	// Token: 0x02001020 RID: 4128
	// (Invoke) Token: 0x060081DB RID: 33243
	public delegate void CommandHandlerDelegate(XUiC_Radial _xuiRadial, int _commandIndex, XUiC_Radial.RadialContextAbs _context);

	// Token: 0x02001021 RID: 4129
	// (Invoke) Token: 0x060081DF RID: 33247
	public delegate bool RadialStillValidDelegate(XUiC_Radial _xuiRadial, XUiC_Radial.RadialContextAbs _context);

	// Token: 0x02001022 RID: 4130
	public class RadialContextAbs
	{
	}

	// Token: 0x02001023 RID: 4131
	public class RadialContextHoldingSlotIndex : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x060081E3 RID: 33251 RVA: 0x003175A8 File Offset: 0x003157A8
		public RadialContextHoldingSlotIndex(int _itemSlotIndex)
		{
			this.ItemSlotIndex = _itemSlotIndex;
		}

		// Token: 0x0400605B RID: 24667
		public readonly int ItemSlotIndex;
	}

	// Token: 0x02001024 RID: 4132
	public class RadialContextBlock : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x060081E4 RID: 33252 RVA: 0x003175B7 File Offset: 0x003157B7
		public RadialContextBlock(Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _entityFocusing, BlockActivationCommand[] _commands, BlockActivationCommand[] _customCommands)
		{
			this.BlockPos = _blockPos;
			this.BlockValue = _blockValue;
			this.EntityFocusing = _entityFocusing;
			this.Commands = _commands;
			this.CustomCommands = _customCommands;
		}

		// Token: 0x0400605C RID: 24668
		public readonly Vector3i BlockPos;

		// Token: 0x0400605D RID: 24669
		public readonly BlockValue BlockValue;

		// Token: 0x0400605E RID: 24670
		public readonly EntityPlayerLocal EntityFocusing;

		// Token: 0x0400605F RID: 24671
		public readonly BlockActivationCommand[] Commands;

		// Token: 0x04006060 RID: 24672
		public readonly BlockActivationCommand[] CustomCommands;
	}

	// Token: 0x02001025 RID: 4133
	public class RadialContextEntity : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x060081E5 RID: 33253 RVA: 0x003175E4 File Offset: 0x003157E4
		public RadialContextEntity(Vector3i _blockPos, EntityAlive _entityFocusing, Entity _entityFocused, EntityActivationCommand[] _commands)
		{
			this.BlockPos = _blockPos;
			this.EntityFocusing = _entityFocusing;
			this.EntityFocused = _entityFocused;
			this.Commands = _commands;
		}

		// Token: 0x04006061 RID: 24673
		public readonly Vector3i BlockPos;

		// Token: 0x04006062 RID: 24674
		public readonly EntityAlive EntityFocusing;

		// Token: 0x04006063 RID: 24675
		public readonly Entity EntityFocused;

		// Token: 0x04006064 RID: 24676
		public readonly EntityActivationCommand[] Commands;
	}

	// Token: 0x02001026 RID: 4134
	public enum eDefaultQuickAction
	{
		// Token: 0x04006066 RID: 24678
		CameraChange,
		// Token: 0x04006067 RID: 24679
		ToggleAttachment
	}
}
