using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Challenges;
using UnityEngine;

// Token: 0x02001167 RID: 4455
public class XUiM_Recipes : XUiModel
{
	// Token: 0x140000EA RID: 234
	// (add) Token: 0x06008C47 RID: 35911 RVA: 0x00357DF8 File Offset: 0x00355FF8
	// (remove) Token: 0x06008C48 RID: 35912 RVA: 0x00357E30 File Offset: 0x00356030
	public event XUiEvent_TrackedQuestChanged OnTrackedRecipeChanged;

	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06008C49 RID: 35913 RVA: 0x00357E65 File Offset: 0x00356065
	// (set) Token: 0x06008C4A RID: 35914 RVA: 0x00357E70 File Offset: 0x00356070
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

	// Token: 0x06008C4B RID: 35915 RVA: 0x00357EC0 File Offset: 0x003560C0
	public void SetPreviousTracked(EntityPlayerLocal player)
	{
		XUi xui = player.PlayerUI.xui;
		Quest trackedQuest = player.QuestJournal.TrackedQuest;
		XUiM_Recipes.sPreviouslyTrackedChallenge = xui.QuestTracker.TrackedChallenge;
		player.QuestJournal.TrackedQuest = null;
		xui.QuestTracker.TrackedChallenge = null;
		XUiM_Recipes.sPreviouslyTrackedQuest = trackedQuest;
	}

	// Token: 0x06008C4C RID: 35916 RVA: 0x00357F14 File Offset: 0x00356114
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

	// Token: 0x06008C4D RID: 35917 RVA: 0x00357FA2 File Offset: 0x003561A2
	public void RefreshTrackedRecipe()
	{
		if (this.OnTrackedRecipeChanged != null)
		{
			this.OnTrackedRecipeChanged();
		}
	}

	// Token: 0x06008C4E RID: 35918 RVA: 0x00357FB7 File Offset: 0x003561B7
	public static ReadOnlyCollection<Recipe> GetRecipes()
	{
		return CraftingManager.NonScrapableRecipes;
	}

	// Token: 0x06008C4F RID: 35919 RVA: 0x00357FC0 File Offset: 0x003561C0
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

	// Token: 0x06008C50 RID: 35920 RVA: 0x00358074 File Offset: 0x00356274
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

	// Token: 0x06008C51 RID: 35921 RVA: 0x0035814C File Offset: 0x0035634C
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

	// Token: 0x06008C52 RID: 35922 RVA: 0x003583C4 File Offset: 0x003565C4
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

	// Token: 0x06008C53 RID: 35923 RVA: 0x00358534 File Offset: 0x00356734
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

	// Token: 0x06008C54 RID: 35924 RVA: 0x00358588 File Offset: 0x00356788
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

	// Token: 0x06008C55 RID: 35925 RVA: 0x003585D0 File Offset: 0x003567D0
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

	// Token: 0x06008C56 RID: 35926 RVA: 0x00358614 File Offset: 0x00356814
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

	// Token: 0x06008C57 RID: 35927 RVA: 0x003586BE File Offset: 0x003568BE
	public static int GetCount(XUi xui)
	{
		return CraftingManager.GetRecipes().Count;
	}

	// Token: 0x06008C58 RID: 35928 RVA: 0x003586CA File Offset: 0x003568CA
	public static string GetRecipeName(Recipe _recipe)
	{
		return _recipe.GetName();
	}

	// Token: 0x06008C59 RID: 35929 RVA: 0x003586D2 File Offset: 0x003568D2
	public static string GetRecipeSpriteName(Recipe _recipe)
	{
		return _recipe.GetIcon();
	}

	// Token: 0x06008C5A RID: 35930 RVA: 0x003586DA File Offset: 0x003568DA
	public static bool GetRecipeIsUnlocked(XUi xui, Recipe _recipe)
	{
		return _recipe.IsUnlocked(xui.playerUI.entityPlayer);
	}

	// Token: 0x06008C5B RID: 35931 RVA: 0x003586F0 File Offset: 0x003568F0
	public static bool GetRecipeIsUnlocked(XUi xui, string itemName)
	{
		Recipe recipe = CraftingManager.GetRecipe(itemName);
		return recipe != null && XUiM_Recipes.GetRecipeIsUnlocked(xui, recipe);
	}

	// Token: 0x06008C5C RID: 35932 RVA: 0x00358710 File Offset: 0x00356910
	public static bool GetRecipeIsUnlockable(XUi xui, Recipe _recipe)
	{
		return _recipe.IsLearnable;
	}

	// Token: 0x06008C5D RID: 35933 RVA: 0x00358718 File Offset: 0x00356918
	public static bool GetRecipeIsFavorite(XUi xui, Recipe _recipe)
	{
		return CraftingManager.RecipeIsFavorite(_recipe);
	}

	// Token: 0x06008C5E RID: 35934 RVA: 0x00358720 File Offset: 0x00356920
	public static float GetRecipeCraftTime(XUi xui, Recipe _recipe)
	{
		float num = EffectManager.GetValue(PassiveEffects.CraftingTime, null, _recipe.craftingTime, xui.playerUI.entityPlayer, _recipe, _recipe.tags, true, true, true, true, true, 1, true, false) * XUiM_Recipes.CraftingTimeModifier;
		if (num < 0f)
		{
			return 0f;
		}
		return num;
	}

	// Token: 0x06008C5F RID: 35935 RVA: 0x0035876C File Offset: 0x0035696C
	public static int GetRecipeCraftOutputCount(XUi xui, Recipe _recipe)
	{
		float num = XUiM_Recipes.CraftingOutputModifier;
		if (_recipe.tags.Test_AnySet(XUiM_Recipes.SandboxIgnoreTag))
		{
			num = 1f;
		}
		return (int)Mathf.Max(EffectManager.GetValue(PassiveEffects.CraftingOutputCount, null, (float)_recipe.count, xui.playerUI.entityPlayer, _recipe, _recipe.tags, true, true, true, true, true, 1, true, false) * num, 1f);
	}

