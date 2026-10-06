using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000298 RID: 664
[Preserve]
public class ConsoleCmdShutdown : ConsoleCmdAbstract
{
	// Token: 0x06001345 RID: 4933 RVA: 0x00076591 File Offset: 0x00074791
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shutdown" };
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06001346 RID: 4934 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x000765A1 File Offset: 0x000747A1
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Shutting server down...");
		Application.Quit();
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x000765B7 File Offset: 0x000747B7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shuts down the game";
	}
}
