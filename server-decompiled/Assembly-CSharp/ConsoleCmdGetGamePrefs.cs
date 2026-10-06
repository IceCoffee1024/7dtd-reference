using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021C RID: 540
[Preserve]
public class ConsoleCmdGetGamePrefs : ConsoleCmdAbstract
{
	// Token: 0x0600104D RID: 4173 RVA: 0x000676DC File Offset: 0x000658DC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool prefAccessAllowed(EnumGamePrefs gp)
	{
		string text = gp.ToStringCached<EnumGamePrefs>();
		foreach (string text2 in this.forbiddenPrefs)
		{
			if (text.IndexOf(text2, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x00067717 File Offset: 0x00065917
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamepref", "gg" };
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x0600104F RID: 4175 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06001050 RID: 4176 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0006772F File Offset: 0x0006592F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game preferences";
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x00067736 File Offset: 0x00065936
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game preferences or only those matching a given substring";
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x00067740 File Offset: 0x00065940
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		string text = null;
		if (_params.Count > 0)
		{
			text = _params[0];
		}
		SortedList<string, string> sortedList = new SortedList<string, string>();
		foreach (EnumGamePrefs enumGamePrefs in EnumUtils.Values<EnumGamePrefs>())
		{
			if ((string.IsNullOrEmpty(text) || enumGamePrefs.ToStringCached<EnumGamePrefs>().ContainsCaseInsensitive(text)) && this.prefAccessAllowed(enumGamePrefs))
			{
				sortedList.Add(enumGamePrefs.ToStringCached<EnumGamePrefs>(), string.Format("GamePref.{0} = {1}", enumGamePrefs.ToStringCached<EnumGamePrefs>(), GamePrefs.GetObject(enumGamePrefs)));
			}
		}
		foreach (string text2 in sortedList.Keys)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(sortedList[text2]);
		}
	}

	// Token: 0x04000CAE RID: 3246
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "telnet", "adminfilename", "controlpanel", "password", "historycache", "userdatafolder", "options", "token", "last" };
}
