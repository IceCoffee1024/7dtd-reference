using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E5 RID: 485
[Preserve]
public class ConsoleCmdAIDirectorSpawnHorde : ConsoleCmdAbstract
{
	// Token: 0x06000EE1 RID: 3809 RVA: 0x000610AA File Offset: 0x0005F2AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnwandering", "spawnw" };
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x000610C2 File Offset: 0x0005F2C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawn wandering entities";
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x000610C9 File Offset: 0x0005F2C9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\nb - bandits\nh - horde";
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x000610D0 File Offset: 0x0005F2D0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		string text = _params[0].ToLower();
		if (text == "b")
		{
			GameManager.Instance.World.aiDirector.GetComponent<AIDirectorWanderingHordeComponent>().StartSpawning(AIWanderingHordeSpawner.SpawnType.Bandits);
			return;
		}
		if (!(text == "h"))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown command " + text);
			return;
		}
		GameManager.Instance.World.aiDirector.GetComponent<AIDirectorWanderingHordeComponent>().StartSpawning(AIWanderingHordeSpawner.SpawnType.Horde);
	}
}
