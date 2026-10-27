using System;
using InControl;

// Token: 0x0200056F RID: 1391
public class PlayerActionsVehicle : PlayerActionsBase
{
	// Token: 0x06002CE0 RID: 11488 RVA: 0x0011F785 File Offset: 0x0011D985
	public PlayerActionsVehicle()
	{
		base.Name = "vehicle";
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x0011F798 File Offset: 0x0011D998
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

	// Token: 0x06002CE2 RID: 11490 RVA: 0x0011FC74 File Offset: 0x0011DE74
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

	// Token: 0x06002CE3 RID: 11491 RVA: 0x0011FECC File Offset: 0x0011E0CC
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

	// Token: 0x04002340 RID: 9024
	public PlayerTwoAxisAction Move;

	// Token: 0x04002341 RID: 9025
	public PlayerAction MoveLeft;

	// Token: 0x04002342 RID: 9026
	public PlayerAction MoveRight;

	// Token: 0x04002343 RID: 9027
	public PlayerAction MoveForward;

	// Token: 0x04002344 RID: 9028
	public PlayerAction MoveBack;

	// Token: 0x04002345 RID: 9029
	public PlayerTwoAxisAction Look;

	// Token: 0x04002346 RID: 9030
	public PlayerAction LookLeft;

	// Token: 0x04002347 RID: 9031
	public PlayerAction LookRight;

	// Token: 0x04002348 RID: 9032
	public PlayerAction LookUp;

	// Token: 0x04002349 RID: 9033
	public PlayerAction LookDown;

	// Token: 0x0400234A RID: 9034
	public PlayerAction Turbo;

	// Token: 0x0400234B RID: 9035
	public PlayerAction Brake;

	// Token: 0x0400234C RID: 9036
	public PlayerAction Hop;

	// Token: 0x0400234D RID: 9037
	public PlayerAction Activate;

	// Token: 0x0400234E RID: 9038
	public PlayerAction ToggleFlashlight;

	// Token: 0x0400234F RID: 9039
	public PlayerAction HonkHorn;

	// Token: 0x04002350 RID: 9040
	public PlayerAction Menu;

	// Token: 0x04002351 RID: 9041
	public PlayerAction Inventory;

	// Token: 0x04002352 RID: 9042
	public PlayerAction Scoreboard;

	// Token: 0x04002353 RID: 9043
	public PlayerAction ToggleTurnMode;

	// Token: 0x04002354 RID: 9044
	public PlayerAction ScrollUp;

	// Token: 0x04002355 RID: 9045
	public PlayerAction ScrollDown;

	// Token: 0x04002356 RID: 9046
	public PlayerOneAxisAction Scroll;

	// Token: 0x04002357 RID: 9047
	public PlayerTwoAxisAction LeftStick;

	// Token: 0x04002358 RID: 9048
	public PlayerAction LeftStickLeft;

	// Token: 0x04002359 RID: 9049
	public PlayerAction LeftStickRight;

	// Token: 0x0400235A RID: 9050
	public PlayerAction LeftStickForward;

	// Token: 0x0400235B RID: 9051
	public PlayerAction LeftStickBack;
}
