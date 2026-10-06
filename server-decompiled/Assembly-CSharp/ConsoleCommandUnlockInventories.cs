using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BC RID: 700
[Preserve]
public class ConsoleCommandUnlockInventories : ConsoleCmdAbstract
{
	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06001414 RID: 5140 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06001415 RID: 5141 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsExecuteOnClient
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0007A2B4 File Offset: 0x000784B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force unlock inventories for everyone or a specific player.";
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x0007A2BB File Offset: 0x000784BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  unlock\n  unlock <player id>\n";
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x0007A2C2 File Offset: 0x000784C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "unlock" };
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x0007A2D4 File Offset: 0x000784D4
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
