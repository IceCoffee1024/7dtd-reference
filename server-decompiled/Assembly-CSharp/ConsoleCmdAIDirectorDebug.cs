using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001E4 RID: 484
[Preserve]
public class ConsoleCmdAIDirectorDebug : ConsoleCmdAbstract
{
	// Token: 0x06000ED7 RID: 3799 RVA: 0x000612AC File Offset: 0x0005F4AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "aiddebug" };
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x000612BC File Offset: 0x0005F4BC
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

	// Token: 0x06000EDB RID: 3803 RVA: 0x000612F1 File Offset: 0x0005F4F1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles AIDirector debug output.";
	}
}
