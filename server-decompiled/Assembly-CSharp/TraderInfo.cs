using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public class TraderInfo
{
	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x0600400D RID: 16397 RVA: 0x001905E7 File Offset: 0x0018E7E7
	// (set) Token: 0x0600400E RID: 16398 RVA: 0x001905EE File Offset: 0x0018E7EE
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

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x0600400F RID: 16399 RVA: 0x001905F6 File Offset: 0x0018E7F6
	// (set) Token: 0x06004010 RID: 16400 RVA: 0x001905FD File Offset: 0x0018E7FD
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

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06004011 RID: 16401 RVA: 0x00190605 File Offset: 0x0018E805
	// (set) Token: 0x06004012 RID: 16402 RVA: 0x0019060C File Offset: 0x0018E80C
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

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06004013 RID: 16403 RVA: 0x00190614 File Offset: 0x0018E814
	// (set) Token: 0x06004014 RID: 16404 RVA: 0x0019061B File Offset: 0x0018E81B
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

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06004015 RID: 16405 RVA: 0x00190623 File Offset: 0x0018E823
	// (set) Token: 0x06004016 RID: 16406 RVA: 0x0019062A File Offset: 0x0018E82A
	public static string CurrencyItem { get; set; }

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06004017 RID: 16407 RVA: 0x00190632 File Offset: 0x0018E832
	// (set) Token: 0x06004018 RID: 16408 RVA: 0x00190668 File Offset: 0x0018E868
	public int ResetInterval
	{
		get
		{
			if (this.IsVendingMachine)
			{
				if (this.PlayerOwned)
				{
					return -1;
				}
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

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06004019 RID: 16409 RVA: 0x00190671 File Offset: 0x0018E871
	// (set) Token: 0x0600401A RID: 16410 RVA: 0x001906A7 File Offset: 0x0018E8A7
	public int ResetIntervalInTicks
	{
		get
		{
			if (this.IsVendingMachine)
			{
				if (this.PlayerOwned)
				{
					return -1;
				}
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

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x0600401B RID: 16411 RVA: 0x001906B0 File Offset: 0x0018E8B0
	// (set) Token: 0x0600401C RID: 16412 RVA: 0x001906B8 File Offset: 0x0018E8B8
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

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x0600401D RID: 16413 RVA: 0x001907C0 File Offset: 0x0018E9C0
	public int RentTimeInSeconds
	{
		get
		{
			return this.RentTimeInDays * 60 * GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x0600401E RID: 16414 RVA: 0x001907D3 File Offset: 0x0018E9D3
	public int RentTimeInTicks
	{
		get
		{
			return this.RentTimeInDays * 24000;
		}
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x001907E1 File Offset: 0x0018E9E1
	public ulong GetOpenTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalOpenTime;
		}
		return this.OpenTime;
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x001907F6 File Offset: 0x0018E9F6
	public ulong GetCloseTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalCloseTime;
		}
		return this.CloseTime;
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x0019080B File Offset: 0x0018EA0B
	public ulong GetWarningTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalWarningTime;
		}
		return this.WarningTime;
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06004022 RID: 16418 RVA: 0x00190820 File Offset: 0x0018EA20
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

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06004023 RID: 16419 RVA: 0x001908F4 File Offset: 0x0018EAF4
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

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06004024 RID: 16420 RVA: 0x0019095C File Offset: 0x0018EB5C
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

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06004025 RID: 16421 RVA: 0x001909D0 File Offset: 0x0018EBD0
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

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06004026 RID: 16422 RVA: 0x00190A30 File Offset: 0x0018EC30
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

	// Token: 0x06004028 RID: 16424 RVA: 0x00190B63 File Offset: 0x0018ED63
	public static void InitStatic()
	{
		TraderInfo.traderInfoList = new TraderInfo[256];
		TraderInfo.traderItemGroups = new Dictionary<string, TraderInfo.TraderItemGroup>();
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x00190B7E File Offset: 0x0018ED7E
	public void Init()
	{
		TraderInfo.traderInfoList[this.Id] = this;
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x00190B8D File Offset: 0x0018ED8D
	public static void Cleanup()
	{
		TraderInfo.traderInfoList = null;
		TraderInfo.traderItemGroups = null;
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x00190B9B File Offset: 0x0018ED9B
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

	// Token: 0x0600402C RID: 16428 RVA: 0x00190BD0 File Offset: 0x0018EDD0
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

	// Token: 0x0600402D RID: 16429 RVA: 0x00190C08 File Offset: 0x0018EE08
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

	// Token: 0x0600402E RID: 16430 RVA: 0x00190C6C File Offset: 0x0018EE6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyQuality(ref ItemValue _itemValue, int minQuality = 1, int maxQuality = 6)
	{
		if (ItemClass.list[_itemValue.type].HasQuality || ItemClass.list[_itemValue.type].HasSubItems)
		{
			_itemValue = new ItemValue(_itemValue.type, Mathf.Clamp(minQuality, 1, maxQuality), maxQuality, false, null, 1f);
		}
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x00190CC0 File Offset: 0x0018EEC0
	public List<ItemStack> Spawn(GameRandom random)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.SpawnLootItemsFromList(random, this.traderItems, -1, list, null);
		return list;
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x00190CE4 File Offset: 0x0018EEE4
	public List<ItemStack> SpawnTierGroup(GameRandom random, int tierGroupIndex)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.TierItemGroups[tierGroupIndex].traderItems.Shuffle<TraderInfo.TraderItemEntry>();
		int num = this.RandomSpawnCount(random, this.minCount, this.maxCount);
		this.SpawnLootItemsFromList(random, this.TierItemGroups[tierGroupIndex].traderItems, num, list, null);
		return list;
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x00190D40 File Offset: 0x0018EF40
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
		countToSpawn = Math.Min(countToSpawn, itemClass.Stacknumber.Value);
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
			int value = ItemClass.GetForId(item.type).Stacknumber.Value;
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
					int num3 = value - itemStack.count;
					itemStack.count = value;
					spawnedItems[i] = itemStack;
					countToSpawn -= num3;
				}
			}
		}
		int num4 = template.minQuality;
		int num5 = template.maxQuality;
		ItemValue itemValue;
		if (item.HasQuality)
		{
			if (num4 <= -1)
			{
				num4 = 1;
				num5 = 6;
			}
			if (num5 > TraderInfo.TraderMaxTier)
			{
				num5 = TraderInfo.TraderMaxTier;
			}
			if (num5 < num4)
			{
				return;
			}
			if (template != null && template.parentGroup != null && template.parentGroup.modsToInstall.Length != 0)
			{
				itemValue = new ItemValue(item.type, num4, num5, true, template.parentGroup.modsToInstall, template.parentGroup.modChance);
			}
			else
			{
				itemValue = new ItemValue(item.type, num4, num5, true, template.modsToInstall, template.modChance);
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

	// Token: 0x06004032 RID: 16434 RVA: 0x00190F98 File Offset: 0x0018F198
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

	// Token: 0x06004033 RID: 16435 RVA: 0x00190FE8 File Offset: 0x0018F1E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnAllItemsFromList(GameRandom random, List<TraderInfo.TraderItemEntry> itemSet, List<ItemStack> spawnedItems)
	{
		float num = (this.IsVendingMachine ? TraderInfo.VendingItemAbundance : TraderInfo.TraderItemAbundance);
		for (int i = 0; i < itemSet.Count; i++)
		{
			TraderInfo.TraderItemEntry traderItemEntry = itemSet[i];
			int num2 = this.RandomSpawnCount(random, (int)((float)traderItemEntry.minCount * num), (int)((float)traderItemEntry.maxCount * num));
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

	// Token: 0x06004034 RID: 16436 RVA: 0x00191074 File Offset: 0x0018F274
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

	// Token: 0x06004035 RID: 16437 RVA: 0x001910C8 File Offset: 0x0018F2C8
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

	// Token: 0x0400337A RID: 13178
	public static TraderInfo[] traderInfoList;

	// Token: 0x0400337B RID: 13179
	public static Dictionary<string, TraderInfo.TraderItemGroup> traderItemGroups;

	// Token: 0x0400337C RID: 13180
	[PublicizedFrom(EAccessModifier.Private)]
	public static float buyMarkup;

	// Token: 0x0400337D RID: 13181
	[PublicizedFrom(EAccessModifier.Private)]
	public static float sellMarkdown;

	// Token: 0x0400337E RID: 13182
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMinMod;

	// Token: 0x0400337F RID: 13183
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMaxMod;

	// Token: 0x04003381 RID: 13185
	public int Id;

	// Token: 0x04003382 RID: 13186
	public float SalesMarkup;

	// Token: 0x04003383 RID: 13187
	public static bool TraderDialog = true;

	// Token: 0x04003384 RID: 13188
	public static int GlobalResetInterval = -1;

	// Token: 0x04003385 RID: 13189
	public static int GlobalResetIntervalInTicks = -1;

	// Token: 0x04003386 RID: 13190
	public static int VendingResetInterval = -1;

	// Token: 0x04003387 RID: 13191
	public static int VendingResetIntervalInTicks = -1;

	// Token: 0x04003388 RID: 13192
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetInterval = 1;

	// Token: 0x04003389 RID: 13193
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetIntervalInTicks = 24000;

	// Token: 0x0400338A RID: 13194
	public int MaxItems = 50;

	// Token: 0x0400338B RID: 13195
	public int minCount;

	// Token: 0x0400338C RID: 13196
	public int maxCount;

	// Token: 0x0400338D RID: 13197
	public bool AllowBuy = true;

	// Token: 0x0400338E RID: 13198
	public bool AllowSell = true;

	// Token: 0x0400338F RID: 13199
	public bool IsVendingMachine;

	// Token: 0x04003390 RID: 13200
	public float OverrideBuyMarkup = -1f;

	// Token: 0x04003391 RID: 13201
	public float OverrideSellMarkdown = -1f;

	// Token: 0x04003392 RID: 13202
	public bool UseOpenHours;

	// Token: 0x04003393 RID: 13203
	public ulong OpenTime;

	// Token: 0x04003394 RID: 13204
	public ulong CloseTime;

	// Token: 0x04003395 RID: 13205
	public ulong WarningTime;

	// Token: 0x04003396 RID: 13206
	public bool PlayerOwned;

	// Token: 0x04003397 RID: 13207
	public bool Rentable;

	// Token: 0x04003398 RID: 13208
	public int RentCost;

	// Token: 0x04003399 RID: 13209
	public int RentTimeInDays;

	// Token: 0x0400339A RID: 13210
	public static int TraderMaxTier = 6;

	// Token: 0x0400339B RID: 13211
	public static int TraderBuyLimit = 3;

	// Token: 0x0400339C RID: 13212
	public static float TraderItemAbundance = 1f;

	// Token: 0x0400339D RID: 13213
	public static float VendingItemAbundance = 1f;

	// Token: 0x0400339E RID: 13214
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClosedBM;

	// Token: 0x0400339F RID: 13215
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderHourPresets traderHoursPreset;

	// Token: 0x040033A0 RID: 13216
	public static TraderInfo.TraderDaysPresets TraderDayPreset;

	// Token: 0x040033A1 RID: 13217
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalOpenTime;

	// Token: 0x040033A2 RID: 13218
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalCloseTime;

	// Token: 0x040033A3 RID: 13219
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalWarningTime;

	// Token: 0x040033A4 RID: 13220
	public List<TraderInfo.TierItemGroup> TierItemGroups = new List<TraderInfo.TierItemGroup>();

	// Token: 0x040033A5 RID: 13221
	public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();

	// Token: 0x040033A6 RID: 13222
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastTime = 0f;

	// Token: 0x040033A7 RID: 13223
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderDisplayInfo traderDisplayInfo = null;

	// Token: 0x020008A1 RID: 2209
	public enum TraderHourPresets
	{
		// Token: 0x040033A9 RID: 13225
		Default,
		// Token: 0x040033AA RID: 13226
		MorningOnly,
		// Token: 0x040033AB RID: 13227
		MidDayOnly,
		// Token: 0x040033AC RID: 13228
		EveningOnly,
		// Token: 0x040033AD RID: 13229
		NightOnly,
		// Token: 0x040033AE RID: 13230
		OnlyClosedOnBM,
		// Token: 0x040033AF RID: 13231
		AlwaysOpen
	}

	// Token: 0x020008A2 RID: 2210
	public enum TraderDaysPresets
	{
		// Token: 0x040033B1 RID: 13233
		Default,
		// Token: 0x040033B2 RID: 13234
		Everyday,
		// Token: 0x040033B3 RID: 13235
		EveryOtherDay,
		// Token: 0x040033B4 RID: 13236
		EveryThreeDays,
		// Token: 0x040033B5 RID: 13237
		EveryFourDays,
		// Token: 0x040033B6 RID: 13238
		EveryFiveDays
	}

	// Token: 0x020008A3 RID: 2211
	public class TraderItem
	{
		// Token: 0x040033B7 RID: 13239
		public ItemValue itemValue;
	}

	// Token: 0x020008A4 RID: 2212
	public class TraderItemGroup
	{
		// Token: 0x040033B8 RID: 13240
		public string name;

		// Token: 0x040033B9 RID: 13241
		public int minCount;

		// Token: 0x040033BA RID: 13242
		public int maxCount;

		// Token: 0x040033BB RID: 13243
		public int minQuality = -1;

		// Token: 0x040033BC RID: 13244
		public int maxQuality = -1;

		// Token: 0x040033BD RID: 13245
		public string[] modsToInstall;

		// Token: 0x040033BE RID: 13246
		public float modChance = 1f;

		// Token: 0x040033BF RID: 13247
		public bool uniqueOnly;

		// Token: 0x040033C0 RID: 13248
		public List<TraderInfo.TraderItemEntry> items = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008A5 RID: 2213
	public class TraderItemEntry
	{
		// Token: 0x040033C1 RID: 13249
		public int minCount;

		// Token: 0x040033C2 RID: 13250
		public int maxCount;

		// Token: 0x040033C3 RID: 13251
		public int minQuality;

		// Token: 0x040033C4 RID: 13252
		public int maxQuality;

		// Token: 0x040033C5 RID: 13253
		public float prob;

		// Token: 0x040033C6 RID: 13254
		public string[] modsToInstall;

		// Token: 0x040033C7 RID: 13255
		public float modChance = 1f;

		// Token: 0x040033C8 RID: 13256
		public bool uniqueOnly;

		// Token: 0x040033C9 RID: 13257
		public TraderInfo.TraderItem item;

		// Token: 0x040033CA RID: 13258
		public TraderInfo.TraderItemGroup group;

		// Token: 0x040033CB RID: 13259
		public TraderInfo.TraderItemGroup parentGroup;
	}

	// Token: 0x020008A6 RID: 2214
	public class TierItemGroup
	{
		// Token: 0x040033CC RID: 13260
		public int minLevel;

		// Token: 0x040033CD RID: 13261
		public int maxLevel;

		// Token: 0x040033CE RID: 13262
		public int minCount;

		// Token: 0x040033CF RID: 13263
		public int maxCount;

		// Token: 0x040033D0 RID: 13264
		public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008A7 RID: 2215
	public class TraderDisplayInfo
	{
		// Token: 0x0600403C RID: 16444 RVA: 0x001912A4 File Offset: 0x0018F4A4
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

		// Token: 0x0600403D RID: 16445 RVA: 0x0019139B File Offset: 0x0018F59B
		public string GetTimeTitle()
		{
			if (!this.IsOpen)
			{
				return "Opens at";
			}
			return "Closes at";
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x001913B0 File Offset: 0x0018F5B0
		public string GetTimeText()
		{
			if (!this.IsOpen)
			{
				return this.OpenTimeText;
			}
			return this.CloseTimeText;
		}

		// Token: 0x040033D1 RID: 13265
		public ulong OpenTime;

		// Token: 0x040033D2 RID: 13266
		public ulong CloseTime;

		// Token: 0x040033D3 RID: 13267
		public string OpenTimeText;

		// Token: 0x040033D4 RID: 13268
		public string CloseTimeText;

		// Token: 0x040033D5 RID: 13269
		public bool IsOpen;

		// Token: 0x040033D6 RID: 13270
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate;

		// Token: 0x040033D7 RID: 13271
		[PublicizedFrom(EAccessModifier.Private)]
		public TraderInfo info;
	}
}
