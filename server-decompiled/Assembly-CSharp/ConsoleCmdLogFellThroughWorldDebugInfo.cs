using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023A RID: 570
[Preserve]
public class ConsoleCmdLogFellThroughWorldDebugInfo : ConsoleCmdAbstract
{
	// Token: 0x06001120 RID: 4384 RVA: 0x0006C9FC File Offset: 0x0006ABFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ftw", "fellthroughworld" };
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x0006CA14 File Offset: 0x0006AC14
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Log the fell through world debug information for testing purposes.";
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06001122 RID: 4386 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x0006CA1B File Offset: 0x0006AC1B
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.GetPrimaryPlayer().LogFellThroughWorldDebugInfo();
	}
}
