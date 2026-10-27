using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028A RID: 650
[Preserve]
public class ConsoleCmdSetGameStat : ConsoleCmdAbstract
{
	// Token: 0x060012EB RID: 4843 RVA: 0x00075723 File Offset: 0x00073923
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamestat", "sgs" };
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060012EC RID: 4844 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x0007573C File Offset: 0x0007393C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Parameters: <game stat> <value>");
			return;
		}
		EnumGameStats enumGameStats;
		try
		{
			enumGameStats = EnumUtils.Parse<EnumGameStats>(_params[0], true);
		}
		catch (Exception)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Error parsing parameter: " + _params[0]);
			return;
		}
		object obj;
		try
		{
			obj = GameStats.Parse(enumGameStats, _params[1]);
		}
		catch (Exception)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Error parsing value: " + _params[1]);
			return;
		}
		GameStats.SetObject(enumGameStats, obj);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(enumGameStats.ToStringCached<EnumGameStats>() + " set to " + ((obj != null) ? obj.ToString() : null));
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x0007580C File Offset: 0x00073A0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game stat";
	}
}
