using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002AA RID: 682
[Preserve]
public class ConsoleCmdStab : ConsoleCmdAbstract
{
	// Token: 0x060013AB RID: 5035 RVA: 0x000784C7 File Offset: 0x000766C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "stab" };
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x000784D7 File Offset: 0x000766D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "stability";
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000784E0 File Offset: 0x000766E0
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
