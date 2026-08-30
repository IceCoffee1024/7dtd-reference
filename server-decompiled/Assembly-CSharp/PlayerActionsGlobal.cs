using System;
using InControl;

// Token: 0x0200055B RID: 1371
public class PlayerActionsGlobal : PlayerActionsBase
{
	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06002C9A RID: 11418 RVA: 0x0011A1E2 File Offset: 0x001183E2
	public static PlayerActionsGlobal Instance
	{
		get
		{
			return PlayerActionsGlobal.m_Instance;
		}
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x0011A1E9 File Offset: 0x001183E9
	public static void Init()
	{
		if (PlayerActionsGlobal.m_Instance == null)
		{
			PlayerActionsGlobal.m_Instance = new PlayerActionsGlobal();
		}
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x0011A1FC File Offset: 0x001183FC
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsGlobal()
	{
		base.Name = "global";
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x0011A210 File Offset: 0x00118410
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateActions()
	{
		this.Console = base.CreatePlayerAction("Console");
		this.Console.UserData = new PlayerActionData.ActionUserData("inpActConsoleName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.ShowDebugData = base.CreatePlayerAction("Show Debug Data");
		this.ShowDebugData.UserData = new PlayerActionData.ActionUserData("inpActShowDebugDataName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.Fullscreen = base.CreatePlayerAction("Fullscreen");
		this.Fullscreen.UserData = new PlayerActionData.ActionUserData("inpActFullscreenName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.DebugSpawn = base.CreatePlayerAction("DebugSpawn");
		this.DebugSpawn.UserData = new PlayerActionData.ActionUserData("inpActDebugSpawnName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.DebugGameEvent = base.CreatePlayerAction("DebugGameEvent");
		this.DebugGameEvent.UserData = new PlayerActionData.ActionUserData("inpActDebugGameEventName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.SwitchHUD = base.CreatePlayerAction("SwitchHUD");
		this.SwitchHUD.UserData = new PlayerActionData.ActionUserData("inpActSwitchHUDName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.ShowFPS = base.CreatePlayerAction("ShowFPS");
		this.ShowFPS.UserData = new PlayerActionData.ActionUserData("inpActShowFPSName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.Screenshot = base.CreatePlayerAction("Screenshot");
		this.Screenshot.UserData = new PlayerActionData.ActionUserData("inpActScreenshotName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.DebugScreenshot = base.CreatePlayerAction("DebugScreenshot");
		this.DebugScreenshot.UserData = new PlayerActionData.ActionUserData("inpActDebugScreenshotName", null, PlayerActionData.GroupGlobalFunctions, PlayerActionData.EAppliesToInputType.KbdMouseOnly, false, false, false, true);
		this.BackgroundedScreenshot = base.CreatePlayerAction("BackgroundedScreenshot");
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x0011A3E8 File Offset: 0x001185E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultKeyboardBindings()
	{
		if (!Submission.Enabled)
		{
			this.Console.AddDefaultBinding(new Key[] { Key.F1 });
			this.ShowDebugData.AddDefaultBinding(new Key[] { Key.F3 });
			this.Fullscreen.AddDefaultBinding(new Key[] { Key.F4 });
			this.DebugSpawn.AddDefaultBinding(new Key[] { Key.F6 });
			this.DebugGameEvent.AddDefaultBinding(new Key[]
			{
				Key.F6,
				Key.Shift
			});
			this.SwitchHUD.AddDefaultBinding(new Key[] { Key.F7 });
			this.ShowFPS.AddDefaultBinding(new Key[] { Key.F8 });
			this.Screenshot.AddDefaultBinding(new Key[] { Key.F9 });
			this.BackgroundedScreenshot.AddDefaultBinding(new Key[] { Key.F10 });
			this.DebugScreenshot.AddDefaultBinding(new Key[] { Key.F11 });
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
	}

	// Token: 0x0400224E RID: 8782
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerActionsGlobal m_Instance;

	// Token: 0x0400224F RID: 8783
	public PlayerAction Console;

	// Token: 0x04002250 RID: 8784
	public PlayerAction ShowDebugData;

	// Token: 0x04002251 RID: 8785
	public PlayerAction Fullscreen;

	// Token: 0x04002252 RID: 8786
	public PlayerAction DebugSpawn;

	// Token: 0x04002253 RID: 8787
	public PlayerAction DebugGameEvent;

	// Token: 0x04002254 RID: 8788
	public PlayerAction SwitchHUD;

	// Token: 0x04002255 RID: 8789
	public PlayerAction ShowFPS;

	// Token: 0x04002256 RID: 8790
	public PlayerAction Screenshot;

	// Token: 0x04002257 RID: 8791
	public PlayerAction DebugScreenshot;

	// Token: 0x04002258 RID: 8792
	public PlayerAction BackgroundedScreenshot;
}
