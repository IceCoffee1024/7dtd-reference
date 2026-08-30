using System;
using InControl;

// Token: 0x0200055C RID: 1372
public class PlayerActionsGUI : PlayerActionsBase
{
	// Token: 0x06002CA0 RID: 11424 RVA: 0x0011A4DF File Offset: 0x001186DF
	public PlayerActionsGUI()
	{
		base.Name = "gui";
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x0011A4F4 File Offset: 0x001186F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateActions()
	{
		this.Left = base.CreatePlayerAction("GUI Left");
		this.Left.UserData = new PlayerActionData.ActionUserData("inpActGuiCursor", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Right = base.CreatePlayerAction("GUI Right");
		this.Right.UserData = new PlayerActionData.ActionUserData("inpActGuiCursor", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.None, true, false, false, true);
		this.Up = base.CreatePlayerAction("GUI Up");
		this.Up.UserData = new PlayerActionData.ActionUserData("inpActGuiCursor", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.None, true, false, false, true);
		this.Down = base.CreatePlayerAction("GUI Down");
		this.Down.UserData = new PlayerActionData.ActionUserData("inpActGuiCursor", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.None, true, false, false, true);
		this.Look = base.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
		this.LeftClick = base.CreatePlayerAction("GUI Left Click");
		this.LeftClick.UserData = new PlayerActionData.ActionUserData("inpActGuiLeftclick", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.RightClick = base.CreatePlayerAction("GUI RightClick");
		this.RightClick.UserData = new PlayerActionData.ActionUserData("inpActGuiRightclick", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.MiddleClick = base.CreatePlayerAction("GUI Middle Click");
		this.MiddleClick.UserData = new PlayerActionData.ActionUserData("inpActGuiMiddleclick", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.Submit = base.CreatePlayerAction("GUI Submit");
		this.Submit.UserData = new PlayerActionData.ActionUserData("inpActUiSelectTakeFullName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Cancel = base.CreatePlayerAction("GUI Cancel");
		this.Cancel.UserData = new PlayerActionData.ActionUserData("inpActUiCancelName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.HalfStack = base.CreatePlayerAction("GUI HalfStack");
		this.HalfStack.UserData = new PlayerActionData.ActionUserData("inpActTakeHalfName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Inspect = base.CreatePlayerAction("GUI Inspect");
		this.Inspect.UserData = new PlayerActionData.ActionUserData("inpActInspectName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.FocusSearch = base.CreatePlayerAction("GUI FocusSearch");
		this.FocusSearch.UserData = new PlayerActionData.ActionUserData("inpActFocusSearchName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.DPad_Left = base.CreatePlayerAction("GUI D-Pad Left");
		this.DPad_Left.UserData = new PlayerActionData.ActionUserData("inpActActionHotkey1Name", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.DPad_Right = base.CreatePlayerAction("GUI D-Pad Right");
		this.DPad_Right.UserData = new PlayerActionData.ActionUserData("inpActActionHotkey2Name", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.DPad_Up = base.CreatePlayerAction("GUI D-Pad Up");
		this.DPad_Up.UserData = new PlayerActionData.ActionUserData("inpActActionHotkey3Name", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.DPad_Down = base.CreatePlayerAction("GUI D-Pad Down");
		this.DPad_Down.UserData = new PlayerActionData.ActionUserData("inpActActionHotkey4Name", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.CameraLeft = base.CreatePlayerAction("GUI Camera Left");
		this.CameraLeft.UserData = new PlayerActionData.ActionUserData("inpActPageDownName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.CameraRight = base.CreatePlayerAction("GUI Camera Right");
		this.CameraRight.UserData = new PlayerActionData.ActionUserData("inpActPageUpName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.CameraUp = base.CreatePlayerAction("GUI Camera Up");
		this.CameraUp.UserData = new PlayerActionData.ActionUserData("inpActZoomInName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.CameraDown = base.CreatePlayerAction("GUI Camera Down");
		this.CameraDown.UserData = new PlayerActionData.ActionUserData("inpActZoomOutName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.Camera = base.CreateTwoAxisPlayerAction(this.CameraLeft, this.CameraRight, this.CameraDown, this.CameraUp);
		this.WindowPagingLeft = base.CreatePlayerAction("GUI Window Paging Up");
		this.WindowPagingLeft.UserData = new PlayerActionData.ActionUserData("inpActUiTabLeftName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.WindowPagingRight = base.CreatePlayerAction("GUI Window Paging Down");
		this.WindowPagingRight.UserData = new PlayerActionData.ActionUserData("inpActUiTabRightName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.BumperAxis = base.CreateOneAxisPlayerAction(this.WindowPagingRight, this.WindowPagingLeft);
		this.PageDown = base.CreatePlayerAction("GUI Page Down");
		this.PageDown.UserData = new PlayerActionData.ActionUserData("inpActCategoryLeftName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.PageUp = base.CreatePlayerAction("GUI Page Up");
		this.PageUp.UserData = new PlayerActionData.ActionUserData("inpActCategoryRightName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, true, false, false, true);
		this.TriggerAxis = base.CreateOneAxisPlayerAction(this.PageUp, this.PageDown);
		this.BackButton = base.CreatePlayerAction("GUI Back Button");
		this.BackButton.UserData = new PlayerActionData.ActionUserData("inpActBackButton", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.RightStick = base.CreatePlayerAction("GUI Window RightStick In");
		this.RightStick.UserData = new PlayerActionData.ActionUserData("inpActQuickMoveName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.LeftStick = base.CreatePlayerAction("GUI Window LeftStick In");
		this.LeftStick.UserData = new PlayerActionData.ActionUserData("inpActTakeAllName", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, true, false, false, true);
		this.NavUp = base.CreatePlayerAction("GUI Window Navigate Up");
		this.NavUp.UserData = new PlayerActionData.ActionUserData("inpActNavUp", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.NavDown = base.CreatePlayerAction("GUI Window Navigate Down");
		this.NavDown.UserData = new PlayerActionData.ActionUserData("inpActNavDown", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.NavLeft = base.CreatePlayerAction("GUI Window Navigate Left");
		this.NavLeft.UserData = new PlayerActionData.ActionUserData("inpActNavLeft", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.NavRight = base.CreatePlayerAction("GUI Window Navigate Right");
		this.NavRight.UserData = new PlayerActionData.ActionUserData("inpActNavRight", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.Nav = base.CreateTwoAxisPlayerAction(this.NavLeft, this.NavRight, this.NavDown, this.NavUp);
		this.scrollUp = base.CreatePlayerAction("GUI Scroll Up");
		this.scrollUp.UserData = new PlayerActionData.ActionUserData("inpScrollUp", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, false, false, true, true);
		this.scrollDown = base.CreatePlayerAction("GUI Scroll Down");
		this.scrollDown.UserData = new PlayerActionData.ActionUserData("inpscrollDown", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, false, false, true, true);
		this.scroll = base.CreateOneAxisPlayerAction(this.scrollDown, this.scrollUp);
		this.Apply = base.CreatePlayerAction("GUI Apply");
		this.Apply.UserData = new PlayerActionData.ActionUserData("inpActUiApply", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.ControllerOnly, false, false, true, true);
		this.ActionUp = base.CreatePlayerAction("GUI Action Up");
		this.ActionUp.UserData = new PlayerActionData.ActionUserData("inpActUp", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, false, false, true, true);
		this.ActionDown = base.CreatePlayerAction("GUI Action Down");
		this.ActionDown.UserData = new PlayerActionData.ActionUserData("inpActDown", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, false, false, true, true);
		this.ActionLeft = base.CreatePlayerAction("GUI Action Left");
		this.ActionLeft.UserData = new PlayerActionData.ActionUserData("inpActLeft", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, false, false, true, true);
		this.ActionRight = base.CreatePlayerAction("GUI Action Right");
		this.ActionRight.UserData = new PlayerActionData.ActionUserData("inpActRight", null, PlayerActionData.GroupUI, PlayerActionData.EAppliesToInputType.Both, false, false, true, true);
		this.Left.Raw = false;
		this.Right.Raw = false;
		this.Up.Raw = false;
		this.Down.Raw = false;
		this.CameraUp.Raw = false;
		this.CameraDown.Raw = false;
		this.CameraLeft.StateThreshold = 0.25f;
		this.CameraRight.StateThreshold = 0.25f;
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x0011AD64 File Offset: 0x00118F64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
		this.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.Right.AddDefaultBinding(InputControlType.LeftStickRight);
		this.Up.AddDefaultBinding(InputControlType.LeftStickUp);
		this.Down.AddDefaultBinding(InputControlType.LeftStickDown);
		this.DPad_Left.AddDefaultBinding(InputControlType.DPadLeft);
		this.DPad_Right.AddDefaultBinding(InputControlType.DPadRight);
		this.DPad_Up.AddDefaultBinding(InputControlType.DPadUp);
		this.DPad_Down.AddDefaultBinding(InputControlType.DPadDown);
		this.CameraLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		this.CameraRight.AddDefaultBinding(InputControlType.RightStickRight);
		this.CameraUp.AddDefaultBinding(InputControlType.RightStickUp);
		this.CameraDown.AddDefaultBinding(InputControlType.RightStickDown);
		this.Submit.AddDefaultBinding(InputControlType.Action1);
		this.Cancel.AddDefaultBinding(InputControlType.Action2);
		this.HalfStack.AddDefaultBinding(InputControlType.Action3);
		this.Inspect.AddDefaultBinding(InputControlType.Action4);
		this.Apply.AddDefaultBinding(InputControlType.Start);
		this.Apply.AddDefaultBinding(InputControlType.Menu);
		this.Apply.AddDefaultBinding(InputControlType.Options);
		this.Apply.AddDefaultBinding(InputControlType.Plus);
		this.WindowPagingLeft.AddDefaultBinding(InputControlType.LeftBumper);
		this.WindowPagingRight.AddDefaultBinding(InputControlType.RightBumper);
		this.PageUp.AddDefaultBinding(InputControlType.RightTrigger);
		this.PageDown.AddDefaultBinding(InputControlType.LeftTrigger);
		this.RightStick.AddDefaultBinding(InputControlType.RightStickButton);
		this.LeftStick.AddDefaultBinding(InputControlType.LeftStickButton);
		this.NavUp.AddDefaultBinding(InputControlType.DPadUp);
		this.NavDown.AddDefaultBinding(InputControlType.DPadDown);
		this.NavLeft.AddDefaultBinding(InputControlType.DPadLeft);
		this.NavRight.AddDefaultBinding(InputControlType.DPadRight);
		this.scrollUp.AddDefaultBinding(InputControlType.RightStickUp);
		this.scrollDown.AddDefaultBinding(InputControlType.RightStickDown);
		this.BackButton.AddDefaultBinding(InputControlType.Back);
		this.BackButton.AddDefaultBinding(InputControlType.View);
		this.BackButton.AddDefaultBinding(InputControlType.TouchPadButton);
		this.BackButton.AddDefaultBinding(InputControlType.Minus);
		this.BackButton.AddDefaultBinding(InputControlType.Select);
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x0011AF4C File Offset: 0x0011914C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultKeyboardBindings()
	{
		this.Left.AddDefaultBinding(new Key[] { Key.LeftArrow });
		this.Right.AddDefaultBinding(new Key[] { Key.RightArrow });
		this.Up.AddDefaultBinding(new Key[] { Key.UpArrow });
		this.Down.AddDefaultBinding(new Key[] { Key.DownArrow });
		this.DPad_Left.AddDefaultBinding(new Key[] { Key.A });
		this.DPad_Right.AddDefaultBinding(new Key[] { Key.S });
		this.DPad_Up.AddDefaultBinding(new Key[] { Key.W });
		this.DPad_Down.AddDefaultBinding(new Key[] { Key.D });
		this.Cancel.AddDefaultBinding(new Key[] { Key.Escape });
		this.FocusSearch.AddDefaultBinding(new Key[] { Key.F });
		this.LeftClick.AddDefaultBinding(Mouse.LeftButton);
		this.RightClick.AddDefaultBinding(Mouse.RightButton);
		this.MiddleClick.AddDefaultBinding(Mouse.MiddleButton);
		this.scrollUp.AddDefaultBinding(Mouse.PositiveScrollWheel);
		this.scrollDown.AddDefaultBinding(Mouse.NegativeScrollWheel);
		this.scrollUp.AddDefaultBinding(new Key[] { Key.UpArrow });
		this.scrollDown.AddDefaultBinding(new Key[] { Key.DownArrow });
	}

	// Token: 0x04002259 RID: 8793
	public PlayerAction Left;

	// Token: 0x0400225A RID: 8794
	public PlayerAction Right;

	// Token: 0x0400225B RID: 8795
	public PlayerAction Up;

	// Token: 0x0400225C RID: 8796
	public PlayerAction Down;

	// Token: 0x0400225D RID: 8797
	public PlayerTwoAxisAction Look;

	// Token: 0x0400225E RID: 8798
	public PlayerAction DPad_Left;

	// Token: 0x0400225F RID: 8799
	public PlayerAction DPad_Right;

	// Token: 0x04002260 RID: 8800
	public PlayerAction DPad_Up;

	// Token: 0x04002261 RID: 8801
	public PlayerAction DPad_Down;

	// Token: 0x04002262 RID: 8802
	public PlayerAction Submit;

	// Token: 0x04002263 RID: 8803
	public PlayerAction Cancel;

	// Token: 0x04002264 RID: 8804
	public PlayerAction HalfStack;

	// Token: 0x04002265 RID: 8805
	public PlayerAction Inspect;

	// Token: 0x04002266 RID: 8806
	public PlayerAction FocusSearch;

	// Token: 0x04002267 RID: 8807
	public PlayerAction LeftClick;

	// Token: 0x04002268 RID: 8808
	public PlayerAction RightClick;

	// Token: 0x04002269 RID: 8809
	public PlayerAction MiddleClick;

	// Token: 0x0400226A RID: 8810
	public PlayerAction CameraLeft;

	// Token: 0x0400226B RID: 8811
	public PlayerAction CameraRight;

	// Token: 0x0400226C RID: 8812
	public PlayerAction CameraUp;

	// Token: 0x0400226D RID: 8813
	public PlayerAction CameraDown;

	// Token: 0x0400226E RID: 8814
	public PlayerTwoAxisAction Camera;

	// Token: 0x0400226F RID: 8815
	public PlayerAction WindowPagingLeft;

	// Token: 0x04002270 RID: 8816
	public PlayerAction WindowPagingRight;

	// Token: 0x04002271 RID: 8817
	public PlayerAction PageUp;

	// Token: 0x04002272 RID: 8818
	public PlayerAction PageDown;

	// Token: 0x04002273 RID: 8819
	public PlayerAction RightStick;

	// Token: 0x04002274 RID: 8820
	public PlayerAction LeftStick;

	// Token: 0x04002275 RID: 8821
	public PlayerAction BackButton;

	// Token: 0x04002276 RID: 8822
	[PublicizedFrom(EAccessModifier.Private)]
	public const float LOWER_STICK_DEADZONE = 0.6f;

	// Token: 0x04002277 RID: 8823
	[PublicizedFrom(EAccessModifier.Private)]
	public const float UPPER_STICK_DEADZONE = 0.9f;

	// Token: 0x04002278 RID: 8824
	[PublicizedFrom(EAccessModifier.Private)]
	public const float LOWER_LEFT_STICKDEADZONE = 0.8f;

	// Token: 0x04002279 RID: 8825
	public PlayerAction NavUp;

	// Token: 0x0400227A RID: 8826
	public PlayerAction NavDown;

	// Token: 0x0400227B RID: 8827
	public PlayerAction NavLeft;

	// Token: 0x0400227C RID: 8828
	public PlayerAction NavRight;

	// Token: 0x0400227D RID: 8829
	public PlayerTwoAxisAction Nav;

	// Token: 0x0400227E RID: 8830
	public PlayerOneAxisAction BumperAxis;

	// Token: 0x0400227F RID: 8831
	public PlayerOneAxisAction TriggerAxis;

	// Token: 0x04002280 RID: 8832
	public PlayerAction scrollUp;

	// Token: 0x04002281 RID: 8833
	public PlayerAction scrollDown;

	// Token: 0x04002282 RID: 8834
	public PlayerOneAxisAction scroll;

	// Token: 0x04002283 RID: 8835
	public PlayerAction ActionUp;

	// Token: 0x04002284 RID: 8836
	public PlayerAction ActionDown;

	// Token: 0x04002285 RID: 8837
	public PlayerAction ActionLeft;

	// Token: 0x04002286 RID: 8838
	public PlayerAction ActionRight;

	// Token: 0x04002287 RID: 8839
	public PlayerAction Apply;
}
