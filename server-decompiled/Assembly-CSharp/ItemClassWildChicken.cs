using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class ItemClassWildChicken : ItemClassHeldEntity
{
	// Token: 0x0600327E RID: 12926 RVA: 0x001473EC File Offset: 0x001455EC
	public override void Init()
	{
		base.Init();
		this.Properties.ParseInt(ItemClassWildChicken.PropZombieHordeGS, ref this.ZombieHordeGS);
		this.Properties.ParseInt(ItemClassWildChicken.PropAngryChickenGS, ref this.AngryChickenGS);
		this.Properties.ParseInt(ItemClassWildChicken.PropMurderChickensGS, ref this.MurderChickensGS);
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x00147444 File Offset: 0x00145644
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

	// Token: 0x040027BC RID: 10172
	public static ItemClassWildChicken.FullStressEvents FullStressEventType;

	// Token: 0x040027BD RID: 10173
	[PublicizedFrom(EAccessModifier.Protected)]
	public int ZombieHordeGS = 15;

	// Token: 0x040027BE RID: 10174
	[PublicizedFrom(EAccessModifier.Protected)]
	public int AngryChickenGS = 30;

	// Token: 0x040027BF RID: 10175
	[PublicizedFrom(EAccessModifier.Protected)]
	public int MurderChickensGS = 100;

	// Token: 0x040027C0 RID: 10176
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropZombieHordeGS = "ZombieHordeGS";

	// Token: 0x040027C1 RID: 10177
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropAngryChickenGS = "AngryChickenGS";

	// Token: 0x040027C2 RID: 10178
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropMurderChickensGS = "MurderChickensGS";

	// Token: 0x040027C3 RID: 10179
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemClassWildChicken.FullStressEvents> StressEvents = new List<ItemClassWildChicken.FullStressEvents>();

	// Token: 0x0200060C RID: 1548
	public enum FullStressEvents
	{
		// Token: 0x040027C5 RID: 10181
		None,
		// Token: 0x040027C6 RID: 10182
		Random,
		// Token: 0x040027C7 RID: 10183
		DropChicken,
		// Token: 0x040027C8 RID: 10184
		AngryChicken,
		// Token: 0x040027C9 RID: 10185
		SpawnZombieHorde,
		// Token: 0x040027CA RID: 10186
		SpawnMurderChickens,
		// Token: 0x040027CB RID: 10187
		EnrageZombies
	}
}
