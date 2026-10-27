using System;
using InControl;

// Token: 0x0200056B RID: 1387
public class PlayerActionsGUI : PlayerActionsBase
{
	// Token: 0x06002CC8 RID: 11464 RVA: 0x0011C8AF File Offset: 0x0011AAAF
	public PlayerActionsGUI()
	{
		base.Name = "gui";
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x0011C8C4 File Offset: 0x0011AAC4
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

	// Token: 0x06002CCA RID: 11466 RVA: 0x0011D134 File Offset: 0x0011B334
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

	// Token: 0x06002CCB RID: 11467 RVA: 0x0011D31C File Offset: 0x0011B51C
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

	// Token: 0x040022AA RID: 8874
	public PlayerAction Left;

	// Token: 0x040022AB RID: 8875
	public PlayerAction Right;

	// Token: 0x040022AC RID: 8876
	public PlayerAction Up;

	// Token: 0x040022AD RID: 8877
	public PlayerAction Down;

	// Token: 0x040022AE RID: 8878
	public PlayerTwoAxisAction Look;

	// Token: 0x040022AF RID: 8879
	public PlayerAction DPad_Left;

	// Token: 0x040022B0 RID: 8880
	public PlayerAction DPad_Right;

	// Token: 0x040022B1 RID: 8881
	public PlayerAction DPad_Up;

	// Token: 0x040022B2 RID: 8882
	public PlayerAction DPad_Down;

	// Token: 0x040022B3 RID: 8883
	public PlayerAction Submit;

	// Token: 0x040022B4 RID: 8884
	public PlayerAction Cancel;

	// Token: 0x040022B5 RID: 8885
	public PlayerAction HalfStack;

	// Token: 0x040022B6 RID: 8886
	public PlayerAction Inspect;

	// Token: 0x040022B7 RID: 8887
	public PlayerAction FocusSearch;

	// Token: 0x040022B8 RID: 8888
	public PlayerAction LeftClick;

	// Token: 0x040022B9 RID: 8889
	public PlayerAction RightClick;

	// Token: 0x040022BA RID: 8890
	public PlayerAction MiddleClick;

	// Token: 0x040022BB RID: 8891
	public PlayerAction CameraLeft;

	// Token: 0x040022BC RID: 8892
	public PlayerAction CameraRight;

	// Token: 0x040022BD RID: 8893
	public PlayerAction CameraUp;

	// Token: 0x040022BE RID: 8894
	public PlayerAction CameraDown;

	// Token: 0x040022BF RID: 8895
	public PlayerTwoAxisAction Camera;

	// Token: 0x040022C0 RID: 8896
	public PlayerAction WindowPagingLeft;

	// Token: 0x040022C1 RID: 8897
	public PlayerAction WindowPagingRight;

	// Token: 0x040022C2 RID: 8898
	public PlayerAction PageUp;

	// Token: 0x040022C3 RID: 8899
	public PlayerAction PageDown;

	// Token: 0x040022C4 RID: 8900
	public PlayerAction RightStick;

	// Token: 0x040022C5 RID: 8901
	public PlayerAction LeftStick;

	// Token: 0x040022C6 RID: 8902
	public PlayerAction BackButton;

	// Token: 0x040022C7 RID: 8903
	[PublicizedFrom(EAccessModifier.Private)]
	public const float LOWER_STICK_DEADZONE = 0.6f;

	// Token: 0x040022C8 RID: 8904
	[PublicizedFrom(EAccessModifier.Private)]
	public const float UPPER_STICK_DEADZONE = 0.9f;

	// Token: 0x040022C9 RID: 8905
	[PublicizedFrom(EAccessModifier.Private)]
	public const float LOWER_LEFT_STICKDEADZONE = 0.8f;

	// Token: 0x040022CA RID: 8906
	public PlayerAction NavUp;

	// Token: 0x040022CB RID: 8907
	public PlayerAction NavDown;

	// Token: 0x040022CC RID: 8908
	public PlayerAction NavLeft;

	// Token: 0x040022CD RID: 8909
	public PlayerAction NavRight;

	// Token: 0x040022CE RID: 8910
	public PlayerTwoAxisAction Nav;

	// Token: 0x040022CF RID: 8911
	public PlayerOneAxisAction BumperAxis;

	// Token: 0x040022D0 RID: 8912
	public PlayerOneAxisAction TriggerAxis;

	// Token: 0x040022D1 RID: 8913
	public PlayerAction scrollUp;

	// Token: 0x040022D2 RID: 8914
	public PlayerAction scrollDown;

	// Token: 0x040022D3 RID: 8915
	public PlayerOneAxisAction scroll;

	// Token: 0x040022D4 RID: 8916
	public PlayerAction ActionUp;

	// Token: 0x040022D5 RID: 8917
	public PlayerAction ActionDown;

	// Token: 0x040022D6 RID: 8918
	public PlayerAction ActionLeft;

	// Token: 0x040022D7 RID: 8919
	public PlayerAction ActionRight;

	// Token: 0x040022D8 RID: 8920
	public PlayerAction Apply;
}
