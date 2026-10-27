using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000214 RID: 532
[Preserve]
public class ConsoleCmdDynamicProperties : ConsoleCmdAbstract
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600100F RID: 4111 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06001010 RID: 4112 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x00066E3E File Offset: 0x0006503E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dynamicproperties", "dprop" };
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x00066E58 File Offset: 0x00065058
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

	// Token: 0x06001013 RID: 4115 RVA: 0x00066EA4 File Offset: 0x000650A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dynamic Properties debugging";
	}
}
