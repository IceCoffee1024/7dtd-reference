using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021A RID: 538
[Preserve]
public class ConsoleCmdGetGameStats : ConsoleCmdAbstract
{
	// Token: 0x06001057 RID: 4183 RVA: 0x0006734C File Offset: 0x0006554C
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

	// Token: 0x06001058 RID: 4184 RVA: 0x00067387 File Offset: 0x00065587
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamestat", "ggs" };
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06001059 RID: 4185 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600105A RID: 4186 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0006739F File Offset: 0x0006559F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game stats";
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x000673A6 File Offset: 0x000655A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game stats or only those matching a given substring";
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x000673B0 File Offset: 0x000655B0
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

	// Token: 0x04000CA9 RID: 3241
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "last" };
}
