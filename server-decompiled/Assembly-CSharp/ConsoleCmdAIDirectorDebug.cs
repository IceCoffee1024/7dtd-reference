using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001DF RID: 479
[Preserve]
public class ConsoleCmdAIDirectorDebug : ConsoleCmdAbstract
{
	// Token: 0x06000ED5 RID: 3797 RVA: 0x00060AB0 File Offset: 0x0005ECB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "aiddebug" };
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x00060AC0 File Offset: 0x0005ECC0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		AIDirectorConstants.DebugOutput = !AIDirectorConstants.DebugOutput;
		if (AIDirectorConstants.DebugOutput)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("AIDirector debug output is ON.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("AIDirector debug output is OFF.");
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x00060AF5 File Offset: 0x0005ECF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles AIDirector debug output.";
	}
}
