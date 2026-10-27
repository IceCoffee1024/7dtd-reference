using System;
using UnityEngine.Scripting;

// Token: 0x0200024B RID: 587
[Preserve]
public class ConsoleCmdMemCl : ConsoleCmdMem
{
	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001171 RID: 4465 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x0006EF8D File Offset: 0x0006D18D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "memcl" };
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x0006EF9D File Offset: 0x0006D19D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Prints memory information on client and calls garbage collector";
	}
}
