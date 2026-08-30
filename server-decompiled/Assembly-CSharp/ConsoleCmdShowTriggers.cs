using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000290 RID: 656
[Preserve]
public class ConsoleCmdShowTriggers : ConsoleCmdAbstract
{
	// Token: 0x06001332 RID: 4914 RVA: 0x00075BFC File Offset: 0x00073DFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showtriggers" };
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x00075C0C File Offset: 0x00073E0C
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

	// Token: 0x06001334 RID: 4916 RVA: 0x00075C7A File Offset: 0x00073E7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the visibility of the block triggers.";
	}
}
