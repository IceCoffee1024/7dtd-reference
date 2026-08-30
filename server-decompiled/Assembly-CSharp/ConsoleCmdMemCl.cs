using System;
using UnityEngine.Scripting;

// Token: 0x02000242 RID: 578
[Preserve]
public class ConsoleCmdMemCl : ConsoleCmdMem
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x0600115E RID: 4446 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x0006E4B5 File Offset: 0x0006C6B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "memcl" };
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x0006E4C5 File Offset: 0x0006C6C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Prints memory information on client and calls garbage collector";
	}
}
