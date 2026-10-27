using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000289 RID: 649
[Preserve]
public class ConsoleCmdSetGamePref : ConsoleCmdAbstract
{
	// Token: 0x060012E6 RID: 4838 RVA: 0x00075633 File Offset: 0x00073833
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamepref", "sg" };
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x0007564C File Offset: 0x0007384C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Parameters: <game pref> <value>");
			return;
		}
		EnumGamePrefs enumGamePrefs;
		try
		{
			enumGamePrefs = EnumUtils.Parse<EnumGamePrefs>(_params[0], true);
		}
		catch (Exception)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Error parsing parameter: " + _params[0]);
			return;
		}
		object obj;
		try
		{
			obj = GamePrefs.Parse(enumGamePrefs, _params[1]);
		}
		catch (Exception)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Error parsing value: " + _params[1]);
			return;
		}
		GamePrefs.SetObject(enumGamePrefs, obj);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(enumGamePrefs.ToStringCached<EnumGamePrefs>() + " set to " + ((obj != null) ? obj.ToString() : null));
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x0007571C File Offset: 0x0007391C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game pref";
	}
}
