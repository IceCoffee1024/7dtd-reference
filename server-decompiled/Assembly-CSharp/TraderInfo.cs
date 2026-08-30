using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008AC RID: 2220
public class TraderInfo
{
	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06004072 RID: 16498 RVA: 0x001926B3 File Offset: 0x001908B3
	// (set) Token: 0x06004073 RID: 16499 RVA: 0x001926BA File Offset: 0x001908BA
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

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06004074 RID: 16500 RVA: 0x001926C2 File Offset: 0x001908C2
	// (set) Token: 0x06004075 RID: 16501 RVA: 0x001926C9 File Offset: 0x001908C9
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

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06004076 RID: 16502 RVA: 0x001926D1 File Offset: 0x001908D1
	// (set) Token: 0x06004077 RID: 16503 RVA: 0x001926D8 File Offset: 0x001908D8
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

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06004078 RID: 16504 RVA: 0x001926E0 File Offset: 0x001908E0
	// (set) Token: 0x06004079 RID: 16505 RVA: 0x001926E7 File Offset: 0x001908E7
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

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x0600407A RID: 16506 RVA: 0x001926EF File Offset: 0x001908EF
	// (set) Token: 0x0600407B RID: 16507 RVA: 0x001926F6 File Offset: 0x001908F6
	public static string CurrencyItem { get; set; }

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x0600407C RID: 16508 RVA: 0x001926FE File Offset: 0x001908FE
	// (set) Token: 0x0600407D RID: 16509 RVA: 0x0019273C File Offset: 0x0019093C
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

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x0600407E RID: 16510 RVA: 0x00192745 File Offset: 0x00190945
	// (set) Token: 0x0600407F RID: 16511 RVA: 0x00192783 File Offset: 0x00190983
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

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06004080 RID: 16512 RVA: 0x0019278C File Offset: 0x0019098C
	// (set) Token: 0x06004081 RID: 16513 RVA: 0x00192794 File Offset: 0x00190994
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

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06004082 RID: 16514 RVA: 0x0019289C File Offset: 0x00190A9C
	public int RentTimeInSeconds
	{
		get
		{
			return this.RentTimeInDays * 60 * GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06004083 RID: 16515 RVA: 0x001928AF File Offset: 0x00190AAF
	public int RentTimeInTicks
	{
		get
		{
			return this.RentTimeInDays * 24000;
		}
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x001928BD File Offset: 0x00190ABD
	public ulong GetOpenTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalOpenTime;
		}
		return this.OpenTime;
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x001928D2 File Offset: 0x00190AD2
	public ulong GetCloseTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalCloseTime;
		}
		return this.CloseTime;
	}

