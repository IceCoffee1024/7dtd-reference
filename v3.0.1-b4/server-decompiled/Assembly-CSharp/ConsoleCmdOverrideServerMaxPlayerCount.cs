using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024C RID: 588
[Preserve]
public class ConsoleCmdOverrideServerMaxPlayerCount : ConsoleCmdAbstract
{
	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06001198 RID: 4504 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x0006EBF6 File Offset: 0x0006CDF6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overridemaxplayercount" };
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x0006EC06 File Offset: 0x0006CE06
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Override Max Server Player Count";
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x0006EC10 File Offset: 0x0006CE10
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
