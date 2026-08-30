using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000284 RID: 644
[Preserve]
public class ConsoleCmdSetGamePref : ConsoleCmdAbstract
{
	// Token: 0x060012E5 RID: 4837 RVA: 0x000751AF File Offset: 0x000733AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setgamepref", "sg" };
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x000751C8 File Offset: 0x000733C8
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

	// Token: 0x060012E8 RID: 4840 RVA: 0x00075298 File Offset: 0x00073498
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "sets a game pref";
	}
}
