using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A3 RID: 675
[Preserve]
public class ConsoleCmdStab : ConsoleCmdAbstract
{
	// Token: 0x0600139C RID: 5020 RVA: 0x00077BB7 File Offset: 0x00075DB7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "stab" };
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x00077BC7 File Offset: 0x00075DC7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "stability";
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00077BD0 File Offset: 0x00075DD0
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
