using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Challenges;
using UnityEngine;

// Token: 0x02001157 RID: 4439
public class XUiM_Recipes : XUiModel
{
	// Token: 0x140000EA RID: 234
	// (add) Token: 0x06008BF3 RID: 35827 RVA: 0x00355D4C File Offset: 0x00353F4C
	// (remove) Token: 0x06008BF4 RID: 35828 RVA: 0x00355D84 File Offset: 0x00353F84
	public event XUiEvent_TrackedQuestChanged OnTrackedRecipeChanged;

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x06008BF5 RID: 35829 RVA: 0x00355DB9 File Offset: 0x00353FB9
	// (set) Token: 0x06008BF6 RID: 35830 RVA: 0x00355DC4 File Offset: 0x00353FC4
	public Recipe TrackedRecipe
	{
		get
		{
			return this.trackedRecipe;
		}
		set
		{
			if (this.trackedRecipe != null)
			{
				this.trackedRecipe.IsTracked = false;
			}
			this.trackedRecipe = value;
			if (this.trackedRecipe != null)
			{
				this.trackedRecipe.IsTracked = true;
			}
			if (this.OnTrackedRecipeChanged != null)
			{
				this.OnTrackedRecipeChanged();
			}
		}
	}

	// Token: 0x06008BF7 RID: 35831 RVA: 0x00355E14 File Offset: 0x00354014
	public void SetPreviousTracked(EntityPlayerLocal player)
	{
		XUi xui = player.PlayerUI.xui;
		Quest trackedQuest = player.QuestJournal.TrackedQuest;
		XUiM_Recipes.sPreviouslyTrackedChallenge = xui.QuestTracker.TrackedChallenge;
		player.QuestJournal.TrackedQuest = null;
		xui.QuestTracker.TrackedChallenge = null;
		XUiM_Recipes.sPreviouslyTrackedQuest = trackedQuest;
	}

	// Token: 0x06008BF8 RID: 35832 RVA: 0x00355E68 File Offset: 0x00354068
	public void ResetToPreviousTracked(EntityPlayerLocal player)
	{
		XUi xui = player.PlayerUI.xui;
		if (XUiM_Recipes.sPreviouslyTrackedQuest != null)
		{
			if (!player.QuestJournal.QuestIsActive(XUiM_Recipes.sPreviouslyTrackedQuest))
			{
				XUiM_Recipes.sPreviouslyTrackedQuest = player.QuestJournal.FindActiveQuest();
			}
		}
		else if (XUiM_Recipes.sPreviouslyTrackedChallenge != null && !XUiM_Recipes.sPreviouslyTrackedChallenge.IsActive)
		{
			XUiM_Recipes.sPreviouslyTrackedChallenge = XUiM_Recipes.sPreviouslyTrackedChallenge.Owner.GetNextChallenge(XUiM_Recipes.sPreviouslyTrackedChallenge);
		}
		player.QuestJournal.TrackedQuest = XUiM_Recipes.sPreviouslyTrackedQuest;
		xui.QuestTracker.TrackedChallenge = XUiM_Recipes.sPreviouslyTrackedChallenge;
	}

	// Token: 0x06008BF9 RID: 35833 RVA: 0x00355EF6 File Offset: 0x003540F6
	public void RefreshTrackedRecipe()
	{
		if (this.OnTrackedRecipeChanged != null)
		{
			this.OnTrackedRecipeChanged();
		}
	}

	// Token: 0x06008BFA RID: 35834 RVA: 0x00355F0B File Offset: 0x0035410B
	public static ReadOnlyCollection<Recipe> GetRecipes()
	{
		return CraftingManager.NonScrapableRecipes;
	}

	// Token: 0x06008BFB RID: 35835 RVA: 0x00355F14 File Offset: 0x00354114
	public static void UpdateRecipesforBackpackCrafting()
	{
		ReadOnlyCollection<Recipe> recipes = XUiM_Recipes.GetRecipes();
		for (int i = 0; i < recipes.Count; i++)
		{
			if (recipes[i].craftingArea == "")
			{
				BackpackCraftingOptions backpackCrafting = XUiM_Recipes.BackpackCrafting;
				if (backpackCrafting != BackpackCraftingOptions.BasicsOnly)
				{
					if (backpackCrafting != BackpackCraftingOptions.WorkbenchOnly)
					{
						recipes[i].craftingArea = "workbench";
					}
					else if (!recipes[i].tags.Test_AnySet(XUiM_Recipes.workbenchOnlyTag))
					{
						recipes[i].craftingArea = "workbench";
					}
				}
				else if (!recipes[i].GetOutputItemClass().IsLimitedItem)
				{
					recipes[i].craftingArea = "workbench";
				}
			}
		}
	}