	// Token: 0x06004086 RID: 16518 RVA: 0x001928E7 File Offset: 0x00190AE7
	public ulong GetWarningTime()
	{
		if (TraderInfo.traderHoursPreset != TraderInfo.TraderHourPresets.Default)
		{
			return TraderInfo.GlobalWarningTime;
		}
		return this.WarningTime;
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06004087 RID: 16519 RVA: 0x001928FC File Offset: 0x00190AFC
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

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06004088 RID: 16520 RVA: 0x001929D0 File Offset: 0x00190BD0
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

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06004089 RID: 16521 RVA: 0x00192A38 File Offset: 0x00190C38
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

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x0600408A RID: 16522 RVA: 0x00192AAC File Offset: 0x00190CAC
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

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x0600408B RID: 16523 RVA: 0x00192B0C File Offset: 0x00190D0C
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

	// Token: 0x0600408D RID: 16525 RVA: 0x00192C3F File Offset: 0x00190E3F
	public static void InitStatic()
	{
		TraderInfo.traderInfoList = new TraderInfo[256];
		TraderInfo.traderItemGroups = new Dictionary<string, TraderInfo.TraderItemGroup>();
	}

	// Token: 0x0600408E RID: 16526 RVA: 0x00192C5A File Offset: 0x00190E5A
	public void Init()
	{
		TraderInfo.traderInfoList[this.Id] = this;
	}

	// Token: 0x0600408F RID: 16527 RVA: 0x00192C69 File Offset: 0x00190E69
	public static void Cleanup()
	{
		TraderInfo.traderInfoList = null;
		TraderInfo.traderItemGroups = null;
	}

	// Token: 0x06004090 RID: 16528 RVA: 0x00192C77 File Offset: 0x00190E77
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

	// Token: 0x06004091 RID: 16529 RVA: 0x00192CAC File Offset: 0x00190EAC
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

	// Token: 0x06004092 RID: 16530 RVA: 0x00192CE4 File Offset: 0x00190EE4
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

	// Token: 0x06004093 RID: 16531 RVA: 0x00192D48 File Offset: 0x00190F48
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyQuality(ref ItemValue _itemValue, int minQuality = 1, int maxQuality = 6)
	{
		if (ItemClass.list[_itemValue.type].HasQuality || ItemClass.list[_itemValue.type].HasSubItems)
		{
			_itemValue = new ItemValue(_itemValue.type, Mathf.Clamp(minQuality, 1, maxQuality), maxQuality, false, null, 1f);
		}
	}

	// Token: 0x06004094 RID: 16532 RVA: 0x00192D9C File Offset: 0x00190F9C
	public List<ItemStack> Spawn(GameRandom random)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.SpawnLootItemsFromList(random, this.traderItems, -1, list, null);
		return list;
	}