	// Token: 0x06008C60 RID: 35936 RVA: 0x003587CD File Offset: 0x003569CD
	public static ItemStack GetRecipeOutput(Recipe _recipe)
	{
		return new ItemStack(new ItemValue(_recipe.itemValueType, false), _recipe.count);
	}

	// Token: 0x06008C61 RID: 35937 RVA: 0x003587E6 File Offset: 0x003569E6
	public static List<ItemStack> GetRecipeIngredients(Recipe _recipe)
	{
		return _recipe.GetIngredientsSummedUp();
	}

	// Token: 0x06008C62 RID: 35938 RVA: 0x000027FC File Offset: 0x000009FC
	public static void GetCurrentSlots()
	{
	}

	// Token: 0x06008C63 RID: 35939 RVA: 0x003587EE File Offset: 0x003569EE
	public static bool HasIngredientsForRecipe(IList<ItemStack> allItems, Recipe _recipe, EntityAlive _ea = null)
	{
		return _recipe.CanCraftAny(allItems, _ea);
	}

	// Token: 0x06008C64 RID: 35940 RVA: 0x003587F8 File Offset: 0x003569F8
	public static float GetCraftingInputModifier(Recipe recipe)
	{
		if (recipe.tags.Test_AnySet(XUiM_Recipes.SandboxIgnoreTag))
		{
			return 1f;
		}
		return XUiM_Recipes.CraftingInputModifier;
	}

	// Token: 0x040067B4 RID: 26548
	public static float CraftingTimeModifier = 1f;

	// Token: 0x040067B5 RID: 26549
	public static float CraftingInputModifier = 1f;

	// Token: 0x040067B6 RID: 26550
	public static float CraftingOutputModifier = 1f;

	// Token: 0x040067B7 RID: 26551
	public static float SmeltingTimeModifier = 1f;

	// Token: 0x040067B8 RID: 26552
	public static bool DisableSmelter = false;

	// Token: 0x040067B9 RID: 26553
	public static float ScrappingOutputModifier = 1f;

	// Token: 0x040067BA RID: 26554
	public static float SmeltingOutputModifier = 1f;

	// Token: 0x040067BB RID: 26555
	public static float DewCollectorTimeModifier = 1f;

	// Token: 0x040067BC RID: 26556
	public static float DewCollectorOutput = 1f;

	// Token: 0x040067BD RID: 26557
	public static float DewCollectorInput = 1f;

	// Token: 0x040067BE RID: 26558
	public static float ApiaryTimeModifier = 1f;

	// Token: 0x040067BF RID: 26559
	public static float ApiaryOutput = 1f;

	// Token: 0x040067C0 RID: 26560
	public static float ApiaryInput = 1f;

	// Token: 0x040067C1 RID: 26561
	public static float ChickenCoopTimeModifier = 1f;

	// Token: 0x040067C2 RID: 26562
	public static float ChickenCoopOutput = 1f;

	// Token: 0x040067C3 RID: 26563
	public static float ChickenCoopInput = 1f;

	// Token: 0x040067C4 RID: 26564
	public static float CollectorTimeModifier = 1f;

	// Token: 0x040067C5 RID: 26565
	public static int CollectorOutput = 1;

	// Token: 0x040067C6 RID: 26566
	public static float MiningOutputModifier = 1f;

	// Token: 0x040067C7 RID: 26567
	public static float HarvestingOutputModifier = 1f;

	// Token: 0x040067C8 RID: 26568
	public static float CropOutputModifier = 1f;

	// Token: 0x040067C9 RID: 26569
	public static float SeedDropOutputModifier = 1f;

	// Token: 0x040067CA RID: 26570
	public static int CraftingMaxTier = 6;

	// Token: 0x040067CB RID: 26571
	public static bool CraftingProgression = true;

	// Token: 0x040067CC RID: 26572
	public static BackpackCraftingOptions BackpackCrafting = BackpackCraftingOptions.Enabled;

	// Token: 0x040067CD RID: 26573
	public static bool WorkstationCrafting = true;

	// Token: 0x040067CF RID: 26575
	[PublicizedFrom(EAccessModifier.Private)]
	public static Quest sPreviouslyTrackedQuest = null;

	// Token: 0x040067D0 RID: 26576
	[PublicizedFrom(EAccessModifier.Private)]
	public static Challenge sPreviouslyTrackedChallenge = null;

	// Token: 0x040067D1 RID: 26577
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> SandboxIgnoreTag = FastTags<TagGroup.Global>.Parse("sandboxIgnore");

	// Token: 0x040067D2 RID: 26578
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> workbenchTag = FastTags<TagGroup.Global>.Parse("workbench");

	// Token: 0x040067D3 RID: 26579
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe trackedRecipe;

	// Token: 0x040067D4 RID: 26580
	public int TrackedRecipeQuality = 1;

	// Token: 0x040067D5 RID: 26581
	public int TrackedRecipeCount = 1;

	// Token: 0x040067D6 RID: 26582
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> workbenchOnlyTag = FastTags<TagGroup.Global>.Parse("isworkbench");

	// Token: 0x040067D7 RID: 26583
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> biomeprogressionTag = FastTags<TagGroup.Global>.Parse("biomeProgression");

	// Token: 0x040067D8 RID: 26584
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> useSmelterTag = FastTags<TagGroup.Global>.Parse("use_smelter");

	// Token: 0x040067D9 RID: 26585
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> replaceSmelterTag = FastTags<TagGroup.Global>.Parse("replace_smelter");
}
