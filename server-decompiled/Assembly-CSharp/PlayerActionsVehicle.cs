using System;
using InControl;

// Token: 0x02000565 RID: 1381
public class PlayerActionsVehicle : PlayerActionsBase
{
	// Token: 0x06002CE2 RID: 11490 RVA: 0x0011E375 File Offset: 0x0011C575
	public PlayerActionsVehicle()
	{
		base.Name = "vehicle";
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x0011E388 File Offset: 0x0011C588
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

	// Token: 0x06002CE4 RID: 11492 RVA: 0x0011E864 File Offset: 0x0011CA64
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

	// Token: 0x06002CE5 RID: 11493 RVA: 0x0011EABC File Offset: 0x0011CCBC
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

	// Token: 0x0400231F RID: 8991
	public PlayerTwoAxisAction Move;

	// Token: 0x04002320 RID: 8992
	public PlayerAction MoveLeft;

	// Token: 0x04002321 RID: 8993
	public PlayerAction MoveRight;

	// Token: 0x04002322 RID: 8994
	public PlayerAction MoveForward;

	// Token: 0x04002323 RID: 8995
	public PlayerAction MoveBack;

	// Token: 0x04002324 RID: 8996
	public PlayerTwoAxisAction Look;

	// Token: 0x04002325 RID: 8997
	public PlayerAction LookLeft;

	// Token: 0x04002326 RID: 8998
	public PlayerAction LookRight;

	// Token: 0x04002327 RID: 8999
	public PlayerAction LookUp;

	// Token: 0x04002328 RID: 9000
	public PlayerAction LookDown;

	// Token: 0x04002329 RID: 9001
	public PlayerAction Turbo;

	// Token: 0x0400232A RID: 9002
	public PlayerAction Brake;

	// Token: 0x0400232B RID: 9003
	public PlayerAction Hop;

	// Token: 0x0400232C RID: 9004
	public PlayerAction Activate;

	// Token: 0x0400232D RID: 9005
	public PlayerAction ToggleFlashlight;

	// Token: 0x0400232E RID: 9006
	public PlayerAction HonkHorn;

	// Token: 0x0400232F RID: 9007
	public PlayerAction Menu;

	// Token: 0x04002330 RID: 9008
	public PlayerAction Inventory;

	// Token: 0x04002331 RID: 9009
	public PlayerAction Scoreboard;

	// Token: 0x04002332 RID: 9010
	public PlayerAction ToggleTurnMode;

	// Token: 0x04002333 RID: 9011
	public PlayerAction ScrollUp;

	// Token: 0x04002334 RID: 9012
	public PlayerAction ScrollDown;

	// Token: 0x04002335 RID: 9013
	public PlayerOneAxisAction Scroll;

	// Token: 0x04002336 RID: 9014
	public PlayerTwoAxisAction LeftStick;

	// Token: 0x04002337 RID: 9015
	public PlayerAction LeftStickLeft;

	// Token: 0x04002338 RID: 9016
	public PlayerAction LeftStickRight;

	// Token: 0x04002339 RID: 9017
	public PlayerAction LeftStickForward;

	// Token: 0x0400233A RID: 9018
	public PlayerAction LeftStickBack;
}
