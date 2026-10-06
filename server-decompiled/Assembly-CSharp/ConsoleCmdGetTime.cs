using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000222 RID: 546
[Preserve]
public class ConsoleCmdGetTime : ConsoleCmdAbstract
{
	// Token: 0x06001078 RID: 4216 RVA: 0x00067DDD File Offset: 0x00065FDD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "gettime", "gt" };
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x00067DF5 File Offset: 0x00065FF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the current game time";
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x0600107A RID: 4218 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x0600107B RID: 4219 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x00067DFC File Offset: 0x00065FFC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ValueDisplayFormatters.WorldTime(GameManager.Instance.World.worldTime, "Day {0}, {1:00}:{2:00}"));
	}
}
