using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000279 RID: 633
[Preserve]
public class ConsoleCmdSaveWorld : ConsoleCmdAbstract
{
	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060012BD RID: 4797 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x0007479E File Offset: 0x0007299E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "saveworld", "sa" };
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x000747B6 File Offset: 0x000729B6
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

	// Token: 0x060012C0 RID: 4800 RVA: 0x000747E8 File Offset: 0x000729E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Saves the world manually.";
	}
}
