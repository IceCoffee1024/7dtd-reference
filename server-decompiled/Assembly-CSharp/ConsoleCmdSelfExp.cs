using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000282 RID: 642
[Preserve]
public class ConsoleCmdSelfExp : ConsoleCmdAbstract
{
	// Token: 0x060012D9 RID: 4825 RVA: 0x00075066 File Offset: 0x00073266
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "giveselfxp" };
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00075076 File Offset: 0x00073276
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Give yourself experience\nUsage:\n   giveselfxp <number> [1 (use xp bonuses)]";
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060012DB RID: 4827 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060012DC RID: 4828 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x00075080 File Offset: 0x00073280
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

	// Token: 0x060012DE RID: 4830 RVA: 0x00075139 File Offset: 0x00073339
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: giveselfxp 10000";
	}
}
