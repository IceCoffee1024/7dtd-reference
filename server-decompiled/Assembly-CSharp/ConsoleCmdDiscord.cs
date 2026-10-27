using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200033A RID: 826
[Preserve]
public class ConsoleCmdDiscord : ConsoleCmdAbstract
{
	// Token: 0x060017CF RID: 6095 RVA: 0x000894E4 File Offset: 0x000876E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "discord", "dc" };
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x060017D2 RID: 6098 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000894FC File Offset: 0x000876FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle Discord debug window";
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x00089503 File Offset: 0x00087703
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		LocalPlayerUI.primaryUI.windowManager.SwitchVisible(XUiC_DiscordWindow.ID, false);
	}
}
