using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x0200029B RID: 667
[Preserve]
public class ConsoleCmdSleep : ConsoleCmdAbstract
{
	// Token: 0x06001358 RID: 4952 RVA: 0x000767BD File Offset: 0x000749BD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sleep" };
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x000767CD File Offset: 0x000749CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the main thread sleep for the given number of seconds (allows decimals)";
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x000767D4 File Offset: 0x000749D4
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
