using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000602 RID: 1538
public class ItemClassWildChicken : ItemClassHeldEntity
{
	// Token: 0x0600322D RID: 12845 RVA: 0x00145E6C File Offset: 0x0014406C
	public override void Init()
	{
		base.Init();
		this.Properties.ParseInt(ItemClassWildChicken.PropZombieHordeGS, ref this.ZombieHordeGS);
		this.Properties.ParseInt(ItemClassWildChicken.PropAngryChickenGS, ref this.AngryChickenGS);
		this.Properties.ParseInt(ItemClassWildChicken.PropMurderChickensGS, ref this.MurderChickensGS);
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x00145EC4 File Offset: 0x001440C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool HandleFullStress(EntityPlayerLocal epl)
	{
		if (this.FullStressEvent != "")
		{
			GameEventManager.Current.HandleAction(this.FullStressEvent, null, epl, false, "", "", false, true, "", null);
			return false;
		}
		ItemClassWildChicken.FullStressEvents fullStressEvents = ItemClassWildChicken.FullStressEventType;
		if (ItemClassWildChicken.FullStressEventType == ItemClassWildChicken.FullStressEvents.Random)
		{
			int gameStage = epl.gameStage;
			this.StressEvents.Clear();
			this.StressEvents.Add(ItemClassWildChicken.FullStressEvents.DropChicken);
			if (gameStage >= this.ZombieHordeGS)
			{
				this.StressEvents.Add(ItemClassWildChicken.FullStressEvents.SpawnZombieHorde);
			}
			if (gameStage >= this.AngryChickenGS)
			{
				this.StressEvents.Add(ItemClassWildChicken.FullStressEvents.AngryChicken);
			}
			if (gameStage >= this.MurderChickensGS)
			{
				this.StressEvents.Add(ItemClassWildChicken.FullStressEvents.SpawnMurderChickens);
			}
			Vector3 vector = Vector3.one * 80f;
			List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(epl, new Bounds(epl.position, vector), true);
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				if (entitiesInBounds[i] is EntityEnemy)
				{
					this.StressEvents.Add(ItemClassWildChicken.FullStressEvents.EnrageZombies);
					break;
				}
			}
			fullStressEvents = this.StressEvents[epl.rand.RandomRange(0, this.StressEvents.Count)];
		}
		switch (fullStressEvents)
		{
		case ItemClassWildChicken.FullStressEvents.DropChicken:
			GameEventManager.Current.HandleAction("chicken_drop_chicken", null, epl, false, "", "", false, true, "", null);
			return true;
		case ItemClassWildChicken.FullStressEvents.AngryChicken:
			GameEventManager.Current.HandleAction("chicken_angry_chicken", null, epl, false, "", "", false, true, "", null);
			epl.SetCVar(this.CVarUseAltVersion, 1f);
			return true;
		case ItemClassWildChicken.FullStressEvents.SpawnZombieHorde:
			GameEventManager.Current.HandleAction("chicken_zombie_horde", null, epl, false, "", "", false, true, "", null);
			return true;
		case ItemClassWildChicken.FullStressEvents.SpawnMurderChickens:
			GameEventManager.Current.HandleAction("chicken_murder_chicken", null, epl, false, "", "", false, true, "", null);
			return true;
		case ItemClassWildChicken.FullStressEvents.EnrageZombies:
			GameEventManager.Current.HandleAction("chicken_enrage_zombies", null, epl, false, "", "", false, true, "", null);
			return true;
		default:
			return false;
		}
	}

	// Token: 0x040027AA RID: 10154
	public static ItemClassWildChicken.FullStressEvents FullStressEventType;

	// Token: 0x040027AB RID: 10155
	[PublicizedFrom(EAccessModifier.Protected)]
	public int ZombieHordeGS = 15;

	// Token: 0x040027AC RID: 10156
	[PublicizedFrom(EAccessModifier.Protected)]
	public int AngryChickenGS = 30;

	// Token: 0x040027AD RID: 10157
	[PublicizedFrom(EAccessModifier.Protected)]
	public int MurderChickensGS = 100;

	// Token: 0x040027AE RID: 10158
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropZombieHordeGS = "ZombieHordeGS";

	// Token: 0x040027AF RID: 10159
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropAngryChickenGS = "AngryChickenGS";

	// Token: 0x040027B0 RID: 10160
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropMurderChickensGS = "MurderChickensGS";

	// Token: 0x040027B1 RID: 10161
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemClassWildChicken.FullStressEvents> StressEvents = new List<ItemClassWildChicken.FullStressEvents>();

	// Token: 0x02000603 RID: 1539
	public enum FullStressEvents
	{
		// Token: 0x040027B3 RID: 10163
		None,
		// Token: 0x040027B4 RID: 10164
		Random,
		// Token: 0x040027B5 RID: 10165
		DropChicken,
		// Token: 0x040027B6 RID: 10166
		AngryChicken,
		// Token: 0x040027B7 RID: 10167
		SpawnZombieHorde,
		// Token: 0x040027B8 RID: 10168
		SpawnMurderChickens,
		// Token: 0x040027B9 RID: 10169
		EnrageZombies
	}
}
