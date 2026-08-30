using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027D RID: 637
[Preserve]
public class ConsoleCmdSaveWorld : ConsoleCmdAbstract
{
	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060012CF RID: 4815 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x00074DFA File Offset: 0x00072FFA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "saveworld", "sa" };
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x00074E12 File Offset: 0x00073012
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		GameManager.Instance.SaveLocalPlayerData();
		GameManager.Instance.SaveWorld();
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("World saved");
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x00074E44 File Offset: 0x00073044
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Saves the world manually.";
	}
}
