using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BE RID: 702
[Preserve]
public class ConsoleCmdVersionUi : ConsoleCmdAbstract
{
	// Token: 0x06001421 RID: 5153 RVA: 0x0007A3C9 File Offset: 0x000785C9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "versionui" };
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06001422 RID: 5154 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06001423 RID: 5155 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x0007A3D9 File Offset: 0x000785D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle version number display";
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x0007A3E0 File Offset: 0x000785E0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		NGUIWindowManager nguiWindowManager = LocalPlayerUI.primaryUI.nguiWindowManager;
		nguiWindowManager.AlwaysShowVersionUi = !nguiWindowManager.AlwaysShowVersionUi;
	}
}
