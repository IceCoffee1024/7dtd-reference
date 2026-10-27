using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200027C RID: 636
[Preserve]
public class ConsoleCmdReloadEntityClasses : ConsoleCmdAbstract
{
	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x00074C8B File Offset: 0x00072E8B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "reloadentityclasses", "rec" };
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x00074CA3 File Offset: 0x00072EA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "reloads entityclasses xml data.";
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x00074CAA File Offset: 0x00072EAA
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		WorldStaticData.Reset("entityclasses");
		WorldStaticData.Reset("entitybandits");
	}
}
