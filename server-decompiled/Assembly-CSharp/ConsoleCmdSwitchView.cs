using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002AE RID: 686
[Preserve]
public class ConsoleCmdSwitchView : ConsoleCmdAbstract
{
	// Token: 0x060013B9 RID: 5049 RVA: 0x000787FE File Offset: 0x000769FE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "switchview", "sv" };
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x00078816 File Offset: 0x00076A16
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Switch between fpv and tpv";
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x00078820 File Offset: 0x00076A20
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.World.GetPrimaryPlayer() == null)
		{
			return;
		}
		GameManager.Instance.World.GetPrimaryPlayer().SwitchFirstPersonViewFromInput();
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Switched to " + (GameManager.Instance.World.GetPrimaryPlayer().bFirstPersonView ? "FPV" : "TPV"));
	}
}
