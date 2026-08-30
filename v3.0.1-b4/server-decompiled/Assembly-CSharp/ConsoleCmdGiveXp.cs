using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000222 RID: 546
[Preserve]
public class ConsoleCmdGiveXp : ConsoleCmdAbstract
{
	// Token: 0x06001097 RID: 4247 RVA: 0x00069530 File Offset: 0x00067730
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "givexp" };
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00069540 File Offset: 0x00067740
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Give XP to a player";
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00069547 File Offset: 0x00067747
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Give a player experience points\nUsage:\n   givexp <entity id / player name / user id> <number>";
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00069550 File Offset: 0x00067750
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("givexp requires a target entity id and xp amount");
			return;
		}
		EntityPlayer entityPlayer = null;
		ClientInfo forNameOrId = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.GetForNameOrId(_params[0], true, false);
		int num;
		if (forNameOrId != null)
		{
			entityPlayer = GameManager.Instance.World.Players.dict[forNameOrId.entityId];
		}
		else if (int.TryParse(_params[0], out num))
		{
			GameManager.Instance.World.Players.dict.TryGetValue(num, out entityPlayer);
		}
		if (entityPlayer == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Playername or entity id not found.");
			return;
		}
		int num2;
		if (!int.TryParse(_params[1], out num2))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("xp amount must be a number.");
			return;
		}
		if (num2 < 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("xp amount must be positive.");
			return;
		}
		num2 = Mathf.Clamp(num2, 0, 1073741823);
		if (entityPlayer.isEntityRemote)
		{
			NetPackageEntityAddExpClient netPackageEntityAddExpClient = NetPackageManager.GetPackage<NetPackageEntityAddExpClient>().Setup(entityPlayer.entityId, num2, Progression.XPTypes.Debug, null);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageEntityAddExpClient, false, entityPlayer.entityId, -1, -1, null, 192, false);
			return;
		}
		entityPlayer.Progression.AddLevelExp(num2, "_xpOther", Progression.XPTypes.Debug, true, true, -1, null);
	}
}
