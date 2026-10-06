using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200061B RID: 1563
public class LootContainer
{
	// Token: 0x0600332C RID: 13100 RVA: 0x0014B847 File Offset: 0x00149A47
	public static void InitStatic()
	{
		LootContainer.Cleanup();
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x0014B84E File Offset: 0x00149A4E
	public void Init()
	{
		LootContainer.lootContainers[this.Name] = this;
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x0014B861 File Offset: 0x00149A61
	public static void Cleanup()
	{
		LootContainer.lootContainers.Clear();
		LootContainer.lootGroups.Clear();
		LootContainer.lootQualityTemplates.Clear();
		LootContainer.lootProbTemplates.Clear();
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x0014B88B File Offset: 0x00149A8B
	public static bool IsLoaded()
	{
		return LootContainer.lootContainers.Count > 0;
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x0014B89C File Offset: 0x00149A9C
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

	// Token: 0x06003331 RID: 13105 RVA: 0x0014B8E0 File Offset: 0x00149AE0
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

	// Token: 0x06003332 RID: 13106 RVA: 0x0014B968 File Offset: 0x00149B68
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

	// Token: 0x06003333 RID: 13107 RVA: 0x0014BD44 File Offset: 0x00149F44
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

	// Token: 0x06003334 RID: 13108 RVA: 0x0014BD98 File Offset: 0x00149F98
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

	// Token: 0x06003335 RID: 13109 RVA: 0x0014BEE4 File Offset: 0x0014A0E4
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

	// Token: 0x06003336 RID: 13110 RVA: 0x0014BFA4 File Offset: 0x0014A1A4
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

	// Token: 0x06003337 RID: 13111 RVA: 0x0014C22C File Offset: 0x0014A42C
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

	// Token: 0x06003338 RID: 13112 RVA: 0x0014C354 File Offset: 0x0014A554
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

	// Token: 0x06003339 RID: 13113 RVA: 0x0014C39C File Offset: 0x0014A59C
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

	// Token: 0x0600333A RID: 13114 RVA: 0x0014C426 File Offset: 0x0014A626
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetSandboxProb(ItemClass _item)
	{
		if (_item.ItemTags.Test_AnySet(LootContainer.treasureTags))
		{
			return LootContainer.TreasureMapChance;
		}
		return 1f;
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x0014C448 File Offset: 0x0014A648
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

	// Token: 0x0600333C RID: 13116 RVA: 0x0014C640 File Offset: 0x0014A840
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

	// Token: 0x0600333D RID: 13117 RVA: 0x0014C68C File Offset: 0x0014A88C
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

	// Token: 0x0600333E RID: 13118 RVA: 0x0014C76C File Offset: 0x0014A96C
	[Conditional("DEBUG_LOOTLOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LootLog(string _name, string _format = "", params object[] _args)
	{
		_format = string.Format("{0} LootContainer {1} {2}", GameManager.frameCount, _name, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x04002850 RID: 10320
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, LootContainer> lootContainers = new CaseInsensitiveStringDictionary<LootContainer>();

	// Token: 0x04002851 RID: 10321
	public static readonly Dictionary<string, LootContainer.LootGroup> lootGroups = new Dictionary<string, LootContainer.LootGroup>();

	// Token: 0x04002852 RID: 10322
	public static readonly Dictionary<string, LootContainer.LootQualityTemplate> lootQualityTemplates = new Dictionary<string, LootContainer.LootQualityTemplate>();

	// Token: 0x04002853 RID: 10323
	public static readonly Dictionary<string, LootContainer.LootProbabilityTemplate> lootProbTemplates = new Dictionary<string, LootContainer.LootProbabilityTemplate>();

	// Token: 0x04002854 RID: 10324
	public string Name;

	// Token: 0x04002855 RID: 10325
	public string soundOpen;

	// Token: 0x04002856 RID: 10326
	public string soundClose;

	// Token: 0x04002857 RID: 10327
	public Vector2i size;

	// Token: 0x04002858 RID: 10328
	public float openTime;

	// Token: 0x04002859 RID: 10329
	public int minCount;

	// Token: 0x0400285A RID: 10330
	public int maxCount;

	// Token: 0x0400285B RID: 10331
	public LootContainer.DestroyOnClose destroyOnClose;

	// Token: 0x0400285C RID: 10332
	public string lootQualityTemplate;

	// Token: 0x0400285D RID: 10333
	public List<string> BuffActions;

	// Token: 0x0400285E RID: 10334
	public string OnOpenEvent = "";

	// Token: 0x0400285F RID: 10335
	public bool ignoreLootAbundance;

	// Token: 0x04002860 RID: 10336
	public bool useUnmodifiedLootstage;

	// Token: 0x04002861 RID: 10337
	public bool UniqueItems;

	// Token: 0x04002862 RID: 10338
	public bool IgnoreLootProb;

	// Token: 0x04002863 RID: 10339
	public readonly List<LootContainer.LootEntry> itemsToSpawn = new List<LootContainer.LootEntry>();

	// Token: 0x04002864 RID: 10340
	public static Dictionary<EntityPlayer, string[]> OverrideItems = new Dictionary<EntityPlayer, string[]>();

	// Token: 0x04002865 RID: 10341
	public static float GlobalCountModifier = 1f;

	// Token: 0x04002866 RID: 10342
	public static float FoodCountModifier = 1f;

	// Token: 0x04002867 RID: 10343
	public static float DrinkCountModifier = 1f;

	// Token: 0x04002868 RID: 10344
	public static float AmmoCountModifier = 1f;

	// Token: 0x04002869 RID: 10345
	public static float MedicalCountModifier = 1f;

	// Token: 0x0400286A RID: 10346
	public static float ResourceCountModifier = 1f;

	// Token: 0x0400286B RID: 10347
	public static float ArmorCountModifier = 1f;

	// Token: 0x0400286C RID: 10348
	public static float MeleeCountModifier = 1f;

	// Token: 0x0400286D RID: 10349
	public static float RangedCountModifier = 1f;

	// Token: 0x0400286E RID: 10350
	public static float DukesCountModifier = 1f;

	// Token: 0x0400286F RID: 10351
	public static float MagazinesCountModifier = 1f;

	// Token: 0x04002870 RID: 10352
	public static float BookCountModifier = 1f;

	// Token: 0x04002871 RID: 10353
	public static float TreasureMapChance = 1f;

	// Token: 0x04002872 RID: 10354
	public static int LootMaxTier = 6;

	// Token: 0x04002873 RID: 10355
	public static float LootTimerModifier = 1f;

	// Token: 0x04002874 RID: 10356
	public static float LootBagChance = 1f;

	// Token: 0x04002875 RID: 10357
	public static bool NoLoot = false;

	// Token: 0x04002876 RID: 10358
	public static FastTags<TagGroup.Global> foodTags = FastTags<TagGroup.Global>.Parse("food");

	// Token: 0x04002877 RID: 10359
	public static FastTags<TagGroup.Global> drinksTags = FastTags<TagGroup.Global>.Parse("drinks");

	// Token: 0x04002878 RID: 10360
	public static FastTags<TagGroup.Global> ammoTags = FastTags<TagGroup.Global>.Parse("ammo");

	// Token: 0x04002879 RID: 10361
	public static FastTags<TagGroup.Global> medicalTags = FastTags<TagGroup.Global>.Parse("medical");

	// Token: 0x0400287A RID: 10362
	public static FastTags<TagGroup.Global> junkTags = FastTags<TagGroup.Global>.Parse("junk");

	// Token: 0x0400287B RID: 10363
	public static FastTags<TagGroup.Global> armorTags = FastTags<TagGroup.Global>.Parse("armor,armorParts");

	// Token: 0x0400287C RID: 10364
	public static FastTags<TagGroup.Global> meleeTags = FastTags<TagGroup.Global>.Parse("melee,meleeParts");

	// Token: 0x0400287D RID: 10365
	public static FastTags<TagGroup.Global> rangedTags = FastTags<TagGroup.Global>.Parse("ranged,rangedParts");

	// Token: 0x0400287E RID: 10366
	public static FastTags<TagGroup.Global> dukesTags = FastTags<TagGroup.Global>.Parse("dukes");

	// Token: 0x0400287F RID: 10367
	public static FastTags<TagGroup.Global> magazineTags = FastTags<TagGroup.Global>.Parse("csm");

	// Token: 0x04002880 RID: 10368
	public static FastTags<TagGroup.Global> booksTags = FastTags<TagGroup.Global>.Parse("books");

	// Token: 0x04002881 RID: 10369
	public static FastTags<TagGroup.Global> treasureTags = FastTags<TagGroup.Global>.Parse("treasureMap");

	// Token: 0x0200061C RID: 1564
	public enum DestroyOnClose
	{
		// Token: 0x04002883 RID: 10371
		False,
		// Token: 0x04002884 RID: 10372
		True,
		// Token: 0x04002885 RID: 10373
		Empty
	}

	// Token: 0x0200061D RID: 1565
	public class LootItem
	{
		// Token: 0x04002886 RID: 10374
		public ItemValue itemValue;
	}

	// Token: 0x0200061E RID: 1566
	public class LootGroup
	{
		// Token: 0x04002887 RID: 10375
		public string name;

		// Token: 0x04002888 RID: 10376
		public string lootQualityTemplate;

		// Token: 0x04002889 RID: 10377
		public int minCount;

		// Token: 0x0400288A RID: 10378
		public int maxCount;

		// Token: 0x0400288B RID: 10379
		public int minQuality = -1;

		// Token: 0x0400288C RID: 10380
		public int maxQuality = -1;

		// Token: 0x0400288D RID: 10381
		public float minLevel;

		// Token: 0x0400288E RID: 10382
		public float maxLevel;

		// Token: 0x0400288F RID: 10383
		public string[] modsToInstall;

		// Token: 0x04002890 RID: 10384
		public float modChance = 1f;

		// Token: 0x04002891 RID: 10385
		public readonly List<LootContainer.LootEntry> items = new List<LootContainer.LootEntry>();

		// Token: 0x04002892 RID: 10386
		public LootContainer.LootGroup.AbundanceLootModTypes abundanceType;

		// Token: 0x0200061F RID: 1567
		public enum AbundanceLootModTypes
		{
			// Token: 0x04002894 RID: 10388
			None,
			// Token: 0x04002895 RID: 10389
			Food,
			// Token: 0x04002896 RID: 10390
			Drinks,
			// Token: 0x04002897 RID: 10391
			Ammo,
			// Token: 0x04002898 RID: 10392
			Meds,
			// Token: 0x04002899 RID: 10393
			Resources,
			// Token: 0x0400289A RID: 10394
			Armor,
			// Token: 0x0400289B RID: 10395
			Melee,
			// Token: 0x0400289C RID: 10396
			Ranged,
			// Token: 0x0400289D RID: 10397
			Dukes,
			// Token: 0x0400289E RID: 10398
			Magazines,
			// Token: 0x0400289F RID: 10399
			Books
		}
	}

	// Token: 0x02000620 RID: 1568
	public class LootEntry
	{
		// Token: 0x06003343 RID: 13123 RVA: 0x0014C970 File Offset: 0x0014AB70
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

		// Token: 0x040028A0 RID: 10400
		public string lootProbTemplate;

		// Token: 0x040028A1 RID: 10401
		public int minCount;

		// Token: 0x040028A2 RID: 10402
		public int maxCount;

		// Token: 0x040028A3 RID: 10403
		public int minQuality;

		// Token: 0x040028A4 RID: 10404
		public int maxQuality;

		// Token: 0x040028A5 RID: 10405
		public float minLevel;

		// Token: 0x040028A6 RID: 10406
		public float maxLevel;

		// Token: 0x040028A7 RID: 10407
		public float prob;

		// Token: 0x040028A8 RID: 10408
		public bool forceProb;

		// Token: 0x040028A9 RID: 10409
		public string[] modsToInstall;

		// Token: 0x040028AA RID: 10410
		public float modChance = 1f;

		// Token: 0x040028AB RID: 10411
		public float lootstageCountMod;

		// Token: 0x040028AC RID: 10412
		public LootContainer.LootItem item;

		// Token: 0x040028AD RID: 10413
		public LootContainer.LootGroup group;

		// Token: 0x040028AE RID: 10414
		public LootContainer.LootGroup parentGroup;

		// Token: 0x040028AF RID: 10415
		public FastTags<TagGroup.Global> tags;

		// Token: 0x040028B0 RID: 10416
		public string[] buffsToAdd;

		// Token: 0x040028B1 RID: 10417
		public bool randomDurability = true;

		// Token: 0x040028B2 RID: 10418
		public List<BaseLootEntryRequirement> Requirements;
	}

	// Token: 0x02000621 RID: 1569
	public class LootProbabilityTemplate
	{
		// Token: 0x040028B3 RID: 10419
		public string name;

		// Token: 0x040028B4 RID: 10420
		public readonly List<LootContainer.LootEntry> templates = new List<LootContainer.LootEntry>();
	}

	// Token: 0x02000622 RID: 1570
	public class LootQualityTemplate
	{
		// Token: 0x040028B5 RID: 10421
		public string name;

		// Token: 0x040028B6 RID: 10422
		public readonly List<LootContainer.LootGroup> templates = new List<LootContainer.LootGroup>();
	}
}
