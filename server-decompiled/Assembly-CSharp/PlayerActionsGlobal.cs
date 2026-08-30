using System;
using InControl;

// Token: 0x02000560 RID: 1376
public class PlayerActionsGlobal : PlayerActionsBase
{
	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x0011B256 File Offset: 0x00119456
	public static PlayerActionsGlobal Instance
	{
		get
		{
			return PlayerActionsGlobal.m_Instance;
		}
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x0011B25D File Offset: 0x0011945D
	public static void Init()
	{
		if (PlayerActionsGlobal.m_Instance == null)
		{
			PlayerActionsGlobal.m_Instance = new PlayerActionsGlobal();
		}
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x0011B270 File Offset: 0x00119470
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsGlobal()
	{
		base.Name = "global";
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x0011B284 File Offset: 0x00119484
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

	// Token: 0x06002CC8 RID: 11464 RVA: 0x0011B45C File Offset: 0x0011965C
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

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
	}

	// Token: 0x04002280 RID: 8832
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerActionsGlobal m_Instance;

	// Token: 0x04002281 RID: 8833
	public PlayerAction Console;

	// Token: 0x04002282 RID: 8834
	public PlayerAction ShowDebugData;

	// Token: 0x04002283 RID: 8835
	public PlayerAction Fullscreen;

	// Token: 0x04002284 RID: 8836
	public PlayerAction DebugSpawn;

	// Token: 0x04002285 RID: 8837
	public PlayerAction DebugGameEvent;

	// Token: 0x04002286 RID: 8838
	public PlayerAction SwitchHUD;

	// Token: 0x04002287 RID: 8839
	public PlayerAction ShowFPS;

	// Token: 0x04002288 RID: 8840
	public PlayerAction Screenshot;

	// Token: 0x04002289 RID: 8841
	public PlayerAction DebugScreenshot;

	// Token: 0x0400228A RID: 8842
	public PlayerAction BackgroundedScreenshot;
}
