using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021D RID: 541
[Preserve]
public class ConsoleCmdGetGameStats : ConsoleCmdAbstract
{
	// Token: 0x06001055 RID: 4181 RVA: 0x00067894 File Offset: 0x00065A94
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

	// Token: 0x06001056 RID: 4182 RVA: 0x000678CF File Offset: 0x00065ACF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamestat", "ggs" };
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06001057 RID: 4183 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06001058 RID: 4184 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x000678E7 File Offset: 0x00065AE7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game stats";
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x000678EE File Offset: 0x00065AEE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game stats or only those matching a given substring";
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x000678F8 File Offset: 0x00065AF8
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

	// Token: 0x04000CAF RID: 3247
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "last" };
}
