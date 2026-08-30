using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024C RID: 588
[Preserve]
public class ConsoleCmdNewAvatarTest : ConsoleCmdAbstract
{
	// Token: 0x06001194 RID: 4500 RVA: 0x0006EF67 File Offset: 0x0006D167
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test new HD stuff.";
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x0006EF6E File Offset: 0x0006D16E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. na\n";
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0006EF75 File Offset: 0x0006D175
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "na" };
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x0006EF85 File Offset: 0x0006D185
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Log.Warning("No New Avatar!");
	}
}
