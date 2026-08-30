using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002B7 RID: 695
[Preserve]
public class ConsoleCmdVersionUi : ConsoleCmdAbstract
{
	// Token: 0x06001412 RID: 5138 RVA: 0x00079AB9 File Offset: 0x00077CB9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "versionui" };
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001413 RID: 5139 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001414 RID: 5140 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x00079AC9 File Offset: 0x00077CC9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle version number display";
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x00079AD0 File Offset: 0x00077CD0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		NGUIWindowManager nguiWindowManager = LocalPlayerUI.primaryUI.nguiWindowManager;
		nguiWindowManager.AlwaysShowVersionUi = !nguiWindowManager.AlwaysShowVersionUi;
	}
}
