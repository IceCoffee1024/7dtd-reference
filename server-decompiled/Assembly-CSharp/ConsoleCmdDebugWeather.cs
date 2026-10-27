using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000210 RID: 528
[Preserve]
public class ConsoleCmdDebugWeather : ConsoleCmdAbstract
{
	// Token: 0x06000FF5 RID: 4085 RVA: 0x00066988 File Offset: 0x00064B88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugweather" };
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x00066998 File Offset: 0x00064B98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dumps internal weather state to the console.";
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x000669A0 File Offset: 0x00064BA0
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
