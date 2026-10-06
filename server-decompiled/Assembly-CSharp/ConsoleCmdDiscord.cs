using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000338 RID: 824
[Preserve]
public class ConsoleCmdDiscord : ConsoleCmdAbstract
{
	// Token: 0x060017CB RID: 6091 RVA: 0x000892B0 File Offset: 0x000874B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "discord", "dc" };
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060017CC RID: 6092 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x060017CE RID: 6094 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x000892C8 File Offset: 0x000874C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle Discord debug window";
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000892CF File Offset: 0x000874CF
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		LocalPlayerUI.primaryUI.windowManager.SwitchVisible(XUiC_DiscordWindow.ID, false);
	}
}
