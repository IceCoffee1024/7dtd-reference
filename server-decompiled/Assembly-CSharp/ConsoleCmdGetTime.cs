using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000224 RID: 548
[Preserve]
public class ConsoleCmdGetTime : ConsoleCmdAbstract
{
	// Token: 0x0600107C RID: 4220 RVA: 0x00067FAD File Offset: 0x000661AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "gettime", "gt" };
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x00067FC5 File Offset: 0x000661C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the current game time";
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x0600107E RID: 4222 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x0600107F RID: 4223 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x00067FCC File Offset: 0x000661CC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ValueDisplayFormatters.WorldTime(GameManager.Instance.World.worldTime, "Day {0}, {1:00}:{2:00}"));
	}
}
