using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000295 RID: 661
[Preserve]
public class ConsoleCmdShutdown : ConsoleCmdAbstract
{
	// Token: 0x06001348 RID: 4936 RVA: 0x000762DD File Offset: 0x000744DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shutdown" };
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06001349 RID: 4937 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x000762ED File Offset: 0x000744ED
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Shutting server down...");
		Application.Quit();
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00076303 File Offset: 0x00074503
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shuts down the game";
	}
}
