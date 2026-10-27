using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000299 RID: 665
[Preserve]
public class ConsoleCmdShowTriggers : ConsoleCmdAbstract
{
	// Token: 0x06001345 RID: 4933 RVA: 0x000766DC File Offset: 0x000748DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showtriggers" };
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x000766EC File Offset: 0x000748EC
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

	// Token: 0x06001347 RID: 4935 RVA: 0x0007675A File Offset: 0x0007495A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the visibility of the block triggers.";
	}
}
