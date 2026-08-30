using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A1 RID: 673
[Preserve]
public class ConsoleCmdSpectrum : ConsoleCmdAbstract
{
	// Token: 0x06001390 RID: 5008 RVA: 0x000778AA File Offset: 0x00075AAA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectrum" };
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x000778BA File Offset: 0x00075ABA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force a particular lighting spectrum.";
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000778C1 File Offset: 0x00075AC1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "spectrum <Auto, Biome, BloodMoon, Foggy, Rainy, Stormy, Snowy>\n";
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06001393 RID: 5011 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x000778C8 File Offset: 0x00075AC8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool isDedicatedServer = GameManager.IsDedicatedServer;
	}
}
