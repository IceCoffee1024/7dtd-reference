using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000563 RID: 1379
public class PlayerActionsLocal : PlayerActionsBase
{
	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06002CCE RID: 11470 RVA: 0x0011C114 File Offset: 0x0011A314
	public int InventorySlotWasPressed
	{
		get
		{
			for (int i = 0; i < this.InventoryActions.Count; i++)
			{
				if (this.InventoryActions[i].WasPressed)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06002CCF RID: 11471 RVA: 0x0011C150 File Offset: 0x0011A350
	public int InventorySlotWasReleased
	{
		get
		{
			for (int i = 0; i < this.InventoryActions.Count; i++)
			{
				if (this.InventoryActions[i].WasReleased)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x0011C18C File Offset: 0x0011A38C
	public int InventorySlotIsPressed
	{
		get
		{
			for (int i = 0; i < this.InventoryActions.Count; i++)
			{
				if (this.InventoryActions[i].IsPressed)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x0011C1C5 File Offset: 0x0011A3C5
	public PlayerActionsGUI GUIActions { get; }

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x0011C1CD File Offset: 0x0011A3CD
	public PlayerActionsVehicle VehicleActions { get; }

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x0011C1D5 File Offset: 0x0011A3D5
	public PlayerActionsPermanent PermanentActions { get; }

	// Token: 0x06002CD4 RID: 11476 RVA: 0x0011C1E0 File Offset: 0x0011A3E0
	public PlayerActionsLocal()
	{
		base.Name = "local";
		this.GUIActions = new PlayerActionsGUI
		{
			Enabled = false
		};
		this.PermanentActions = new PlayerActionsPermanent
		{
			Enabled = true
		};
		this.VehicleActions = new PlayerActionsVehicle
		{
			Enabled = false
		};
		base.UserData = new PlayerActionData.ActionSetUserData(new PlayerActionsBase[] { this.PermanentActions });
		this.VehicleActions.UserData = new PlayerActionData.ActionSetUserData(new PlayerActionsBase[] { this.PermanentActions });
		this.GUIActions.UserData = new PlayerActionData.ActionSetUserData(new PlayerActionsBase[] { this.PermanentActions });
		this.PermanentActions.UserData = new PlayerActionData.ActionSetUserData(new PlayerActionsBase[] { this, this.VehicleActions, this.GUIActions });
		this.InventoryActions.Add(this.InventorySlot1);
		this.InventoryActions.Add(this.InventorySlot2);
		this.InventoryActions.Add(this.InventorySlot3);
		this.InventoryActions.Add(this.InventorySlot4);
		this.InventoryActions.Add(this.InventorySlot5);
		this.InventoryActions.Add(this.InventorySlot6);
		this.InventoryActions.Add(this.InventorySlot7);
		this.InventoryActions.Add(this.InventorySlot8);
		this.InventoryActions.Add(this.InventorySlot9);
		this.InventoryActions.Add(this.InventorySlot10);
		InputManager.OnActiveDeviceChanged += delegate(InputDevice inputDevice)
		{
			this.UpdateDeadzones();
		};
		this.UpdateDeadzones();
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x0011C39C File Offset: 0x0011A59C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateActions()
	{
		this.MoveForward = base.CreatePlayerAction("Forward");
		this.MoveForward.UserData = new PlayerActionData.ActionUserData("inpActPlayerMoveForwardName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.MoveBack = base.CreatePlayerAction("Back");
		this.MoveBack.UserData = new PlayerActionData.ActionUserData("inpActPlayerMoveBackName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.MoveLeft = base.CreatePlayerAction("Left");
		this.MoveLeft.UserData = new PlayerActionData.ActionUserData("inpActPlayerMoveLeftName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.MoveRight = base.CreatePlayerAction("Right");
		this.MoveRight.UserData = new PlayerActionData.ActionUserData("inpActPlayerMoveRightName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Move = base.CreateTwoAxisPlayerAction(this.MoveLeft, this.MoveRight, this.MoveBack, this.MoveForward);
		this.LookLeft = base.CreatePlayerAction("LookLeft");
		this.LookLeft.UserData = new PlayerActionData.ActionUserData("inpActPlayerLookLeft", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.LookRight = base.CreatePlayerAction("LookRight");
		this.LookRight.UserData = new PlayerActionData.ActionUserData("inpActPlayerLookRight", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.LookUp = base.CreatePlayerAction("LookUp");
		this.LookUp.UserData = new PlayerActionData.ActionUserData("inpActPlayerLookUp", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.LookDown = base.CreatePlayerAction("LookDown");
		this.LookDown.UserData = new PlayerActionData.ActionUserData("inpActPlayerLookDown", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Look = base.CreateTwoAxisPlayerAction(this.LookLeft, this.LookRight, this.LookDown, this.LookUp);
		this.Primary = base.CreatePlayerAction("Primary");
		this.Primary.UserData = new PlayerActionData.ActionUserData("inpActPlayerPrimaryName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Primary.StateThreshold = 0.25f;
		this.Secondary = base.CreatePlayerAction("Secondary");
		this.Secondary.UserData = new PlayerActionData.ActionUserData("inpActPlayerSecondaryName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Secondary.StateThreshold = 0.25f;
		this.Run = base.CreatePlayerAction("Run");
		this.Run.UserData = new PlayerActionData.ActionUserData("inpActPlayerRunName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Jump = base.CreatePlayerAction("Jump");
		this.Jump.UserData = new PlayerActionData.ActionUserData("inpActPlayerJumpName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Crouch = base.CreatePlayerAction("Crouch");
		this.Crouch.UserData = new PlayerActionData.ActionUserData("inpActPlayerCrouchName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.JumpCrouch = base.CreateOneAxisPlayerAction(this.Crouch, this.Jump);
		this.ToggleCrouch = base.CreatePlayerAction("ToggleCrouch");
		this.ToggleCrouch.UserData = new PlayerActionData.ActionUserData("inpActPlayerToggleCrouchName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.ScrollUp = base.CreatePlayerAction("ScrollUp");
		this.ScrollUp.UserData = new PlayerActionData.ActionUserData("inpActScopeZoomInName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, true, false, true);
		this.ScrollDown = base.CreatePlayerAction("ScrollDown");
		this.ScrollDown.UserData = new PlayerActionData.ActionUserData("inpActScopeZoomOutName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, true, false, true);
		this.Scroll = base.CreateOneAxisPlayerAction(this.ScrollDown, this.ScrollUp);
		this.Activate = base.CreatePlayerAction("Activate");
		this.Activate.UserData = new PlayerActionData.ActionUserData("inpActActivateName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Activate.FirstRepeatDelay = 0.3f;
		this.Drop = base.CreatePlayerAction("Drop");
		this.Drop.UserData = new PlayerActionData.ActionUserData("inpActPlayerDropName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Swap = base.CreatePlayerAction("Swap");
		this.Swap.UserData = new PlayerActionData.ActionUserData("inpActSwapName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Reload = base.CreatePlayerAction("Reload");
		this.Reload.UserData = new PlayerActionData.ActionUserData("inpActPlayerReloadName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Reload.FirstRepeatDelay = 0.3f;
		this.ToggleFlashlight = base.CreatePlayerAction("ToggleFlashlight");
		this.ToggleFlashlight.UserData = new PlayerActionData.ActionUserData("inpActPlayerToggleFlashlightName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.ToggleFlashlight.FirstRepeatDelay = 0.3f;
		this.InventorySlot1 = base.CreatePlayerAction("Inventory1");
		this.InventorySlot1.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot1Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot2 = base.CreatePlayerAction("Inventory2");
		this.InventorySlot2.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot2Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot3 = base.CreatePlayerAction("Inventory3");
		this.InventorySlot3.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot3Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot4 = base.CreatePlayerAction("Inventory4");
		this.InventorySlot4.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot4Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot5 = base.CreatePlayerAction("Inventory5");
		this.InventorySlot5.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot5Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot6 = base.CreatePlayerAction("Inventory6");
		this.InventorySlot6.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot6Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot7 = base.CreatePlayerAction("Inventory7");
		this.InventorySlot7.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot7Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot8 = base.CreatePlayerAction("Inventory8");
		this.InventorySlot8.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot8Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot9 = base.CreatePlayerAction("Inventory9");
		this.InventorySlot9.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot9Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlot10 = base.CreatePlayerAction("Inventory10");
		this.InventorySlot10.UserData = new PlayerActionData.ActionUserData("inpActInventorySlot10Name", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlotLeft = base.CreatePlayerAction("InventorySelectLeft");
		this.InventorySlotLeft.UserData = new PlayerActionData.ActionUserData("inpActInventorySlotLeftName", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.InventorySlotRight = base.CreatePlayerAction("InventorySelectRight");
		this.InventorySlotRight.UserData = new PlayerActionData.ActionUserData("inpActInventorySlotRightName", null, PlayerActionData.GroupToolbelt, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Menu = base.CreatePlayerAction("Menu");
		this.Menu.UserData = new PlayerActionData.ActionUserData("inpActMenuName", null, PlayerActionData.GroupMenu, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.God = base.CreatePlayerAction("God");
		this.God.UserData = new PlayerActionData.ActionUserData("inpActGodModeName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.Fly = base.CreatePlayerAction("Fly");
		this.Fly.UserData = new PlayerActionData.ActionUserData("inpActFlyModeName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.Invisible = base.CreatePlayerAction("Invisible");
		this.Invisible.UserData = new PlayerActionData.ActionUserData("inpActInvisibleModeName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.IncSpeed = base.CreatePlayerAction("IncSpeed");
		this.IncSpeed.UserData = new PlayerActionData.ActionUserData("inpActIncGodSpeedName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.DecSpeed = base.CreatePlayerAction("DecSpeed");
		this.DecSpeed.UserData = new PlayerActionData.ActionUserData("inpActDecGodSpeedName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.GodAlternate = base.CreatePlayerAction("GodAlternate");
		this.GodAlternate.UserData = new PlayerActionData.ActionUserData("inpActGodAlternateModeName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.ControllerOnly, false, true, true, true);
		this.TeleportAlternate = base.CreatePlayerAction("TeleportAlternate");
		this.TeleportAlternate.UserData = new PlayerActionData.ActionUserData("inpActTeleportAlternateModeName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.ControllerOnly, false, true, true, true);
		this.CameraChange = base.CreatePlayerAction("CameraChange");
		this.CameraChange.UserData = new PlayerActionData.ActionUserData("inpActCameraChangeName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.CameraFunction = base.CreatePlayerAction("FlipCamera");
		this.CameraFunction.UserData = new PlayerActionData.ActionUserData("inpActFlipCameraName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.DetachCamera = base.CreatePlayerAction("DetachCamera");
		this.DetachCamera.UserData = new PlayerActionData.ActionUserData("inpActDetachCameraName", null, PlayerActionData.GroupEditCamera, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.ToggleDCMove = base.CreatePlayerAction("ToggleDCMove");
		this.ToggleDCMove.UserData = new PlayerActionData.ActionUserData("inpActToggleDCMoveName", null, PlayerActionData.GroupEditCamera, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.LockFreeCamera = base.CreatePlayerAction("LockFreeCamera");
		this.LockFreeCamera.UserData = new PlayerActionData.ActionUserData("inpActLockFreeCameraName", null, PlayerActionData.GroupEditCamera, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.SelectionFill = base.CreatePlayerAction("SelectionFill");
		this.SelectionFill.UserData = new PlayerActionData.ActionUserData("inpActSelectionFillName", null, PlayerActionData.GroupEditSelection, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.SelectionClear = base.CreatePlayerAction("SelectionClear");
		this.SelectionClear.UserData = new PlayerActionData.ActionUserData("inpActSelectionClearName", null, PlayerActionData.GroupEditSelection, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.SelectionSet = base.CreatePlayerAction("SelectionSet");
		this.SelectionSet.UserData = new PlayerActionData.ActionUserData("inpActSelectionSetName", null, PlayerActionData.GroupEditSelection, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.SelectionRotate = base.CreatePlayerAction("SelectionRotate");
		this.SelectionRotate.UserData = new PlayerActionData.ActionUserData("inpActSelectionRotateName", null, PlayerActionData.GroupEditSelection, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.SelectionDelete = base.CreatePlayerAction("SelectionDelete");
		this.SelectionDelete.UserData = new PlayerActionData.ActionUserData("inpActSelectionDeleteName", null, PlayerActionData.GroupEditSelection, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.SelectionMoveMode = base.CreatePlayerAction("SelectionMoveMode");
		this.SelectionMoveMode.UserData = new PlayerActionData.ActionUserData("inpActSelectionMoveModeName", null, PlayerActionData.GroupEditSelection, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.FocusCopyBlock = base.CreatePlayerAction("FocusCopyBlock");
		this.FocusCopyBlock.UserData = new PlayerActionData.ActionUserData("inpActFocusCopyBlockName", null, PlayerActionData.GroupEditOther, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.Prefab = base.CreatePlayerAction("Prefab");
		this.Prefab.UserData = new PlayerActionData.ActionUserData("inpActPrefabName", null, PlayerActionData.GroupEditOther, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.DensityM1 = base.CreatePlayerAction("DensityM1");
		this.DensityM1.UserData = new PlayerActionData.ActionUserData("inpActDensityM1Name", null, PlayerActionData.GroupEditOther, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.DensityP1 = base.CreatePlayerAction("DensityP1");
		this.DensityP1.UserData = new PlayerActionData.ActionUserData("inpActDensityP1Name", null, PlayerActionData.GroupEditOther, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.DensityM10 = base.CreatePlayerAction("DensityM10");
		this.DensityM10.UserData = new PlayerActionData.ActionUserData("inpActDensityM10Name", null, PlayerActionData.GroupEditOther, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.DensityP10 = base.CreatePlayerAction("DensityP10");
		this.DensityP10.UserData = new PlayerActionData.ActionUserData("inpActDensityP10Name", null, PlayerActionData.GroupEditOther, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.Inventory = base.CreatePlayerAction("Inventory");
		this.Inventory.UserData = new PlayerActionData.ActionUserData("inpActInventoryName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Scoreboard = base.CreatePlayerAction("Scoreboard");
		this.Scoreboard.UserData = new PlayerActionData.ActionUserData("inpActScoreboardName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.QuickMenu = base.CreatePlayerAction("QuickMenu");
		this.QuickMenu.UserData = new PlayerActionData.ActionUserData("inpQuickMenuName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.AiFreeze = base.CreatePlayerAction("AiFreeze");
		this.AiFreeze.UserData = new PlayerActionData.ActionUserData("inpActAiFreezeName", null, PlayerActionData.GroupDebugFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x0011D050 File Offset: 0x0011B250
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
		base.ListenOptions.IncludeControllers = true;
		this.ConfigureJoystickLayout();
		this.Run.AddDefaultBinding(InputControlType.LeftStickButton);
		this.Jump.AddDefaultBinding(InputControlType.Action1);
		this.ToggleCrouch.AddDefaultBinding(InputControlType.Action2);
		this.Activate.AddDefaultBinding(InputControlType.Action4);
		this.Reload.AddDefaultBinding(InputControlType.Action3);
		this.Primary.AddDefaultBinding(InputControlType.RightTrigger);
		this.Secondary.AddDefaultBinding(InputControlType.LeftTrigger);
		this.InventorySlotLeft.AddDefaultBinding(InputControlType.LeftBumper);
		this.InventorySlotRight.AddDefaultBinding(InputControlType.RightBumper);
		this.Swap.AddDefaultBinding(InputControlType.DPadLeft);
		this.Menu.AddDefaultBinding(InputControlType.Menu);
		this.Menu.AddDefaultBinding(InputControlType.Options);
		this.Menu.AddDefaultBinding(InputControlType.Start);
		this.Menu.AddDefaultBinding(InputControlType.Plus);
		this.Inventory.AddDefaultBinding(InputControlType.DPadUp);
		this.Scoreboard.AddDefaultBinding(InputControlType.View);
		this.Scoreboard.AddDefaultBinding(InputControlType.TouchPadButton);
		this.Scoreboard.AddDefaultBinding(InputControlType.Back);
		this.GodAlternate.AddDefaultBinding(InputControlType.Action4);
		this.TeleportAlternate.AddDefaultBinding(InputControlType.Action1);
		this.QuickMenu.AddDefaultBinding(InputControlType.DPadDown);
		this.CameraFunction.AddDefaultBinding(InputControlType.RightStickButton);
		this.ControllerRebindableActions.Clear();
		this.ControllerRebindableActions.Add(this.Jump);
		this.ControllerRebindableActions.Add(this.ToggleCrouch);
		this.ControllerRebindableActions.Add(this.Reload);
		this.ControllerRebindableActions.Add(this.Activate);
		this.ControllerRebindableActions.Add(this.Run);
		this.ControllerRebindableActions.Add(this.Primary);
		this.ControllerRebindableActions.Add(this.Secondary);
		this.ControllerRebindableActions.Add(this.Inventory);
		this.ControllerRebindableActions.Add(this.Swap);
		this.ControllerRebindableActions.Add(this.QuickMenu);
		this.ControllerRebindableActions.Add(this.CameraFunction);
		this.ControllerRebindableActions.Add(this.CameraChange);
		this.ControllerRebindableActions.Add(this.ToggleFlashlight);
		this.ControllerRebindableActions.Add(this.Drop);
		this.ControllerRebindableActions.Add(this.InventorySlotRight);
		this.ControllerRebindableActions.Add(this.InventorySlotLeft);
		this.ControllerRebindableActions.Add(this.InventorySlot1);
		this.ControllerRebindableActions.Add(this.InventorySlot2);
		this.ControllerRebindableActions.Add(this.InventorySlot3);
		this.ControllerRebindableActions.Add(this.InventorySlot4);
		this.ControllerRebindableActions.Add(this.InventorySlot5);
		this.ControllerRebindableActions.Add(this.InventorySlot6);
		this.ControllerRebindableActions.Add(this.InventorySlot7);
		this.ControllerRebindableActions.Add(this.InventorySlot8);
		this.ControllerRebindableActions.Add(this.InventorySlot9);
		this.ControllerRebindableActions.Add(this.InventorySlot10);
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x0011D354 File Offset: 0x0011B554
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultKeyboardBindings()
	{
		base.ListenOptions.IncludeKeys = true;
		base.ListenOptions.IncludeMouseButtons = true;
		base.ListenOptions.IncludeMouseScrollWheel = true;
		this.MoveLeft.AddDefaultBinding(new Key[] { Key.A });
		this.MoveRight.AddDefaultBinding(new Key[] { Key.D });
		this.MoveForward.AddDefaultBinding(new Key[] { Key.W });
		this.MoveBack.AddDefaultBinding(new Key[] { Key.S });
		this.LookLeft.AddDefaultBinding(Mouse.NegativeX);
		this.LookRight.AddDefaultBinding(Mouse.PositiveX);
		this.LookUp.AddDefaultBinding(Mouse.PositiveY);
		this.LookDown.AddDefaultBinding(Mouse.NegativeY);
		this.Run.AddDefaultBinding(new Key[] { Key.LeftShift });
		this.Jump.AddDefaultBinding(new Key[] { Key.Space });
		this.Crouch.AddDefaultBinding(new Key[] { Key.C });
		this.ToggleCrouch.AddDefaultBinding(new Key[] { Key.LeftControl });
		this.Drop.AddDefaultBinding(new Key[] { Key.G });
		this.God.AddDefaultBinding(new Key[] { Key.Q });
		this.Fly.AddDefaultBinding(new Key[] { Key.H });
		this.Invisible.AddDefaultBinding(new Key[] { Key.PadDivide });
		this.IncSpeed.AddDefaultBinding(new Key[]
		{
			Key.Shift,
			Key.Equals
		});
		this.DecSpeed.AddDefaultBinding(new Key[]
		{
			Key.Shift,
			Key.Minus
		});
		this.CameraFunction.AddDefaultBinding(Mouse.MiddleButton);
		this.Primary.AddDefaultBinding(Mouse.LeftButton);
		this.Secondary.AddDefaultBinding(Mouse.RightButton);
		this.SelectionFill.AddDefaultBinding(new Key[] { Key.L });
		this.SelectionClear.AddDefaultBinding(new Key[] { Key.J });
		this.SelectionSet.AddDefaultBinding(new Key[] { Key.Z });
		this.SelectionRotate.AddDefaultBinding(new Key[] { Key.X });
		this.SelectionDelete.AddDefaultBinding(new Key[] { Key.Backspace });
		this.SelectionMoveMode.AddDefaultBinding(new Key[] { Key.Insert });
		this.FocusCopyBlock.AddDefaultBinding(Mouse.MiddleButton);
		this.Prefab.AddDefaultBinding(new Key[] { Key.K });
		this.DetachCamera.AddDefaultBinding(new Key[] { Key.P });
		this.ToggleDCMove.AddDefaultBinding(new Key[] { Key.LeftBracket });
		this.LockFreeCamera.AddDefaultBinding(new Key[] { Key.Pad1 });
		this.DensityM1.AddDefaultBinding(new Key[] { Key.RightArrow });
		this.DensityP1.AddDefaultBinding(new Key[] { Key.LeftArrow });
		this.DensityM10.AddDefaultBinding(new Key[] { Key.UpArrow });
		this.DensityP10.AddDefaultBinding(new Key[] { Key.DownArrow });
		this.ScrollUp.AddDefaultBinding(Mouse.PositiveScrollWheel);
		this.ScrollDown.AddDefaultBinding(Mouse.NegativeScrollWheel);
		this.Menu.AddDefaultBinding(new Key[] { Key.Escape });
		this.InventorySlot1.AddDefaultBinding(new Key[] { Key.Key1 });
		this.InventorySlot2.AddDefaultBinding(new Key[] { Key.Key2 });
		this.InventorySlot3.AddDefaultBinding(new Key[] { Key.Key3 });
		this.InventorySlot4.AddDefaultBinding(new Key[] { Key.Key4 });
		this.InventorySlot5.AddDefaultBinding(new Key[] { Key.Key5 });
		this.InventorySlot6.AddDefaultBinding(new Key[] { Key.Key6 });
		this.InventorySlot7.AddDefaultBinding(new Key[] { Key.Key7 });
		this.InventorySlot8.AddDefaultBinding(new Key[] { Key.Key8 });
		this.InventorySlot9.AddDefaultBinding(new Key[] { Key.Key9 });
		this.InventorySlot10.AddDefaultBinding(new Key[] { Key.Key0 });
		this.InventorySlotRight.AddDefaultBinding(Mouse.NegativeScrollWheel);
		this.InventorySlotLeft.AddDefaultBinding(Mouse.PositiveScrollWheel);
		this.AiFreeze.AddDefaultBinding(new Key[] { Key.PadMultiply });
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x0011D78D File Offset: 0x0011B98D
	public void SetDeadzones(float _left, float _right)
	{
		this.leftStickDeadzone = _left;
		this.rightStickDeadzone = _right;
		this.UpdateDeadzones();
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x0011D7A3 File Offset: 0x0011B9A3
	public void UpdateDeadzones()
	{
		InputManager.ActiveDevice.LeftStick.LowerDeadZone = this.leftStickDeadzone;
		InputManager.ActiveDevice.RightStick.LowerDeadZone = this.rightStickDeadzone;
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x0011D7CF File Offset: 0x0011B9CF
	public void SetJoyStickLayout(eControllerJoystickLayout layout)
	{
		this.joystickLayout = layout;
		this.ConfigureJoystickLayout();
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x0011D7E0 File Offset: 0x0011B9E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ConfigureJoystickLayout()
	{
		switch (this.joystickLayout)
		{
		case eControllerJoystickLayout.Standard:
			this.MoveForward.AddDefaultBinding(InputControlType.LeftStickUp);
			this.MoveForward.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickUp));
			this.MoveBack.AddDefaultBinding(InputControlType.LeftStickDown);
			this.MoveBack.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickDown));
			this.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
			this.MoveLeft.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickLeft));
			this.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
			this.MoveRight.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickRight));
			this.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
			this.LookUp.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickUp));
			this.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
			this.LookDown.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickDown));
			this.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			this.LookLeft.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickLeft));
			this.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
			this.LookRight.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickRight));
			return;
		case eControllerJoystickLayout.Southpaw:
			this.MoveForward.AddDefaultBinding(InputControlType.RightStickUp);
			this.MoveForward.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickUp));
			this.MoveBack.AddDefaultBinding(InputControlType.RightStickDown);
			this.MoveBack.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickDown));
			this.MoveLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			this.MoveLeft.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickLeft));
			this.MoveRight.AddDefaultBinding(InputControlType.RightStickRight);
			this.MoveRight.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickRight));
			this.LookUp.AddDefaultBinding(InputControlType.LeftStickUp);
			this.LookUp.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickUp));
			this.LookDown.AddDefaultBinding(InputControlType.LeftStickDown);
			this.LookDown.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickDown));
			this.LookLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
			this.LookLeft.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickLeft));
			this.LookRight.AddDefaultBinding(InputControlType.LeftStickRight);
			this.LookRight.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickRight));
			return;
		case eControllerJoystickLayout.Legacy:
			this.MoveForward.AddDefaultBinding(InputControlType.LeftStickUp);
			this.MoveForward.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickUp));
			this.MoveBack.AddDefaultBinding(InputControlType.LeftStickDown);
			this.MoveBack.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickDown));
			this.MoveLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			this.MoveLeft.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickLeft));
			this.MoveRight.AddDefaultBinding(InputControlType.RightStickRight);
			this.MoveRight.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickRight));
			this.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
			this.LookUp.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickUp));
			this.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
			this.LookDown.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickDown));
			this.LookLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
			this.LookLeft.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickLeft));
			this.LookRight.AddDefaultBinding(InputControlType.LeftStickRight);
			this.LookRight.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickRight));
			return;
		case eControllerJoystickLayout.LegacySouthpaw:
			this.MoveForward.AddDefaultBinding(InputControlType.RightStickUp);
			this.MoveForward.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickUp));
			this.MoveBack.AddDefaultBinding(InputControlType.RightStickDown);
			this.MoveBack.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickDown));
			this.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
			this.MoveLeft.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickLeft));
			this.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
			this.MoveRight.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickRight));
			this.LookUp.AddDefaultBinding(InputControlType.LeftStickUp);
			this.LookUp.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickUp));
			this.LookDown.AddDefaultBinding(InputControlType.LeftStickDown);
			this.LookDown.RemoveBinding(new DeviceBindingSource(InputControlType.RightStickDown));
			this.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			this.LookLeft.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickLeft));
			this.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
			this.LookRight.RemoveBinding(new DeviceBindingSource(InputControlType.LeftStickRight));
			return;
		default:
			return;
		}
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x0011DBB8 File Offset: 0x0011BDB8
	public bool AnyGUIActionPressed()
	{
		using (IEnumerator<PlayerAction> enumerator = this.GUIActions.Actions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.WasPressed)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040022BF RID: 8895
	public PlayerTwoAxisAction Move;

	// Token: 0x040022C0 RID: 8896
	public PlayerAction MoveLeft;

	// Token: 0x040022C1 RID: 8897
	public PlayerAction MoveRight;

	// Token: 0x040022C2 RID: 8898
	public PlayerAction MoveForward;

	// Token: 0x040022C3 RID: 8899
	public PlayerAction MoveBack;

	// Token: 0x040022C4 RID: 8900
	public PlayerTwoAxisAction Look;

	// Token: 0x040022C5 RID: 8901
	public PlayerAction LookLeft;

	// Token: 0x040022C6 RID: 8902
	public PlayerAction LookRight;

	// Token: 0x040022C7 RID: 8903
	public PlayerAction LookUp;

	// Token: 0x040022C8 RID: 8904
	public PlayerAction LookDown;

	// Token: 0x040022C9 RID: 8905
	public PlayerAction Run;

	// Token: 0x040022CA RID: 8906
	public PlayerAction Jump;

	// Token: 0x040022CB RID: 8907
	public PlayerAction Crouch;

	// Token: 0x040022CC RID: 8908
	public PlayerOneAxisAction JumpCrouch;

	// Token: 0x040022CD RID: 8909
	public PlayerAction ToggleCrouch;

	// Token: 0x040022CE RID: 8910
	public PlayerAction Activate;

	// Token: 0x040022CF RID: 8911
	public PlayerAction Drop;

	// Token: 0x040022D0 RID: 8912
	public PlayerAction Swap;

	// Token: 0x040022D1 RID: 8913
	public PlayerAction Reload;

	// Token: 0x040022D2 RID: 8914
	public PlayerAction Primary;

	// Token: 0x040022D3 RID: 8915
	public PlayerAction Secondary;

	// Token: 0x040022D4 RID: 8916
	public PlayerAction ToggleFlashlight;

	// Token: 0x040022D5 RID: 8917
	public PlayerAction God;

	// Token: 0x040022D6 RID: 8918
	public PlayerAction Fly;

	// Token: 0x040022D7 RID: 8919
	public PlayerAction Invisible;

	// Token: 0x040022D8 RID: 8920
	public PlayerAction IncSpeed;

	// Token: 0x040022D9 RID: 8921
	public PlayerAction DecSpeed;

	// Token: 0x040022DA RID: 8922
	public PlayerAction GodAlternate;

	// Token: 0x040022DB RID: 8923
	public PlayerAction TeleportAlternate;

	// Token: 0x040022DC RID: 8924
	public PlayerAction CameraChange;

	// Token: 0x040022DD RID: 8925
	public PlayerAction CameraFunction;

	// Token: 0x040022DE RID: 8926
	public PlayerAction SelectionFill;

	// Token: 0x040022DF RID: 8927
	public PlayerAction SelectionClear;

	// Token: 0x040022E0 RID: 8928
	public PlayerAction SelectionSet;

	// Token: 0x040022E1 RID: 8929
	public PlayerAction SelectionRotate;

	// Token: 0x040022E2 RID: 8930
	public PlayerAction SelectionDelete;

	// Token: 0x040022E3 RID: 8931
	public PlayerAction SelectionMoveMode;

	// Token: 0x040022E4 RID: 8932
	public PlayerAction FocusCopyBlock;

	// Token: 0x040022E5 RID: 8933
	public PlayerAction Prefab;

	// Token: 0x040022E6 RID: 8934
	public PlayerAction DetachCamera;

	// Token: 0x040022E7 RID: 8935
	public PlayerAction ToggleDCMove;

	// Token: 0x040022E8 RID: 8936
	public PlayerAction LockFreeCamera;

	// Token: 0x040022E9 RID: 8937
	public PlayerAction DensityM1;

	// Token: 0x040022EA RID: 8938
	public PlayerAction DensityP1;

	// Token: 0x040022EB RID: 8939
	public PlayerAction DensityM10;

	// Token: 0x040022EC RID: 8940
	public PlayerAction DensityP10;

	// Token: 0x040022ED RID: 8941
	public PlayerAction ScrollUp;

	// Token: 0x040022EE RID: 8942
	public PlayerAction ScrollDown;

	// Token: 0x040022EF RID: 8943
	public PlayerOneAxisAction Scroll;

	// Token: 0x040022F0 RID: 8944
	public PlayerAction Menu;

	// Token: 0x040022F1 RID: 8945
	public PlayerAction Inventory;

	// Token: 0x040022F2 RID: 8946
	public PlayerAction Scoreboard;

	// Token: 0x040022F3 RID: 8947
	public PlayerAction InventorySlot1;

	// Token: 0x040022F4 RID: 8948
	public PlayerAction InventorySlot2;

	// Token: 0x040022F5 RID: 8949
	public PlayerAction InventorySlot3;

	// Token: 0x040022F6 RID: 8950
	public PlayerAction InventorySlot4;

	// Token: 0x040022F7 RID: 8951
	public PlayerAction InventorySlot5;

	// Token: 0x040022F8 RID: 8952
	public PlayerAction InventorySlot6;

	// Token: 0x040022F9 RID: 8953
	public PlayerAction InventorySlot7;

	// Token: 0x040022FA RID: 8954
	public PlayerAction InventorySlot8;

	// Token: 0x040022FB RID: 8955
	public PlayerAction InventorySlot9;

	// Token: 0x040022FC RID: 8956
	public PlayerAction InventorySlot10;

	// Token: 0x040022FD RID: 8957
	public PlayerAction InventorySlotLeft;

	// Token: 0x040022FE RID: 8958
	public PlayerAction InventorySlotRight;

	// Token: 0x040022FF RID: 8959
	public PlayerAction AiFreeze;

	// Token: 0x04002300 RID: 8960
	public PlayerAction QuickMenu;

	// Token: 0x04002301 RID: 8961
	public float leftStickDeadzone = 0.1f;

	// Token: 0x04002302 RID: 8962
	public float rightStickDeadzone = 0.1f;

	// Token: 0x04002303 RID: 8963
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PlayerAction> InventoryActions = new List<PlayerAction>();

	// Token: 0x04002304 RID: 8964
	public eControllerJoystickLayout joystickLayout;
}
