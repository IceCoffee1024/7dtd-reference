using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000239 RID: 569
[Preserve]
public class ConsoleCmdListGameObjects : ConsoleCmdAbstract
{
	// Token: 0x06001100 RID: 4352 RVA: 0x0006C849 File Offset: 0x0006AA49
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "lgo", "listgameobjects" };
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06001101 RID: 4353 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06001102 RID: 4354 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0006C861 File Offset: 0x0006AA61
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "List all active game objects";
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0006C868 File Offset: 0x0006AA68
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		int num = global::UnityEngine.Object.FindObjectsByType<global::UnityEngine.Object>(FindObjectsSortMode.None).Length;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("GOs: {0}, took {1} ms", num, microStopwatch.ElapsedMilliseconds));
	}
}
