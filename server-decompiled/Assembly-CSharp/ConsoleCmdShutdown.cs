using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000291 RID: 657
[Preserve]
public class ConsoleCmdShutdown : ConsoleCmdAbstract
{
	// Token: 0x06001336 RID: 4918 RVA: 0x00075C81 File Offset: 0x00073E81
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shutdown" };
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06001337 RID: 4919 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x00075C91 File Offset: 0x00073E91
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Shutting server down...");
		Application.Quit();
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x00075CA7 File Offset: 0x00073EA7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shuts down the game";
	}
}
