using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000283 RID: 643
[Preserve]
public class ConsoleCmdSetTempUnit : ConsoleCmdAbstract
{
	// Token: 0x060012E4 RID: 4836 RVA: 0x00074E34 File Offset: 0x00073034
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "settempunit", "stu" };
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x00074E4C File Offset: 0x0007304C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Set the current temperature units.";
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00074E53 File Offset: 0x00073053
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set the current temperature units.\nUsage:\n  1. settempunit F\n  2. settempunit C\n1. sets the temperature unit to Fahrenheit.\n2. sets the temperature unit to Celsius.\n";
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x00074E5C File Offset: 0x0007305C
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
