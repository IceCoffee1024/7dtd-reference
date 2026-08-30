using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BB RID: 699
[Preserve]
public class ConsoleCmdVersionUi : ConsoleCmdAbstract
{
	// Token: 0x06001424 RID: 5156 RVA: 0x0007A115 File Offset: 0x00078315
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "versionui" };
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001425 RID: 5157 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06001426 RID: 5158 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x0007A125 File Offset: 0x00078325
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle version number display";
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x0007A12C File Offset: 0x0007832C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		NGUIWindowManager nguiWindowManager = LocalPlayerUI.primaryUI.nguiWindowManager;
		nguiWindowManager.AlwaysShowVersionUi = !nguiWindowManager.AlwaysShowVersionUi;
	}
}
