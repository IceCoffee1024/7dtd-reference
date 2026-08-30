using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200022E RID: 558
[Preserve]
public class ConsoleCmdKill : ConsoleCmdAbstract
{
	// Token: 0x060010E2 RID: 4322 RVA: 0x0006B451 File Offset: 0x00069651
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kill" };
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0006B461 File Offset: 0x00069661
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kill a given entity";
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0006B468 File Offset: 0x00069668
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Kill a given entity.\nUsage:\n   1. kill <entity id>\n   2. kill <player name / steam id>\n1. can be used to kill any entity that can be killed (zombies, players).\n2. can only be used to kill players.";
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0006B470 File Offset: 0x00069670
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 1, found " + _params.Count.ToString() + ".");
			return;
		}
		Entity entity = null;
		ClientInfo forNameOrId = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.GetForNameOrId(_params[0], true, false);
		int num;
		if (forNameOrId != null)
		{
			entity = GameManager.Instance.World.Players.dict[forNameOrId.entityId];
		}
		else if (int.TryParse(_params[0], out num) && GameManager.Instance.World.Entities.dict.ContainsKey(num))
		{
			entity = GameManager.Instance.World.Entities.dict[num];
		}
		if (entity == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Playername or entity id not found.");
			return;
		}
		entity.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Gave 99999 damage to entity " + _params[0]);
	}
}
