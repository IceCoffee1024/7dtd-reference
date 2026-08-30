using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002BC RID: 700
[Preserve]
public class ConsoleCmdWeatherSurvival : ConsoleCmdAbstract
{
	// Token: 0x0600143D RID: 5181 RVA: 0x0007ACCF File Offset: 0x00078ECF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "weathersurvival" };
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x0007ACDF File Offset: 0x00078EDF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enables/disables weather survival";
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x0007ACE8 File Offset: 0x00078EE8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count > 0)
		{
			if (_params.Count > 1 || (_params[0] != "on" && _params[0] != "off"))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Usage: weathersurvival [on/off]");
				return;
			}
			EntityStats.WeatherSurvivalEnabled = _params[0] == "on";
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Weather survival is " + (EntityStats.WeatherSurvivalEnabled ? "on" : "off"));
	}
}
