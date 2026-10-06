using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000200 RID: 512
[Preserve]
public class ConsoleCmdCreativeMenu : ConsoleCmdAbstract
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x00065846 File Offset: 0x00063A46
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "creativemenu", "cm" };
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0006585E File Offset: 0x00063A5E
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.CreativeMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("creativemenu " + (GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0006589A File Offset: 0x00063A9A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the creativemenu";
	}
}