	// Token: 0x06008BFC RID: 35836 RVA: 0x00355FC8 File Offset: 0x003541C8
	public static void FilterRecipesByCategory(string _category, ref List<Recipe> recipeList)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < recipeList.Count; i++)
		{
			Recipe recipe = recipeList[i];
			if (recipe.isChallenge)
			{
				list.Add(recipe);
			}
			else if (recipe.isQuest)
			{
				list.Add(recipe);
			}
			else if (recipe.IsTracked)
			{
				list.Add(recipe);
			}
			else
			{
				ItemClass forId = ItemClass.GetForId(recipe.itemValueType);
				if (forId != null)
				{
					string[] array;
					if (!forId.IsBlock())
					{
						array = forId.Groups;
					}
					else
					{
						array = Block.list[forId.Id].GroupNames;
					}
					if (array != null)
					{
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j] != null && array[j].EqualsCaseInsensitive(_category))
							{
								list.Add(recipe);
								break;
							}
						}
					}
				}
			}
		}
		recipeList = list;
	}

	// Token: 0x06008BFD RID: 35837 RVA: 0x003560A0 File Offset: 0x003542A0
	public static List<Recipe> FilterRecipesByWorkstation(string _workstation, IList<Recipe> recipeList)
	{
		if (_workstation == null)
		{
			_workstation = "";
		}
		List<Recipe> list = new List<Recipe>();
		if (_workstation != "")
		{
			if (!XUiM_Recipes.WorkstationCrafting)
			{
				return list;
			}
		}
		else if (XUiM_Recipes.BackpackCrafting == BackpackCraftingOptions.Disabled)
		{
			return list;
		}
		for (int i = 0; i < recipeList.Count; i++)
		{
			if (recipeList[i] != null)
			{
				Recipe recipe = recipeList[i];
				if ((World.BiomeProgressionEnabled || !recipe.tags.Test_AnySet(XUiM_Recipes.biomeprogressionTag)) && (ItemClass.MaxTechType >= ItemClass.ItemTechTypes.T3 || recipe.GetOutputItemClass().ItemTechType <= ItemClass.MaxTechType))
				{
					if (_workstation != "")
					{
						Block blockByName = Block.GetBlockByName(_workstation, false);
						if (blockByName != null && blockByName.Properties.Contains("Workstation", "CraftingAreaRecipes"))
						{
							string @string = blockByName.Properties.GetString("Workstation", "CraftingAreaRecipes");
							string[] array = new string[] { @string };
							if (@string.Contains(","))
							{
								array = @string.Replace(", ", ",").Replace(" ,", ",").Replace(" , ", ",")
									.Split(',', StringSplitOptions.None);
							}
							bool flag = false;
							for (int j = 0; j < array.Length; j++)
							{
								if (recipe.craftingArea != null && recipe.craftingArea.EqualsCaseInsensitive(array[j]))
								{
									if (recipe.craftingArea == "forge")
									{
										if (XUiM_Recipes.DisableSmelter && recipe.tags.Test_AnySet(XUiM_Recipes.useSmelterTag))
										{
											flag = true;
											goto IL_01DC;
										}
										if (!XUiM_Recipes.DisableSmelter && recipe.tags.Test_AnySet(XUiM_Recipes.replaceSmelterTag))
										{
											flag = true;
											goto IL_01DC;
										}
									}
									list.Add(recipe);
									flag = true;
									break;
								}
								if ((recipe.craftingArea == null || recipe.craftingArea == "") && array[j].EqualsCaseInsensitive("player"))
								{
									list.Add(recipe);
									flag = true;
									break;
								}
								IL_01DC:;
							}
							if (!flag && recipe.craftingArea != null && recipe.craftingArea.EqualsCaseInsensitive(_workstation))
							{
								list.Add(recipe);
							}
						}
						else if (recipe.craftingArea != null && recipe.craftingArea.EqualsCaseInsensitive(_workstation))
						{
							list.Add(recipe);
						}
					}
					else if ((recipe.craftingArea == null || recipe.craftingArea == "") && XUiM_Recipes.FilterByBackpackSandboxSetting(recipe))
					{
						list.Add(recipe);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06008BFE RID: 35838 RVA: 0x00356318 File Offset: 0x00354518
	public static List<Recipe> FilterRecipesByName(string _name, IList<Recipe> recipeList)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < recipeList.Count; i++)
		{
			Recipe recipe = recipeList[i];
			if (!recipe.craftingArea.EqualsCaseInsensitive("assembly") && (World.BiomeProgressionEnabled || !recipe.tags.Test_AnySet(XUiM_Recipes.biomeprogressionTag)))
			{
				ItemClass forId = ItemClass.GetForId(recipe.itemValueType);
				if (forId != null && (ItemClass.MaxTechType >= ItemClass.ItemTechTypes.T3 || forId.ItemTechType <= ItemClass.MaxTechType) && (!(recipe.craftingArea == "forge") || ((!XUiM_Recipes.DisableSmelter || !recipe.tags.Test_AnySet(XUiM_Recipes.useSmelterTag)) && (XUiM_Recipes.DisableSmelter || !recipe.tags.Test_AnySet(XUiM_Recipes.replaceSmelterTag)))))
				{
					string text;
					if (Localization.TryGet(recipe.GetName(), out text) && text.ContainsCaseInsensitive(_name))
					{
						list.Add(recipe);
					}
					else if (!forId.IsBlock())
					{
						if (forId.GetItemName().ContainsCaseInsensitive(_name))
						{
							list.Add(recipe);
						}
						else if (forId.GetLocalizedItemName().ContainsCaseInsensitive(_name))
						{
							list.Add(recipe);
						}
					}
					else
					{
						Block block = Block.list[forId.Id];
						if (block != null)
						{
							if (block.GetBlockName().ContainsCaseInsensitive(_name))
							{
								list.Add(recipe);
							}
							else if (block.GetLocalizedBlockName().ContainsCaseInsensitive(_name))
							{
								list.Add(recipe);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06008BFF RID: 35839 RVA: 0x00356488 File Offset: 0x00354688
	public static List<Recipe> FilterRecipesByIngredient(ItemStack stack, IList<Recipe> recipeList)
	{
		List<Recipe> list = new List<Recipe>();
		ItemValue[] array = new ItemValue[] { stack.itemValue };
		for (int i = 0; i < recipeList.Count; i++)
		{
			if (recipeList[i].ContainsIngredients(array))
			{
				list.Add(recipeList[i]);
			}
		}
		return list;
	}

	// Token: 0x06008C00 RID: 35840 RVA: 0x003564DC File Offset: 0x003546DC
	public static List<Recipe> FilterRecipesByItem(List<int> itemIDs, IList<Recipe> recipeList)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < recipeList.Count; i++)
		{
			if (itemIDs.Contains(recipeList[i].itemValueType))
			{
				list.Add(recipeList[i]);
			}
		}
		return list;
	}

	// Token: 0x06008C01 RID: 35841 RVA: 0x00356524 File Offset: 0x00354724
	public static List<Recipe> FilterRecipesByID(int itemID, IList<Recipe> recipeList)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < recipeList.Count; i++)
		{
			if (itemID == recipeList[i].itemValueType)
			{
				list.Add(recipeList[i]);
			}
		}
		return list;
	}

	// Token: 0x06008C02 RID: 35842 RVA: 0x00356568 File Offset: 0x00354768
	public static bool FilterByBackpackSandboxSetting(Recipe recipe)
	{
		if ((XUiM_Recipes.BackpackCrafting == BackpackCraftingOptions.Disabled && recipe.craftingArea == "") || (XUiM_Recipes.BackpackCrafting == BackpackCraftingOptions.WorkbenchOnly && !recipe.tags.Test_AnySet(XUiM_Recipes.workbenchOnlyTag)))
		{
			return false;
		}
		if (XUiM_Recipes.BackpackCrafting != BackpackCraftingOptions.BasicsOnly)
		{
			return true;
		}
		ItemClass forId = ItemClass.GetForId(recipe.itemValueType);
		if (forId == null)
		{
			return false;
		}
		string[] array;
		if (!forId.IsBlock())
		{
			array = forId.Groups;
		}
		else
		{
			array = Block.list[forId.Id].GroupNames;
		}
		if (array == null)
		{
			return false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null && array[i].EqualsCaseInsensitive("limited"))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008C03 RID: 35843 RVA: 0x00356612 File Offset: 0x00354812
	public static int GetCount(XUi xui)
	{
		return CraftingManager.GetRecipes().Count;
	}

	// Token: 0x06008C04 RID: 35844 RVA: 0x0035661E File Offset: 0x0035481E
	public static string GetRecipeName(Recipe _recipe)
	{
		return _recipe.GetName();
	}

	// Token: 0x06008C05 RID: 35845 RVA: 0x00356626 File Offset: 0x00354826
	public static string GetRecipeSpriteName(Recipe _recipe)
	{
		return _recipe.GetIcon();
	}

	// Token: 0x06008C06 RID: 35846 RVA: 0x0035662E File Offset: 0x0035482E
	public static bool GetRecipeIsUnlocked(XUi xui, Recipe _recipe)
	{
		return _recipe.IsUnlocked(xui.playerUI.entityPlayer);
	}

	// Token: 0x06008C07 RID: 35847 RVA: 0x00356644 File Offset: 0x00354844
	public static bool GetRecipeIsUnlocked(XUi xui, string itemName)
	{
		Recipe recipe = CraftingManager.GetRecipe(itemName);
		return recipe != null && XUiM_Recipes.GetRecipeIsUnlocked(xui, recipe);
	}

	// Token: 0x06008C08 RID: 35848 RVA: 0x00356664 File Offset: 0x00354864
	public static bool GetRecipeIsUnlockable(XUi xui, Recipe _recipe)
	{
		return _recipe.IsLearnable;
	}

	// Token: 0x06008C09 RID: 35849 RVA: 0x0035666C File Offset: 0x0035486C
	public static bool GetRecipeIsFavorite(XUi xui, Recipe _recipe)
	{
		return CraftingManager.RecipeIsFavorite(_recipe);
	}

	// Token: 0x06008C0A RID: 35850 RVA: 0x00356674 File Offset: 0x00354874
	public static float GetRecipeCraftTime(XUi xui, Recipe _recipe)
	{
		float num = EffectManager.GetValue(PassiveEffects.CraftingTime, null, _recipe.craftingTime, xui.playerUI.entityPlayer, _recipe, _recipe.tags, true, true, true, true, true, 1, true, false) * XUiM_Recipes.CraftingTimeModifier;
		if (num < 0f)
		{
			return 0f;
		}
		return num;
	}

	// Token: 0x06008C0B RID: 35851 RVA: 0x003566C0 File Offset: 0x003548C0
	public static int GetRecipeCraftOutputCount(XUi xui, Recipe _recipe)
	{
		float num = XUiM_Recipes.CraftingOutputModifier;
		if (_recipe.tags.Test_AnySet(XUiM_Recipes.SandboxIgnoreTag))
		{
			num = 1f;
		}
		return (int)Mathf.Max(EffectManager.GetValue(PassiveEffects.CraftingOutputCount, null, (float)_recipe.count, xui.playerUI.entityPlayer, _recipe, _recipe.tags, true, true, true, true, true, 1, true, false) * num, 1f);
	}

	// Token: 0x06008C0C RID: 35852 RVA: 0x00356721 File Offset: 0x00354921
	public static ItemStack GetRecipeOutput(Recipe _recipe)
	{
		return new ItemStack(new ItemValue(_recipe.itemValueType, false), _recipe.count);
	}

	// Token: 0x06008C0D RID: 35853 RVA: 0x0035673A File Offset: 0x0035493A
	public static List<ItemStack> GetRecipeIngredients(Recipe _recipe)
	{
		return _recipe.GetIngredientsSummedUp();
	}

	// Token: 0x06008C0E RID: 35854 RVA: 0x000027FC File Offset: 0x000009FC
	public static void GetCurrentSlots()
	{
	}

	// Token: 0x06008C0F RID: 35855 RVA: 0x00356742 File Offset: 0x00354942
	public static bool HasIngredientsForRecipe(IList<ItemStack> allItems, Recipe _recipe, EntityAlive _ea = null)
	{
		return _recipe.CanCraftAny(allItems, _ea);
	}

	// Token: 0x06008C10 RID: 35856 RVA: 0x0035674C File Offset: 0x0035494C
	public static float GetCraftingInputModifier(Recipe recipe)
	{
		if (recipe.tags.Test_AnySet(XUiM_Recipes.SandboxIgnoreTag))
		{
			return 1f;
		}
		return XUiM_Recipes.CraftingInputModifier;
	}

	// Token: 0x04006774 RID: 26484
	public static float CraftingTimeModifier = 1f;

	// Token: 0x04006775 RID: 26485
	public static float CraftingInputModifier = 1f;

	// Token: 0x04006776 RID: 26486
	public static float CraftingOutputModifier = 1f;

	// Token: 0x04006777 RID: 26487
	public static float SmeltingTimeModifier = 1f;

	// Token: 0x04006778 RID: 26488
	public static bool DisableSmelter = false;

	// Token: 0x04006779 RID: 26489
	public static float ScrappingOutputModifier = 1f;

	// Token: 0x0400677A RID: 26490
	public static float SmeltingOutputModifier = 1f;

	// Token: 0x0400677B RID: 26491
	public static float DewCollectorTimeModifier = 1f;

	// Token: 0x0400677C RID: 26492
	public static float DewCollectorOutput = 1f;

	// Token: 0x0400677D RID: 26493
	public static float DewCollectorInput = 1f;

	// Token: 0x0400677E RID: 26494
	public static float ApiaryTimeModifier = 1f;

	// Token: 0x0400677F RID: 26495
	public static float ApiaryOutput = 1f;

	// Token: 0x04006780 RID: 26496
	public static float ApiaryInput = 1f;

	// Token: 0x04006781 RID: 26497
	public static float ChickenCoopTimeModifier = 1f;

	// Token: 0x04006782 RID: 26498
	public static float ChickenCoopOutput = 1f;

	// Token: 0x04006783 RID: 26499
	public static float ChickenCoopInput = 1f;

	// Token: 0x04006784 RID: 26500
	public static float CollectorTimeModifier = 1f;

	// Token: 0x04006785 RID: 26501
	public static int CollectorOutput = 1;

	// Token: 0x04006786 RID: 26502
	public static float MiningOutputModifier = 1f;

	// Token: 0x04006787 RID: 26503
	public static float HarvestingOutputModifier = 1f;

	// Token: 0x04006788 RID: 26504
	public static float CropOutputModifier = 1f;

	// Token: 0x04006789 RID: 26505
	public static float SeedDropOutputModifier = 1f;

	// Token: 0x0400678A RID: 26506
	public static int CraftingMaxTier = 6;

	// Token: 0x0400678B RID: 26507
	public static bool CraftingProgression = true;

	// Token: 0x0400678C RID: 26508
	public static BackpackCraftingOptions BackpackCrafting = BackpackCraftingOptions.Enabled;

	// Token: 0x0400678D RID: 26509
	public static bool WorkstationCrafting = true;

	// Token: 0x0400678F RID: 26511
	[PublicizedFrom(EAccessModifier.Private)]
	public static Quest sPreviouslyTrackedQuest = null;

	// Token: 0x04006790 RID: 26512
	[PublicizedFrom(EAccessModifier.Private)]
	public static Challenge sPreviouslyTrackedChallenge = null;

	// Token: 0x04006791 RID: 26513
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> SandboxIgnoreTag = FastTags<TagGroup.Global>.Parse("sandboxIgnore");

	// Token: 0x04006792 RID: 26514
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> workbenchTag = FastTags<TagGroup.Global>.Parse("workbench");

	// Token: 0x04006793 RID: 26515
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe trackedRecipe;

	// Token: 0x04006794 RID: 26516
	public int TrackedRecipeQuality = 1;

	// Token: 0x04006795 RID: 26517
	public int TrackedRecipeCount = 1;

	// Token: 0x04006796 RID: 26518
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> workbenchOnlyTag = FastTags<TagGroup.Global>.Parse("isworkbench");

	// Token: 0x04006797 RID: 26519
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> biomeprogressionTag = FastTags<TagGroup.Global>.Parse("biomeProgression");

	// Token: 0x04006798 RID: 26520
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> useSmelterTag = FastTags<TagGroup.Global>.Parse("use_smelter");

	// Token: 0x04006799 RID: 26521
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> replaceSmelterTag = FastTags<TagGroup.Global>.Parse("replace_smelter");
}
