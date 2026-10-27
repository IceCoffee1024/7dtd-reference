using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000229 RID: 553
[Preserve]
public class ConsoleCmdGiveXp : ConsoleCmdAbstract
{
	// Token: 0x0600109F RID: 4255 RVA: 0x00069E60 File Offset: 0x00068060
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "givexp" };
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x00069E70 File Offset: 0x00068070
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Give XP to a player";
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x00069E77 File Offset: 0x00068077
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Give a player experience points\nUsage:\n   givexp <entity id / player name / user id> <number>";
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x00069E80 File Offset: 0x00068080
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
