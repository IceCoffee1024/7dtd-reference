using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028C RID: 652
[Preserve]
public class ConsoleCmdSetTriggerEffect : ConsoleCmdAbstract
{
	// Token: 0x06001300 RID: 4864 RVA: 0x00075A35 File Offset: 0x00073C35
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sette" };
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06001301 RID: 4865 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06001302 RID: 4866 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x00075A45 File Offset: 0x00073C45
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the UseTriggerEffects flag, if true controller trigger effects are to be used";
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x00075A4C File Offset: 0x00073C4C
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
