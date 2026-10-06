using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000285 RID: 645
[Preserve]
public class ConsoleCmdSelfExp : ConsoleCmdAbstract
{
	// Token: 0x060012D6 RID: 4822 RVA: 0x0007531A File Offset: 0x0007351A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "giveselfxp" };
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x0007532A File Offset: 0x0007352A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Give yourself experience\nUsage:\n   giveselfxp <number> [1 (use xp bonuses)]";
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060012D8 RID: 4824 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060012D9 RID: 4825 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00075334 File Offset: 0x00073534
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("cannot execute giveselfxp on dedicated server, please execute as a client");
		}
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("giveselfxp requires xp amount");
			return;
		}
		float num;
		if (!float.TryParse(_params[0], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("xp amount must be a number.");
			return;
		}
		if (num < 0f)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("xp amount must be positive.");
			return;
		}
		num = Mathf.Clamp(num, 0f, 1.0737418E+09f);
		bool flag = _params.Count >= 2;
		GameManager.Instance.World.GetPrimaryPlayer().Progression.AddLevelExp((int)num, "_xpOther", Progression.XPTypes.Debug, flag, true, -1, null);
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x000753ED File Offset: 0x000735ED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: giveselfxp 10000";
	}
}
