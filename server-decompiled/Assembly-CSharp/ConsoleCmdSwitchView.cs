using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002AC RID: 684
[Preserve]
public class ConsoleCmdSwitchView : ConsoleCmdAbstract
{
	// Token: 0x060013B5 RID: 5045 RVA: 0x0007862E File Offset: 0x0007682E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "switchview", "sv" };
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x00078646 File Offset: 0x00076846
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Switch between fpv and tpv";
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x00078650 File Offset: 0x00076850
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
