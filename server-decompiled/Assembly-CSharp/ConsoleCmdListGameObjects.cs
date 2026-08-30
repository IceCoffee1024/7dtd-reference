using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000235 RID: 565
[Preserve]
public class ConsoleCmdListGameObjects : ConsoleCmdAbstract
{
	// Token: 0x06001104 RID: 4356 RVA: 0x0006C4B5 File Offset: 0x0006A6B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "lgo", "listgameobjects" };
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06001105 RID: 4357 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06001106 RID: 4358 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0006C4CD File Offset: 0x0006A6CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List all active game objects";
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0006C4D4 File Offset: 0x0006A6D4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		int num = global::UnityEngine.Object.FindObjectsByType<global::UnityEngine.Object>(FindObjectsSortMode.None).Length;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("GOs: {0}, took {1} ms", num, microStopwatch.ElapsedMilliseconds));
	}
}
