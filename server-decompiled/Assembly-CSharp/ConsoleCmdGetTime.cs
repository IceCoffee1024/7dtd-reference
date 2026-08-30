using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000221 RID: 545
[Preserve]
public class ConsoleCmdGetTime : ConsoleCmdAbstract
{
	// Token: 0x06001085 RID: 4229 RVA: 0x00067DED File Offset: 0x00065FED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "gettime", "gt" };
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x00067E05 File Offset: 0x00066005
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the current game time";
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06001087 RID: 4231 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06001088 RID: 4232 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x00067E0C File Offset: 0x0006600C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ValueDisplayFormatters.WorldTime(GameManager.Instance.World.worldTime, "Day {0}, {1:00}:{2:00}"));
	}
}
