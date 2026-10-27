using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021E RID: 542
[Preserve]
public class ConsoleCmdGetGamePrefs : ConsoleCmdAbstract
{
	// Token: 0x06001051 RID: 4177 RVA: 0x000678AC File Offset: 0x00065AAC
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

	// Token: 0x06001052 RID: 4178 RVA: 0x000678E7 File Offset: 0x00065AE7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamepref", "gg" };
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06001053 RID: 4179 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06001054 RID: 4180 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x000678FF File Offset: 0x00065AFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game preferences";
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x00067906 File Offset: 0x00065B06
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game preferences or only those matching a given substring";
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x00067910 File Offset: 0x00065B10
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

	// Token: 0x04000CAF RID: 3247
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "telnet", "adminfilename", "controlpanel", "password", "historycache", "userdatafolder", "options", "token", "last" };
}
