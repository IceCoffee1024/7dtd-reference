using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
public class TraderInfo
{
	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06004008 RID: 16392 RVA: 0x001927B7 File Offset: 0x001909B7
	// (set) Token: 0x06004009 RID: 16393 RVA: 0x001927BE File Offset: 0x001909BE
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

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x0600400A RID: 16394 RVA: 0x001927C6 File Offset: 0x001909C6
	// (set) Token: 0x0600400B RID: 16395 RVA: 0x001927CD File Offset: 0x001909CD
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

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x0600400C RID: 16396 RVA: 0x001927D5 File Offset: 0x001909D5
	// (set) Token: 0x0600400D RID: 16397 RVA: 0x001927DC File Offset: 0x001909DC
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

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x0600400E RID: 16398 RVA: 0x001927E4 File Offset: 0x001909E4
	// (set) Token: 0x0600400F RID: 16399 RVA: 0x001927EB File Offset: 0x001909EB
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

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06004010 RID: 16400 RVA: 0x001927F3 File Offset: 0x001909F3
	// (set) Token: 0x06004011 RID: 16401 RVA: 0x001927FA File Offset: 0x001909FA
	public static string CurrencyItem { get; set; }

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06004012 RID: 16402 RVA: 0x00192802 File Offset: 0x00190A02
	// (set) Token: 0x06004013 RID: 16403 RVA: 0x00192840 File Offset: 0x00190A40
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

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06004014 RID: 16404 RVA: 0x00192849 File Offset: 0x00190A49
	// (set) Token: 0x06004015 RID: 16405 RVA: 0x00192887 File Offset: 0x00190A87
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

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06004016 RID: 16406 RVA: 0x00192890 File Offset: 0x00190A90
	// (set) Token: 0x06004017 RID: 16407 RVA: 0x00192898 File Offset: 0x00190A98
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

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06004018 RID: 16408 RVA: 0x001929A0 File Offset: 0x00190BA0
	public int RentTimeInSeconds
	{
		get
		{
			return this.RentTimeInDays * 60 * GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
		}
	}

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06004019 RID: 16409 RVA: 0x001929B3 File Offset: 0x00190BB3
	public int RentTimeInTicks
	{
		get
		{
			return this.RentTimeInDays * 24000;
		}
	}

