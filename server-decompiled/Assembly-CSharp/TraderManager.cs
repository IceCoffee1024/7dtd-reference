using System;
using System.Collections.Generic;

// Token: 0x020008B4 RID: 2228
public class TraderManager
{
	// Token: 0x060040A4 RID: 16548 RVA: 0x001934AC File Offset: 0x001916AC
	public TraderManager(WorldBase _world)
	{
		this.world = _world;
		this.Random = GameRandomManager.Instance.CreateGameRandom((int)DateTime.Now.Ticks);
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x001934E4 File Offset: 0x001916E4
	public bool TraderInventoryRequested(TraderData trader, int _entityIdThatOpenedIt)
	{
		TraderInfo traderInfo = TraderInfo.traderInfoList[trader.TraderID];
		if (traderInfo == null)
		{
			return false;
		}
		if (traderInfo.ResetInterval < 1)
		{
			return false;
		}
		ulong worldTime = this.world.GetWorldTime();
		if (worldTime < trader.lastInventoryUpdate)
		{
			trader.lastInventoryUpdate = 1UL;
		}
		if ((int)(worldTime - trader.lastInventoryUpdate) < traderInfo.ResetIntervalInTicks && trader.lastInventoryUpdate > 0UL)
		{
			return false;
		}
		ulong num = (ulong)((long)traderInfo.ResetIntervalInTicks);
		trader.lastInventoryUpdate = worldTime / num * num + 1UL;
		this.HandleFullReset(trader, traderInfo);
		trader.TierItemGroups.Clear();
		for (int i = 0; i < traderInfo.TierItemGroups.Count; i++)
		{
			List<ItemStack> list = traderInfo.SpawnTierGroup(this.Random, i);
			trader.TierItemGroups.Add(list.ToArray());
		}
		return true;
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x001935AC File Offset: 0x001917AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFullReset(TraderData trader, TraderInfo info)
	{
		trader.PrimaryInventory.Clear();
		if (info.ResetInterval != -1)
		{
			List<ItemStack> list = info.Spawn(this.Random);
			for (int i = 0; i < list.Count; i++)
			{
				ItemStack itemStack = list[i];
				ItemClass forId = ItemClass.GetForId(itemStack.itemValue.type);
				if (!forId.HasQuality)
				{
					for (int j = 0; j < trader.PrimaryInventory.Count; j++)
					{
						ItemStack item = trader.PrimaryInventory[j].Item;
						if (item.itemValue.type == itemStack.itemValue.type && item.count < forId.MaxCount)
						{
							int num = Math.Min(forId.MaxCount - item.count, itemStack.count);
							item.count += num;
							itemStack.count -= num;
							if (itemStack.count == 0)
							{
								list[i] = ItemStack.Empty;
							}
						}
					}
				}
				else
				{
					itemStack.itemValue.MaxDurabilityModifier = 1f;
				}
				if (!list[i].IsEmpty())
				{
					trader.PrimaryInventoryAdd(new TraderData.Entry(list[i], 0, false));
				}
			}
		}
	}

	// Token: 0x04003459 RID: 13401
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBase world;

	// Token: 0x0400345A RID: 13402
	public GameRandom Random;

	// Token: 0x0400345B RID: 13403
	public static float[] QuestTierMod;

	// Token: 0x0400345C RID: 13404
	public static Dictionary<string, TraderStageTemplateGroup> TraderStageTemplates = new Dictionary<string, TraderStageTemplateGroup>();

	// Token: 0x0400345D RID: 13405
	public static bool VendingEnabled = true;
}
