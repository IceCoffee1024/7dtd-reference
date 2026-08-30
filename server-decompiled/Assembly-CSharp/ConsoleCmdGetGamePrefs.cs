using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000219 RID: 537
[Preserve]
public class ConsoleCmdGetGamePrefs : ConsoleCmdAbstract
{
	// Token: 0x0600104F RID: 4175 RVA: 0x0006719C File Offset: 0x0006539C
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

	// Token: 0x06001050 RID: 4176 RVA: 0x000671D7 File Offset: 0x000653D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamepref", "gg" };
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06001051 RID: 4177 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06001052 RID: 4178 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x000671EF File Offset: 0x000653EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game preferences";
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x000671F6 File Offset: 0x000653F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game preferences or only those matching a given substring";
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x00067200 File Offset: 0x00065400
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

	// Token: 0x04000CA8 RID: 3240
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "telnet", "adminfilename", "controlpanel", "password", "historycache", "userdatafolder", "options", "last" };
}
