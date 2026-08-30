using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BA RID: 698
[Preserve]
public class ConsoleCmdVersion : ConsoleCmdAbstract
{
	// Token: 0x0600141E RID: 5150 RVA: 0x0007A072 File Offset: 0x00078272
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "version" };
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x0600141F RID: 5151 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001420 RID: 5152 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x0007A082 File Offset: 0x00078282
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the currently running version of the game and loaded mods";
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x0007A08C File Offset: 0x0007828C
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
