using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000287 RID: 647
[Preserve]
public class ConsoleCmdSetTempUnit : ConsoleCmdAbstract
{
	// Token: 0x060012F6 RID: 4854 RVA: 0x00075490 File Offset: 0x00073690
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "settempunit", "stu" };
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x000754A8 File Offset: 0x000736A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Set the current temperature units.";
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x000754AF File Offset: 0x000736AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set the current temperature units.\nUsage:\n  1. settempunit F\n  2. settempunit C\n1. sets the temperature unit to Fahrenheit.\n2. sets the temperature unit to Celsius.\n";
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060012FB RID: 4859 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x000754B8 File Offset: 0x000736B8
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
