using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200020F RID: 527
[Preserve]
public class ConsoleCmdDynamicProperties : ConsoleCmdAbstract
{
	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600100D RID: 4109 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x0600100E RID: 4110 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x00066736 File Offset: 0x00064936
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dynamicproperties", "dprop" };
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x00066750 File Offset: 0x00064950
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params[0] == "block")
		{
			if (_params.Count == 1)
			{
				Debug.LogError("Needs sub-command - cachestats");
				return;
			}
			if (_params[1] == "cachestats")
			{
				Block.CacheStats();
			}
		}
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0006679C File Offset: 0x0006499C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dynamic Properties debugging";
	}
}
