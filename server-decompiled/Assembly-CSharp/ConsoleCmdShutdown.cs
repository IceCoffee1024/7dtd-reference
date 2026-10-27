using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200029A RID: 666
[Preserve]
public class ConsoleCmdShutdown : ConsoleCmdAbstract
{
	// Token: 0x06001349 RID: 4937 RVA: 0x00076761 File Offset: 0x00074961
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shutdown" };
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x0600134A RID: 4938 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00076771 File Offset: 0x00074971
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Shutting server down...");
		Application.Quit();
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x00076787 File Offset: 0x00074987
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shuts down the game";
	}
}
