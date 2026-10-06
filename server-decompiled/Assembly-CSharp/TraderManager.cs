using System;
using System.Collections.Generic;

// Token: 0x020008B9 RID: 2233
public class TraderManager
{
	// Token: 0x0600403A RID: 16442 RVA: 0x001935B0 File Offset: 0x001917B0
	public TraderManager(WorldBase _world)
	{
		this.world = _world;
		this.Random = GameRandomManager.Instance.CreateGameRandom((int)DateTime.Now.Ticks);
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x001935E8 File Offset: 0x001917E8
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

	// Token: 0x0600403C RID: 16444 RVA: 0x001936B0 File Offset: 0x001918B0
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

	// Token: 0x0400344D RID: 13389
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBase world;

	// Token: 0x0400344E RID: 13390
	public GameRandom Random;

	// Token: 0x0400344F RID: 13391
	public static float[] QuestTierMod;

	// Token: 0x04003450 RID: 13392
	public static Dictionary<string, TraderStageTemplateGroup> TraderStageTemplates = new Dictionary<string, TraderStageTemplateGroup>();

	// Token: 0x04003451 RID: 13393
	public static bool VendingEnabled = true;
}
