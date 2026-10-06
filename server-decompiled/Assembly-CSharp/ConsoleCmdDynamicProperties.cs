using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000212 RID: 530
[Preserve]
public class ConsoleCmdDynamicProperties : ConsoleCmdAbstract
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600100B RID: 4107 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600100C RID: 4108 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x00066C6E File Offset: 0x00064E6E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dynamicproperties", "dprop" };
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x00066C88 File Offset: 0x00064E88
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

	// Token: 0x0600100F RID: 4111 RVA: 0x00066CD4 File Offset: 0x00064ED4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dynamic Properties debugging";
	}
}
