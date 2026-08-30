using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000249 RID: 585
[Preserve]
public class ConsoleCmdNewWeatherSurvival : ConsoleCmdAbstract
{
	// Token: 0x06001187 RID: 4487 RVA: 0x0006E935 File Offset: 0x0006CB35
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "newweathersurvival" };
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x0006E945 File Offset: 0x0006CB45
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enables/disables new weather survival";
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x0006E94C File Offset: 0x0006CB4C
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
