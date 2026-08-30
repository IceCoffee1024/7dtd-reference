using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200020D RID: 525
[Preserve]
public class ConsoleCmdDebugWeather : ConsoleCmdAbstract
{
	// Token: 0x06000FFE RID: 4094 RVA: 0x000667C8 File Offset: 0x000649C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugweather" };
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06001000 RID: 4096 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06001001 RID: 4097 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x000667D8 File Offset: 0x000649D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dumps internal weather state to the console.";
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06001003 RID: 4099 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x000667E0 File Offset: 0x000649E0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count > 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Usage: debugweather");
			return;
		}
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Outside Temp: " + primaryPlayer.PlayerStats.CoreTemp.ToCultureInvariantString());
		}
	}
}
