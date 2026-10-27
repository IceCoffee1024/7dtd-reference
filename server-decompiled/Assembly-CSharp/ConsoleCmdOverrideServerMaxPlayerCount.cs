using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000255 RID: 597
[Preserve]
public class ConsoleCmdOverrideServerMaxPlayerCount : ConsoleCmdAbstract
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060011AB RID: 4523 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x0006F6CE File Offset: 0x0006D8CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overridemaxplayercount" };
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x0006F6DE File Offset: 0x0006D8DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Override Max Server Player Count";
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x0006F6E8 File Offset: 0x0006D8E8
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
