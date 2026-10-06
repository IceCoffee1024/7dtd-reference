using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023D RID: 573
[Preserve]
public class ConsoleCmdLogFellThroughWorldDebugInfo : ConsoleCmdAbstract
{
	// Token: 0x0600111D RID: 4381 RVA: 0x0006CCA4 File Offset: 0x0006AEA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ftw", "fellthroughworld" };
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x0006CCBC File Offset: 0x0006AEBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Log the fell through world debug information for testing purposes.";
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x0600111F RID: 4383 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x0006CCC3 File Offset: 0x0006AEC3
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.GetPrimaryPlayer().LogFellThroughWorldDebugInfo();
	}
}
