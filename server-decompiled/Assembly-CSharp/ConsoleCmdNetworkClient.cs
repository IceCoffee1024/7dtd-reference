using System;
using UnityEngine.Scripting;

// Token: 0x0200024D RID: 589
[Preserve]
public class ConsoleCmdNetworkClient : ConsoleCmdNetworkServer
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06001187 RID: 4487 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x0006F0CB File Offset: 0x0006D2CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "networkclient", "netc" };
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x0006F0E3 File Offset: 0x0006D2E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Client side network commands";
	}
}
