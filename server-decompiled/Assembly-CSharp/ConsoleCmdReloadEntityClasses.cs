using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200027A RID: 634
[Preserve]
public class ConsoleCmdReloadEntityClasses : ConsoleCmdAbstract
{
	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060012A7 RID: 4775 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x00074ABB File Offset: 0x00072CBB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "reloadentityclasses", "rec" };
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x00074AD3 File Offset: 0x00072CD3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "reloads entityclasses xml data.";
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x00074ADA File Offset: 0x00072CDA
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		WorldStaticData.Reset("entityclasses");
		WorldStaticData.Reset("entitybandits");
	}
}
