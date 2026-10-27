using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BB RID: 2235
public class TraderInfo
{
	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x0600402B RID: 16427 RVA: 0x0019385F File Offset: 0x00191A5F
	// (set) Token: 0x0600402C RID: 16428 RVA: 0x00193866 File Offset: 0x00191A66
	public static float BuyMarkup
	{
		get
		{
			return TraderInfo.buyMarkup;
		}
		set
		{
			TraderInfo.buyMarkup = value;
		}
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x0600402D RID: 16429 RVA: 0x0019386E File Offset: 0x00191A6E
	// (set) Token: 0x0600402E RID: 16430 RVA: 0x00193875 File Offset: 0x00191A75
	public static float SellMarkdown
	{
		get
		{
			return TraderInfo.sellMarkdown;
		}
		set
		{
			TraderInfo.sellMarkdown = value;
		}
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x0600402F RID: 16431 RVA: 0x0019387D File Offset: 0x00191A7D
	// (set) Token: 0x06004030 RID: 16432 RVA: 0x00193884 File Offset: 0x00191A84
	public static float QualityMinMod
	{
		get
		{
			return TraderInfo.qualityMinMod;
		}
		set
		{
			TraderInfo.qualityMinMod = value;
		}
	}

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06004031 RID: 16433 RVA: 0x0019388C File Offset: 0x00191A8C
	// (set) Token: 0x06004032 RID: 16434 RVA: 0x00193893 File Offset: 0x00191A93
	public static float QualityMaxMod
	{
		get
		{
			return TraderInfo.qualityMaxMod;
		}
		set
		{
			TraderInfo.qualityMaxMod = value;
		}
	}

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06004033 RID: 16435 RVA: 0x0019389B File Offset: 0x00191A9B
	// (set) Token: 0x06004034 RID: 16436 RVA: 0x001938A2 File Offset: 0x00191AA2
	public static string CurrencyItem { get; set; }

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06004035 RID: 16437 RVA: 0x001938AA File Offset: 0x00191AAA
	// (set) Token: 0x06004036 RID: 16438 RVA: 0x001938E8 File Offset: 0x00191AE8
	public int ResetInterval
	{
		get
		{
			if (this.PlayerOwned || this.Rentable)
			{
				return -1;
			}
			if (this.IsVendingMachine)
			{
				if (TraderInfo.VendingResetInterval != -1)
				{
					return TraderInfo.VendingResetInterval;
				}
			}
			else if (TraderInfo.GlobalResetInterval != -1)
			{
				return TraderInfo.GlobalResetInterval;
			}
			return this.resetInterval;
		}
		set
		{
			this.resetInterval = value;
		}
	}

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x06004037 RID: 16439 RVA: 0x001938F1 File Offset: 0x00191AF1
	// (set) Token: 0x06004038 RID: 16440 RVA: 0x0019392F File Offset: 0x00191B2F
	public int ResetIntervalInTicks
	{
		get
		{
			if (this.PlayerOwned || this.Rentable)
			{
				return -1;
			}
			if (this.IsVendingMachine)
			{
				if (TraderInfo.VendingResetIntervalInTicks != -1)
				{
					return TraderInfo.VendingResetIntervalInTicks;
				}
			}
			else if (TraderInfo.GlobalResetIntervalInTicks != -1)
			{
				return TraderInfo.GlobalResetIntervalInTicks;
			}
			return this.resetIntervalInTicks;
		}
		set
		{
			this.resetIntervalInTicks = value;
		}
	}

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06004039 RID: 16441 RVA: 0x00193938 File Offset: 0x00191B38
	// (set) Token: 0x0600403A RID: 16442 RVA: 0x00193940 File Offset: 0x00191B40
	public static TraderInfo.TraderHourPresets TraderHoursPreset
	{
		get
		{
			return TraderInfo.traderHoursPreset;
		}
		set
		{
			TraderInfo.traderHoursPreset = value;
			if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
			{
				switch (TraderInfo.traderHoursPreset)
				{
				case TraderInfo.TraderHourPresets.MorningOnly:
					TraderInfo.GlobalOpenTime = GameUtils.DayTimeToWorldTime(1, 4, 1);
					TraderInfo.GlobalCloseTime = GameUtils.DayTimeToWorldTime(1, 11, 50);
					break;
				case TraderInfo.TraderHourPresets.MidDayOnly:
					TraderInfo.GlobalOpenTime = GameUtils.DayTimeToWorldTime(1, 10, 1);
					TraderInfo.GlobalCloseTime = GameUtils.DayTimeToWorldTime(1, 17, 50);
					break;
				case TraderInfo.TraderHourPresets.EveningOnly:
					TraderInfo.GlobalOpenTime = GameUtils.DayTimeToWorldTime(1, 16, 1);
					TraderInfo.GlobalCloseTime = GameUtils.DayTimeToWorldTime(1, 21, 50);
					break;
				case TraderInfo.TraderHourPresets.NightOnly:
					TraderInfo.GlobalOpenTime = GameUtils.DayTimeToWorldTime(1, 22, 1);
					TraderInfo.GlobalCloseTime = GameUtils.DayTimeToWorldTime(1, 3, 50);
					break;
				case TraderInfo.TraderHourPresets.OnlyClosedOnBM:
				{
					ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
					int item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					TraderInfo.GlobalOpenTime = GameUtils.DayTimeToWorldTime(1, item2, 0);
					TraderInfo.GlobalCloseTime = GameUtils.DayTimeToWorldTime(1, item - 1, 50);
					break;
				}
				}
				TraderInfo.GlobalWarningTime = TraderInfo.GlobalCloseTime - 300UL;
			}
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x0600403B RID: 16443 RVA: 0x00193A48 File Offset: 0x00191C48
	public int RentTimeInSeconds
	{
		get
		{
			return this.RentTimeInDays * 60 * GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
		}
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x0600403C RID: 16444 RVA: 0x00193A5B File Offset: 0x00191C5B
	public int RentTimeInTicks
	{
		get
		{
			return this.RentTimeInDays * 24000;
		}
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x00193A69 File Offset: 0x00191C69
	public ulong GetOpenTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalOpenTime;
		}
		return this.OpenTime;
	}

	// Token: 0x0600403E RID: 16446 RVA: 0x00193A7E File Offset: 0x00191C7E
	public ulong GetCloseTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalCloseTime;
		}
		return this.CloseTime;
	}

	// Token: 0x0600403F RID: 16447 RVA: 0x00193A93 File Offset: 0x00191C93
	public ulong GetWarningTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalWarningTime;
		}
		return this.WarningTime;
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06004040 RID: 16448 RVA: 0x00193AA8 File Offset: 0x00191CA8
	public bool IsOpen
	{
		get
		{
			if (TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.OnlyClosedOnBM)
			{
				int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
				int num = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
				ulong num2 = GameManager.Instance.World.worldTime % 24000UL;
				ulong openTime = this.GetOpenTime();
				ulong closeTime = this.GetCloseTime();
				return (num != @int || num2 < closeTime) && (num != @int + 1 || num2 > openTime);
			}
			if (!this.UseOpenHours || TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.AlwaysOpen || World.SandboxUseTraderArea != TraderAreaStates.Default)
			{
				return true;
			}
			ulong num3 = GameManager.Instance.World.worldTime % 24000UL;
			ulong openTime2 = this.GetOpenTime();
			ulong closeTime2 = this.GetCloseTime();
			if (openTime2 < closeTime2)
			{
				return openTime2 < num3 && num3 < closeTime2;
			}
			return num3 > openTime2 || num3 < closeTime2;
		}
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06004041 RID: 16449 RVA: 0x00193B7C File Offset: 0x00191D7C
	public bool IsTraderActivitiesOpen
	{
		get
		{
			if (TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.OnlyClosedOnBM)
			{
				return !GameManager.Instance.World.isEventBloodMoon;
			}
			ulong num = GameManager.Instance.World.worldTime % 24000UL;
			ulong openTime = this.GetOpenTime();
			ulong closeTime = this.GetCloseTime();
			if (openTime < closeTime)
			{
				return openTime < num && num < closeTime;
			}
			return num > openTime || num < closeTime;
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06004042 RID: 16450 RVA: 0x00193BE4 File Offset: 0x00191DE4
	public bool ShouldPlayOpenSound
	{
		get
		{
			if (TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.OnlyClosedOnBM)
			{
				bool isEventBloodMoon = GameManager.Instance.World.isEventBloodMoon;
				if (this.lastClosedBM != isEventBloodMoon)
				{
					if (!this.lastClosedBM)
					{
						this.lastClosedBM = isEventBloodMoon;
						return false;
					}
					this.lastClosedBM = isEventBloodMoon;
				}
			}
			ulong openTime = this.GetOpenTime();
			ulong num = GameManager.Instance.World.worldTime % 24000UL;
			return num > openTime && num < openTime + 100UL;
		}
	}

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06004043 RID: 16451 RVA: 0x00193C58 File Offset: 0x00191E58
	public bool ShouldPlayCloseSound
	{
		get
		{
			if (TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.OnlyClosedOnBM && GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) != GameStats.GetInt(EnumGameStats.BloodMoonDay))
			{
				return false;
			}
			ulong closeTime = this.GetCloseTime();
			ulong num = GameManager.Instance.World.worldTime % 24000UL;
			return num > closeTime && num < closeTime + 100UL;
		}
	}

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06004044 RID: 16452 RVA: 0x00193CB8 File Offset: 0x00191EB8
	public bool IsWarningTime
	{
		get
		{
			if (TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.OnlyClosedOnBM)
			{
				int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
				int num = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
				ulong num2 = GameManager.Instance.World.worldTime % 24000UL;
				ulong warningTime = this.GetWarningTime();
				return num == @int && warningTime < num2 && num2 < warningTime + 100UL;
			}
			if (!this.UseOpenHours || TraderInfo.traderHoursPreset == TraderInfo.TraderHourPresets.AlwaysOpen || World.SandboxUseTraderArea != TraderAreaStates.Default)
			{
				return false;
			}
			ulong num3 = GameManager.Instance.World.worldTime % 24000UL;
			ulong openTime = this.GetOpenTime();
			ulong warningTime2 = this.GetWarningTime();
			if (openTime < warningTime2)
			{
				return warningTime2 < num3 && num3 < warningTime2 + 100UL;
			}
			return warningTime2 > openTime || num3 < warningTime2 + 100UL;
		}
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x00193DEB File Offset: 0x00191FEB
	public static void InitStatic()
	{
		TraderInfo.traderInfoList = new TraderInfo[256];
		TraderInfo.traderItemGroups = new Dictionary<string, TraderInfo.TraderItemGroup>();
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00193E06 File Offset: 0x00192006
	public void Init()
	{
		TraderInfo.traderInfoList[this.Id] = this;
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x00193E15 File Offset: 0x00192015
	public static void Cleanup()
	{
		TraderInfo.traderInfoList = null;
		TraderInfo.traderItemGroups = null;
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x00193E23 File Offset: 0x00192023
	public static string GetNextTraderTime()
	{
		if (TraderInfo.traderDisplayInfo == null)
		{
			if (TraderInfo.traderInfoList.Length == 0)
			{
				return "";
			}
			TraderInfo.traderDisplayInfo = new TraderInfo.TraderDisplayInfo();
		}
		TraderInfo.traderDisplayInfo.Refresh();
		return TraderInfo.traderDisplayInfo.GetTimeText();
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x00193E58 File Offset: 0x00192058
	public static string GetNextTraderText()
	{
		if (TraderInfo.traderDisplayInfo == null)
		{
			if (TraderInfo.traderInfoList.Length == 0)
			{
				return "";
			}
			TraderInfo.traderDisplayInfo = new TraderInfo.TraderDisplayInfo();
		}
		TraderInfo.traderDisplayInfo.Refresh();
		return TraderInfo.traderDisplayInfo.GetTimeTitle();
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x00193E90 File Offset: 0x00192090
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyRandomDegradation(ref ItemValue _itemValue)
	{
		_itemValue.Meta = ItemClass.GetForId(_itemValue.type).GetInitialMetadata(_itemValue);
		int maxUseTimes = _itemValue.MaxUseTimes;
		if (maxUseTimes == 0)
		{
			return;
		}
		float num = GameManager.Instance.World.GetGameRandom().RandomFloat * 0.6f + 0.2f;
		_itemValue.UseTimes = (float)((int)((float)maxUseTimes * num));
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00193EF4 File Offset: 0x001920F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyQuality(ref ItemValue _itemValue, int minQuality = 1, int maxQuality = 6)
	{
		if (ItemClass.list[_itemValue.type].HasQuality || ItemClass.list[_itemValue.type].HasSubItems)
		{
			_itemValue = new ItemValue(_itemValue.type, Mathf.Clamp(minQuality, 1, maxQuality), maxQuality, false, null, 1f);
		}
	}

	// Token: 0x0600404D RID: 16461 RVA: 0x00193F48 File Offset: 0x00192148
	public List<ItemStack> Spawn(GameRandom random)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.SpawnLootItemsFromList(random, this.traderItems, -1, list, null);
		return list;
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x00193F6C File Offset: 0x0019216C
	public List<ItemStack> SpawnTierGroup(GameRandom random, int tierGroupIndex)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.TierItemGroups[tierGroupIndex].traderItems.Shuffle<TraderInfo.TraderItemEntry>();
		int num = this.RandomSpawnCount(random, this.minCount, this.maxCount);
		this.SpawnLootItemsFromList(random, this.TierItemGroups[tierGroupIndex].traderItems, num, list, null);
		return list;
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x00193FC8 File Offset: 0x001921C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnItem(GameRandom random, TraderInfo.TraderItemEntry template, ItemValue item, int countToSpawn, List<ItemStack> spawnedItems)
	{
		if (countToSpawn < 1)
		{
			return;
		}
		ItemClass itemClass = item.ItemClass;
		if (itemClass == null)
		{
			return;
		}
		if (item.HasQuality && TraderInfo.TraderMaxTier == 0)
		{
			return;
		}
		ItemClass itemClass2 = ItemClass.HandleSandboxTechType(itemClass);
		if (itemClass2 != itemClass)
		{
			if (itemClass2 == null)
			{
				return;
			}
			item = ItemClass.GetItem(itemClass2.Name, false);
			itemClass = itemClass2;
		}
		countToSpawn = Math.Min(countToSpawn, itemClass.MaxCount);
		int num = (itemClass.IsBlock() ? Block.list[item.type].EconomicBundleSize : itemClass.EconomicBundleSize);
		if (itemClass.EconomicValue == -1f)
		{
			return;
		}
		if (num > 1)
		{
			int num2 = countToSpawn % num;
			if (num2 > 0)
			{
				countToSpawn -= num2;
			}
			if (countToSpawn == 0)
			{
				countToSpawn = num;
			}
		}
		if (itemClass.CanStack())
		{
			int num3 = ItemClass.GetForId(item.type).MaxCount;
			for (int i = 0; i < spawnedItems.Count; i++)
			{
				ItemStack itemStack = spawnedItems[i];
				if (itemStack.itemValue.type == item.type)
				{
					if (itemStack.CanStack(countToSpawn))
					{
						itemStack.count += countToSpawn;
						spawnedItems[i] = itemStack;
						return;
					}
					int num4 = num3 - itemStack.count;
					itemStack.count = num3;
					spawnedItems[i] = itemStack;
					countToSpawn -= num4;
				}
			}
		}
		int num5 = template.minQuality;
		int num6 = template.maxQuality;
		ItemValue itemValue;
		if (item.HasQuality)
		{
			if (num5 <= -1)
			{
				num5 = 1;
				num6 = 6;
			}
			if (TraderInfo.TraderMaxTier != -1 && num6 > TraderInfo.TraderMaxTier)
			{
				num6 = TraderInfo.TraderMaxTier;
			}
			if (num6 < num5)
			{
				return;
			}
			if (template != null && template.parentGroup != null && template.parentGroup.modsToInstall.Length != 0)
			{
				itemValue = new ItemValue(item.type, num5, num6, true, template.parentGroup.modsToInstall, template.parentGroup.modChance);
			}
			else
			{
				itemValue = new ItemValue(item.type, num5, num6, true, template.modsToInstall, template.modChance);
			}
		}
		else
		{
			itemValue = new ItemValue(item.type, true);
		}
		if (itemValue.ItemClass != null && itemValue.ItemClass.Actions != null && itemValue.ItemClass.Actions.Length != 0 && itemValue.ItemClass.Actions[0] != null)
		{
			itemValue.Meta = 0;
		}
		itemClass.AddGSStats(itemValue, -1, random);
		ItemStack itemStack2 = new ItemStack(itemValue, countToSpawn);
		spawnedItems.Add(itemStack2);
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x00194220 File Offset: 0x00192420
	[PublicizedFrom(EAccessModifier.Private)]
	public int RandomSpawnCount(GameRandom random, int min, int max)
	{
		if (min < 0)
		{
			return -1;
		}
		float num = random.RandomRange((float)min - 0.49f, (float)max + 0.49f);
		if (num <= (float)min)
		{
			return min;
		}
		if (num > (float)max)
		{
			num = (float)max;
		}
		int num2 = (int)num;
		float num3 = num - (float)num2;
		if (random.RandomFloat < num3)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x06004051 RID: 16465 RVA: 0x00194270 File Offset: 0x00192470
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnAllItemsFromList(GameRandom random, List<TraderInfo.TraderItemEntry> itemSet, List<ItemStack> spawnedItems)
	{
		float num = (this.IsVendingMachine ? TraderInfo.VendingItemAbundance : TraderInfo.TraderItemAbundance);
		for (int i = 0; i < itemSet.Count; i++)
		{
			TraderInfo.TraderItemEntry traderItemEntry = itemSet[i];
			int num2 = this.RandomSpawnCount(random, traderItemEntry.minCount, traderItemEntry.maxCount);
			num2 = (int)Utils.FastMax((float)num2 * num, 1f);
			if (traderItemEntry.group != null)
			{
				this.SpawnItemsFromGroup(random, traderItemEntry.group, num2, spawnedItems, traderItemEntry.uniqueOnly);
			}
			else
			{
				this.SpawnItem(random, traderItemEntry, traderItemEntry.item.itemValue, num2, spawnedItems);
			}
		}
	}

	// Token: 0x06004052 RID: 16466 RVA: 0x00194304 File Offset: 0x00192504
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnItemsFromGroup(GameRandom random, TraderInfo.TraderItemGroup group, int numToSpawn, List<ItemStack> spawnedItems, bool uniqueOnly)
	{
		List<int> list = null;
		if (group.uniqueOnly || uniqueOnly)
		{
			list = new List<int>();
		}
		for (int i = 0; i < numToSpawn; i++)
		{
			int num = this.RandomSpawnCount(random, group.minCount, group.maxCount);
			this.SpawnLootItemsFromList(random, group.items, num, spawnedItems, list);
		}
	}

	// Token: 0x06004053 RID: 16467 RVA: 0x00194358 File Offset: 0x00192558
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnLootItemsFromList(GameRandom random, List<TraderInfo.TraderItemEntry> itemSet, int numToSpawn, List<ItemStack> spawnedItems, List<int> usedIndices)
	{
		if (numToSpawn < 1)
		{
			if (numToSpawn == -1)
			{
				this.SpawnAllItemsFromList(random, itemSet, spawnedItems);
			}
			return;
		}
		float num = 0f;
		for (int i = 0; i < itemSet.Count; i++)
		{
			TraderInfo.TraderItemEntry traderItemEntry = itemSet[i];
			if (usedIndices == null || !usedIndices.Contains(i))
			{
				num += traderItemEntry.prob;
			}
		}
		if (num == 0f)
		{
			return;
		}
		for (int j = 0; j < numToSpawn; j++)
		{
			float num2 = 0f;
			float randomFloat = random.RandomFloat;
			for (int k = 0; k < itemSet.Count; k++)
			{
				TraderInfo.TraderItemEntry traderItemEntry2 = itemSet[k];
				if (usedIndices == null || !usedIndices.Contains(k))
				{
					num2 += traderItemEntry2.prob / num;
					if (randomFloat <= num2)
					{
						int num3 = this.RandomSpawnCount(random, traderItemEntry2.minCount, traderItemEntry2.maxCount);
						if (usedIndices != null)
						{
							usedIndices.Add(k);
						}
						if (traderItemEntry2.group != null)
						{
							this.SpawnItemsFromGroup(random, traderItemEntry2.group, num3, spawnedItems, traderItemEntry2.uniqueOnly);
							break;
						}
						this.SpawnItem(random, traderItemEntry2, traderItemEntry2.item.itemValue, num3, spawnedItems);
						break;
					}
				}
			}
		}
	}

	// Token: 0x0400340F RID: 13327
	public static TraderInfo[] traderInfoList;

	// Token: 0x04003410 RID: 13328
	public static Dictionary<string, TraderInfo.TraderItemGroup> traderItemGroups;

	// Token: 0x04003411 RID: 13329
	[PublicizedFrom(EAccessModifier.Private)]
	public static float buyMarkup;

	// Token: 0x04003412 RID: 13330
	[PublicizedFrom(EAccessModifier.Private)]
	public static float sellMarkdown;

	// Token: 0x04003413 RID: 13331
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMinMod;

	// Token: 0x04003414 RID: 13332
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMaxMod;

	// Token: 0x04003416 RID: 13334
	public int Id;

	// Token: 0x04003417 RID: 13335
	public float SalesMarkup;

	// Token: 0x04003418 RID: 13336
	public static bool TraderDialog = true;

	// Token: 0x04003419 RID: 13337
	public static int GlobalResetInterval = -1;

	// Token: 0x0400341A RID: 13338
	public static int GlobalResetIntervalInTicks = -1;

	// Token: 0x0400341B RID: 13339
	public static int VendingResetInterval = -1;

	// Token: 0x0400341C RID: 13340
	public static int VendingResetIntervalInTicks = -1;

	// Token: 0x0400341D RID: 13341
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetInterval = 1;

	// Token: 0x0400341E RID: 13342
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetIntervalInTicks = 24000;

	// Token: 0x0400341F RID: 13343
	public int MaxItems = 50;

	// Token: 0x04003420 RID: 13344
	public int minCount;

	// Token: 0x04003421 RID: 13345
	public int maxCount;

	// Token: 0x04003422 RID: 13346
	public bool AllowBuy = true;

	// Token: 0x04003423 RID: 13347
	public bool AllowSell = true;

	// Token: 0x04003424 RID: 13348
	public bool IsVendingMachine;

	// Token: 0x04003425 RID: 13349
	public float OverrideBuyMarkup = -1f;

	// Token: 0x04003426 RID: 13350
	public float OverrideSellMarkdown = -1f;

	// Token: 0x04003427 RID: 13351
	public bool UseOpenHours;

	// Token: 0x04003428 RID: 13352
	public ulong OpenTime;

	// Token: 0x04003429 RID: 13353
	public ulong CloseTime;

	// Token: 0x0400342A RID: 13354
	public ulong WarningTime;

	// Token: 0x0400342B RID: 13355
	public bool PlayerOwned;

	// Token: 0x0400342C RID: 13356
	public bool Rentable;

	// Token: 0x0400342D RID: 13357
	public int RentCost;

	// Token: 0x0400342E RID: 13358
	public int RentTimeInDays;

	// Token: 0x0400342F RID: 13359
	public static int TraderMaxTier = 6;

	// Token: 0x04003430 RID: 13360
	public static int TraderBuyLimit = 3;

	// Token: 0x04003431 RID: 13361
	public static float TraderItemAbundance = 1f;

	// Token: 0x04003432 RID: 13362
	public static float VendingItemAbundance = 1f;

	// Token: 0x04003433 RID: 13363
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClosedBM;

	// Token: 0x04003434 RID: 13364
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderHourPresets traderHoursPreset;

	// Token: 0x04003435 RID: 13365
	public static TraderInfo.TraderDaysPresets TraderDayPreset;

	// Token: 0x04003436 RID: 13366
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalOpenTime;

	// Token: 0x04003437 RID: 13367
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalCloseTime;

	// Token: 0x04003438 RID: 13368
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalWarningTime;

	// Token: 0x04003439 RID: 13369
	public List<TraderInfo.TierItemGroup> TierItemGroups = new List<TraderInfo.TierItemGroup>();

	// Token: 0x0400343A RID: 13370
	public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();

	// Token: 0x0400343B RID: 13371
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastTime = 0f;

	// Token: 0x0400343C RID: 13372
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderDisplayInfo traderDisplayInfo = null;

	// Token: 0x020008BC RID: 2236
	public enum TraderHourPresets
	{
		// Token: 0x0400343E RID: 13374
		Default,
		// Token: 0x0400343F RID: 13375
		MorningOnly,
		// Token: 0x04003440 RID: 13376
		MidDayOnly,
		// Token: 0x04003441 RID: 13377
		EveningOnly,
		// Token: 0x04003442 RID: 13378
		NightOnly,
		// Token: 0x04003443 RID: 13379
		OnlyClosedOnBM,
		// Token: 0x04003444 RID: 13380
		AlwaysOpen
	}

	// Token: 0x020008BD RID: 2237
	public enum TraderDaysPresets
	{
		// Token: 0x04003446 RID: 13382
		Default,
		// Token: 0x04003447 RID: 13383
		Everyday,
		// Token: 0x04003448 RID: 13384
		EveryOtherDay,
		// Token: 0x04003449 RID: 13385
		EveryThreeDays,
		// Token: 0x0400344A RID: 13386
		EveryFourDays,
		// Token: 0x0400344B RID: 13387
		EveryFiveDays
	}

	// Token: 0x020008BE RID: 2238
	public class TraderItem
	{
		// Token: 0x0400344C RID: 13388
		public ItemValue itemValue;
	}

	// Token: 0x020008BF RID: 2239
	public class TraderItemGroup
	{
		// Token: 0x0400344D RID: 13389
		public string name;

		// Token: 0x0400344E RID: 13390
		public int minCount;

		// Token: 0x0400344F RID: 13391
		public int maxCount;

		// Token: 0x04003450 RID: 13392
		public int minQuality = -1;

		// Token: 0x04003451 RID: 13393
		public int maxQuality = -1;

		// Token: 0x04003452 RID: 13394
		public string[] modsToInstall;

		// Token: 0x04003453 RID: 13395
		public float modChance = 1f;

		// Token: 0x04003454 RID: 13396
		public bool uniqueOnly;

		// Token: 0x04003455 RID: 13397
		public List<TraderInfo.TraderItemEntry> items = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008C0 RID: 2240
	public class TraderItemEntry
	{
		// Token: 0x04003456 RID: 13398
		public int minCount;

		// Token: 0x04003457 RID: 13399
		public int maxCount;

		// Token: 0x04003458 RID: 13400
		public int minQuality;

		// Token: 0x04003459 RID: 13401
		public int maxQuality;

		// Token: 0x0400345A RID: 13402
		public float prob;

		// Token: 0x0400345B RID: 13403
		public string[] modsToInstall;

		// Token: 0x0400345C RID: 13404
		public float modChance = 1f;

		// Token: 0x0400345D RID: 13405
		public bool uniqueOnly;

		// Token: 0x0400345E RID: 13406
		public TraderInfo.TraderItem item;

		// Token: 0x0400345F RID: 13407
		public TraderInfo.TraderItemGroup group;

		// Token: 0x04003460 RID: 13408
		public TraderInfo.TraderItemGroup parentGroup;
	}

	// Token: 0x020008C1 RID: 2241
	public class TierItemGroup
	{
		// Token: 0x04003461 RID: 13409
		public int minLevel;

		// Token: 0x04003462 RID: 13410
		public int maxLevel;

		// Token: 0x04003463 RID: 13411
		public int minCount;

		// Token: 0x04003464 RID: 13412
		public int maxCount;

		// Token: 0x04003465 RID: 13413
		public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008C2 RID: 2242
	public class TraderDisplayInfo
	{
		// Token: 0x0600405A RID: 16474 RVA: 0x00194534 File Offset: 0x00192734
		public void Refresh()
		{
			if (GameManager.Instance.World == null)
			{
				return;
			}
			if (this.info == null)
			{
				this.info = TraderInfo.traderInfoList[1];
				if (this.info == null)
				{
					return;
				}
				this.OpenTime = this.info.GetOpenTime();
				this.CloseTime = this.info.GetCloseTime();
				this.OpenTimeText = GameUtils.WorldTimeToHourMinutesString(this.OpenTime);
				this.CloseTimeText = GameUtils.WorldTimeToHourMinutesString(this.CloseTime);
			}
			if (Time.time - this.lastUpdate > 3f)
			{
				ulong num = GameManager.Instance.World.worldTime % 24000UL;
				if (this.OpenTime < this.CloseTime)
				{
					this.IsOpen = this.OpenTime < num && num < this.CloseTime;
				}
				else
				{
					this.IsOpen = num > this.OpenTime || num < this.CloseTime;
				}
				this.lastUpdate = Time.time;
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0019462B File Offset: 0x0019282B
		public string GetTimeTitle()
		{
			if (!this.IsOpen)
			{
				return "Opens at";
			}
			return "Closes at";
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x00194640 File Offset: 0x00192840
		public string GetTimeText()
		{
			if (!this.IsOpen)
			{
				return this.OpenTimeText;
			}
			return this.CloseTimeText;
		}

		// Token: 0x04003466 RID: 13414
		public ulong OpenTime;

		// Token: 0x04003467 RID: 13415
		public ulong CloseTime;

		// Token: 0x04003468 RID: 13416
		public string OpenTimeText;

		// Token: 0x04003469 RID: 13417
		public string CloseTimeText;

		// Token: 0x0400346A RID: 13418
		public bool IsOpen;

		// Token: 0x0400346B RID: 13419
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate;

		// Token: 0x0400346C RID: 13420
		[PublicizedFrom(EAccessModifier.Private)]
		public TraderInfo info;
	}
}
