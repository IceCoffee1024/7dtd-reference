using System;
using InControl;

// Token: 0x02000560 RID: 1376
public class PlayerActionsVehicle : PlayerActionsBase
{
	// Token: 0x06002CB8 RID: 11448 RVA: 0x0011D301 File Offset: 0x0011B501
	public PlayerActionsVehicle()
	{
		base.Name = "vehicle";
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x0011D314 File Offset: 0x0011B514
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateActions()
	{
		this.MoveForward = base.CreatePlayerAction("Forward");
		this.MoveForward.UserData = new PlayerActionData.ActionUserData("inpActVehicleMoveForwardName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.MoveBack = base.CreatePlayerAction("Back");
		this.MoveBack.UserData = new PlayerActionData.ActionUserData("inpActVehicleMoveBackName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.MoveLeft = base.CreatePlayerAction("Left");
		this.MoveLeft.UserData = new PlayerActionData.ActionUserData("inpActVehicleMoveLeftName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.MoveRight = base.CreatePlayerAction("Right");
		this.MoveRight.UserData = new PlayerActionData.ActionUserData("inpActVehicleMoveRightName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Move = base.CreateTwoAxisPlayerAction(this.MoveLeft, this.MoveRight, this.MoveBack, this.MoveForward);
		this.LookLeft = base.CreatePlayerAction("LookLeft");
		this.LookLeft.UserData = new PlayerActionData.ActionUserData("inpActVehicleLookName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.LookRight = base.CreatePlayerAction("LookRight");
		this.LookUp = base.CreatePlayerAction("LookUp");
		this.LookDown = base.CreatePlayerAction("LookDown");
		this.Look = base.CreateTwoAxisPlayerAction(this.LookLeft, this.LookRight, this.LookDown, this.LookUp);
		this.Turbo = base.CreatePlayerAction("Run");
		this.Turbo.UserData = new PlayerActionData.ActionUserData("inpActVehicleTurboName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Brake = base.CreatePlayerAction("Jump");
		this.Brake.UserData = new PlayerActionData.ActionUserData("inpActVehicleBrakeName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Hop = base.CreatePlayerAction("Crouch");
		this.Hop.UserData = new PlayerActionData.ActionUserData("inpActVehicleHopName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Activate = base.CreatePlayerAction("Activate");
		this.Activate.UserData = new PlayerActionData.ActionUserData("inpActActivateName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Activate.FirstRepeatDelay = 0.3f;
		this.ToggleFlashlight = base.CreatePlayerAction("ToggleFlashlight");
		this.ToggleFlashlight.UserData = new PlayerActionData.ActionUserData("inpActVehicleToggleLightName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.HonkHorn = base.CreatePlayerAction("HonkHorn");
		this.HonkHorn.UserData = new PlayerActionData.ActionUserData("inpActHonkHornName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Menu = base.CreatePlayerAction("Menu");
		this.Menu.UserData = new PlayerActionData.ActionUserData("inpActMenuName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Inventory = base.CreatePlayerAction("Inventory");
		this.Inventory.UserData = new PlayerActionData.ActionUserData("inpActInventoryName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Scoreboard = base.CreatePlayerAction("Scoreboard");
		this.Scoreboard.UserData = new PlayerActionData.ActionUserData("inpActScoreboardName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.ToggleTurnMode = base.CreatePlayerAction("ToggleTurnMode");
		this.ToggleTurnMode.UserData = new PlayerActionData.ActionUserData("inpActToggleTurnMode", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.ScrollUp = base.CreatePlayerAction("ScrollUp");
		this.ScrollUp.UserData = new PlayerActionData.ActionUserData("inpActCameraZoomInName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.ScrollDown = base.CreatePlayerAction("ScrollDown");
		this.ScrollDown.UserData = new PlayerActionData.ActionUserData("inpActCameraZoomOutName", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.Scroll = base.CreateOneAxisPlayerAction(this.ScrollDown, this.ScrollUp);
		this.LeftStickLeft = base.CreatePlayerAction("LeftStickLeft");
		this.LeftStickLeft.UserData = new PlayerActionData.ActionUserData("inpActVehicleLeftStickLeft", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.LeftStickRight = base.CreatePlayerAction("LeftStickRight");
		this.LeftStickRight.UserData = new PlayerActionData.ActionUserData("inpActVehicleLeftStickRight", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.LeftStickForward = base.CreatePlayerAction("LeftStickForward");
		this.LeftStickForward.UserData = new PlayerActionData.ActionUserData("inpActVehicleLeftStickForward", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.LeftStickBack = base.CreatePlayerAction("LeftStickBack");
		this.LeftStickBack.UserData = new PlayerActionData.ActionUserData("inpActVehicleLeftStickBack", null, PlayerActionData.GroupVehicle, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.LeftStick = base.CreateTwoAxisPlayerAction(this.LeftStickLeft, this.LeftStickRight, this.LeftStickBack, this.LeftStickForward);
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x0011D7F0 File Offset: 0x0011B9F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
		base.ListenOptions.IncludeControllers = true;
		this.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.MoveForward.AddDefaultBinding(InputControlType.RightTrigger);
		this.MoveBack.AddDefaultBinding(InputControlType.LeftTrigger);
		this.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		this.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
		this.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
		this.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
		this.Turbo.AddDefaultBinding(InputControlType.RightBumper);
		this.Brake.AddDefaultBinding(InputControlType.Action3);
		this.Hop.AddDefaultBinding(InputControlType.Action1);
		this.Activate.AddDefaultBinding(InputControlType.Action4);
		this.ToggleFlashlight.AddDefaultBinding(InputControlType.DPadLeft);
		this.HonkHorn.AddDefaultBinding(InputControlType.LeftBumper);
		this.Menu.AddDefaultBinding(InputControlType.Menu);
		this.Menu.AddDefaultBinding(InputControlType.Options);
		this.Menu.AddDefaultBinding(InputControlType.Start);
		this.Inventory.AddDefaultBinding(InputControlType.Action2);
		this.Scoreboard.AddDefaultBinding(InputControlType.View);
		this.Scoreboard.AddDefaultBinding(InputControlType.TouchPadButton);
		this.Scoreboard.AddDefaultBinding(InputControlType.Back);
		this.ToggleTurnMode.AddDefaultBinding(InputControlType.RightStickButton);
		this.ScrollUp.AddDefaultBinding(InputControlType.DPadUp);
		this.ScrollDown.AddDefaultBinding(InputControlType.DPadDown);
		this.LeftStickLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.LeftStickRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.LeftStickForward.AddDefaultBinding(InputControlType.LeftStickUp);
		this.LeftStickBack.AddDefaultBinding(InputControlType.LeftStickDown);
		this.ControllerRebindableActions.Clear();
		this.ControllerRebindableActions.Add(this.MoveForward);
		this.ControllerRebindableActions.Add(this.MoveBack);
		this.ControllerRebindableActions.Add(this.Hop);
		this.ControllerRebindableActions.Add(this.Brake);
		this.ControllerRebindableActions.Add(this.Inventory);
		this.ControllerRebindableActions.Add(this.Activate);
		this.ControllerRebindableActions.Add(this.Turbo);
		this.ControllerRebindableActions.Add(this.HonkHorn);
		this.ControllerRebindableActions.Add(this.ToggleFlashlight);
		this.ControllerRebindableActions.Add(this.ToggleTurnMode);
		this.ControllerRebindableActions.Add(this.ScrollUp);
		this.ControllerRebindableActions.Add(this.ScrollDown);
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x0011DA48 File Offset: 0x0011BC48
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
		this.Turbo.AddDefaultBinding(new Key[] { Key.LeftShift });
		this.Brake.AddDefaultBinding(new Key[] { Key.Space });
		this.Hop.AddDefaultBinding(new Key[] { Key.C });
		this.HonkHorn.AddDefaultBinding(new Key[] { Key.X });
		this.Menu.AddDefaultBinding(new Key[] { Key.Escape });
		this.ToggleTurnMode.AddDefaultBinding(Mouse.LeftButton);
		this.ScrollUp.AddDefaultBinding(Mouse.PositiveScrollWheel);
		this.ScrollDown.AddDefaultBinding(Mouse.NegativeScrollWheel);
	}

	// Token: 0x040022ED RID: 8941
	public PlayerTwoAxisAction Move;

	// Token: 0x040022EE RID: 8942
	public PlayerAction MoveLeft;

	// Token: 0x040022EF RID: 8943
	public PlayerAction MoveRight;

	// Token: 0x040022F0 RID: 8944
	public PlayerAction MoveForward;

	// Token: 0x040022F1 RID: 8945
	public PlayerAction MoveBack;

	// Token: 0x040022F2 RID: 8946
	public PlayerTwoAxisAction Look;

	// Token: 0x040022F3 RID: 8947
	public PlayerAction LookLeft;

	// Token: 0x040022F4 RID: 8948
	public PlayerAction LookRight;

	// Token: 0x040022F5 RID: 8949
	public PlayerAction LookUp;

	// Token: 0x040022F6 RID: 8950
	public PlayerAction LookDown;

	// Token: 0x040022F7 RID: 8951
	public PlayerAction Turbo;

	// Token: 0x040022F8 RID: 8952
	public PlayerAction Brake;

	// Token: 0x040022F9 RID: 8953
	public PlayerAction Hop;

	// Token: 0x040022FA RID: 8954
	public PlayerAction Activate;

	// Token: 0x040022FB RID: 8955
	public PlayerAction ToggleFlashlight;

	// Token: 0x040022FC RID: 8956
	public PlayerAction HonkHorn;

	// Token: 0x040022FD RID: 8957
	public PlayerAction Menu;

	// Token: 0x040022FE RID: 8958
	public PlayerAction Inventory;

	// Token: 0x040022FF RID: 8959
	public PlayerAction Scoreboard;

	// Token: 0x04002300 RID: 8960
	public PlayerAction ToggleTurnMode;

	// Token: 0x04002301 RID: 8961
	public PlayerAction ScrollUp;

	// Token: 0x04002302 RID: 8962
	public PlayerAction ScrollDown;

	// Token: 0x04002303 RID: 8963
	public PlayerOneAxisAction Scroll;

	// Token: 0x04002304 RID: 8964
	public PlayerTwoAxisAction LeftStick;

	// Token: 0x04002305 RID: 8965
	public PlayerAction LeftStickLeft;

	// Token: 0x04002306 RID: 8966
	public PlayerAction LeftStickRight;

	// Token: 0x04002307 RID: 8967
	public PlayerAction LeftStickForward;

	// Token: 0x04002308 RID: 8968
	public PlayerAction LeftStickBack;
}
