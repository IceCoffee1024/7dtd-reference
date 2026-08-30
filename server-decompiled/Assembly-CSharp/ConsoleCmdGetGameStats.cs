using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021C RID: 540
[Preserve]
public class ConsoleCmdGetGameStats : ConsoleCmdAbstract
{
	// Token: 0x06001062 RID: 4194 RVA: 0x000678A4 File Offset: 0x00065AA4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool prefAccessAllowed(EnumGameStats gp)
	{
		string text = gp.ToStringCached<EnumGameStats>();
		foreach (string text2 in this.forbiddenPrefs)
		{
			if (text.IndexOf(text2, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x000678DF File Offset: 0x00065ADF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamestat", "ggs" };
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x000678F7 File Offset: 0x00065AF7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game stats";
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x000678FE File Offset: 0x00065AFE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game stats or only those matching a given substring";
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x00067908 File Offset: 0x00065B08
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		string text = null;
		if (_params.Count > 0)
		{
			text = _params[0];
		}
		SortedList<string, string> sortedList = new SortedList<string, string>();
		foreach (EnumGameStats enumGameStats in EnumUtils.Values<EnumGameStats>())
		{
			if ((string.IsNullOrEmpty(text) || enumGameStats.ToStringCached<EnumGameStats>().ContainsCaseInsensitive(text)) && this.prefAccessAllowed(enumGameStats))
			{
				sortedList.Add(enumGameStats.ToStringCached<EnumGameStats>(), string.Format("GameStat.{0} = {1}", enumGameStats.ToStringCached<EnumGameStats>(), GameStats.GetObject(enumGameStats)));
			}
		}
		foreach (string text2 in sortedList.Keys)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(sortedList[text2]);
		}
	}

	// Token: 0x04000CB6 RID: 3254
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "last" };
}
