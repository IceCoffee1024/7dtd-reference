using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BE RID: 702
[Preserve]
public class ConsoleCommandUnlockInventories : ConsoleCmdAbstract
{
	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06001418 RID: 5144 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06001419 RID: 5145 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsExecuteOnClient
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x0007A484 File Offset: 0x00078684
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force unlock inventories for everyone or a specific player.";
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x0007A48B File Offset: 0x0007868B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  unlock\n  unlock <player id>\n";
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x0007A492 File Offset: 0x00078692
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "unlock" };
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x0007A4A4 File Offset: 0x000786A4
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
