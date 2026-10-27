using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023D RID: 573
[Preserve]
public class ConsoleCmdListThreads : ConsoleCmdAbstract
{
	// Token: 0x06001115 RID: 4373 RVA: 0x0006CD0D File Offset: 0x0006AF0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listthreads", "lt" };
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06001116 RID: 4374 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06001117 RID: 4375 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x0006CD28 File Offset: 0x0006AF28
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

	// Token: 0x06001119 RID: 4377 RVA: 0x0006CDAC File Offset: 0x0006AFAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all threads";
	}
}
