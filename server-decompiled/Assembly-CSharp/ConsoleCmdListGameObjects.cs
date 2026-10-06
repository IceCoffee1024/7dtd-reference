using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000237 RID: 567
[Preserve]
public class ConsoleCmdListGameObjects : ConsoleCmdAbstract
{
	// Token: 0x060010FC RID: 4348 RVA: 0x0006C679 File Offset: 0x0006A879
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "lgo", "listgameobjects" };
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060010FD RID: 4349 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060010FE RID: 4350 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x0006C691 File Offset: 0x0006A891
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List all active game objects";
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x0006C698 File Offset: 0x0006A898
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		int num = global::UnityEngine.Object.FindObjectsByType<global::UnityEngine.Object>(FindObjectsSortMode.None).Length;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("GOs: {0}, took {1} ms", num, microStopwatch.ElapsedMilliseconds));
	}
}
