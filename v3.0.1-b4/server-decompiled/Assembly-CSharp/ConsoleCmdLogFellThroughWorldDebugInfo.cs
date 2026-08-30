using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000236 RID: 566
[Preserve]
public class ConsoleCmdLogFellThroughWorldDebugInfo : ConsoleCmdAbstract
{
	// Token: 0x0600110E RID: 4366 RVA: 0x0006C39F File Offset: 0x0006A59F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ftw", "fellthroughworld" };
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0006C3B7 File Offset: 0x0006A5B7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Log the fell through world debug information for testing purposes.";
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06001110 RID: 4368 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x0006C3BE File Offset: 0x0006A5BE
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.GetPrimaryPlayer().LogFellThroughWorldDebugInfo();
	}
}
