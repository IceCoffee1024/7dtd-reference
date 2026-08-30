using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000232 RID: 562
[Preserve]
public class ConsoleCmdListGameObjects : ConsoleCmdAbstract
{
	// Token: 0x060010F8 RID: 4344 RVA: 0x0006BF19 File Offset: 0x0006A119
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "lgo", "listgameobjects" };
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060010FA RID: 4346 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0006BF31 File Offset: 0x0006A131
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List all active game objects";
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x0006BF38 File Offset: 0x0006A138
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		int num = global::UnityEngine.Object.FindObjectsByType<global::UnityEngine.Object>(FindObjectsSortMode.None).Length;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("GOs: {0}, took {1} ms", num, microStopwatch.ElapsedMilliseconds));
	}
}
