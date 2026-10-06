using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023B RID: 571
[Preserve]
public class ConsoleCmdListThreads : ConsoleCmdAbstract
{
	// Token: 0x06001111 RID: 4369 RVA: 0x0006CB3D File Offset: 0x0006AD3D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listthreads", "lt" };
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06001112 RID: 4370 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06001113 RID: 4371 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x0006CB58 File Offset: 0x0006AD58
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

	// Token: 0x06001115 RID: 4373 RVA: 0x0006CBDC File Offset: 0x0006ADDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all threads";
	}
}
