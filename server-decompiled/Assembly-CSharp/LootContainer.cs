using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000622 RID: 1570
public class LootContainer
{
	// Token: 0x0600333A RID: 13114 RVA: 0x0014C43B File Offset: 0x0014A63B
	public static void InitStatic()
	{
		LootContainer.Cleanup();
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x0014C442 File Offset: 0x0014A642
	public void Init()
	{
		LootContainer.lootContainers[this.Name] = this;
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x0014C455 File Offset: 0x0014A655
	public static void Cleanup()
	{
		LootContainer.lootContainers.Clear();
		LootContainer.lootGroups.Clear();
		LootContainer.lootQualityTemplates.Clear();
		LootContainer.lootProbTemplates.Clear();
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x0014C47F File Offset: 0x0014A67F
	public static bool IsLoaded()
	{
		return LootContainer.lootContainers.Count > 0;
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0014C490 File Offset: 0x0014A690
	public static LootContainer GetLootContainer(string _name, bool _errorOnMiss = true)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return null;
		}
		LootContainer lootContainer;
		if (LootContainer.lootContainers.TryGetValue(_name, out lootContainer))
		{
			return lootContainer;
		}
		if (_errorOnMiss)
		{
			Log.Error("LootContainer '" + _name + "' unknown");
		}
		return null;
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0014C4D4 File Offset: 0x0014A6D4
	public static ItemStack GetRewardItem(string lootGroup, float questDifficulty)
	{
		if (!LootContainer.lootGroups.ContainsKey(lootGroup))
		{
			return ItemStack.Empty;
		}
		List<ItemStack> list = new List<ItemStack>();
		int num = 1;
		LootContainer.SpawnItemsFromGroup(GameManager.Instance.lootManager.Random, LootContainer.lootGroups[lootGroup], 1, 1f, list, ref num, questDifficulty, 0f, LootContainer.lootGroups[lootGroup].lootQualityTemplate, null, FastTags<TagGroup.Global>.none, true, true, false, true, null, true);
		if (list.Count == 0)
		{
			return ItemStack.Empty;
		}
		return list[0];
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x0014C55C File Offset: 0x0014A75C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool SpawnItem(GameRandom random, LootContainer.LootEntry template, ItemValue lootItemValue, int countToSpawn, List<ItemStack> spawnedItems, ref int slotsLeft, float gameStage, string lootQualityTemplate, EntityPlayer player, FastTags<TagGroup.Global> containerTags, bool _forceStacking)
	{
		if (lootItemValue.ItemClass == null)
		{
			return false;
		}
		if (player)
		{
			countToSpawn = Math.Min((int)EffectManager.GetValue(PassiveEffects.LootQuantity, player.inventory.holdingItemItemValue, (float)countToSpawn, player, null, lootItemValue.ItemClass.ItemTags | containerTags, true, true, true, true, true, 1, true, false), lootItemValue.ItemClass.MaxCount);
		}
		if (countToSpawn < 1 || (player && LootContainer.GlobalCountModifier == 0f))
		{
			return false;
		}
		if (lootItemValue.HasQuality && LootContainer.LootMaxTier == 0)
		{
			return false;
		}
		ItemClass itemClass = ItemClass.HandleSandboxTechType(lootItemValue.ItemClass);
		if (itemClass != lootItemValue.ItemClass)
		{
			if (itemClass == null)
			{
				return false;
			}
			lootItemValue = ItemClass.GetItem(itemClass.Name, false);
		}
		if (lootItemValue.ItemClass.CanStack())
		{
			int num = lootItemValue.ItemClass.MaxCount;
			for (int i = 0; i < spawnedItems.Count; i++)
			{
				ItemStack itemStack = spawnedItems[i];
				if (itemStack.itemValue.type == lootItemValue.type)
				{
					if (itemStack.CanStack(countToSpawn) || _forceStacking)
					{
						itemStack.count += countToSpawn;
						return true;
					}
					int num2 = num - itemStack.count;
					itemStack.count = num;
					countToSpawn -= num2;
				}
			}
		}
		if (slotsLeft < 1)
		{
			return false;
		}
		int num3 = template.minQuality;
		int num4 = template.maxQuality;
		string text = lootQualityTemplate;
		if (!string.IsNullOrEmpty(text))
		{
			LootContainer.LootGroup parentGroup = template.parentGroup;
			if (((parentGroup != null) ? parentGroup.lootQualityTemplate : null) == null)
			{
				goto IL_017C;
			}
		}
		LootContainer.LootGroup parentGroup2 = template.parentGroup;
		text = ((parentGroup2 != null) ? parentGroup2.lootQualityTemplate : null);
		IL_017C:
		if (!string.IsNullOrEmpty(text))
		{
			bool flag = false;
			for (int j = 0; j < LootContainer.lootQualityTemplates[text].templates.Count; j++)
			{
				float randomFloat = random.RandomFloat;
				LootContainer.LootGroup lootGroup = LootContainer.lootQualityTemplates[text].templates[j];
				num3 = lootGroup.minQuality;
				num4 = lootGroup.maxQuality;
				if (lootGroup.minLevel <= gameStage && lootGroup.maxLevel >= gameStage)
				{
					for (int k = 0; k < lootGroup.items.Count; k++)
					{
						LootContainer.LootEntry lootEntry = lootGroup.items[k];
						if (random.RandomFloat <= lootEntry.prob)
						{
							num3 = lootEntry.minQuality;
							num4 = lootEntry.maxQuality;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}
		string[] array = template.modsToInstall;
		float num5 = template.modChance;
		if (template.parentGroup != null && template.parentGroup.modsToInstall.Length != 0)
		{
			array = template.parentGroup.modsToInstall;
			num5 = template.parentGroup.modChance;
		}
		ItemValue itemValue;
		if (lootItemValue.HasQuality)
		{
			if (num3 <= -1)
			{
				num3 = 1;
				num4 = 6;
			}
			if (LootContainer.LootMaxTier != -1 && num4 > LootContainer.LootMaxTier)
			{
				num4 = LootContainer.LootMaxTier;
			}
			if (num3 > num4)
			{
				num3 = num4;
			}
			itemValue = new ItemValue(lootItemValue.type, num3, num4, true, array, num5);
		}
		else
		{
			itemValue = new ItemValue(lootItemValue.type, 1, 6, true, array, num5);
		}
		ItemClass itemClass2 = itemValue.ItemClass;
		if (itemClass2 != null)
		{
			itemClass2.AddGSStats(itemValue, (int)gameStage, random);
			if (itemClass2.Actions != null && itemClass2.Actions.Length != 0 && itemClass2.Actions[0] != null)
			{
				itemValue.Meta = 0;
			}
			if (itemValue.MaxUseTimes > 0)
			{
				if (template.randomDurability)
				{
					itemValue.UseTimes = (float)((int)((float)itemValue.MaxUseTimes * random.RandomRange(0.2f, 0.8f)));
				}
				else
				{
					itemValue.UseTimes = 0f;
				}
			}
		}
		ItemStack itemStack2;
		if (player)
		{
			if (!LootContainer.OverrideItems.ContainsKey(player))
			{
				itemStack2 = new ItemStack(itemValue, countToSpawn);
			}
			else
			{
				string[] array2 = LootContainer.OverrideItems[player];
				itemStack2 = new ItemStack(ItemClass.GetItem(array2[random.RandomRange(array2.Length)], false), 1);
			}
		}
		else
		{
			itemStack2 = new ItemStack(itemValue, countToSpawn);
		}
		spawnedItems.Add(itemStack2);
		slotsLeft--;
		return true;
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0014C938 File Offset: 0x0014AB38
	public static int RandomSpawnCount(GameRandom random, int min, int max, float abundance)
	{
		if (min < 0)
		{
			return -1;
		}
		float num = random.RandomRange((float)min - 0.49f, (float)max + 0.49f);
		if (num < (float)min)
		{
			num = (float)min;
		}
		if (num > (float)max)
		{
			num = (float)max;
		}
		num *= abundance;
		int num2 = (int)num;
		float num3 = num - (float)num2;
		if (random.RandomFloat < num3)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x0014C98C File Offset: 0x0014AB8C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool SpawnAllItemsFromList(GameRandom random, List<LootContainer.LootEntry> itemSet, float abundance, List<ItemStack> spawnedItems, ref int slotsLeft, float playerLevelPercentage, float rareLootChance, string lootQualityTemplate, EntityPlayer player, FastTags<TagGroup.Global> containerTags, bool uniqueItems, bool ignoreLootProb, bool _forceStacking, bool _ignoreLootAbundance, List<string> _buffsToAdd)
	{
		bool flag = false;
		for (int i = 0; i < itemSet.Count; i++)
		{
			LootContainer.LootEntry lootEntry = itemSet[i];
			if (!player || lootEntry.HasRequirements(player))
			{
				bool flag2 = false;
				if (!lootEntry.forceProb || random.RandomFloat <= LootContainer.getProbability(player, lootEntry, playerLevelPercentage, ignoreLootProb))
				{
					float num = ((lootEntry.group == null) ? abundance : 1f);
					ValueTuple<int, bool> valueTuple = LootContainer.RandomCountFromSandbox(random, lootEntry.group, lootEntry.minCount, lootEntry.maxCount, num);
					int num2 = valueTuple.Item1;
					bool item = valueTuple.Item2;
					if (lootEntry.group != null)
					{
						if (lootEntry.group.minLevel <= playerLevelPercentage && lootEntry.group.maxLevel >= playerLevelPercentage)
						{
							flag2 = LootContainer.SpawnItemsFromGroup(random, lootEntry.group, num2, abundance, spawnedItems, ref slotsLeft, playerLevelPercentage, rareLootChance, lootQualityTemplate, player, containerTags, uniqueItems, ignoreLootProb, _forceStacking, _ignoreLootAbundance, _buffsToAdd, item);
						}
					}
					else
					{
						if (!_ignoreLootAbundance)
						{
							num2 = LootContainer.RandomCountFromSandboxTags(random, lootEntry.item.itemValue, num2);
						}
						flag2 = LootContainer.SpawnItem(random, lootEntry, lootEntry.item.itemValue, num2, spawnedItems, ref slotsLeft, playerLevelPercentage, lootQualityTemplate, player, containerTags, _forceStacking);
					}
				}
				flag = flag || flag2;
				if (flag2 && lootEntry.buffsToAdd != null)
				{
					_buffsToAdd.AddRange(lootEntry.buffsToAdd);
				}
			}
		}
		return flag;
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x0014CAD8 File Offset: 0x0014ACD8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool SpawnItemsFromGroup(GameRandom random, LootContainer.LootGroup group, int numToSpawn, float abundance, List<ItemStack> spawnedItems, ref int slotsLeft, float gameStage, float rareLootChance, string lootQualityTemplate, EntityPlayer player, FastTags<TagGroup.Global> containerTags, bool uniqueItems, bool ignoreLootProb, bool _forceStacking, bool _ignoreLootAbundance, List<string> _buffsToAdd, bool _sandboxModifierUsed)
	{
		bool flag = false;
		int num = 0;
		while (num < numToSpawn && slotsLeft > 0)
		{
			if (_sandboxModifierUsed)
			{
				flag |= LootContainer.SpawnLootItemsFromList(random, group.items, LootContainer.RandomSpawnCount(random, group.minCount, group.maxCount, 1f), abundance, spawnedItems, ref slotsLeft, gameStage, rareLootChance, lootQualityTemplate, player, containerTags, uniqueItems, ignoreLootProb, _forceStacking, _ignoreLootAbundance, _buffsToAdd, _sandboxModifierUsed);
			}
			else
			{
				ValueTuple<int, bool> valueTuple = LootContainer.RandomCountFromSandbox(random, group, group.minCount, group.maxCount, 1f);
				int item = valueTuple.Item1;
				bool item2 = valueTuple.Item2;
				flag |= LootContainer.SpawnLootItemsFromList(random, group.items, item, abundance, spawnedItems, ref slotsLeft, gameStage, rareLootChance, lootQualityTemplate, player, containerTags, uniqueItems, ignoreLootProb, _forceStacking, _ignoreLootAbundance, _buffsToAdd, item2);
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x0014CB98 File Offset: 0x0014AD98
	public static bool SpawnLootItemsFromList(GameRandom random, List<LootContainer.LootEntry> itemSet, int numToSpawn, float abundance, List<ItemStack> spawnedItems, ref int slotsLeft, float lootStage, float rareLootChance, string lootQualityTemplate, EntityPlayer player, FastTags<TagGroup.Global> containerTags, bool uniqueItems, bool ignoreLootProb, bool _forceStacking, bool _ignoreLootAbundance, List<string> _buffsToAdd, bool _sandboxModifierUsed)
	{
		if (numToSpawn < 1)
		{
			return numToSpawn == -1 && LootContainer.SpawnAllItemsFromList(random, itemSet, abundance, spawnedItems, ref slotsLeft, lootStage, rareLootChance, lootQualityTemplate, player, containerTags, uniqueItems, ignoreLootProb, _forceStacking, _ignoreLootAbundance, _buffsToAdd);
		}
		float num = 0f;
		for (int i = 0; i < itemSet.Count; i++)
		{
			LootContainer.LootEntry lootEntry = itemSet[i];
			if (!lootEntry.forceProb)
			{
				num += LootContainer.getProbability(player, lootEntry, lootStage, ignoreLootProb);
			}
		}
		if (num == 0f)
		{
			return false;
		}
		List<int> list = new List<int>();
		bool flag = false;
		for (int j = 0; j < numToSpawn; j++)
		{
			float num2 = 0f;
			float randomFloat = random.RandomFloat;
			for (int k = 0; k < itemSet.Count; k++)
			{
				bool flag2 = false;
				LootContainer.LootEntry lootEntry2 = itemSet[k];
				if (!list.Contains(k) || (!lootEntry2.forceProb && !uniqueItems))
				{
					float probability = LootContainer.getProbability(player, lootEntry2, lootStage, ignoreLootProb);
					bool flag3;
					if (lootEntry2.forceProb)
					{
						flag3 = random.RandomFloat <= probability;
					}
					else
					{
						num2 += probability / num;
						flag3 = randomFloat <= num2 + rareLootChance;
					}
					if (flag3)
					{
						list.Add(k);
						if (uniqueItems)
						{
							num -= LootContainer.getProbability(player, lootEntry2, lootStage, ignoreLootProb);
						}
						bool flag4 = _sandboxModifierUsed;
						float num3 = ((lootEntry2.group == null) ? abundance : 1f);
						int num4;
						if (_sandboxModifierUsed)
						{
							num4 = LootContainer.RandomSpawnCount(random, lootEntry2.minCount, lootEntry2.maxCount, num3);
						}
						else
						{
							ValueTuple<int, bool> valueTuple = LootContainer.RandomCountFromSandbox(random, lootEntry2.group, lootEntry2.minCount, lootEntry2.maxCount, num3);
							num4 = valueTuple.Item1;
							flag4 = valueTuple.Item2;
						}
						num4 += Mathf.RoundToInt((float)num4 * (lootEntry2.lootstageCountMod * lootStage));
						if (lootEntry2.group != null)
						{
							if (lootEntry2.group.minLevel <= lootStage && lootEntry2.group.maxLevel >= lootStage)
							{
								flag2 = LootContainer.SpawnItemsFromGroup(random, lootEntry2.group, num4, abundance, spawnedItems, ref slotsLeft, lootStage, rareLootChance, lootQualityTemplate, player, containerTags, uniqueItems, ignoreLootProb, _forceStacking, _ignoreLootAbundance, _buffsToAdd, flag4);
							}
						}
						else
						{
							if (!_ignoreLootAbundance && !_sandboxModifierUsed)
							{
								num4 = LootContainer.RandomCountFromSandboxTags(random, lootEntry2.item.itemValue, num4);
							}
							flag2 = LootContainer.SpawnItem(random, lootEntry2, lootEntry2.item.itemValue, num4, spawnedItems, ref slotsLeft, lootStage, lootQualityTemplate, player, containerTags, _forceStacking);
						}
						flag = flag || flag2;
						if (flag2 && lootEntry2.buffsToAdd != null)
						{
							_buffsToAdd.AddRange(lootEntry2.buffsToAdd);
							break;
						}
						break;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x0014CE20 File Offset: 0x0014B020
	[PublicizedFrom(EAccessModifier.Private)]
	public static int RandomCountFromSandboxTags(GameRandom rand, ItemValue itemValue, int itemSpawnCount)
	{
		float num = 1f;
		ItemClass itemClass = itemValue.ItemClass;
		if (itemClass.HasAnyTags(LootContainer.foodTags))
		{
			num = LootContainer.FoodCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.drinksTags))
		{
			num = LootContainer.DrinkCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.ammoTags))
		{
			num = LootContainer.AmmoCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.medicalTags))
		{
			num = LootContainer.MedicalCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.junkTags))
		{
			num = LootContainer.ResourceCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.armorTags))
		{
			num = LootContainer.ArmorCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.meleeTags))
		{
			num = LootContainer.MeleeCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.rangedTags))
		{
			num = LootContainer.RangedCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.dukesTags))
		{
			num = LootContainer.DukesCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.magazineTags))
		{
			num = LootContainer.MagazinesCountModifier;
		}
		else if (itemClass.HasAnyTags(LootContainer.booksTags))
		{
			num = LootContainer.BookCountModifier;
		}
		if (num == 0f)
		{
			return 0;
		}
		if (num == 1f)
		{
			return itemSpawnCount;
		}
		return LootContainer.RandomSpawnCount(rand, itemSpawnCount, itemSpawnCount, num);
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x0014CF48 File Offset: 0x0014B148
	[PublicizedFrom(EAccessModifier.Private)]
	public static ValueTuple<int, bool> RandomCountFromSandbox(GameRandom _random, LootContainer.LootGroup _group, int _minCount, int _maxCount, float _abundance)
	{
		float countMultiplierFromSandbox = LootContainer.GetCountMultiplierFromSandbox((_group != null) ? _group.abundanceType : LootContainer.LootGroup.AbundanceLootModTypes.None);
		if (countMultiplierFromSandbox >= 0f)
		{
			_abundance *= countMultiplierFromSandbox;
		}
		return new ValueTuple<int, bool>(LootContainer.RandomSpawnCount(_random, _minCount, _maxCount, _abundance), countMultiplierFromSandbox >= 0f);
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x0014CF90 File Offset: 0x0014B190
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetCountMultiplierFromSandbox(LootContainer.LootGroup.AbundanceLootModTypes abundanceType)
	{
		switch (abundanceType)
		{
		case LootContainer.LootGroup.AbundanceLootModTypes.Food:
			return LootContainer.FoodCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Drinks:
			return LootContainer.DrinkCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Ammo:
			return LootContainer.AmmoCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Meds:
			return LootContainer.MedicalCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Resources:
			return LootContainer.ResourceCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Armor:
			return LootContainer.ArmorCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Melee:
			return LootContainer.MeleeCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Ranged:
			return LootContainer.RangedCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Dukes:
			return LootContainer.DukesCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Magazines:
			return LootContainer.MagazinesCountModifier;
		case LootContainer.LootGroup.AbundanceLootModTypes.Books:
			return LootContainer.BookCountModifier;
		default:
			return -1f;
		}
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x0014D01A File Offset: 0x0014B21A
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetSandboxProb(ItemClass _item)
	{
		if (_item.ItemTags.Test_AnySet(LootContainer.treasureTags))
		{
			return LootContainer.TreasureMapChance;
		}
		return 1f;
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x0014D03C File Offset: 0x0014B23C
	[PublicizedFrom(EAccessModifier.Private)]
	public static float getProbability(EntityPlayer _player, LootContainer.LootEntry _item, float _lootstage, bool _ignoreLootProb)
	{
		if (_player != null && !_item.HasRequirements(_player))
		{
			return 0f;
		}
		if (_item.lootProbTemplate != string.Empty && LootContainer.lootProbTemplates.ContainsKey(_item.lootProbTemplate))
		{
			LootContainer.LootProbabilityTemplate lootProbabilityTemplate = LootContainer.lootProbTemplates[_item.lootProbTemplate];
			int i = 0;
			while (i < lootProbabilityTemplate.templates.Count)
			{
				LootContainer.LootEntry lootEntry = lootProbabilityTemplate.templates[i];
				if (lootEntry.minLevel <= _lootstage && lootEntry.maxLevel >= _lootstage)
				{
					if (_item.item != null && !_item.item.itemValue.ItemClass.ItemTags.IsEmpty)
					{
						float sandboxProb = LootContainer.GetSandboxProb(_item.item.itemValue.ItemClass);
						if (_ignoreLootProb)
						{
							return lootEntry.prob * sandboxProb;
						}
						return EffectManager.GetValue(PassiveEffects.LootProb, null, lootEntry.prob, _player, null, _item.item.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * sandboxProb;
					}
					else
					{
						if (_item.tags.IsEmpty)
						{
							return lootEntry.prob;
						}
						return EffectManager.GetValue(PassiveEffects.LootProb, null, lootEntry.prob, _player, null, _item.tags, true, true, true, true, true, 1, true, false);
					}
				}
				else
				{
					i++;
				}
			}
		}
		if (_item.item != null && !_item.item.itemValue.ItemClass.ItemTags.IsEmpty)
		{
			float sandboxProb2 = LootContainer.GetSandboxProb(_item.item.itemValue.ItemClass);
			if (_ignoreLootProb)
			{
				return _item.prob * sandboxProb2;
			}
			return EffectManager.GetValue(PassiveEffects.LootProb, null, _item.prob, _player, null, _item.item.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * sandboxProb2;
		}
		else
		{
			if (_item.tags.IsEmpty)
			{
				return _item.prob;
			}
			return EffectManager.GetValue(PassiveEffects.LootProb, null, _item.prob, _player, null, _item.tags, true, true, true, true, true, 1, true, false);
		}
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x0014D234 File Offset: 0x0014B434
	public void ExecuteBuffActions(int instigatorId, EntityAlive target)
	{
		if (this.BuffActions != null)
		{
			for (int i = 0; i < this.BuffActions.Count; i++)
			{
				target.Buffs.AddBuff(this.BuffActions[i], -1, true, false, -1f);
			}
		}
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x0014D280 File Offset: 0x0014B480
	public List<ItemStack> Spawn(GameRandom random, int _maxItems, float playerLevelPercentage, float rareLootChance, EntityPlayer player, FastTags<TagGroup.Global> containerTags, bool uniqueItems, bool ignoreLootProb, bool ignoreLootAbundance)
	{
		float num = 1f;
		float num2 = 1f;
		if (!ignoreLootAbundance)
		{
			if (LootContainer.GlobalCountModifier <= 1f)
			{
				num = LootContainer.GlobalCountModifier;
			}
			else
			{
				num2 = LootContainer.GlobalCountModifier;
			}
		}
		int num3 = LootContainer.RandomSpawnCount(random, this.minCount, this.maxCount, num2);
		num3 = Utils.FastMin(num3, _maxItems);
		List<ItemStack> list = new List<ItemStack>();
		List<string> list2 = new List<string>();
		if (LootContainer.SpawnLootItemsFromList(random, this.itemsToSpawn, num3, num, list, ref _maxItems, playerLevelPercentage, rareLootChance, this.lootQualityTemplate, player, containerTags, uniqueItems, ignoreLootProb, false, ignoreLootAbundance, list2, ignoreLootAbundance))
		{
			foreach (string text in list2)
			{
				player.Buffs.AddBuff(text, -1, true, false, -1f);
			}
		}
		return list;
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x0014D360 File Offset: 0x0014B560
	[Conditional("DEBUG_LOOTLOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LootLog(string _name, string _format = "", params object[] _args)
	{
		_format = string.Format("{0} LootContainer {1} {2}", GameManager.frameCount, _name, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x0400286B RID: 10347
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, LootContainer> lootContainers = new CaseInsensitiveStringDictionary<LootContainer>();

	// Token: 0x0400286C RID: 10348
	public static readonly Dictionary<string, LootContainer.LootGroup> lootGroups = new Dictionary<string, LootContainer.LootGroup>();

	// Token: 0x0400286D RID: 10349
	public static readonly Dictionary<string, LootContainer.LootQualityTemplate> lootQualityTemplates = new Dictionary<string, LootContainer.LootQualityTemplate>();

	// Token: 0x0400286E RID: 10350
	public static readonly Dictionary<string, LootContainer.LootProbabilityTemplate> lootProbTemplates = new Dictionary<string, LootContainer.LootProbabilityTemplate>();

	// Token: 0x0400286F RID: 10351
	public string Name;

	// Token: 0x04002870 RID: 10352
	public string soundOpen;

	// Token: 0x04002871 RID: 10353
	public string soundClose;

	// Token: 0x04002872 RID: 10354
	public Vector2i size;

	// Token: 0x04002873 RID: 10355
	public float openTime;

	// Token: 0x04002874 RID: 10356
	public int minCount;

	// Token: 0x04002875 RID: 10357
	public int maxCount;

	// Token: 0x04002876 RID: 10358
	public LootContainer.DestroyOnClose destroyOnClose;

	// Token: 0x04002877 RID: 10359
	public string lootQualityTemplate;

	// Token: 0x04002878 RID: 10360
	public List<string> BuffActions;

	// Token: 0x04002879 RID: 10361
	public string OnOpenEvent = "";

	// Token: 0x0400287A RID: 10362
	public bool ignoreLootAbundance;

	// Token: 0x0400287B RID: 10363
	public bool useUnmodifiedLootstage;

	// Token: 0x0400287C RID: 10364
	public bool UniqueItems;

	// Token: 0x0400287D RID: 10365
	public bool IgnoreLootProb;

	// Token: 0x0400287E RID: 10366
	public readonly List<LootContainer.LootEntry> itemsToSpawn = new List<LootContainer.LootEntry>();

	// Token: 0x0400287F RID: 10367
	public static Dictionary<EntityPlayer, string[]> OverrideItems = new Dictionary<EntityPlayer, string[]>();

	// Token: 0x04002880 RID: 10368
	public static float GlobalCountModifier = 1f;

	// Token: 0x04002881 RID: 10369
	public static float FoodCountModifier = 1f;

	// Token: 0x04002882 RID: 10370
	public static float DrinkCountModifier = 1f;

	// Token: 0x04002883 RID: 10371
	public static float AmmoCountModifier = 1f;

	// Token: 0x04002884 RID: 10372
	public static float MedicalCountModifier = 1f;

	// Token: 0x04002885 RID: 10373
	public static float ResourceCountModifier = 1f;

	// Token: 0x04002886 RID: 10374
	public static float ArmorCountModifier = 1f;

	// Token: 0x04002887 RID: 10375
	public static float MeleeCountModifier = 1f;

	// Token: 0x04002888 RID: 10376
	public static float RangedCountModifier = 1f;

	// Token: 0x04002889 RID: 10377
	public static float DukesCountModifier = 1f;

	// Token: 0x0400288A RID: 10378
	public static float MagazinesCountModifier = 1f;

	// Token: 0x0400288B RID: 10379
	public static float BookCountModifier = 1f;

	// Token: 0x0400288C RID: 10380
	public static float TreasureMapChance = 1f;

	// Token: 0x0400288D RID: 10381
	public static int LootMaxTier = 6;

	// Token: 0x0400288E RID: 10382
	public static float LootTimerModifier = 1f;

	// Token: 0x0400288F RID: 10383
	public static float LootBagChance = 1f;

	// Token: 0x04002890 RID: 10384
	public static bool NoLoot = false;

	// Token: 0x04002891 RID: 10385
	public static FastTags<TagGroup.Global> foodTags = FastTags<TagGroup.Global>.Parse("food");

	// Token: 0x04002892 RID: 10386
	public static FastTags<TagGroup.Global> drinksTags = FastTags<TagGroup.Global>.Parse("drinks");

	// Token: 0x04002893 RID: 10387
	public static FastTags<TagGroup.Global> ammoTags = FastTags<TagGroup.Global>.Parse("ammo");

	// Token: 0x04002894 RID: 10388
	public static FastTags<TagGroup.Global> medicalTags = FastTags<TagGroup.Global>.Parse("medical");

	// Token: 0x04002895 RID: 10389
	public static FastTags<TagGroup.Global> junkTags = FastTags<TagGroup.Global>.Parse("junk");

	// Token: 0x04002896 RID: 10390
	public static FastTags<TagGroup.Global> armorTags = FastTags<TagGroup.Global>.Parse("armor,armorParts");

	// Token: 0x04002897 RID: 10391
	public static FastTags<TagGroup.Global> meleeTags = FastTags<TagGroup.Global>.Parse("melee,meleeParts");

	// Token: 0x04002898 RID: 10392
	public static FastTags<TagGroup.Global> rangedTags = FastTags<TagGroup.Global>.Parse("ranged,rangedParts");

	// Token: 0x04002899 RID: 10393
	public static FastTags<TagGroup.Global> dukesTags = FastTags<TagGroup.Global>.Parse("dukes");

	// Token: 0x0400289A RID: 10394
	public static FastTags<TagGroup.Global> magazineTags = FastTags<TagGroup.Global>.Parse("csm");

	// Token: 0x0400289B RID: 10395
	public static FastTags<TagGroup.Global> booksTags = FastTags<TagGroup.Global>.Parse("books");

	// Token: 0x0400289C RID: 10396
	public static FastTags<TagGroup.Global> treasureTags = FastTags<TagGroup.Global>.Parse("treasureMap");

	// Token: 0x02000623 RID: 1571
	public enum DestroyOnClose
	{
		// Token: 0x0400289E RID: 10398
		False,
		// Token: 0x0400289F RID: 10399
		True,
		// Token: 0x040028A0 RID: 10400
		Empty
	}

	// Token: 0x02000624 RID: 1572
	public class LootItem
	{
		// Token: 0x040028A1 RID: 10401
		public ItemValue itemValue;
	}

	// Token: 0x02000625 RID: 1573
	public class LootGroup
	{
		// Token: 0x040028A2 RID: 10402
		public string name;

		// Token: 0x040028A3 RID: 10403
		public string lootQualityTemplate;

		// Token: 0x040028A4 RID: 10404
		public int minCount;

		// Token: 0x040028A5 RID: 10405
		public int maxCount;

		// Token: 0x040028A6 RID: 10406
		public int minQuality = -1;

		// Token: 0x040028A7 RID: 10407
		public int maxQuality = -1;

		// Token: 0x040028A8 RID: 10408
		public float minLevel;

		// Token: 0x040028A9 RID: 10409
		public float maxLevel;

		// Token: 0x040028AA RID: 10410
		public string[] modsToInstall;

		// Token: 0x040028AB RID: 10411
		public float modChance = 1f;

		// Token: 0x040028AC RID: 10412
		public readonly List<LootContainer.LootEntry> items = new List<LootContainer.LootEntry>();

		// Token: 0x040028AD RID: 10413
		public LootContainer.LootGroup.AbundanceLootModTypes abundanceType;

		// Token: 0x02000626 RID: 1574
		public enum AbundanceLootModTypes
		{
			// Token: 0x040028AF RID: 10415
			None,
			// Token: 0x040028B0 RID: 10416
			Food,
			// Token: 0x040028B1 RID: 10417
			Drinks,
			// Token: 0x040028B2 RID: 10418
			Ammo,
			// Token: 0x040028B3 RID: 10419
			Meds,
			// Token: 0x040028B4 RID: 10420
			Resources,
			// Token: 0x040028B5 RID: 10421
			Armor,
			// Token: 0x040028B6 RID: 10422
			Melee,
			// Token: 0x040028B7 RID: 10423
			Ranged,
			// Token: 0x040028B8 RID: 10424
			Dukes,
			// Token: 0x040028B9 RID: 10425
			Magazines,
			// Token: 0x040028BA RID: 10426
			Books
		}
	}

	// Token: 0x02000627 RID: 1575
	public class LootEntry
	{
		// Token: 0x06003351 RID: 13137 RVA: 0x0014D564 File Offset: 0x0014B764
		public bool HasRequirements(EntityPlayer player)
		{
			if (this.Requirements != null)
			{
				for (int i = 0; i < this.Requirements.Count; i++)
				{
					if (!this.Requirements[i].CheckRequirement(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040028BB RID: 10427
		public string lootProbTemplate;

		// Token: 0x040028BC RID: 10428
		public int minCount;

		// Token: 0x040028BD RID: 10429
		public int maxCount;

		// Token: 0x040028BE RID: 10430
		public int minQuality;

		// Token: 0x040028BF RID: 10431
		public int maxQuality;

		// Token: 0x040028C0 RID: 10432
		public float minLevel;

		// Token: 0x040028C1 RID: 10433
		public float maxLevel;

		// Token: 0x040028C2 RID: 10434
		public float prob;

		// Token: 0x040028C3 RID: 10435
		public bool forceProb;

		// Token: 0x040028C4 RID: 10436
		public string[] modsToInstall;

		// Token: 0x040028C5 RID: 10437
		public float modChance = 1f;

		// Token: 0x040028C6 RID: 10438
		public float lootstageCountMod;

		// Token: 0x040028C7 RID: 10439
		public LootContainer.LootItem item;

		// Token: 0x040028C8 RID: 10440
		public LootContainer.LootGroup group;

		// Token: 0x040028C9 RID: 10441
		public LootContainer.LootGroup parentGroup;

		// Token: 0x040028CA RID: 10442
		public FastTags<TagGroup.Global> tags;

		// Token: 0x040028CB RID: 10443
		public string[] buffsToAdd;

		// Token: 0x040028CC RID: 10444
		public bool randomDurability = true;

		// Token: 0x040028CD RID: 10445
		public List<BaseLootEntryRequirement> Requirements;
	}

	// Token: 0x02000628 RID: 1576
	public class LootProbabilityTemplate
	{
		// Token: 0x040028CE RID: 10446
		public string name;

		// Token: 0x040028CF RID: 10447
		public readonly List<LootContainer.LootEntry> templates = new List<LootContainer.LootEntry>();
	}

	// Token: 0x02000629 RID: 1577
	public class LootQualityTemplate
	{
		// Token: 0x040028D0 RID: 10448
		public string name;

		// Token: 0x040028D1 RID: 10449
		public readonly List<LootContainer.LootGroup> templates = new List<LootContainer.LootGroup>();
	}
}
