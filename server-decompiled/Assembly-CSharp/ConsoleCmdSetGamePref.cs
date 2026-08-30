using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000280 RID: 640
[Preserve]
public class ConsoleCmdSetGamePref : ConsoleCmdAbstract
{
	// Token: 0x060012D3 RID: 4819 RVA: 0x00074B53 File Offset: 0x00072D53
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamepref", "sg" };
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x060012D4 RID: 4820 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00074B6C File Offset: 0x00072D6C
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

	// Token: 0x060012D6 RID: 4822 RVA: 0x00074C3C File Offset: 0x00072E3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game pref";
	}
}
