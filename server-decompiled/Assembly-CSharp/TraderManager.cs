using System;
using System.Collections.Generic;

// Token: 0x020008A8 RID: 2216
public class TraderManager
{
	// Token: 0x0600403F RID: 16447 RVA: 0x001913C8 File Offset: 0x0018F5C8
	public TraderManager(WorldBase _world)
	{
		this.world = _world;
		this.Random = GameRandomManager.Instance.CreateGameRandom((int)DateTime.Now.Ticks);
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x00191400 File Offset: 0x0018F600
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

	// Token: 0x06004041 RID: 16449 RVA: 0x001914C8 File Offset: 0x0018F6C8
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
						if (item.itemValue.type == itemStack.itemValue.type && item.count < forId.Stacknumber.Value)
						{
							int num = Math.Min(forId.Stacknumber.Value - item.count, itemStack.count);
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

	// Token: 0x040033D8 RID: 13272
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBase world;

	// Token: 0x040033D9 RID: 13273
	public GameRandom Random;

	// Token: 0x040033DA RID: 13274
	public static float[] QuestTierMod;

	// Token: 0x040033DB RID: 13275
	public static Dictionary<string, TraderStageTemplateGroup> TraderStageTemplates = new Dictionary<string, TraderStageTemplateGroup>();

	// Token: 0x040033DC RID: 13276
	public static bool VendingEnabled = true;
}
