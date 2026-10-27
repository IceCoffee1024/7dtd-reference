using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002AC RID: 684
[Preserve]
public class ConsoleCmdStab : ConsoleCmdAbstract
{
	// Token: 0x060013AF RID: 5039 RVA: 0x00078697 File Offset: 0x00076897
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "stab" };
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x000786A7 File Offset: 0x000768A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "stability";
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x000786B0 File Offset: 0x000768B0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Running stability");
		GameManager.Instance.CreateStabilityViewer();
		if (_params.Count == 0)
		{
			GameManager.Instance.stabilityViewer.StartSearch(100);
			return;
		}
		if (_params.Count != 1)
		{
			return;
		}
		if (_params[0].EqualsCaseInsensitive("Clear"))
		{
			GameManager.Instance.ClearStabilityViewer();
			return;
		}
		if (_params[0].EqualsCaseInsensitive("Redo"))
		{
			GameManager.Instance.stabilityViewer.StartSearch(100);
			return;
		}
		int num = 31;
		int.TryParse(_params[0], out num);
		GameManager.Instance.stabilityViewer.StartSearch(num);
	}
}
