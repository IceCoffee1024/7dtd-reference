using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200021B RID: 539
[Preserve]
public class ConsoleCmdGetLogfilePath : ConsoleCmdAbstract
{
	// Token: 0x0600105F RID: 4191 RVA: 0x000674B8 File Offset: 0x000656B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getlogpath", "glp" };
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06001060 RID: 4192 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06001061 RID: 4193 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06001062 RID: 4194 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x000674D0 File Offset: 0x000656D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the path of the logfile the game currently writes to";
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x000674D7 File Offset: 0x000656D7
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(Application.consoleLogPath);
	}
}
