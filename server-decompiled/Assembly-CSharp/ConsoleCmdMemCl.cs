using System;
using UnityEngine.Scripting;

// Token: 0x02000249 RID: 585
[Preserve]
public class ConsoleCmdMemCl : ConsoleCmdMem
{
	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x0600116D RID: 4461 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x0600116E RID: 4462 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x0006EDBD File Offset: 0x0006CFBD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "memcl" };
	}

	// Token: 0x06001170 RID: 4464 RVA: 0x0006EDCD File Offset: 0x0006CFCD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Prints memory information on client and calls garbage collector";
	}
}
