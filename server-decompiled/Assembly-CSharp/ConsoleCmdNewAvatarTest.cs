using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024F RID: 591
[Preserve]
public class ConsoleCmdNewAvatarTest : ConsoleCmdAbstract
{
	// Token: 0x06001191 RID: 4497 RVA: 0x0006F213 File Offset: 0x0006D413
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test new HD stuff.";
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x0006F21A File Offset: 0x0006D41A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. na\n";
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x0006F221 File Offset: 0x0006D421
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "na" };
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x0006F231 File Offset: 0x0006D431
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Log.Warning("No New Avatar!");
	}
}
