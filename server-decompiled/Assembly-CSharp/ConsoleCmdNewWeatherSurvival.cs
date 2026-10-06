using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000250 RID: 592
[Preserve]
public class ConsoleCmdNewWeatherSurvival : ConsoleCmdAbstract
{
	// Token: 0x06001196 RID: 4502 RVA: 0x0006F23D File Offset: 0x0006D43D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "newweathersurvival" };
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x0006F24D File Offset: 0x0006D44D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enables/disables new weather survival";
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x0006F254 File Offset: 0x0006D454
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count > 0)
		{
			if (_params.Count > 1 || (_params[0] != "on" && _params[0] != "off"))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Usage: newweathersurvival [on/off]");
				return;
			}
			EntityStats.NewWeatherSurvivalEnabled = _params[0] == "on";
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("New Weather survival is " + (EntityStats.NewWeatherSurvivalEnabled ? "on" : "off"));
	}
}
