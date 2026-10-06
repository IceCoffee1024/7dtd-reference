using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000253 RID: 595
[Preserve]
public class ConsoleCmdOverrideServerMaxPlayerCount : ConsoleCmdAbstract
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060011A7 RID: 4519 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x0006F4FE File Offset: 0x0006D6FE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overridemaxplayercount" };
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x0006F50E File Offset: 0x0006D70E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Override Max Server Player Count";
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x0006F518 File Offset: 0x0006D718
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		int num;
		if (_params.Count >= 1 && int.TryParse(_params[0], out num))
		{
			GameModeSurvival.OverrideMaxPlayerCount = num;
			Log.Out(string.Format("Survival Max Player Count Override set to {0}", num));
			return;
		}
		Log.Out("Incorrect param, expected an integer for max player count.");
	}
}
