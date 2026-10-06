using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E4 RID: 484
[Preserve]
public class ConsoleCmdAIDirectorSpawnAirDrop : ConsoleCmdAbstract
{
	// Token: 0x06000EDD RID: 3805 RVA: 0x00061077 File Offset: 0x0005F277
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnairdrop" };
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00061087 File Offset: 0x0005F287
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>().SpawnAirDrop();
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x000610A3 File Offset: 0x0005F2A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawns an air drop";
	}
}
