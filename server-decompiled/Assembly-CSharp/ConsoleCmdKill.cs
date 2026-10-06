using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000233 RID: 563
[Preserve]
public class ConsoleCmdKill : ConsoleCmdAbstract
{
	// Token: 0x060010E6 RID: 4326 RVA: 0x0006BBB1 File Offset: 0x00069DB1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kill" };
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x0006BBC1 File Offset: 0x00069DC1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kill a given entity";
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0006BBC8 File Offset: 0x00069DC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Kill a given entity.\nUsage:\n   1. kill <entity id>\n   2. kill <player name / steam id>\n1. can be used to kill any entity that can be killed (zombies, players).\n2. can only be used to kill players.";
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0006BBD0 File Offset: 0x00069DD0
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
