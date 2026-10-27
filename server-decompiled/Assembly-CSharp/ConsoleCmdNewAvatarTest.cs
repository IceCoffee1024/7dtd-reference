using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000251 RID: 593
[Preserve]
public class ConsoleCmdNewAvatarTest : ConsoleCmdAbstract
{
	// Token: 0x06001195 RID: 4501 RVA: 0x0006F3E3 File Offset: 0x0006D5E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test new HD stuff.";
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0006F3EA File Offset: 0x0006D5EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. na\n";
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x0006F3F1 File Offset: 0x0006D5F1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "na" };
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x0006F401 File Offset: 0x0006D601
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Log.Warning("No New Avatar!");
	}
}
