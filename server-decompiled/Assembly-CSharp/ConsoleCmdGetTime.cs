using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200021E RID: 542
[Preserve]
public class ConsoleCmdGetTime : ConsoleCmdAbstract
{
	// Token: 0x06001079 RID: 4217 RVA: 0x00067850 File Offset: 0x00065A50
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "gettime", "gt" };
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x00067868 File Offset: 0x00065A68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the current game time";
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x0600107B RID: 4219 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x0600107C RID: 4220 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0006786F File Offset: 0x00065A6F
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ValueDisplayFormatters.WorldTime(GameManager.Instance.World.worldTime, "Day {0}, {1:00}:{2:00}"));
	}
}
