using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000252 RID: 594
[Preserve]
public class ConsoleCmdNewWeatherSurvival : ConsoleCmdAbstract
{
	// Token: 0x0600119A RID: 4506 RVA: 0x0006F40D File Offset: 0x0006D60D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "newweathersurvival" };
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x0006F41D File Offset: 0x0006D61D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enables/disables new weather survival";
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x0006F424 File Offset: 0x0006D624
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
