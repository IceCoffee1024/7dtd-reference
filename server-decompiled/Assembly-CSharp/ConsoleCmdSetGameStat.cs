using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000281 RID: 641
[Preserve]
public class ConsoleCmdSetGameStat : ConsoleCmdAbstract
{
	// Token: 0x060012D8 RID: 4824 RVA: 0x00074C43 File Offset: 0x00072E43
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamestat", "sgs" };
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00074C5C File Offset: 0x00072E5C
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

	// Token: 0x060012DB RID: 4827 RVA: 0x00074D2C File Offset: 0x00072F2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game stat";
	}
}
