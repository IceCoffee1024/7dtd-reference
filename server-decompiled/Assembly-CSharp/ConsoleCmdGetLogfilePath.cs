using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000220 RID: 544
[Preserve]
public class ConsoleCmdGetLogfilePath : ConsoleCmdAbstract
{
	// Token: 0x06001061 RID: 4193 RVA: 0x00067BD0 File Offset: 0x00065DD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getlogpath", "glp" };
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06001062 RID: 4194 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06001063 RID: 4195 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x00067BE8 File Offset: 0x00065DE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Get the path of the logfile the game currently writes to";
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x00067BEF File Offset: 0x00065DEF
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(Application.consoleLogPath);
	}
}
