using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000288 RID: 648
[Preserve]
public class ConsoleCmdSetGameStat : ConsoleCmdAbstract
{
	// Token: 0x060012E7 RID: 4839 RVA: 0x00075553 File Offset: 0x00073753
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamestat", "sgs" };
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x0007556C File Offset: 0x0007376C
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

	// Token: 0x060012EA RID: 4842 RVA: 0x0007563C File Offset: 0x0007383C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game stat";
	}
}
