using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200020E RID: 526
[Preserve]
public class ConsoleCmdDebugWeather : ConsoleCmdAbstract
{
	// Token: 0x06000FF1 RID: 4081 RVA: 0x000667B8 File Offset: 0x000649B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugweather" };
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x000667C8 File Offset: 0x000649C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dumps internal weather state to the console.";
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x000667D0 File Offset: 0x000649D0
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
