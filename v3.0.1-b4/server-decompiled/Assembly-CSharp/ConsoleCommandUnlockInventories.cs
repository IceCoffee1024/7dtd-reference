using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002B5 RID: 693
[Preserve]
public class ConsoleCommandUnlockInventories : ConsoleCmdAbstract
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06001405 RID: 5125 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06001406 RID: 5126 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsExecuteOnClient
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x000799A4 File Offset: 0x00077BA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force unlock inventories for everyone or a specific player.";
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x000799AB File Offset: 0x00077BAB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  unlock\n  unlock <player id>\n";
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x000799B2 File Offset: 0x00077BB2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "unlock" };
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x000799C4 File Offset: 0x00077BC4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		int num = -1;
		if (_params.Count == 2 && !int.TryParse(_params[1], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Failed to parse int for player id.");
			return;
		}
		if (num == -1)
		{
			LockManager.Instance.ForceUnlockAll();
			return;
		}
		LockManager.Instance.ForceUnlockByPlayer(num);
	}
}
