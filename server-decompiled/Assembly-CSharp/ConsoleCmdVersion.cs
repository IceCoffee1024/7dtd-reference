using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BF RID: 703
[Preserve]
public class ConsoleCmdVersion : ConsoleCmdAbstract
{
	// Token: 0x0600141F RID: 5151 RVA: 0x0007A4F6 File Offset: 0x000786F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "version" };
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06001420 RID: 5152 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06001421 RID: 5153 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x0007A506 File Offset: 0x00078706
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the currently running version of the game and loaded mods";
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x0007A510 File Offset: 0x00078710
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
