using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A6 RID: 678
[Preserve]
public class ConsoleCmdSpectrum : ConsoleCmdAbstract
{
	// Token: 0x06001391 RID: 5009 RVA: 0x00077D2E File Offset: 0x00075F2E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectrum" };
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x00077D3E File Offset: 0x00075F3E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force a particular lighting spectrum.";
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x00077D45 File Offset: 0x00075F45
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "spectrum <Auto, Biome, BloodMoon, Foggy, Rainy, Stormy, Snowy>\n";
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001394 RID: 5012 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x00077D4C File Offset: 0x00075F4C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool isDedicatedServer = GameManager.IsDedicatedServer;
	}
}
