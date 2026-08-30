using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000610 RID: 1552
public class LootContainer
{
	// Token: 0x06003296 RID: 12950 RVA: 0x00148D7F File Offset: 0x00146F7F
	public static void InitStatic()
	{
		LootContainer.Cleanup();
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x00148D86 File Offset: 0x00146F86
	public void Init()
	{
		LootContainer.lootContainers[this.Name] = this;
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x00148D99 File Offset: 0x00146F99
	public static void Cleanup()
	{
		LootContainer.lootContainers.Clear();
		LootContainer.lootGroups.Clear();
		LootContainer.lootQualityTemplates.Clear();
		LootContainer.lootProbTemplates.Clear();
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x00148DC3 File Offset: 0x00146FC3
	public static bool IsLoaded()
	{
		return LootContainer.lootContainers.Count > 0;
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x00148DD4 File Offset: 0x00146FD4
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

	// Token: 0x0600329B RID: 12955 RVA: 0x00148E18 File Offset: 0x00147018
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

	// Token: 0x0600329C RID: 12956 RVA: 0x00148EA0 File Offset: 0x001470A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool SpawnItem(GameRandom random, LootContainer.LootEntry template, ItemValue lootItemValue, int countToSpawn, List<ItemStack> spawnedItems, ref int slotsLeft, float gameStage, string lootQualityTemplate, EntityPlayer player, FastTags<TagGroup.Global> containerTags, bool _forceStacking)
	{
		if (lootItemValue.ItemClass == null)
		{
			return false;
		}
		if (player)
		{
			countToSpawn = Math.Min((int)EffectManager.GetValue(PassiveEffects.LootQuantity, player.inventory.holdingItemItemValue, (float)countToSpawn, player, null, lootItemValue.ItemClass.ItemTags | containerTags, true, true, true, true, true, 1, true, false), lootItemValue.ItemClass.Stacknumber.Value);
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
			int value = lootItemValue.ItemClass.Stacknumber.Value;
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
					int num = value - itemStack.count;
					itemStack.count = value;
					countToSpawn -= num;
				}
			}
		}
		if (slotsLeft < 1)
		{
			return false;
		}
		int num2 = template.minQuality;
		int num3 = template.maxQuality;
		string text = lootQualityTemplate;
		if (!string.IsNullOrEmpty(text))
		{
			LootContainer.LootGroup parentGroup = template.parentGroup;
			if (((parentGroup != null) ? parentGroup.lootQualityTemplate : null) == null)
			{
				goto IL_0189;
			}
		}
		LootContainer.LootGroup parentGroup2 = template.parentGroup;
		text = ((parentGroup2 != null) ? parentGroup2.lootQualityTemplate : null);
		IL_0189:
		if (!string.IsNullOrEmpty(text))
		{
			bool flag = false;
			for (int j = 0; j < LootContainer.lootQualityTemplates[text].templates.Count; j++)
			{
				float randomFloat = random.RandomFloat;
				LootContainer.LootGroup lootGroup = LootContainer.lootQualityTemplates[text].templates[j];
				num2 = lootGroup.minQuality;
				num3 = lootGroup.maxQuality;
				if (lootGroup.minLevel <= gameStage && lootGroup.maxLevel >= gameStage)
				{
					for (int k = 0; k < lootGroup.items.Count; k++)
					{
						LootContainer.LootEntry lootEntry = lootGroup.items[k];
						if (random.RandomFloat <= lootEntry.prob)
						{
							num2 = lootEntry.minQuality;
							num3 = lootEntry.maxQuality;
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
		float num4 = template.modChance;
		if (template.parentGroup != null && template.parentGroup.modsToInstall.Length != 0)
		{
			array = template.parentGroup.modsToInstall;
			num4 = template.parentGroup.modChance;
		}
		ItemValue itemValue;
		if (lootItemValue.HasQuality)
		{
			if (num2 <= -1)
			{
				num2 = 1;
				num3 = 6;
			}
			if (num3 > LootContainer.LootMaxTier)
			{
				num3 = LootContainer.LootMaxTier;
			}
			if (num2 > num3)
			{
				num2 = num3;
			}
			itemValue = new ItemValue(lootItemValue.type, num2, num3, true, array, num4);
		}
		else
		{
			itemValue = new ItemValue(lootItemValue.type, 1, 6, true, array, num4);
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

	// Token: 0x0600329D RID: 12957 RVA: 0x00149284 File Offset: 0x00147484
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

	// Token: 0x0600329E RID: 12958 RVA: 0x001492D8 File Offset: 0x001474D8
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

	// Token: 0x0600329F RID: 12959 RVA: 0x00149424 File Offset: 0x00147624
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

	// Token: 0x060032A0 RID: 12960 RVA: 0x001494E4 File Offset: 0x001476E4
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

	// Token: 0x060032A1 RID: 12961 RVA: 0x0014976C File Offset: 0x0014796C
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

	// Token: 0x060032A2 RID: 12962 RVA: 0x00149894 File Offset: 0x00147A94
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

	// Token: 0x060032A3 RID: 12963 RVA: 0x001498DC File Offset: 0x00147ADC
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

	// Token: 0x060032A4 RID: 12964 RVA: 0x00149966 File Offset: 0x00147B66
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetSandboxProb(ItemClass _item)
	{
		if (_item.ItemTags.Test_AnySet(LootContainer.treasureTags))
		{
			return LootContainer.TreasureMapChance;
		}
		return 1f;
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x00149988 File Offset: 0x00147B88
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

	// Token: 0x060032A6 RID: 12966 RVA: 0x00149B80 File Offset: 0x00147D80
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

	// Token: 0x060032A7 RID: 12967 RVA: 0x00149BCC File Offset: 0x00147DCC
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

	// Token: 0x060032A8 RID: 12968 RVA: 0x00149CAC File Offset: 0x00147EAC
	[Conditional("DEBUG_LOOTLOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LootLog(string _name, string _format = "", params object[] _args)
	{
		_format = string.Format("{0} LootContainer {1} {2}", GameManager.frameCount, _name, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x040027E4 RID: 10212
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, LootContainer> lootContainers = new CaseInsensitiveStringDictionary<LootContainer>();

	// Token: 0x040027E5 RID: 10213
	public static readonly Dictionary<string, LootContainer.LootGroup> lootGroups = new Dictionary<string, LootContainer.LootGroup>();

	// Token: 0x040027E6 RID: 10214
	public static readonly Dictionary<string, LootContainer.LootQualityTemplate> lootQualityTemplates = new Dictionary<string, LootContainer.LootQualityTemplate>();

	// Token: 0x040027E7 RID: 10215
	public static readonly Dictionary<string, LootContainer.LootProbabilityTemplate> lootProbTemplates = new Dictionary<string, LootContainer.LootProbabilityTemplate>();

	// Token: 0x040027E8 RID: 10216
	public string Name;

	// Token: 0x040027E9 RID: 10217
	public string soundOpen;

	// Token: 0x040027EA RID: 10218
	public string soundClose;

	// Token: 0x040027EB RID: 10219
	public Vector2i size;

	// Token: 0x040027EC RID: 10220
	public float openTime;

	// Token: 0x040027ED RID: 10221
	public int minCount;

	// Token: 0x040027EE RID: 10222
	public int maxCount;

	// Token: 0x040027EF RID: 10223
	public LootContainer.DestroyOnClose destroyOnClose;

	// Token: 0x040027F0 RID: 10224
	public string lootQualityTemplate;

	// Token: 0x040027F1 RID: 10225
	public List<string> BuffActions;

	// Token: 0x040027F2 RID: 10226
	public string OnOpenEvent = "";

	// Token: 0x040027F3 RID: 10227
	public bool ignoreLootAbundance;

	// Token: 0x040027F4 RID: 10228
	public bool useUnmodifiedLootstage;

	// Token: 0x040027F5 RID: 10229
	public bool UniqueItems;

	// Token: 0x040027F6 RID: 10230
	public bool IgnoreLootProb;

	// Token: 0x040027F7 RID: 10231
	public readonly List<LootContainer.LootEntry> itemsToSpawn = new List<LootContainer.LootEntry>();

	// Token: 0x040027F8 RID: 10232
	public static Dictionary<EntityPlayer, string[]> OverrideItems = new Dictionary<EntityPlayer, string[]>();

	// Token: 0x040027F9 RID: 10233
	public static float GlobalCountModifier = 1f;

	// Token: 0x040027FA RID: 10234
	public static float FoodCountModifier = 1f;

	// Token: 0x040027FB RID: 10235
	public static float DrinkCountModifier = 1f;

	// Token: 0x040027FC RID: 10236
	public static float AmmoCountModifier = 1f;

	// Token: 0x040027FD RID: 10237
	public static float MedicalCountModifier = 1f;

	// Token: 0x040027FE RID: 10238
	public static float ResourceCountModifier = 1f;

	// Token: 0x040027FF RID: 10239
	public static float ArmorCountModifier = 1f;

	// Token: 0x04002800 RID: 10240
	public static float MeleeCountModifier = 1f;

	// Token: 0x04002801 RID: 10241
	public static float RangedCountModifier = 1f;

	// Token: 0x04002802 RID: 10242
	public static float DukesCountModifier = 1f;

	// Token: 0x04002803 RID: 10243
	public static float MagazinesCountModifier = 1f;

	// Token: 0x04002804 RID: 10244
	public static float BookCountModifier = 1f;

	// Token: 0x04002805 RID: 10245
	public static float TreasureMapChance = 1f;

	// Token: 0x04002806 RID: 10246
	public static int LootMaxTier = 6;

	// Token: 0x04002807 RID: 10247
	public static float LootTimerModifier = 1f;

	// Token: 0x04002808 RID: 10248
	public static float LootBagChance = 1f;

	// Token: 0x04002809 RID: 10249
	public static bool NoLoot = false;

	// Token: 0x0400280A RID: 10250
	public static FastTags<TagGroup.Global> foodTags = FastTags<TagGroup.Global>.Parse("food");

	// Token: 0x0400280B RID: 10251
	public static FastTags<TagGroup.Global> drinksTags = FastTags<TagGroup.Global>.Parse("drinks");

	// Token: 0x0400280C RID: 10252
	public static FastTags<TagGroup.Global> ammoTags = FastTags<TagGroup.Global>.Parse("ammo");

	// Token: 0x0400280D RID: 10253
	public static FastTags<TagGroup.Global> medicalTags = FastTags<TagGroup.Global>.Parse("medical");

	// Token: 0x0400280E RID: 10254
	public static FastTags<TagGroup.Global> junkTags = FastTags<TagGroup.Global>.Parse("junk");

	// Token: 0x0400280F RID: 10255
	public static FastTags<TagGroup.Global> armorTags = FastTags<TagGroup.Global>.Parse("armor,armorParts");

	// Token: 0x04002810 RID: 10256
	public static FastTags<TagGroup.Global> meleeTags = FastTags<TagGroup.Global>.Parse("melee,meleeParts");

	// Token: 0x04002811 RID: 10257
	public static FastTags<TagGroup.Global> rangedTags = FastTags<TagGroup.Global>.Parse("ranged,rangedParts");

	// Token: 0x04002812 RID: 10258
	public static FastTags<TagGroup.Global> dukesTags = FastTags<TagGroup.Global>.Parse("dukes");

	// Token: 0x04002813 RID: 10259
	public static FastTags<TagGroup.Global> magazineTags = FastTags<TagGroup.Global>.Parse("csm");

	// Token: 0x04002814 RID: 10260
	public static FastTags<TagGroup.Global> booksTags = FastTags<TagGroup.Global>.Parse("books");

	// Token: 0x04002815 RID: 10261
	public static FastTags<TagGroup.Global> treasureTags = FastTags<TagGroup.Global>.Parse("treasureMap");

	// Token: 0x02000611 RID: 1553
	public enum DestroyOnClose
	{
		// Token: 0x04002817 RID: 10263
		False,
		// Token: 0x04002818 RID: 10264
		True,
		// Token: 0x04002819 RID: 10265
		Empty
	}

	// Token: 0x02000612 RID: 1554
	public class LootItem
	{
		// Token: 0x0400281A RID: 10266
		public ItemValue itemValue;
	}

	// Token: 0x02000613 RID: 1555
	public class LootGroup
	{
		// Token: 0x0400281B RID: 10267
		public string name;

		// Token: 0x0400281C RID: 10268
		public string lootQualityTemplate;

		// Token: 0x0400281D RID: 10269
		public int minCount;

		// Token: 0x0400281E RID: 10270
		public int maxCount;

		// Token: 0x0400281F RID: 10271
		public int minQuality = -1;

		// Token: 0x04002820 RID: 10272
		public int maxQuality = -1;

		// Token: 0x04002821 RID: 10273
		public float minLevel;

		// Token: 0x04002822 RID: 10274
		public float maxLevel;

		// Token: 0x04002823 RID: 10275
		public string[] modsToInstall;

		// Token: 0x04002824 RID: 10276
		public float modChance = 1f;

		// Token: 0x04002825 RID: 10277
		public readonly List<LootContainer.LootEntry> items = new List<LootContainer.LootEntry>();

		// Token: 0x04002826 RID: 10278
		public LootContainer.LootGroup.AbundanceLootModTypes abundanceType;

		// Token: 0x02000614 RID: 1556
		public enum AbundanceLootModTypes
		{
			// Token: 0x04002828 RID: 10280
			None,
			// Token: 0x04002829 RID: 10281
			Food,
			// Token: 0x0400282A RID: 10282
			Drinks,
			// Token: 0x0400282B RID: 10283
			Ammo,
			// Token: 0x0400282C RID: 10284
			Meds,
			// Token: 0x0400282D RID: 10285
			Resources,
			// Token: 0x0400282E RID: 10286
			Armor,
			// Token: 0x0400282F RID: 10287
			Melee,
			// Token: 0x04002830 RID: 10288
			Ranged,
			// Token: 0x04002831 RID: 10289
			Dukes,
			// Token: 0x04002832 RID: 10290
			Magazines,
			// Token: 0x04002833 RID: 10291
			Books
		}
	}

	// Token: 0x02000615 RID: 1557
	public class LootEntry
	{
		// Token: 0x060032AD RID: 12973 RVA: 0x00149EB0 File Offset: 0x001480B0
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

		// Token: 0x04002834 RID: 10292
		public string lootProbTemplate;

		// Token: 0x04002835 RID: 10293
		public int minCount;

		// Token: 0x04002836 RID: 10294
		public int maxCount;

		// Token: 0x04002837 RID: 10295
		public int minQuality;

		// Token: 0x04002838 RID: 10296
		public int maxQuality;

		// Token: 0x04002839 RID: 10297
		public float minLevel;

		// Token: 0x0400283A RID: 10298
		public float maxLevel;

		// Token: 0x0400283B RID: 10299
		public float prob;

		// Token: 0x0400283C RID: 10300
		public bool forceProb;

		// Token: 0x0400283D RID: 10301
		public string[] modsToInstall;

		// Token: 0x0400283E RID: 10302
		public float modChance = 1f;

		// Token: 0x0400283F RID: 10303
		public float lootstageCountMod;

		// Token: 0x04002840 RID: 10304
		public LootContainer.LootItem item;

		// Token: 0x04002841 RID: 10305
		public LootContainer.LootGroup group;

		// Token: 0x04002842 RID: 10306
		public LootContainer.LootGroup parentGroup;

		// Token: 0x04002843 RID: 10307
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04002844 RID: 10308
		public string[] buffsToAdd;

		// Token: 0x04002845 RID: 10309
		public bool randomDurability = true;

		// Token: 0x04002846 RID: 10310
		public List<BaseLootEntryRequirement> Requirements;
	}

	// Token: 0x02000616 RID: 1558
	public class LootProbabilityTemplate
	{
		// Token: 0x04002847 RID: 10311
		public string name;

		// Token: 0x04002848 RID: 10312
		public readonly List<LootContainer.LootEntry> templates = new List<LootContainer.LootEntry>();
	}

	// Token: 0x02000617 RID: 1559
	public class LootQualityTemplate
	{
		// Token: 0x04002849 RID: 10313
		public string name;

		// Token: 0x0400284A RID: 10314
		public readonly List<LootContainer.LootGroup> templates = new List<LootContainer.LootGroup>();
	}
}
