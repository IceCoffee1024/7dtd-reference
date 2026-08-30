using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200029D RID: 669
[Preserve]
public class ConsoleCmdSpectrum : ConsoleCmdAbstract
{
	// Token: 0x0600137E RID: 4990 RVA: 0x0007724E File Offset: 0x0007544E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectrum" };
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x0007725E File Offset: 0x0007545E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force a particular lighting spectrum.";
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x00077265 File Offset: 0x00075465
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "spectrum <Auto, Biome, BloodMoon, Foggy, Rainy, Stormy, Snowy>\n";
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06001381 RID: 4993 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0007726C File Offset: 0x0007546C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool isDedicatedServer = GameManager.IsDedicatedServer;
	}
}
