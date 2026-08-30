using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000273 RID: 627
[Preserve]
public class ConsoleCmdReloadEntityClasses : ConsoleCmdAbstract
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06001298 RID: 4760 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x000741AB File Offset: 0x000723AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "reloadentityclasses", "rec" };
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x000741C3 File Offset: 0x000723C3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "reloads entityclasses xml data.";
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x000741CA File Offset: 0x000723CA
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		WorldStaticData.Reset("entityclasses");
		WorldStaticData.Reset("entitybandits");
	}
}
