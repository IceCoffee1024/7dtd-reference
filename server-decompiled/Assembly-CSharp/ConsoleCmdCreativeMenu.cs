using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000202 RID: 514
[Preserve]
public class ConsoleCmdCreativeMenu : ConsoleCmdAbstract
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x00065A16 File Offset: 0x00063C16
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "creativemenu", "cm" };
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x00065A2E File Offset: 0x00063C2E
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.CreativeMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("creativemenu " + (GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x00065A6A File Offset: 0x00063C6A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the creativemenu";
	}
}
