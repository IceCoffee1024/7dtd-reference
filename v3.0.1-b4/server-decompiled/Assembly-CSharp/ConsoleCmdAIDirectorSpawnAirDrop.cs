using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E1 RID: 481
[Preserve]
public class ConsoleCmdAIDirectorSpawnAirDrop : ConsoleCmdAbstract
{
	// Token: 0x06000EDF RID: 3807 RVA: 0x00060B3F File Offset: 0x0005ED3F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnairdrop" };
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00060B4F File Offset: 0x0005ED4F
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>().SpawnAirDrop();
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00060B6B File Offset: 0x0005ED6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawns an air drop";
	}
}
