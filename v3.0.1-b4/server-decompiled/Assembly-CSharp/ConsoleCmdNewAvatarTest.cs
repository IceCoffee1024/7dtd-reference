using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000248 RID: 584
[Preserve]
public class ConsoleCmdNewAvatarTest : ConsoleCmdAbstract
{
	// Token: 0x06001182 RID: 4482 RVA: 0x0006E90B File Offset: 0x0006CB0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test new HD stuff.";
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x0006E912 File Offset: 0x0006CB12
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. na\n";
	}

	// Token: 0x06001184 RID: 4484 RVA: 0x0006E919 File Offset: 0x0006CB19
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "na" };
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x0006E929 File Offset: 0x0006CB29
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Log.Warning("No New Avatar!");
	}
}
