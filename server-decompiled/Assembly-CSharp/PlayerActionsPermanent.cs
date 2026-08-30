using System;
using InControl;

// Token: 0x02000564 RID: 1380
public class PlayerActionsPermanent : PlayerActionsBase
{
	// Token: 0x06002CDE RID: 11486 RVA: 0x0011DC18 File Offset: 0x0011BE18
	public PlayerActionsPermanent()
	{
		base.Name = "permanent";
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x0011DC2C File Offset: 0x0011BE2C
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

	// Token: 0x06002CE0 RID: 11488 RVA: 0x0011E0A8 File Offset: 0x0011C2A8
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

	// Token: 0x06002CE1 RID: 11489 RVA: 0x0011E1BC File Offset: 0x0011C3BC
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

	// Token: 0x04002308 RID: 8968
	public PlayerAction Reload;

	// Token: 0x04002309 RID: 8969
	public PlayerAction Activate;

	// Token: 0x0400230A RID: 8970
	public PlayerAction ToggleFlashlight;

	// Token: 0x0400230B RID: 8971
	public PlayerAction Inventory;

	// Token: 0x0400230C RID: 8972
	public PlayerAction Skills;

	// Token: 0x0400230D RID: 8973
	public PlayerAction Quests;

	// Token: 0x0400230E RID: 8974
	public PlayerAction Challenges;

	// Token: 0x0400230F RID: 8975
	public PlayerAction Character;

	// Token: 0x04002310 RID: 8976
	public PlayerAction Map;

	// Token: 0x04002311 RID: 8977
	public PlayerAction Creative;

	// Token: 0x04002312 RID: 8978
	public PlayerAction Scoreboard;

	// Token: 0x04002313 RID: 8979
	public PlayerAction DebugControllerLeft;

	// Token: 0x04002314 RID: 8980
	public PlayerAction DebugControllerRight;

	// Token: 0x04002315 RID: 8981
	public PlayerAction Chat;

	// Token: 0x04002316 RID: 8982
	public PlayerAction PushToTalk;

	// Token: 0x04002317 RID: 8983
	public PlayerAction Cancel;

	// Token: 0x04002318 RID: 8984
	public PlayerAction Swap;

	// Token: 0x04002319 RID: 8985
	public PlayerAction PageTipsForward;

	// Token: 0x0400231A RID: 8986
	public PlayerAction PageTipsBack;

	// Token: 0x0400231B RID: 8987
	public PlayerAction QuickMenu;

	// Token: 0x0400231C RID: 8988
	public PlayerAction CameraZoomIn;

	// Token: 0x0400231D RID: 8989
	public PlayerAction CameraZoomOut;

	// Token: 0x0400231E RID: 8990
	public PlayerOneAxisAction CameraZoom;
}
