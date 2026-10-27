using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002C0 RID: 704
[Preserve]
public class ConsoleCmdVersionUi : ConsoleCmdAbstract
{
	// Token: 0x06001425 RID: 5157 RVA: 0x0007A599 File Offset: 0x00078799
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "versionui" };
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06001426 RID: 5158 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06001427 RID: 5159 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x0007A5A9 File Offset: 0x000787A9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle version number display";
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0007A5B0 File Offset: 0x000787B0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		NGUIWindowManager nguiWindowManager = LocalPlayerUI.primaryUI.nguiWindowManager;
		nguiWindowManager.AlwaysShowVersionUi = !nguiWindowManager.AlwaysShowVersionUi;
	}
}
