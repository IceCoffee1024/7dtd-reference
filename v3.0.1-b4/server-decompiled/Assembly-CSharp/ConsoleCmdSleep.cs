using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x02000294 RID: 660
[Preserve]
public class ConsoleCmdSleep : ConsoleCmdAbstract
{
	// Token: 0x06001349 RID: 4937 RVA: 0x00075EAD File Offset: 0x000740AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sleep" };
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x00075EBD File Offset: 0x000740BD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the main thread sleep for the given number of seconds (allows decimals)";
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00075EC4 File Offset: 0x000740C4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		float num = 1f;
		if (_params.Count >= 1 && !StringParsers.TryParseFloat(_params[0], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument is not a valid float");
			return;
		}
		Thread.Sleep((int)(num * 1000f));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Slept for {0} seconds", num));
	}
}
