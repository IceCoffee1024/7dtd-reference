using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200021E RID: 542
[Preserve]
public class ConsoleCmdGetLogfilePath : ConsoleCmdAbstract
{
	// Token: 0x0600105D RID: 4189 RVA: 0x00067A00 File Offset: 0x00065C00
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getlogpath", "glp" };
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600105E RID: 4190 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x0600105F RID: 4191 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06001060 RID: 4192 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x00067A18 File Offset: 0x00065C18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the path of the logfile the game currently writes to";
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x00067A1F File Offset: 0x00065C1F
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(Application.consoleLogPath);
	}
}
