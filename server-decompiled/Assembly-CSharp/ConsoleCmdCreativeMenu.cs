using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001FF RID: 511
[Preserve]
public class ConsoleCmdCreativeMenu : ConsoleCmdAbstract
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x00065856 File Offset: 0x00063A56
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "creativemenu", "cm" };
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0006586E File Offset: 0x00063A6E
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.CreativeMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("creativemenu " + (GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x000658AA File Offset: 0x00063AAA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the creativemenu";
	}
}
