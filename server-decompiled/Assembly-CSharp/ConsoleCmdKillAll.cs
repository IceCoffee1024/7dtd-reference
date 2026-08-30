using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000232 RID: 562
[Preserve]
public class ConsoleCmdKillAll : ConsoleCmdAbstract
{
	// Token: 0x060010F3 RID: 4339 RVA: 0x0006BB1F File Offset: 0x00069D1F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "killall" };
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x0006BB2F File Offset: 0x00069D2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kill all entities";
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x0006BB36 File Offset: 0x00069D36
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Kills all matching entities (but never players)\nUsage:\n   killall (all enemies)\n   killall alive (all EntityAlive types except vehicles and turrets)\n   killall all (all types)";
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x0006BB40 File Offset: 0x00069D40
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		bool flag = _params.Count > 0 && _params[0].EqualsCaseInsensitive("alive");
		bool flag2 = _params.Count > 0 && _params[0].EqualsCaseInsensitive("all");
		List<Entity> list = new List<Entity>(GameManager.Instance.World.Entities.list);
		for (int i = 0; i < list.Count; i++)
		{
			Entity entity = list[i];
			if (entity != null && !(entity is EntityPlayer) && (flag2 || (entity is EntityAlive && ((flag && !(entity is EntityVehicle) && !(entity is EntityTurret)) || EntityClass.list[entity.entityClass].bIsEnemyEntity))))
			{
				entity.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Gave " + 99999.ToString() + " damage to entity " + entity.GetDebugName());
			}
		}
	}
}
