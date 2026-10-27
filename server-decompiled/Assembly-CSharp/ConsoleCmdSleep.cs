using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x0200029D RID: 669
[Preserve]
public class ConsoleCmdSleep : ConsoleCmdAbstract
{
	// Token: 0x0600135C RID: 4956 RVA: 0x0007698D File Offset: 0x00074B8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sleep" };
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0007699D File Offset: 0x00074B9D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the main thread sleep for the given number of seconds (allows decimals)";
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x000769A4 File Offset: 0x00074BA4
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
