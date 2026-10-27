using System;
using UnityEngine.Scripting;

// Token: 0x0200024F RID: 591
[Preserve]
public class ConsoleCmdNetworkClient : ConsoleCmdNetworkServer
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x0600118B RID: 4491 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x0006F29B File Offset: 0x0006D49B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "networkclient", "netc" };
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x0006F2B3 File Offset: 0x0006D4B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Client side network commands";
	}
}
