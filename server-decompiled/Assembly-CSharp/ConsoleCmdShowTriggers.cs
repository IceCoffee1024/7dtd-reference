using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000294 RID: 660
[Preserve]
public class ConsoleCmdShowTriggers : ConsoleCmdAbstract
{
	// Token: 0x06001344 RID: 4932 RVA: 0x00076258 File Offset: 0x00074458
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showtriggers" };
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x00076268 File Offset: 0x00074468
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

	// Token: 0x06001346 RID: 4934 RVA: 0x000762D6 File Offset: 0x000744D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the visibility of the block triggers.";
	}
}
