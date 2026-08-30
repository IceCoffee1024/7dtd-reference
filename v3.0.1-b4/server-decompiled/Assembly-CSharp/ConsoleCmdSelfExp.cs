using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200027E RID: 638
[Preserve]
public class ConsoleCmdSelfExp : ConsoleCmdAbstract
{
	// Token: 0x060012C7 RID: 4807 RVA: 0x00074A0A File Offset: 0x00072C0A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "giveselfxp" };
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x00074A1A File Offset: 0x00072C1A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Give yourself experience\nUsage:\n   giveselfxp <number> [1 (use xp bonuses)]";
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060012CA RID: 4810 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x00074A24 File Offset: 0x00072C24
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

	// Token: 0x060012CC RID: 4812 RVA: 0x00074ADD File Offset: 0x00072CDD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: giveselfxp 10000";
	}
}
