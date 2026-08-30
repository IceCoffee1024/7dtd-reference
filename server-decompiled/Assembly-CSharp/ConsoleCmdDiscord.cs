using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000335 RID: 821
[Preserve]
public class ConsoleCmdDiscord : ConsoleCmdAbstract
{
	// Token: 0x060017CE RID: 6094 RVA: 0x00088FFC File Offset: 0x000871FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "discord", "dc" };
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x060017CF RID: 6095 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x00089014 File Offset: 0x00087214
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle Discord debug window";
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x0008901B File Offset: 0x0008721B
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		LocalPlayerUI.primaryUI.windowManager.SwitchVisible(XUiC_DiscordWindow.ID, false);
	}
}
