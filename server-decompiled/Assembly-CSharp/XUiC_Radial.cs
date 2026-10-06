using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200100F RID: 4111
[Preserve]
public class XUiC_Radial : XUiController
{
	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x0600815C RID: 33116 RVA: 0x003137C4 File Offset: 0x003119C4
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

	// Token: 0x0600815D RID: 33117 RVA: 0x003137F0 File Offset: 0x003119F0
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

	// Token: 0x0600815E RID: 33118 RVA: 0x003138A0 File Offset: 0x00311AA0
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.RadialWindow = null;
	}

	// Token: 0x0600815F RID: 33119 RVA: 0x003138B4 File Offset: 0x00311AB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_Radial_OnPress(XUiC_RadialEntry _sender)
	{
		this.selectedIndex = _sender.MenuItemIndex;
		this.CallContextAction();
	}

	// Token: 0x06008160 RID: 33120 RVA: 0x003138C8 File Offset: 0x00311AC8
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

	// Token: 0x06008161 RID: 33121 RVA: 0x00313907 File Offset: 0x00311B07
	public void Open()
	{
		this.openTime = Time.time;
		this.isOpenRequested = true;
	}

	// Token: 0x06008162 RID: 33122 RVA: 0x0031391C File Offset: 0x00311B1C
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

	// Token: 0x06008163 RID: 33123 RVA: 0x00313A34 File Offset: 0x00311C34
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

	// Token: 0x06008164 RID: 33124 RVA: 0x00313ACC File Offset: 0x00311CCC
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

	// Token: 0x06008165 RID: 33125 RVA: 0x00313C18 File Offset: 0x00311E18
	[PublicizedFrom(EAccessModifier.Private)]
	public bool radialButtonPressed(PlayerActionsLocal _actionSet)
	{
		return _actionSet.Activate.IsPressed || _actionSet.PermanentActions.Activate.IsPressed || _actionSet.Reload.IsPressed || _actionSet.PermanentActions.Reload.IsPressed || _actionSet.ToggleFlashlight.IsPressed || _actionSet.PermanentActions.ToggleFlashlight.IsPressed || _actionSet.Inventory.IsPressed || _actionSet.VehicleActions.Inventory.IsPressed || _actionSet.PermanentActions.Inventory.IsPressed || _actionSet.Swap.IsPressed || _actionSet.PermanentActions.Swap.IsPressed || _actionSet.InventorySlotLeft.IsPressed || _actionSet.InventorySlotRight.IsPressed || _actionSet.QuickMenu.IsPressed || _actionSet.PermanentActions.QuickMenu.IsPressed;
	}

	// Token: 0x06008166 RID: 33126 RVA: 0x00313D1F File Offset: 0x00311F1F
	[PublicizedFrom(EAccessModifier.Private)]
	public bool radialButtonIsLeftSide(PlayerActionsLocal _actionSet)
	{
		return _actionSet.ToggleFlashlight.IsPressed || _actionSet.PermanentActions.ToggleFlashlight.IsPressed;
	}

	// Token: 0x06008167 RID: 33127 RVA: 0x00313D40 File Offset: 0x00311F40
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

	// Token: 0x06008168 RID: 33128 RVA: 0x00313E0C File Offset: 0x0031200C
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

	// Token: 0x06008169 RID: 33129 RVA: 0x00313E78 File Offset: 0x00312078
	public void ResetRadialEntries()
	{
		for (int i = 0; i < this.menuItemState.Length; i++)
		{
			this.menuItemState[i] = false;
			this.menuItem[i].SetHighlighted(false);
		}
	}

	// Token: 0x0600816A RID: 33130 RVA: 0x00313EB0 File Offset: 0x003120B0
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

	// Token: 0x0600816B RID: 33131 RVA: 0x00313FC1 File Offset: 0x003121C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void DefaultSelect()
	{
		this.SetHovered(null);
		this.ResetIconsScale();
	}

	// Token: 0x0600816C RID: 33132 RVA: 0x00313FD0 File Offset: 0x003121D0
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

	// Token: 0x0600816D RID: 33133 RVA: 0x003140F0 File Offset: 0x003122F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetIconsScale()
	{
		for (int i = 0; i < this.menuItem.Length; i++)
		{
			this.menuItem[i].ResetScale();
		}
	}

	// Token: 0x0600816E RID: 33134 RVA: 0x00314120 File Offset: 0x00312320
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

	// Token: 0x0600816F RID: 33135 RVA: 0x0031419E File Offset: 0x0031239E
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

	// Token: 0x06008170 RID: 33136 RVA: 0x003141D8 File Offset: 0x003123D8
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

	// Token: 0x06008171 RID: 33137 RVA: 0x00314209 File Offset: 0x00312409
	public void CreateRadialEntry(int _commandIdx, string _icon, string _atlas = "UIAtlas", string _text = "", string _selectionText = "", bool _highlighted = false)
	{
		this.CreateRadialEntry(_commandIdx, _icon, Color.white, _atlas, _text, _selectionText, _highlighted);
	}

	// Token: 0x06008172 RID: 33138 RVA: 0x00314220 File Offset: 0x00312420
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

	// Token: 0x06008173 RID: 33139 RVA: 0x00314280 File Offset: 0x00312480
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

	// Token: 0x06008174 RID: 33140 RVA: 0x00314330 File Offset: 0x00312530
	public static bool RadialValidSameHoldingSlotIndex(XUiC_Radial _sender, XUiC_Radial.RadialContextAbs _context)
	{
		XUiC_Radial.RadialContextHoldingSlotIndex radialContextHoldingSlotIndex = _context as XUiC_Radial.RadialContextHoldingSlotIndex;
		return radialContextHoldingSlotIndex != null && _sender.xui.playerUI.entityPlayer.inventory.holdingItemIdx == radialContextHoldingSlotIndex.ItemSlotIndex;
	}

	// Token: 0x06008175 RID: 33141 RVA: 0x0031436C File Offset: 0x0031256C
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

	// Token: 0x06008176 RID: 33142 RVA: 0x003144C0 File Offset: 0x003126C0
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

	// Token: 0x06008177 RID: 33143 RVA: 0x003145FC File Offset: 0x003127FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_Event(TimerEventData timerData)
	{
		object[] array = (object[])timerData.Data;
		string text = (string)array[0];
		Vector3i vector3i = (Vector3i)array[1];
		EntityPlayerLocal entityPlayerLocal = array[2] as EntityPlayerLocal;
		GameEventManager.Current.HandleAction(text, null, entityPlayerLocal, false, vector3i, "", "", false, true, "", null, null);
	}

	// Token: 0x06008178 RID: 33144 RVA: 0x00314658 File Offset: 0x00312858
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

	// Token: 0x06008179 RID: 33145 RVA: 0x00314740 File Offset: 0x00312940
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

	// Token: 0x0600817A RID: 33146 RVA: 0x003147A8 File Offset: 0x003129A8
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

	// Token: 0x0600817B RID: 33147 RVA: 0x00314880 File Offset: 0x00312A80
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

	// Token: 0x0600817C RID: 33148 RVA: 0x00314974 File Offset: 0x00312B74
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

	// Token: 0x0600817D RID: 33149 RVA: 0x00314B48 File Offset: 0x00312D48
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

	// Token: 0x0600817E RID: 33150 RVA: 0x00314C34 File Offset: 0x00312E34
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

	// Token: 0x0600817F RID: 33151 RVA: 0x00314D58 File Offset: 0x00312F58
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

	// Token: 0x06008180 RID: 33152 RVA: 0x00314E80 File Offset: 0x00313080
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

	// Token: 0x06008181 RID: 33153 RVA: 0x00315084 File Offset: 0x00313284
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

	// Token: 0x06008182 RID: 33154 RVA: 0x003152F0 File Offset: 0x003134F0
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

	// Token: 0x06008183 RID: 33155 RVA: 0x0031551C File Offset: 0x0031371C
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

	// Token: 0x06008184 RID: 33156 RVA: 0x0031571C File Offset: 0x0031391C
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

	// Token: 0x06008186 RID: 33158 RVA: 0x003157E8 File Offset: 0x003139E8
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

	// Token: 0x06008187 RID: 33159 RVA: 0x0031588E File Offset: 0x00313A8E
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <handleBlockShapeCommand>g__copyRotation|72_1(ItemInventoryData _ibid, BlockValue _bvLookingAt)
	{
		_ibid.rotation = _bvLookingAt.rotation;
		_ibid.mode = BlockPlacement.EnumRotationMode.Advanced;
	}

	// Token: 0x04006002 RID: 24578
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RadialEntry[] menuItem;

	// Token: 0x04006003 RID: 24579
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] menuItemState;

	// Token: 0x04006004 RID: 24580
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex;

	// Token: 0x04006005 RID: 24581
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSpecialActionPriorToRadialVisibility;

	// Token: 0x04006006 RID: 24582
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Radial.RadialContextAbs context;

	// Token: 0x04006007 RID: 24583
	[PublicizedFrom(EAccessModifier.Private)]
	public UIUtils.ButtonIcon selectionControllerButton;

	// Token: 0x04006008 RID: 24584
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Radial.CommandHandlerDelegate commandHandler;

	// Token: 0x04006009 RID: 24585
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Radial.RadialStillValidDelegate validCheckDelegate;

	// Token: 0x0400600A RID: 24586
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpenRequested;

	// Token: 0x0400600B RID: 24587
	[PublicizedFrom(EAccessModifier.Private)]
	public float displayDelay = 0.25f;

	// Token: 0x0400600C RID: 24588
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTime;

	// Token: 0x0400600D RID: 24589
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadialSelectedScale = 1.5f;

	// Token: 0x0400600E RID: 24590
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingCallouts;

	// Token: 0x0400600F RID: 24591
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label selectionText;

	// Token: 0x04006010 RID: 24592
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemValue> activatableItemPool = new List<ItemValue>();

	// Token: 0x04006011 RID: 24593
	[PublicizedFrom(EAccessModifier.Private)]
	public int toolbeltSwapDirection;

	// Token: 0x04006012 RID: 24594
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ALL_BLOCKS = 0;

	// Token: 0x04006013 RID: 24595
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_COPY_SHAPE = 1;

	// Token: 0x04006014 RID: 24596
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_SIMPLE = 2;

	// Token: 0x04006015 RID: 24597
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_ADVANCED = 3;

	// Token: 0x04006016 RID: 24598
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_ON_FACE = 4;

	// Token: 0x04006017 RID: 24599
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_ROTATE_AUTO = 5;

	// Token: 0x04006018 RID: 24600
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PAINT_COPY_BLOCK = 6;

	// Token: 0x04006019 RID: 24601
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PAINT_MATERIALS = 7;

	// Token: 0x0400601A RID: 24602
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_COPY_SHAPE_AND_ROTATION = 8;

	// Token: 0x0400601B RID: 24603
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PLACEMENT_VOXEL = 9;

	// Token: 0x0400601C RID: 24604
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BLOCK_SHAPE_COMMAND_PLACEMENT_FREE = 10;

	// Token: 0x02001010 RID: 4112
	// (Invoke) Token: 0x06008189 RID: 33161
	public delegate void CommandHandlerDelegate(XUiC_Radial _xuiRadial, int _commandIndex, XUiC_Radial.RadialContextAbs _context);

	// Token: 0x02001011 RID: 4113
	// (Invoke) Token: 0x0600818D RID: 33165
	public delegate bool RadialStillValidDelegate(XUiC_Radial _xuiRadial, XUiC_Radial.RadialContextAbs _context);

	// Token: 0x02001012 RID: 4114
	public class RadialContextAbs
	{
	}

	// Token: 0x02001013 RID: 4115
	public class RadialContextHoldingSlotIndex : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x06008191 RID: 33169 RVA: 0x003158A4 File Offset: 0x00313AA4
		public RadialContextHoldingSlotIndex(int _itemSlotIndex)
		{
			this.ItemSlotIndex = _itemSlotIndex;
		}

		// Token: 0x0400601D RID: 24605
		public readonly int ItemSlotIndex;
	}

	// Token: 0x02001014 RID: 4116
	public class RadialContextBlock : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x06008192 RID: 33170 RVA: 0x003158B3 File Offset: 0x00313AB3
		public RadialContextBlock(Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _entityFocusing, BlockActivationCommand[] _commands, BlockActivationCommand[] _customCommands)
		{
			this.BlockPos = _blockPos;
			this.BlockValue = _blockValue;
			this.EntityFocusing = _entityFocusing;
			this.Commands = _commands;
			this.CustomCommands = _customCommands;
		}

		// Token: 0x0400601E RID: 24606
		public readonly Vector3i BlockPos;

		// Token: 0x0400601F RID: 24607
		public readonly BlockValue BlockValue;

		// Token: 0x04006020 RID: 24608
		public readonly EntityPlayerLocal EntityFocusing;

		// Token: 0x04006021 RID: 24609
		public readonly BlockActivationCommand[] Commands;

		// Token: 0x04006022 RID: 24610
		public readonly BlockActivationCommand[] CustomCommands;
	}

	// Token: 0x02001015 RID: 4117
	public class RadialContextEntity : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x06008193 RID: 33171 RVA: 0x003158E0 File Offset: 0x00313AE0
		public RadialContextEntity(Vector3i _blockPos, EntityAlive _entityFocusing, Entity _entityFocused, EntityActivationCommand[] _commands)
		{
			this.BlockPos = _blockPos;
			this.EntityFocusing = _entityFocusing;
			this.EntityFocused = _entityFocused;
			this.Commands = _commands;
		}

		// Token: 0x04006023 RID: 24611
		public readonly Vector3i BlockPos;

		// Token: 0x04006024 RID: 24612
		public readonly EntityAlive EntityFocusing;

		// Token: 0x04006025 RID: 24613
		public readonly Entity EntityFocused;

		// Token: 0x04006026 RID: 24614
		public readonly EntityActivationCommand[] Commands;
	}

	// Token: 0x02001016 RID: 4118
	public enum eDefaultQuickAction
	{
		// Token: 0x04006028 RID: 24616
		CameraChange,
		// Token: 0x04006029 RID: 24617
		ToggleAttachment
	}
}
