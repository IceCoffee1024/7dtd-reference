using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BD RID: 701
[Preserve]
public class ConsoleCmdVersion : ConsoleCmdAbstract
{
	// Token: 0x0600141B RID: 5147 RVA: 0x0007A326 File Offset: 0x00078526
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "version" };
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x0600141C RID: 5148 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x0600141D RID: 5149 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x0007A336 File Offset: 0x00078536
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the currently running version of the game and loaded mods";
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x0007A340 File Offset: 0x00078540
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Game version: " + Constants.cVersionInformation.LongString + " Compatibility Version: " + Constants.cVersionInformation.LongStringNoBuild);
		for (int i = 0; i < ModManager.GetLoadedMods().Count; i++)
		{
			Mod mod = ModManager.GetLoadedMods()[i];
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Mod " + mod.Name + ": " + (mod.VersionString ?? "<unknown version>"));
		}
	}
}
