using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000282 RID: 642
[Preserve]
public class ConsoleCmdSaveWorld : ConsoleCmdAbstract
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0007527E File Offset: 0x0007347E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "saveworld", "sa" };
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x00075296 File Offset: 0x00073496
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

	// Token: 0x060012D3 RID: 4819 RVA: 0x000752C8 File Offset: 0x000734C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Saves the world manually.";
	}
}
