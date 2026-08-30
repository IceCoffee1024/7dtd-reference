using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A5 RID: 677
[Preserve]
public class ConsoleCmdSwitchView : ConsoleCmdAbstract
{
	// Token: 0x060013A6 RID: 5030 RVA: 0x00077D1E File Offset: 0x00075F1E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "switchview", "sv" };
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x00077D36 File Offset: 0x00075F36
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Switch between fpv and tpv";
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x00077D40 File Offset: 0x00075F40
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
