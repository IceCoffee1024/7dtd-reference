using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000280 RID: 640
[Preserve]
public class ConsoleCmdSaveWorld : ConsoleCmdAbstract
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060012CC RID: 4812 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x000750AE File Offset: 0x000732AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "saveworld", "sa" };
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x000750C6 File Offset: 0x000732C6
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

	// Token: 0x060012CF RID: 4815 RVA: 0x000750F8 File Offset: 0x000732F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Saves the world manually.";
	}
}
