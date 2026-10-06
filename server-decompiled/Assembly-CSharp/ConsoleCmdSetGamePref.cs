using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000287 RID: 647
[Preserve]
public class ConsoleCmdSetGamePref : ConsoleCmdAbstract
{
	// Token: 0x060012E2 RID: 4834 RVA: 0x00075463 File Offset: 0x00073663
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamepref", "sg" };
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0007547C File Offset: 0x0007367C
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

	// Token: 0x060012E5 RID: 4837 RVA: 0x0007554C File Offset: 0x0007374C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game pref";
	}
}
