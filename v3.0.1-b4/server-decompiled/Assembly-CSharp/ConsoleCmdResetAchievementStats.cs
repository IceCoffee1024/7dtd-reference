using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000276 RID: 630
[Preserve]
public class ConsoleCmdResetAchievementStats : ConsoleCmdAbstract
{
	// Token: 0x060012A9 RID: 4777 RVA: 0x0007449B File Offset: 0x0007269B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "resetallstats" };
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x000744AB File Offset: 0x000726AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Resets all achievement stats (and achievements when parameter is true)";
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x000744B4 File Offset: 0x000726B4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("cannot execute resetallstats on dedicated server, please execute as a client");
			return;
		}
		IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
		if (achievementManager == null)
		{
			return;
		}
		achievementManager.ResetStats(_params.Count > 0 && ConsoleHelper.ParseParamBool(_params[0], true));
	}
}
