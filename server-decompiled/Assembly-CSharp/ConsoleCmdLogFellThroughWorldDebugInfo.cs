using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023F RID: 575
[Preserve]
public class ConsoleCmdLogFellThroughWorldDebugInfo : ConsoleCmdAbstract
{
	// Token: 0x06001121 RID: 4385 RVA: 0x0006CE74 File Offset: 0x0006B074
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ftw", "fellthroughworld" };
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x0006CE8C File Offset: 0x0006B08C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Log the fell through world debug information for testing purposes.";
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06001123 RID: 4387 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x0006CE93 File Offset: 0x0006B093
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.GetPrimaryPlayer().LogFellThroughWorldDebugInfo();
	}
}
