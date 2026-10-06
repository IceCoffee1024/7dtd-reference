using System;
using InControl;

// Token: 0x02000566 RID: 1382
public class PlayerActionsGlobal : PlayerActionsBase
{
	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06002CBA RID: 11450 RVA: 0x0011C026 File Offset: 0x0011A226
	public static PlayerActionsGlobal Instance
	{
		get
		{
			return PlayerActionsGlobal.m_Instance;
		}
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x0011C02D File Offset: 0x0011A22D
	public static void Init()
	{
		if (PlayerActionsGlobal.m_Instance == null)
		{
			PlayerActionsGlobal.m_Instance = new PlayerActionsGlobal();
		}
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x0011C040 File Offset: 0x0011A240
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsGlobal()
	{
		base.Name = "global";
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x0011C054 File Offset: 0x0011A254
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

	// Token: 0x06002CBE RID: 11454 RVA: 0x0011C22C File Offset: 0x0011A42C
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

	// Token: 0x06002CBF RID: 11455 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
	}

	// Token: 0x04002288 RID: 8840
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerActionsGlobal m_Instance;

	// Token: 0x04002289 RID: 8841
	public PlayerAction Console;

	// Token: 0x0400228A RID: 8842
	public PlayerAction ShowDebugData;

	// Token: 0x0400228B RID: 8843
	public PlayerAction Fullscreen;

	// Token: 0x0400228C RID: 8844
	public PlayerAction DebugSpawn;

	// Token: 0x0400228D RID: 8845
	public PlayerAction DebugGameEvent;

	// Token: 0x0400228E RID: 8846
	public PlayerAction SwitchHUD;

	// Token: 0x0400228F RID: 8847
	public PlayerAction ShowFPS;

	// Token: 0x04002290 RID: 8848
	public PlayerAction Screenshot;

	// Token: 0x04002291 RID: 8849
	public PlayerAction DebugScreenshot;

	// Token: 0x04002292 RID: 8850
	public PlayerAction BackgroundedScreenshot;
}
