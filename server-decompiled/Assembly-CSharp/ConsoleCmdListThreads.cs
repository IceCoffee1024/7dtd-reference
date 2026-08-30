using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000235 RID: 565
[Preserve]
public class ConsoleCmdListThreads : ConsoleCmdAbstract
{
	// Token: 0x06001108 RID: 4360 RVA: 0x0006C2F9 File Offset: 0x0006A4F9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listthreads", "lt" };
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06001109 RID: 4361 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x0600110A RID: 4362 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0006C314 File Offset: 0x0006A514
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Threads:");
		int num = 0;
		foreach (KeyValuePair<string, ThreadManager.ThreadInfo> keyValuePair in ThreadManager.ActiveThreads)
		{
			SdtdConsole instance = SingletonMonoBehaviour<SdtdConsole>.Instance;
			int num2;
			num = (num2 = num + 1);
			instance.Output(num2.ToString() + ". " + keyValuePair.Key);
		}
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x0006C398 File Offset: 0x0006A598
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all threads";
	}
}
