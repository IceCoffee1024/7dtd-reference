using System;
using InControl;

// Token: 0x0200056E RID: 1390
public class PlayerActionsPermanent : PlayerActionsBase
{
	// Token: 0x06002CDC RID: 11484 RVA: 0x0011F028 File Offset: 0x0011D228
	public PlayerActionsPermanent()
	{
		base.Name = "permanent";
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x0011F03C File Offset: 0x0011D23C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateActions()
	{
		this.Reload = base.CreatePlayerAction("Reload");
		this.Reload.UserData = new PlayerActionData.ActionUserData("inpActReloadTakeAllName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.Reload.FirstRepeatDelay = 0.3f;
		this.Activate = base.CreatePlayerAction("Activate");
		this.Activate.UserData = new PlayerActionData.ActionUserData("inpActActivateName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.ToggleFlashlight = base.CreatePlayerAction("ToggleFlashlight");
		this.ToggleFlashlight.UserData = new PlayerActionData.ActionUserData("inpActPlayerToggleFlashlightName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.ToggleFlashlight.FirstRepeatDelay = 0.3f;
		this.Inventory = base.CreatePlayerAction("Inventory");
		this.Inventory.UserData = new PlayerActionData.ActionUserData("inpActInventoryName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Character = base.CreatePlayerAction("Character");
		this.Character.UserData = new PlayerActionData.ActionUserData("inpActCharacterName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Map = base.CreatePlayerAction("Map");
		this.Map.UserData = new PlayerActionData.ActionUserData("inpActMapName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Skills = base.CreatePlayerAction("Skills");
		this.Skills.UserData = new PlayerActionData.ActionUserData("inpActSkillsName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Quests = base.CreatePlayerAction("Quests");
		this.Quests.UserData = new PlayerActionData.ActionUserData("inpActQuestsName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Challenges = base.CreatePlayerAction("Challenges");
		this.Challenges.UserData = new PlayerActionData.ActionUserData("inpActChallengesName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Scoreboard = base.CreatePlayerAction("Scoreboard");
		this.Scoreboard.UserData = new PlayerActionData.ActionUserData("inpActScoreboardName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Creative = base.CreatePlayerAction("Creative");
		this.Creative.UserData = new PlayerActionData.ActionUserData("inpActCreativeName", null, PlayerActionData.GroupDialogs, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.DebugControllerLeft = base.CreatePlayerAction("DebugControllerLeft");
		this.DebugControllerLeft.UserData = new PlayerActionData.ActionUserData("inpActDebugControllerLeftName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.ControllerOnly, false, true, true, true);
		this.DebugControllerRight = base.CreatePlayerAction("DebugControllerRight");
		this.DebugControllerRight.UserData = new PlayerActionData.ActionUserData("inpActDebugControllerRightName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.ControllerOnly, false, true, true, true);
		this.Chat = base.CreatePlayerAction("Chat");
		this.Chat.UserData = new PlayerActionData.ActionUserData("inpActChatName", null, PlayerActionData.GroupMp, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.PushToTalk = base.CreatePlayerAction("PushToTalk");
		this.PushToTalk.UserData = new PlayerActionData.ActionUserData("inpActPushToTalkName", null, PlayerActionData.GroupMp, PlayerActionData.EAppliesToInputType.Both, true, false, false, false);
		this.Cancel = base.CreatePlayerAction("Cancel");
		this.Cancel.UserData = new PlayerActionData.ActionUserData("inpActCancelName", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.Both, false, true, true, true);
		this.Swap = base.CreatePlayerAction("Swap");
		this.Swap.UserData = new PlayerActionData.ActionUserData("inpActSwapName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.PageTipsForward = base.CreatePlayerAction("PageTipsForward");
		this.PageTipsForward.UserData = new PlayerActionData.ActionUserData("inpActPageTipsForward", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.Both, false, true, true, true);
		this.PageTipsBack = base.CreatePlayerAction("PageTipsBack");
		this.PageTipsBack.UserData = new PlayerActionData.ActionUserData("inpActPageTipsBack", null, PlayerActionData.GroupAdmin, PlayerActionData.EAppliesToInputType.Both, false, true, true, true);
		this.QuickMenu = base.CreatePlayerAction("QuickMenu");
		this.QuickMenu.UserData = new PlayerActionData.ActionUserData("inpQuickMenuName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.KbdMouseOnly, true, false, false, true);
		this.CameraZoomIn = base.CreatePlayerAction("ScrollUp");
		this.CameraZoomIn.UserData = new PlayerActionData.ActionUserData("inpActScopeZoomInName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, false, true, true, true);
		this.CameraZoomOut = base.CreatePlayerAction("ScrollDown");
		this.CameraZoomOut.UserData = new PlayerActionData.ActionUserData("inpActScopeZoomOutName", null, PlayerActionData.GroupPlayerControl, PlayerActionData.EAppliesToInputType.Both, false, true, true, true);
		this.CameraZoom = base.CreateOneAxisPlayerAction(this.CameraZoomOut, this.CameraZoomIn);
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x0011F4B8 File Offset: 0x0011D6B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
		base.ListenOptions.IncludeControllers = true;
		this.DebugControllerLeft.AddDefaultBinding(InputControlType.LeftBumper);
		this.DebugControllerRight.AddDefaultBinding(InputControlType.RightBumper);
		this.Cancel.AddDefaultBinding(InputControlType.Action2);
		this.PageTipsForward.AddDefaultBinding(InputControlType.RightTrigger);
		this.PageTipsBack.AddDefaultBinding(InputControlType.LeftTrigger);
		this.PushToTalk.AddDefaultBinding(InputControlType.DPadRight);
		this.CameraZoomIn.AddDefaultBinding(InputControlType.RightTrigger);
		this.CameraZoomOut.AddDefaultBinding(InputControlType.LeftTrigger);
		this.ControllerRebindableActions.Clear();
		this.ControllerRebindableActions.Add(this.PushToTalk);
		this.ControllerRebindableActions.Add(this.Character);
		this.ControllerRebindableActions.Add(this.Map);
		this.ControllerRebindableActions.Add(this.Skills);
		this.ControllerRebindableActions.Add(this.Quests);
		this.ControllerRebindableActions.Add(this.Challenges);
		this.ControllerRebindableActions.Add(this.Scoreboard);
		this.ControllerRebindableActions.Add(this.Creative);
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x0011F5CC File Offset: 0x0011D7CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultKeyboardBindings()
	{
		base.ListenOptions.IncludeKeys = true;
		base.ListenOptions.IncludeMouseButtons = true;
		base.ListenOptions.IncludeMouseScrollWheel = true;
		this.Reload.AddDefaultBinding(new Key[] { Key.R });
		this.Activate.AddDefaultBinding(new Key[] { Key.E });
		this.ToggleFlashlight.AddDefaultBinding(new Key[] { Key.F });
		this.Inventory.AddDefaultBinding(new Key[] { Key.Tab });
		this.Skills.AddDefaultBinding(new Key[] { Key.N });
		this.Quests.AddDefaultBinding(new Key[] { Key.O });
		this.Challenges.AddDefaultBinding(new Key[] { Key.Y });
		this.Character.AddDefaultBinding(new Key[] { Key.B });
		this.Map.AddDefaultBinding(new Key[] { Key.M });
		this.Creative.AddDefaultBinding(new Key[] { Key.U });
		this.Scoreboard.AddDefaultBinding(new Key[] { Key.I });
		this.Chat.AddDefaultBinding(new Key[] { Key.T });
		this.PushToTalk.AddDefaultBinding(new Key[] { Key.V });
		this.Cancel.AddDefaultBinding(new Key[] { Key.Escape });
		this.Swap.AddDefaultBinding(Mouse.Button4);
		this.PageTipsForward.AddDefaultBinding(Mouse.LeftButton);
		this.PageTipsBack.AddDefaultBinding(Mouse.RightButton);
		this.QuickMenu.AddDefaultBinding(new Key[] { Key.Backquote });
		this.CameraZoomIn.AddDefaultBinding(Mouse.PositiveScrollWheel);
		this.CameraZoomOut.AddDefaultBinding(Mouse.NegativeScrollWheel);
	}

	// Token: 0x04002329 RID: 9001
	public PlayerAction Reload;

	// Token: 0x0400232A RID: 9002
	public PlayerAction Activate;

	// Token: 0x0400232B RID: 9003
	public PlayerAction ToggleFlashlight;

	// Token: 0x0400232C RID: 9004
	public PlayerAction Inventory;

	// Token: 0x0400232D RID: 9005
	public PlayerAction Skills;

	// Token: 0x0400232E RID: 9006
	public PlayerAction Quests;

	// Token: 0x0400232F RID: 9007
	public PlayerAction Challenges;

	// Token: 0x04002330 RID: 9008
	public PlayerAction Character;

	// Token: 0x04002331 RID: 9009
	public PlayerAction Map;

	// Token: 0x04002332 RID: 9010
	public PlayerAction Creative;

	// Token: 0x04002333 RID: 9011
	public PlayerAction Scoreboard;

	// Token: 0x04002334 RID: 9012
	public PlayerAction DebugControllerLeft;

	// Token: 0x04002335 RID: 9013
	public PlayerAction DebugControllerRight;

	// Token: 0x04002336 RID: 9014
	public PlayerAction Chat;

	// Token: 0x04002337 RID: 9015
	public PlayerAction PushToTalk;

	// Token: 0x04002338 RID: 9016
	public PlayerAction Cancel;

	// Token: 0x04002339 RID: 9017
	public PlayerAction Swap;

	// Token: 0x0400233A RID: 9018
	public PlayerAction PageTipsForward;

	// Token: 0x0400233B RID: 9019
	public PlayerAction PageTipsBack;

	// Token: 0x0400233C RID: 9020
	public PlayerAction QuickMenu;

	// Token: 0x0400233D RID: 9021
	public PlayerAction CameraZoomIn;

	// Token: 0x0400233E RID: 9022
	public PlayerAction CameraZoomOut;

	// Token: 0x0400233F RID: 9023
	public PlayerOneAxisAction CameraZoom;
}
