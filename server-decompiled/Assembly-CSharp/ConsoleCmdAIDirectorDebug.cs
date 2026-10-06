using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001E2 RID: 482
[Preserve]
public class ConsoleCmdAIDirectorDebug : ConsoleCmdAbstract
{
	// Token: 0x06000ED3 RID: 3795 RVA: 0x00060FE8 File Offset: 0x0005F1E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "aiddebug" };
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x00060FF8 File Offset: 0x0005F1F8
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

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0006102D File Offset: 0x0005F22D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles AIDirector debug output.";
	}
}
