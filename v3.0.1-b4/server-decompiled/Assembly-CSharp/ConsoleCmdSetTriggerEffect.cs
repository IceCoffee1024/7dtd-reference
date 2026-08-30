using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000285 RID: 645
[Preserve]
public class ConsoleCmdSetTriggerEffect : ConsoleCmdAbstract
{
	// Token: 0x060012F1 RID: 4849 RVA: 0x00075125 File Offset: 0x00073325
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sette" };
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060012F2 RID: 4850 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x00075135 File Offset: 0x00073335
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the UseTriggerEffects flag, if true controller trigger effects are to be used";
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0007513C File Offset: 0x0007333C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Client only setting, please execute as a client");
			return;
		}
		if (_params.Count > 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Usage: sette <on/true/y/1/off/false/n/0>");
			return;
		}
		bool flag = false;
		if (_params.Count != 0)
		{
			flag = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerTriggerEffects);
		}
		if (_params.Count == 1)
		{
			try
			{
				flag = ConsoleHelper.ParseParamBool(_params[0], false);
			}
			catch
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Usage: sette <on/true/y/1/off/false/n/0>");
				return;
			}
		}
		GamePrefs.Set(EnumGamePrefs.OptionsControllerTriggerEffects, flag);
		GamePrefs.Instance.Save();
		foreach (EntityPlayerLocal entityPlayerLocal in GameManager.Instance.World.GetLocalPlayers())
		{
			GameManager.Instance.triggerEffectManager.PollSetting();
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("UseTriggerEffects now set to: {0}", flag));
	}
}