	// Token: 0x0600401A RID: 16410 RVA: 0x001929C1 File Offset: 0x00190BC1
	public ulong GetOpenTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalOpenTime;
		}
		return this.OpenTime;
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x001929D6 File Offset: 0x00190BD6
	public ulong GetCloseTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalCloseTime;
		}
		return this.CloseTime;
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x001929EB File Offset: 0x00190BEB
	public ulong GetWarningTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalWarningTime;
		}
		return this.WarningTime;
	}

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x0600401D RID: 16413 RVA: 0x00192A00 File Offset: 0x00190C00
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

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x0600401E RID: 16414 RVA: 0x00192AD4 File Offset: 0x00190CD4
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

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x0600401F RID: 16415 RVA: 0x00192B3C File Offset: 0x00190D3C
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

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06004020 RID: 16416 RVA: 0x00192BB0 File Offset: 0x00190DB0
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

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06004021 RID: 16417 RVA: 0x00192C10 File Offset: 0x00190E10
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

	// Token: 0x06004023 RID: 16419 RVA: 0x00192D43 File Offset: 0x00190F43
	public static void InitStatic()
	{
		TraderInfo.traderInfoList = new TraderInfo[256];
		TraderInfo.traderItemGroups = new Dictionary<string, TraderInfo.TraderItemGroup>();
	}

	// Token: 0x06004024 RID: 16420 RVA: 0x00192D5E File Offset: 0x00190F5E
	public void Init()
	{
		TraderInfo.traderInfoList[this.Id] = this;
	}

	// Token: 0x06004025 RID: 16421 RVA: 0x00192D6D File Offset: 0x00190F6D
	public static void Cleanup()
	{
		TraderInfo.traderInfoList = null;
		TraderInfo.traderItemGroups = null;
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x00192D7B File Offset: 0x00190F7B
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

	// Token: 0x06004027 RID: 16423 RVA: 0x00192DB0 File Offset: 0x00190FB0
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

	// Token: 0x06004028 RID: 16424 RVA: 0x00192DE8 File Offset: 0x00190FE8
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

	// Token: 0x06004029 RID: 16425 RVA: 0x00192E4C File Offset: 0x0019104C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyQuality(ref ItemValue _itemValue, int minQuality = 1, int maxQuality = 6)
	{
		if (ItemClass.list[_itemValue.type].HasQuality || ItemClass.list[_itemValue.type].HasSubItems)
		{
			_itemValue = new ItemValue(_itemValue.type, Mathf.Clamp(minQuality, 1, maxQuality), maxQuality, false, null, 1f);
		}
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x00192EA0 File Offset: 0x001910A0
	public List<ItemStack> Spawn(GameRandom random)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.SpawnLootItemsFromList(random, this.traderItems, -1, list, null);
		return list;
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x00192EC4 File Offset: 0x001910C4
	public List<ItemStack> SpawnTierGroup(GameRandom random, int tierGroupIndex)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.TierItemGroups[tierGroupIndex].traderItems.Shuffle<TraderInfo.TraderItemEntry>();
		int num = this.RandomSpawnCount(random, this.minCount, this.maxCount);
		this.SpawnLootItemsFromList(random, this.TierItemGroups[tierGroupIndex].traderItems, num, list, null);
		return list;
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x00192F20 File Offset: 0x00191120
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

	// Token: 0x0600402D RID: 16429 RVA: 0x00193178 File Offset: 0x00191378
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

	// Token: 0x0600402E RID: 16430 RVA: 0x001931C8 File Offset: 0x001913C8
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

	// Token: 0x0600402F RID: 16431 RVA: 0x0019325C File Offset: 0x0019145C
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

	// Token: 0x06004030 RID: 16432 RVA: 0x001932B0 File Offset: 0x001914B0
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

	// Token: 0x040033EF RID: 13295
	public static TraderInfo[] traderInfoList;

	// Token: 0x040033F0 RID: 13296
	public static Dictionary<string, TraderInfo.TraderItemGroup> traderItemGroups;

	// Token: 0x040033F1 RID: 13297
	[PublicizedFrom(EAccessModifier.Private)]
	public static float buyMarkup;

	// Token: 0x040033F2 RID: 13298
	[PublicizedFrom(EAccessModifier.Private)]
	public static float sellMarkdown;

	// Token: 0x040033F3 RID: 13299
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMinMod;

	// Token: 0x040033F4 RID: 13300
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMaxMod;

	// Token: 0x040033F6 RID: 13302
	public int Id;

	// Token: 0x040033F7 RID: 13303
	public float SalesMarkup;

	// Token: 0x040033F8 RID: 13304
	public static bool TraderDialog = true;

	// Token: 0x040033F9 RID: 13305
	public static int GlobalResetInterval = -1;

	// Token: 0x040033FA RID: 13306
	public static int GlobalResetIntervalInTicks = -1;

	// Token: 0x040033FB RID: 13307
	public static int VendingResetInterval = -1;

	// Token: 0x040033FC RID: 13308
	public static int VendingResetIntervalInTicks = -1;

	// Token: 0x040033FD RID: 13309
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetInterval = 1;

	// Token: 0x040033FE RID: 13310
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetIntervalInTicks = 24000;

	// Token: 0x040033FF RID: 13311
	public int MaxItems = 50;

	// Token: 0x04003400 RID: 13312
	public int minCount;

	// Token: 0x04003401 RID: 13313
	public int maxCount;

	// Token: 0x04003402 RID: 13314
	public bool AllowBuy = true;

	// Token: 0x04003403 RID: 13315
	public bool AllowSell = true;

	// Token: 0x04003404 RID: 13316
	public bool IsVendingMachine;

	// Token: 0x04003405 RID: 13317
	public float OverrideBuyMarkup = -1f;

	// Token: 0x04003406 RID: 13318
	public float OverrideSellMarkdown = -1f;

	// Token: 0x04003407 RID: 13319
	public bool UseOpenHours;

	// Token: 0x04003408 RID: 13320
	public ulong OpenTime;

	// Token: 0x04003409 RID: 13321
	public ulong CloseTime;

	// Token: 0x0400340A RID: 13322
	public ulong WarningTime;

	// Token: 0x0400340B RID: 13323
	public bool PlayerOwned;

	// Token: 0x0400340C RID: 13324
	public bool Rentable;

	// Token: 0x0400340D RID: 13325
	public int RentCost;

	// Token: 0x0400340E RID: 13326
	public int RentTimeInDays;

	// Token: 0x0400340F RID: 13327
	public static int TraderMaxTier = 6;

	// Token: 0x04003410 RID: 13328
	public static int TraderBuyLimit = 3;

	// Token: 0x04003411 RID: 13329
	public static float TraderItemAbundance = 1f;

	// Token: 0x04003412 RID: 13330
	public static float VendingItemAbundance = 1f;

	// Token: 0x04003413 RID: 13331
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClosedBM;

	// Token: 0x04003414 RID: 13332
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderHourPresets traderHoursPreset;

	// Token: 0x04003415 RID: 13333
	public static TraderInfo.TraderDaysPresets TraderDayPreset;

	// Token: 0x04003416 RID: 13334
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalOpenTime;

	// Token: 0x04003417 RID: 13335
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalCloseTime;

	// Token: 0x04003418 RID: 13336
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalWarningTime;

	// Token: 0x04003419 RID: 13337
	public List<TraderInfo.TierItemGroup> TierItemGroups = new List<TraderInfo.TierItemGroup>();

	// Token: 0x0400341A RID: 13338
	public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();

	// Token: 0x0400341B RID: 13339
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastTime = 0f;

	// Token: 0x0400341C RID: 13340
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderDisplayInfo traderDisplayInfo = null;

	// Token: 0x020008B2 RID: 2226
	public enum TraderHourPresets
	{
		// Token: 0x0400341E RID: 13342
		Default,
		// Token: 0x0400341F RID: 13343
		MorningOnly,
		// Token: 0x04003420 RID: 13344
		MidDayOnly,
		// Token: 0x04003421 RID: 13345
		EveningOnly,
		// Token: 0x04003422 RID: 13346
		NightOnly,
		// Token: 0x04003423 RID: 13347
		OnlyClosedOnBM,
		// Token: 0x04003424 RID: 13348
		AlwaysOpen
	}

	// Token: 0x020008B3 RID: 2227
	public enum TraderDaysPresets
	{
		// Token: 0x04003426 RID: 13350
		Default,
		// Token: 0x04003427 RID: 13351
		Everyday,
		// Token: 0x04003428 RID: 13352
		EveryOtherDay,
		// Token: 0x04003429 RID: 13353
		EveryThreeDays,
		// Token: 0x0400342A RID: 13354
		EveryFourDays,
		// Token: 0x0400342B RID: 13355
		EveryFiveDays
	}

	// Token: 0x020008B4 RID: 2228
	public class TraderItem
	{
		// Token: 0x0400342C RID: 13356
		public ItemValue itemValue;
	}

	// Token: 0x020008B5 RID: 2229
	public class TraderItemGroup
	{
		// Token: 0x0400342D RID: 13357
		public string name;

		// Token: 0x0400342E RID: 13358
		public int minCount;

		// Token: 0x0400342F RID: 13359
		public int maxCount;

		// Token: 0x04003430 RID: 13360
		public int minQuality = -1;

		// Token: 0x04003431 RID: 13361
		public int maxQuality = -1;

		// Token: 0x04003432 RID: 13362
		public string[] modsToInstall;

		// Token: 0x04003433 RID: 13363
		public float modChance = 1f;

		// Token: 0x04003434 RID: 13364
		public bool uniqueOnly;

		// Token: 0x04003435 RID: 13365
		public List<TraderInfo.TraderItemEntry> items = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008B6 RID: 2230
	public class TraderItemEntry
	{
		// Token: 0x04003436 RID: 13366
		public int minCount;

		// Token: 0x04003437 RID: 13367
		public int maxCount;

		// Token: 0x04003438 RID: 13368
		public int minQuality;

		// Token: 0x04003439 RID: 13369
		public int maxQuality;

		// Token: 0x0400343A RID: 13370
		public float prob;

		// Token: 0x0400343B RID: 13371
		public string[] modsToInstall;

		// Token: 0x0400343C RID: 13372
		public float modChance = 1f;

		// Token: 0x0400343D RID: 13373
		public bool uniqueOnly;

		// Token: 0x0400343E RID: 13374
		public TraderInfo.TraderItem item;

		// Token: 0x0400343F RID: 13375
		public TraderInfo.TraderItemGroup group;

		// Token: 0x04003440 RID: 13376
		public TraderInfo.TraderItemGroup parentGroup;
	}

	// Token: 0x020008B7 RID: 2231
	public class TierItemGroup
	{
		// Token: 0x04003441 RID: 13377
		public int minLevel;

		// Token: 0x04003442 RID: 13378
		public int maxLevel;

		// Token: 0x04003443 RID: 13379
		public int minCount;

		// Token: 0x04003444 RID: 13380
		public int maxCount;

		// Token: 0x04003445 RID: 13381
		public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008B8 RID: 2232
	public class TraderDisplayInfo
	{
		// Token: 0x06004037 RID: 16439 RVA: 0x0019348C File Offset: 0x0019168C
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

		// Token: 0x06004038 RID: 16440 RVA: 0x00193583 File Offset: 0x00191783
		public string GetTimeTitle()
		{
			if (!this.IsOpen)
			{
				return "Opens at";
			}
			return "Closes at";
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00193598 File Offset: 0x00191798
		public string GetTimeText()
		{
			if (!this.IsOpen)
			{
				return this.OpenTimeText;
			}
			return this.CloseTimeText;
		}

		// Token: 0x04003446 RID: 13382
		public ulong OpenTime;

		// Token: 0x04003447 RID: 13383
		public ulong CloseTime;

		// Token: 0x04003448 RID: 13384
		public string OpenTimeText;

		// Token: 0x04003449 RID: 13385
		public string CloseTimeText;

		// Token: 0x0400344A RID: 13386
		public bool IsOpen;

		// Token: 0x0400344B RID: 13387
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate;

		// Token: 0x0400344C RID: 13388
		[PublicizedFrom(EAccessModifier.Private)]
		public TraderInfo info;
	}
}
