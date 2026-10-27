using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E6 RID: 486
[Preserve]
public class ConsoleCmdAIDirectorSpawnAirDrop : ConsoleCmdAbstract
{
	// Token: 0x06000EE1 RID: 3809 RVA: 0x0006133B File Offset: 0x0005F53B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnairdrop" };
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0006134B File Offset: 0x0005F54B
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>().SpawnAirDrop();
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00061367 File Offset: 0x0005F567
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawns an air drop";
	}
}
