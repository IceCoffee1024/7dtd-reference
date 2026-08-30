using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027A RID: 634
[Preserve]
public class ConsoleCmdResetAchievementStats : ConsoleCmdAbstract
{
	// Token: 0x060012BB RID: 4795 RVA: 0x00074AF7 File Offset: 0x00072CF7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "resetallstats" };
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060012BC RID: 4796 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x060012BD RID: 4797 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x00074B07 File Offset: 0x00072D07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Resets all achievement stats (and achievements when parameter is true)";
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x00074B10 File Offset: 0x00072D10
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
