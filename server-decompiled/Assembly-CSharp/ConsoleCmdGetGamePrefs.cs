using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021B RID: 539
[Preserve]
public class ConsoleCmdGetGamePrefs : ConsoleCmdAbstract
{
	// Token: 0x0600105A RID: 4186 RVA: 0x000676EC File Offset: 0x000658EC
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

	// Token: 0x0600105B RID: 4187 RVA: 0x00067727 File Offset: 0x00065927
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getgamepref", "gg" };
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x0600105C RID: 4188 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600105D RID: 4189 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0006773F File Offset: 0x0006593F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game preferences";
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x00067746 File Offset: 0x00065946
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game preferences or only those matching a given substring";
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x00067750 File Offset: 0x00065950
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

	// Token: 0x04000CB5 RID: 3253
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] forbiddenPrefs = new string[] { "telnet", "adminfilename", "controlpanel", "password", "historycache", "userdatafolder", "options", "token", "last" };
}
