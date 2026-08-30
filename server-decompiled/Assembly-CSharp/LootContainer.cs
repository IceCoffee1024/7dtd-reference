using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class LootContainer
{
	// Token: 0x060032E9 RID: 13033 RVA: 0x0014AEA3 File Offset: 0x001490A3
	public static void InitStatic()
	{
		LootContainer.Cleanup();
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x0014AEAA File Offset: 0x001490AA
	public void Init()
	{
		LootContainer.lootContainers[this.Name] = this;
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x0014AEBD File Offset: 0x001490BD
	public static void Cleanup()
	{
		LootContainer.lootContainers.Clear();
		LootContainer.lootGroups.Clear();
		LootContainer.lootQualityTemplates.Clear();
		LootContainer.lootProbTemplates.Clear();
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x0014AEE7 File Offset: 0x001490E7
	public static bool IsLoaded()
	{
		return LootContainer.lootContainers.Count > 0;
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x0014AEF8 File Offset: 0x001490F8
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

	// Token: 0x060032EE RID: 13038 RVA: 0x0014AF3C File Offset: 0x0014913C
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

	// Token: 0x060032EF RID: 13039 RVA: 0x0014AFC4 File Offset: 0x001491C4
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

	// Token: 0x060032F0 RID: 13040 RVA: 0x0014B3A0 File Offset: 0x001495A0
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

	// Token: 0x060032F1 RID: 13041 RVA: 0x0014B3F4 File Offset: 0x001495F4
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

	// Token: 0x060032F2 RID: 13042 RVA: 0x0014B540 File Offset: 0x00149740
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

	// Token: 0x060032F3 RID: 13043 RVA: 0x0014B600 File Offset: 0x00149800
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

	// Token: 0x060032F4 RID: 13044 RVA: 0x0014B888 File Offset: 0x00149A88
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

	// Token: 0x060032F5 RID: 13045 RVA: 0x0014B9B0 File Offset: 0x00149BB0
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

	// Token: 0x060032F6 RID: 13046 RVA: 0x0014B9F8 File Offset: 0x00149BF8
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

	// Token: 0x060032F7 RID: 13047 RVA: 0x0014BA82 File Offset: 0x00149C82
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetSandboxProb(ItemClass _item)
	{
		if (_item.ItemTags.Test_AnySet(LootContainer.treasureTags))
		{
			return LootContainer.TreasureMapChance;
		}
		return 1f;
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x0014BAA4 File Offset: 0x00149CA4
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

	// Token: 0x060032F9 RID: 13049 RVA: 0x0014BC9C File Offset: 0x00149E9C
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

	// Token: 0x060032FA RID: 13050 RVA: 0x0014BCE8 File Offset: 0x00149EE8
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

	// Token: 0x060032FB RID: 13051 RVA: 0x0014BDC8 File Offset: 0x00149FC8
	[Conditional("DEBUG_LOOTLOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LootLog(string _name, string _format = "", params object[] _args)
	{
		_format = string.Format("{0} LootContainer {1} {2}", GameManager.frameCount, _name, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x04002857 RID: 10327
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, LootContainer> lootContainers = new CaseInsensitiveStringDictionary<LootContainer>();

	// Token: 0x04002858 RID: 10328
	public static readonly Dictionary<string, LootContainer.LootGroup> lootGroups = new Dictionary<string, LootContainer.LootGroup>();

	// Token: 0x04002859 RID: 10329
	public static readonly Dictionary<string, LootContainer.LootQualityTemplate> lootQualityTemplates = new Dictionary<string, LootContainer.LootQualityTemplate>();

	// Token: 0x0400285A RID: 10330
	public static readonly Dictionary<string, LootContainer.LootProbabilityTemplate> lootProbTemplates = new Dictionary<string, LootContainer.LootProbabilityTemplate>();

	// Token: 0x0400285B RID: 10331
	public string Name;

	// Token: 0x0400285C RID: 10332
	public string soundOpen;

	// Token: 0x0400285D RID: 10333
	public string soundClose;

	// Token: 0x0400285E RID: 10334
	public Vector2i size;

	// Token: 0x0400285F RID: 10335
	public float openTime;

	// Token: 0x04002860 RID: 10336
	public int minCount;

	// Token: 0x04002861 RID: 10337
	public int maxCount;

	// Token: 0x04002862 RID: 10338
	public LootContainer.DestroyOnClose destroyOnClose;

	// Token: 0x04002863 RID: 10339
	public string lootQualityTemplate;

	// Token: 0x04002864 RID: 10340
	public List<string> BuffActions;

	// Token: 0x04002865 RID: 10341
	public string OnOpenEvent = "";

	// Token: 0x04002866 RID: 10342
	public bool ignoreLootAbundance;

	// Token: 0x04002867 RID: 10343
	public bool useUnmodifiedLootstage;

	// Token: 0x04002868 RID: 10344
	public bool UniqueItems;

	// Token: 0x04002869 RID: 10345
	public bool IgnoreLootProb;

	// Token: 0x0400286A RID: 10346
	public readonly List<LootContainer.LootEntry> itemsToSpawn = new List<LootContainer.LootEntry>();

	// Token: 0x0400286B RID: 10347
	public static Dictionary<EntityPlayer, string[]> OverrideItems = new Dictionary<EntityPlayer, string[]>();

	// Token: 0x0400286C RID: 10348
	public static float GlobalCountModifier = 1f;

	// Token: 0x0400286D RID: 10349
	public static float FoodCountModifier = 1f;

	// Token: 0x0400286E RID: 10350
	public static float DrinkCountModifier = 1f;

	// Token: 0x0400286F RID: 10351
	public static float AmmoCountModifier = 1f;

	// Token: 0x04002870 RID: 10352
	public static float MedicalCountModifier = 1f;

	// Token: 0x04002871 RID: 10353
	public static float ResourceCountModifier = 1f;

	// Token: 0x04002872 RID: 10354
	public static float ArmorCountModifier = 1f;

	// Token: 0x04002873 RID: 10355
	public static float MeleeCountModifier = 1f;

	// Token: 0x04002874 RID: 10356
	public static float RangedCountModifier = 1f;

	// Token: 0x04002875 RID: 10357
	public static float DukesCountModifier = 1f;

	// Token: 0x04002876 RID: 10358
	public static float MagazinesCountModifier = 1f;

	// Token: 0x04002877 RID: 10359
	public static float BookCountModifier = 1f;

	// Token: 0x04002878 RID: 10360
	public static float TreasureMapChance = 1f;

	// Token: 0x04002879 RID: 10361
	public static int LootMaxTier = 6;

	// Token: 0x0400287A RID: 10362
	public static float LootTimerModifier = 1f;

	// Token: 0x0400287B RID: 10363
	public static float LootBagChance = 1f;

	// Token: 0x0400287C RID: 10364
	public static bool NoLoot = false;

	// Token: 0x0400287D RID: 10365
	public static FastTags<TagGroup.Global> foodTags = FastTags<TagGroup.Global>.Parse("food");

	// Token: 0x0400287E RID: 10366
	public static FastTags<TagGroup.Global> drinksTags = FastTags<TagGroup.Global>.Parse("drinks");

	// Token: 0x0400287F RID: 10367
	public static FastTags<TagGroup.Global> ammoTags = FastTags<TagGroup.Global>.Parse("ammo");

	// Token: 0x04002880 RID: 10368
	public static FastTags<TagGroup.Global> medicalTags = FastTags<TagGroup.Global>.Parse("medical");

	// Token: 0x04002881 RID: 10369
	public static FastTags<TagGroup.Global> junkTags = FastTags<TagGroup.Global>.Parse("junk");

	// Token: 0x04002882 RID: 10370
	public static FastTags<TagGroup.Global> armorTags = FastTags<TagGroup.Global>.Parse("armor,armorParts");

	// Token: 0x04002883 RID: 10371
	public static FastTags<TagGroup.Global> meleeTags = FastTags<TagGroup.Global>.Parse("melee,meleeParts");

	// Token: 0x04002884 RID: 10372
	public static FastTags<TagGroup.Global> rangedTags = FastTags<TagGroup.Global>.Parse("ranged,rangedParts");

	// Token: 0x04002885 RID: 10373
	public static FastTags<TagGroup.Global> dukesTags = FastTags<TagGroup.Global>.Parse("dukes");

	// Token: 0x04002886 RID: 10374
	public static FastTags<TagGroup.Global> magazineTags = FastTags<TagGroup.Global>.Parse("csm");

	// Token: 0x04002887 RID: 10375
	public static FastTags<TagGroup.Global> booksTags = FastTags<TagGroup.Global>.Parse("books");

	// Token: 0x04002888 RID: 10376
	public static FastTags<TagGroup.Global> treasureTags = FastTags<TagGroup.Global>.Parse("treasureMap");

	// Token: 0x0200061A RID: 1562
	public enum DestroyOnClose
	{
		// Token: 0x0400288A RID: 10378
		False,
		// Token: 0x0400288B RID: 10379
		True,
		// Token: 0x0400288C RID: 10380
		Empty
	}

	// Token: 0x0200061B RID: 1563
	public class LootItem
	{
		// Token: 0x0400288D RID: 10381
		public ItemValue itemValue;
	}

	// Token: 0x0200061C RID: 1564
	public class LootGroup
	{
		// Token: 0x0400288E RID: 10382
		public string name;

		// Token: 0x0400288F RID: 10383
		public string lootQualityTemplate;

		// Token: 0x04002890 RID: 10384
		public int minCount;

		// Token: 0x04002891 RID: 10385
		public int maxCount;

		// Token: 0x04002892 RID: 10386
		public int minQuality = -1;

		// Token: 0x04002893 RID: 10387
		public int maxQuality = -1;

		// Token: 0x04002894 RID: 10388
		public float minLevel;

		// Token: 0x04002895 RID: 10389
		public float maxLevel;

		// Token: 0x04002896 RID: 10390
		public string[] modsToInstall;

		// Token: 0x04002897 RID: 10391
		public float modChance = 1f;

		// Token: 0x04002898 RID: 10392
		public readonly List<LootContainer.LootEntry> items = new List<LootContainer.LootEntry>();

		// Token: 0x04002899 RID: 10393
		public LootContainer.LootGroup.AbundanceLootModTypes abundanceType;

		// Token: 0x0200061D RID: 1565
		public enum AbundanceLootModTypes
		{
			// Token: 0x0400289B RID: 10395
			None,
			// Token: 0x0400289C RID: 10396
			Food,
			// Token: 0x0400289D RID: 10397
			Drinks,
			// Token: 0x0400289E RID: 10398
			Ammo,
			// Token: 0x0400289F RID: 10399
			Meds,
			// Token: 0x040028A0 RID: 10400
			Resources,
			// Token: 0x040028A1 RID: 10401
			Armor,
			// Token: 0x040028A2 RID: 10402
			Melee,
			// Token: 0x040028A3 RID: 10403
			Ranged,
			// Token: 0x040028A4 RID: 10404
			Dukes,
			// Token: 0x040028A5 RID: 10405
			Magazines,
			// Token: 0x040028A6 RID: 10406
			Books
		}
	}

	// Token: 0x0200061E RID: 1566
	public class LootEntry
	{
		// Token: 0x06003300 RID: 13056 RVA: 0x0014BFCC File Offset: 0x0014A1CC
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

		// Token: 0x040028A7 RID: 10407
		public string lootProbTemplate;

		// Token: 0x040028A8 RID: 10408
		public int minCount;

		// Token: 0x040028A9 RID: 10409
		public int maxCount;

		// Token: 0x040028AA RID: 10410
		public int minQuality;

		// Token: 0x040028AB RID: 10411
		public int maxQuality;

		// Token: 0x040028AC RID: 10412
		public float minLevel;

		// Token: 0x040028AD RID: 10413
		public float maxLevel;

		// Token: 0x040028AE RID: 10414
		public float prob;

		// Token: 0x040028AF RID: 10415
		public bool forceProb;

		// Token: 0x040028B0 RID: 10416
		public string[] modsToInstall;

		// Token: 0x040028B1 RID: 10417
		public float modChance = 1f;

		// Token: 0x040028B2 RID: 10418
		public float lootstageCountMod;

		// Token: 0x040028B3 RID: 10419
		public LootContainer.LootItem item;

		// Token: 0x040028B4 RID: 10420
		public LootContainer.LootGroup group;

		// Token: 0x040028B5 RID: 10421
		public LootContainer.LootGroup parentGroup;

		// Token: 0x040028B6 RID: 10422
		public FastTags<TagGroup.Global> tags;

		// Token: 0x040028B7 RID: 10423
		public string[] buffsToAdd;

		// Token: 0x040028B8 RID: 10424
		public bool randomDurability = true;

		// Token: 0x040028B9 RID: 10425
		public List<BaseLootEntryRequirement> Requirements;
	}

	// Token: 0x0200061F RID: 1567
	public class LootProbabilityTemplate
	{
		// Token: 0x040028BA RID: 10426
		public string name;

		// Token: 0x040028BB RID: 10427
		public readonly List<LootContainer.LootEntry> templates = new List<LootContainer.LootEntry>();
	}

	// Token: 0x02000620 RID: 1568
	public class LootQualityTemplate
	{
		// Token: 0x040028BC RID: 10428
		public string name;

		// Token: 0x040028BD RID: 10429
		public readonly List<LootContainer.LootGroup> templates = new List<LootContainer.LootGroup>();
	}
}
