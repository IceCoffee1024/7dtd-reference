using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000238 RID: 568
[Preserve]
public class ConsoleCmdListThreads : ConsoleCmdAbstract
{
	// Token: 0x06001114 RID: 4372 RVA: 0x0006C895 File Offset: 0x0006AA95
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listthreads", "lt" };
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06001115 RID: 4373 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06001116 RID: 4374 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x0006C8B0 File Offset: 0x0006AAB0
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

	// Token: 0x06001118 RID: 4376 RVA: 0x0006C934 File Offset: 0x0006AB34
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all threads";
	}
}
