using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E7 RID: 487
[Preserve]
public class ConsoleCmdAIDirectorSpawnHorde : ConsoleCmdAbstract
{
	// Token: 0x06000EE5 RID: 3813 RVA: 0x0006136E File Offset: 0x0005F56E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnwandering", "spawnw" };
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x00061386 File Offset: 0x0005F586
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawn wandering entities";
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0006138D File Offset: 0x0005F58D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\nb - bandits\nh - horde";
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x00061394 File Offset: 0x0005F594
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
