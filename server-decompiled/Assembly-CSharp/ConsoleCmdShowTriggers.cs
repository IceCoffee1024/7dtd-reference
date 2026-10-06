using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000297 RID: 663
[Preserve]
public class ConsoleCmdShowTriggers : ConsoleCmdAbstract
{
	// Token: 0x06001341 RID: 4929 RVA: 0x0007650C File Offset: 0x0007470C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showtriggers" };
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0007651C File Offset: 0x0007471C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		bool flag = !GameManager.Instance.World.triggerManager.ShowNavObjects;
		GameManager.Instance.World.triggerManager.ShowNavObjects = flag;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Show Triggers {0}!", new object[] { flag ? "Enabled" : "Disabled" });
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0007658A File Offset: 0x0007478A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the visibility of the block triggers.";
	}
}
