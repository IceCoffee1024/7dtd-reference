using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A4 RID: 676
[Preserve]
public class ConsoleCmdSpectrum : ConsoleCmdAbstract
{
	// Token: 0x0600138D RID: 5005 RVA: 0x00077B5E File Offset: 0x00075D5E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectrum" };
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00077B6E File Offset: 0x00075D6E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Force a particular lighting spectrum.";
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00077B75 File Offset: 0x00075D75
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "spectrum <Auto, Biome, BloodMoon, Foggy, Rainy, Stormy, Snowy>\n";
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001390 RID: 5008 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x00077B7C File Offset: 0x00075D7C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool isDedicatedServer = GameManager.IsDedicatedServer;
	}
}
