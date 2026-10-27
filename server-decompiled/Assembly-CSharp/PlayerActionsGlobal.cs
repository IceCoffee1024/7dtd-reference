using System;
using InControl;

// Token: 0x0200056A RID: 1386
public class PlayerActionsGlobal : PlayerActionsBase
{
	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x0011C5B2 File Offset: 0x0011A7B2
	public static PlayerActionsGlobal Instance
	{
		get
		{
			return PlayerActionsGlobal.m_Instance;
		}
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x0011C5B9 File Offset: 0x0011A7B9
	public static void Init()
	{
		if (PlayerActionsGlobal.m_Instance == null)
		{
			PlayerActionsGlobal.m_Instance = new PlayerActionsGlobal();
		}
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x0011C5CC File Offset: 0x0011A7CC
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsGlobal()
	{
		base.Name = "global";
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x0011C5E0 File Offset: 0x0011A7E0
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

	// Token: 0x06002CC6 RID: 11462 RVA: 0x0011C7B8 File Offset: 0x0011A9B8
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

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CreateDefaultJoystickBindings()
	{
	}

	// Token: 0x0400229F RID: 8863
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerActionsGlobal m_Instance;

	// Token: 0x040022A0 RID: 8864
	public PlayerAction Console;

	// Token: 0x040022A1 RID: 8865
	public PlayerAction ShowDebugData;

	// Token: 0x040022A2 RID: 8866
	public PlayerAction Fullscreen;

	// Token: 0x040022A3 RID: 8867
	public PlayerAction DebugSpawn;

	// Token: 0x040022A4 RID: 8868
	public PlayerAction DebugGameEvent;

	// Token: 0x040022A5 RID: 8869
	public PlayerAction SwitchHUD;

	// Token: 0x040022A6 RID: 8870
	public PlayerAction ShowFPS;

	// Token: 0x040022A7 RID: 8871
	public PlayerAction Screenshot;

	// Token: 0x040022A8 RID: 8872
	public PlayerAction DebugScreenshot;

	// Token: 0x040022A9 RID: 8873
	public PlayerAction BackgroundedScreenshot;
}
