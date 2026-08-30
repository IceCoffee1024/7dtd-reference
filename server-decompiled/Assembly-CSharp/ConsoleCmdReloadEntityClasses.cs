using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000277 RID: 631
[Preserve]
public class ConsoleCmdReloadEntityClasses : ConsoleCmdAbstract
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x00074807 File Offset: 0x00072A07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "reloadentityclasses", "rec" };
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x0007481F File Offset: 0x00072A1F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "reloads entityclasses xml data.";
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x00074826 File Offset: 0x00072A26
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		WorldStaticData.Reset("entityclasses");
		WorldStaticData.Reset("entitybandits");
	}
}
