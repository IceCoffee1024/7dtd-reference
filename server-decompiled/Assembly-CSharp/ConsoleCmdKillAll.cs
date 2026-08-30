using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200022F RID: 559
[Preserve]
public class ConsoleCmdKillAll : ConsoleCmdAbstract
{
	// Token: 0x060010E7 RID: 4327 RVA: 0x0006B583 File Offset: 0x00069783
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "killall" };
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0006B593 File Offset: 0x00069793
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kill all entities";
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0006B59A File Offset: 0x0006979A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Kills all matching entities (but never players)\nUsage:\n   killall (all enemies)\n   killall alive (all EntityAlive types except vehicles and turrets)\n   killall all (all types)";
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x0006B5A4 File Offset: 0x000697A4
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
