using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028C RID: 652
[Preserve]
public class ConsoleCmdSetTempUnit : ConsoleCmdAbstract
{
	// Token: 0x060012F7 RID: 4855 RVA: 0x00075914 File Offset: 0x00073B14
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "settempunit", "stu" };
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0007592C File Offset: 0x00073B2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Set the current temperature units.";
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060012FA RID: 4858 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x00075933 File Offset: 0x00073B33
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set the current temperature units.\nUsage:\n  1. settempunit F\n  2. settempunit C\n1. sets the temperature unit to Fahrenheit.\n2. sets the temperature unit to Celsius.\n";
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060012FC RID: 4860 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0007593C File Offset: 0x00073B3C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 1)
		{
			if (_params[0].EqualsCaseInsensitive("f"))
			{
				GamePrefs.SetObject(EnumGamePrefs.OptionsTempCelsius, false);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Set temperature units to Fahrenheit.");
			}
			else
			{
				if (!_params[0].EqualsCaseInsensitive("c"))
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid value for single argument variant: \"" + _params[0] + "\"");
					return;
				}
				GamePrefs.SetObject(EnumGamePrefs.OptionsTempCelsius, true);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Set temperature units to Celsius.");
			}
			GamePrefs.Instance.Save();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 1, found " + _params.Count.ToString() + ".");
	}
}
