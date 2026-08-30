using System;
using UnityEngine.Scripting;

// Token: 0x02000246 RID: 582
[Preserve]
public class ConsoleCmdNetworkClient : ConsoleCmdNetworkServer
{
	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06001178 RID: 4472 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x0006E7C3 File Offset: 0x0006C9C3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "networkclient", "netc" };
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x0006E7DB File Offset: 0x0006C9DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Client side network commands";
	}
}