	// Token: 0x06004095 RID: 16533 RVA: 0x00192DC0 File Offset: 0x00190FC0
	public List<ItemStack> SpawnTierGroup(GameRandom random, int tierGroupIndex)
	{
		List<ItemStack> list = new List<ItemStack>();
		this.TierItemGroups[tierGroupIndex].traderItems.Shuffle<TraderInfo.TraderItemEntry>();
		int num = this.RandomSpawnCount(random, this.minCount, this.maxCount);
		this.SpawnLootItemsFromList(random, this.TierItemGroups[tierGroupIndex].traderItems, num, list, null);
		return list;
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x00192E1C File Offset: 0x0019101C
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

	// Token: 0x06004097 RID: 16535 RVA: 0x00193074 File Offset: 0x00191274
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

	// Token: 0x06004098 RID: 16536 RVA: 0x001930C4 File Offset: 0x001912C4
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

	// Token: 0x06004099 RID: 16537 RVA: 0x00193158 File Offset: 0x00191358
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

	// Token: 0x0600409A RID: 16538 RVA: 0x001931AC File Offset: 0x001913AC
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

	// Token: 0x040033FB RID: 13307
	public static TraderInfo[] traderInfoList;

	// Token: 0x040033FC RID: 13308
	public static Dictionary<string, TraderInfo.TraderItemGroup> traderItemGroups;

	// Token: 0x040033FD RID: 13309
	[PublicizedFrom(EAccessModifier.Private)]
	public static float buyMarkup;

	// Token: 0x040033FE RID: 13310
	[PublicizedFrom(EAccessModifier.Private)]
	public static float sellMarkdown;

	// Token: 0x040033FF RID: 13311
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMinMod;

	// Token: 0x04003400 RID: 13312
	[PublicizedFrom(EAccessModifier.Private)]
	public static float qualityMaxMod;

	// Token: 0x04003402 RID: 13314
	public int Id;

	// Token: 0x04003403 RID: 13315
	public float SalesMarkup;

	// Token: 0x04003404 RID: 13316
	public static bool TraderDialog = true;

	// Token: 0x04003405 RID: 13317
	public static int GlobalResetInterval = -1;

	// Token: 0x04003406 RID: 13318
	public static int GlobalResetIntervalInTicks = -1;

	// Token: 0x04003407 RID: 13319
	public static int VendingResetInterval = -1;

	// Token: 0x04003408 RID: 13320
	public static int VendingResetIntervalInTicks = -1;

	// Token: 0x04003409 RID: 13321
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetInterval = 1;

	// Token: 0x0400340A RID: 13322
	[PublicizedFrom(EAccessModifier.Private)]
	public int resetIntervalInTicks = 24000;

	// Token: 0x0400340B RID: 13323
	public int MaxItems = 50;

	// Token: 0x0400340C RID: 13324
	public int minCount;

	// Token: 0x0400340D RID: 13325
	public int maxCount;

	// Token: 0x0400340E RID: 13326
	public bool AllowBuy = true;

	// Token: 0x0400340F RID: 13327
	public bool AllowSell = true;

	// Token: 0x04003410 RID: 13328
	public bool IsVendingMachine;

	// Token: 0x04003411 RID: 13329
	public float OverrideBuyMarkup = -1f;

	// Token: 0x04003412 RID: 13330
	public float OverrideSellMarkdown = -1f;

	// Token: 0x04003413 RID: 13331
	public bool UseOpenHours;

	// Token: 0x04003414 RID: 13332
	public ulong OpenTime;

	// Token: 0x04003415 RID: 13333
	public ulong CloseTime;

	// Token: 0x04003416 RID: 13334
	public ulong WarningTime;

	// Token: 0x04003417 RID: 13335
	public bool PlayerOwned;

	// Token: 0x04003418 RID: 13336
	public bool Rentable;

	// Token: 0x04003419 RID: 13337
	public int RentCost;

	// Token: 0x0400341A RID: 13338
	public int RentTimeInDays;

	// Token: 0x0400341B RID: 13339
	public static int TraderMaxTier = 6;

	// Token: 0x0400341C RID: 13340
	public static int TraderBuyLimit = 3;

	// Token: 0x0400341D RID: 13341
	public static float TraderItemAbundance = 1f;

	// Token: 0x0400341E RID: 13342
	public static float VendingItemAbundance = 1f;

	// Token: 0x0400341F RID: 13343
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClosedBM;

	// Token: 0x04003420 RID: 13344
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderHourPresets traderHoursPreset;

	// Token: 0x04003421 RID: 13345
	public static TraderInfo.TraderDaysPresets TraderDayPreset;

	// Token: 0x04003422 RID: 13346
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalOpenTime;

	// Token: 0x04003423 RID: 13347
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalCloseTime;

	// Token: 0x04003424 RID: 13348
	[PublicizedFrom(EAccessModifier.Private)]
	public static ulong GlobalWarningTime;

	// Token: 0x04003425 RID: 13349
	public List<TraderInfo.TierItemGroup> TierItemGroups = new List<TraderInfo.TierItemGroup>();

	// Token: 0x04003426 RID: 13350
	public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();

	// Token: 0x04003427 RID: 13351
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastTime = 0f;

	// Token: 0x04003428 RID: 13352
	[PublicizedFrom(EAccessModifier.Private)]
	public static TraderInfo.TraderDisplayInfo traderDisplayInfo = null;

	// Token: 0x020008AD RID: 2221
	public enum TraderHourPresets
	{
		// Token: 0x0400342A RID: 13354
		Default,
		// Token: 0x0400342B RID: 13355
		MorningOnly,
		// Token: 0x0400342C RID: 13356
		MidDayOnly,
		// Token: 0x0400342D RID: 13357
		EveningOnly,
		// Token: 0x0400342E RID: 13358
		NightOnly,
		// Token: 0x0400342F RID: 13359
		OnlyClosedOnBM,
		// Token: 0x04003430 RID: 13360
		AlwaysOpen
	}

	// Token: 0x020008AE RID: 2222
	public enum TraderDaysPresets
	{
		// Token: 0x04003432 RID: 13362
		Default,
		// Token: 0x04003433 RID: 13363
		Everyday,
		// Token: 0x04003434 RID: 13364
		EveryOtherDay,
		// Token: 0x04003435 RID: 13365
		EveryThreeDays,
		// Token: 0x04003436 RID: 13366
		EveryFourDays,
		// Token: 0x04003437 RID: 13367
		EveryFiveDays
	}

	// Token: 0x020008AF RID: 2223
	public class TraderItem
	{
		// Token: 0x04003438 RID: 13368
		public ItemValue itemValue;
	}

	// Token: 0x020008B0 RID: 2224
	public class TraderItemGroup
	{
		// Token: 0x04003439 RID: 13369
		public string name;

		// Token: 0x0400343A RID: 13370
		public int minCount;

		// Token: 0x0400343B RID: 13371
		public int maxCount;

		// Token: 0x0400343C RID: 13372
		public int minQuality = -1;

		// Token: 0x0400343D RID: 13373
		public int maxQuality = -1;

		// Token: 0x0400343E RID: 13374
		public string[] modsToInstall;

		// Token: 0x0400343F RID: 13375
		public float modChance = 1f;

		// Token: 0x04003440 RID: 13376
		public bool uniqueOnly;

		// Token: 0x04003441 RID: 13377
		public List<TraderInfo.TraderItemEntry> items = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008B1 RID: 2225
	public class TraderItemEntry
	{
		// Token: 0x04003442 RID: 13378
		public int minCount;

		// Token: 0x04003443 RID: 13379
		public int maxCount;

		// Token: 0x04003444 RID: 13380
		public int minQuality;

		// Token: 0x04003445 RID: 13381
		public int maxQuality;

		// Token: 0x04003446 RID: 13382
		public float prob;

		// Token: 0x04003447 RID: 13383
		public string[] modsToInstall;

		// Token: 0x04003448 RID: 13384
		public float modChance = 1f;

		// Token: 0x04003449 RID: 13385
		public bool uniqueOnly;

		// Token: 0x0400344A RID: 13386
		public TraderInfo.TraderItem item;

		// Token: 0x0400344B RID: 13387
		public TraderInfo.TraderItemGroup group;

		// Token: 0x0400344C RID: 13388
		public TraderInfo.TraderItemGroup parentGroup;
	}

	// Token: 0x020008B2 RID: 2226
	public class TierItemGroup
	{
		// Token: 0x0400344D RID: 13389
		public int minLevel;

		// Token: 0x0400344E RID: 13390
		public int maxLevel;

		// Token: 0x0400344F RID: 13391
		public int minCount;

		// Token: 0x04003450 RID: 13392
		public int maxCount;

		// Token: 0x04003451 RID: 13393
		public List<TraderInfo.TraderItemEntry> traderItems = new List<TraderInfo.TraderItemEntry>();
	}

	// Token: 0x020008B3 RID: 2227
	public class TraderDisplayInfo
	{
		// Token: 0x060040A1 RID: 16545 RVA: 0x00193388 File Offset: 0x00191588
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

		// Token: 0x060040A2 RID: 16546 RVA: 0x0019347F File Offset: 0x0019167F
		public string GetTimeTitle()
		{
			if (!this.IsOpen)
			{
				return "Opens at";
			}
			return "Closes at";
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x00193494 File Offset: 0x00191694
		public string GetTimeText()
		{
			if (!this.IsOpen)
			{
				return this.OpenTimeText;
			}
			return this.CloseTimeText;
		}

		// Token: 0x04003452 RID: 13394
		public ulong OpenTime;

		// Token: 0x04003453 RID: 13395
		public ulong CloseTime;

		// Token: 0x04003454 RID: 13396
		public string OpenTimeText;

		// Token: 0x04003455 RID: 13397
		public string CloseTimeText;

		// Token: 0x04003456 RID: 13398
		public bool IsOpen;

		// Token: 0x04003457 RID: 13399
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate;

		// Token: 0x04003458 RID: 13400
		[PublicizedFrom(EAccessModifier.Private)]
		public TraderInfo info;
	}
}
