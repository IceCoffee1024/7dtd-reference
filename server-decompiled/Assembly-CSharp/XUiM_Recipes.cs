using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Challenges;
using UnityEngine;

// Token: 0x02001126 RID: 4390
public class XUiM_Recipes : XUiModel
{
	// Token: 0x140000EA RID: 234
	// (add) Token: 0x06008A96 RID: 35478 RVA: 0x0034D110 File Offset: 0x0034B310
	// (remove) Token: 0x06008A97 RID: 35479 RVA: 0x0034D148 File Offset: 0x0034B348
	public event XUiEvent_TrackedQuestChanged OnTrackedRecipeChanged;

	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x06008A98 RID: 35480 RVA: 0x0034D17D File Offset: 0x0034B37D
	// (set) Token: 0x06008A99 RID: 35481 RVA: 0x0034D188 File Offset: 0x0034B388
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

	// Token: 0x06008A9A RID: 35482 RVA: 0x0034D1D8 File Offset: 0x0034B3D8
	public void SetPreviousTracked(EntityPlayerLocal player)
	{
		XUi xui = player.PlayerUI.xui;
		Quest trackedQuest = player.QuestJournal.TrackedQuest;
		XUiM_Recipes.sPreviouslyTrackedChallenge = xui.QuestTracker.TrackedChallenge;
		player.QuestJournal.TrackedQuest = null;
		xui.QuestTracker.TrackedChallenge = null;
		XUiM_Recipes.sPreviouslyTrackedQuest = trackedQuest;
	}

	// Token: 0x06008A9B RID: 35483 RVA: 0x0034D22C File Offset: 0x0034B42C
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

	// Token: 0x06008A9C RID: 35484 RVA: 0x0034D2BA File Offset: 0x0034B4BA
	public void RefreshTrackedRecipe()
	{
		if (this.OnTrackedRecipeChanged != null)
		{
			this.OnTrackedRecipeChanged();
		}
	}

	// Token: 0x06008A9D RID: 35485 RVA: 0x0034D2CF File Offset: 0x0034B4CF
	public static ReadOnlyCollection<Recipe> GetRecipes()
	{
		return CraftingManager.NonScrapableRecipes;
	}

	// Token: 0x06008A9E RID: 35486 RVA: 0x0034D2D8 File Offset: 0x0034B4D8
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

	// Token: 0x06008A9F RID: 35487 RVA: 0x0034D38C File Offset: 0x0034B58C
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

	// Token: 0x06008AA0 RID: 35488 RVA: 0x0034D464 File Offset: 0x0034B664
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

	// Token: 0x06008AA1 RID: 35489 RVA: 0x0034D6DC File Offset: 0x0034B8DC
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

	// Token: 0x06008AA2 RID: 35490 RVA: 0x0034D84C File Offset: 0x0034BA4C
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

	// Token: 0x06008AA3 RID: 35491 RVA: 0x0034D8A0 File Offset: 0x0034BAA0
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

	// Token: 0x06008AA4 RID: 35492 RVA: 0x0034D8E8 File Offset: 0x0034BAE8
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

	// Token: 0x06008AA5 RID: 35493 RVA: 0x0034D92C File Offset: 0x0034BB2C
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

	// Token: 0x06008AA6 RID: 35494 RVA: 0x0034D9D6 File Offset: 0x0034BBD6
	public static int GetCount(XUi xui)
	{
		return CraftingManager.GetRecipes().Count;
	}

	// Token: 0x06008AA7 RID: 35495 RVA: 0x0034D9E2 File Offset: 0x0034BBE2
	public static string GetRecipeName(Recipe _recipe)
	{
		return _recipe.GetName();
	}

	// Token: 0x06008AA8 RID: 35496 RVA: 0x0034D9EA File Offset: 0x0034BBEA
	public static string GetRecipeSpriteName(Recipe _recipe)
	{
		return _recipe.GetIcon();
	}

	// Token: 0x06008AA9 RID: 35497 RVA: 0x0034D9F2 File Offset: 0x0034BBF2
	public static bool GetRecipeIsUnlocked(XUi xui, Recipe _recipe)
	{
		return _recipe.IsUnlocked(xui.playerUI.entityPlayer);
	}

	// Token: 0x06008AAA RID: 35498 RVA: 0x0034DA08 File Offset: 0x0034BC08
	public static bool GetRecipeIsUnlocked(XUi xui, string itemName)
	{
		Recipe recipe = CraftingManager.GetRecipe(itemName);
		return recipe != null && XUiM_Recipes.GetRecipeIsUnlocked(xui, recipe);
	}

	// Token: 0x06008AAB RID: 35499 RVA: 0x0034DA28 File Offset: 0x0034BC28
	public static bool GetRecipeIsUnlockable(XUi xui, Recipe _recipe)
	{
		return _recipe.IsLearnable;
	}

	// Token: 0x06008AAC RID: 35500 RVA: 0x0034DA30 File Offset: 0x0034BC30
	public static bool GetRecipeIsFavorite(XUi xui, Recipe _recipe)
	{
		return CraftingManager.RecipeIsFavorite(_recipe);
	}

	// Token: 0x06008AAD RID: 35501 RVA: 0x0034DA38 File Offset: 0x0034BC38
	public static float GetRecipeCraftTime(XUi xui, Recipe _recipe)
	{
		float num = EffectManager.GetValue(PassiveEffects.CraftingTime, null, _recipe.craftingTime, xui.playerUI.entityPlayer, _recipe, _recipe.tags, true, true, true, true, true, 1, true, false) * XUiM_Recipes.CraftingTimeModifier;
		if (num < 0f)
		{
			return 0f;
		}
		return num;
	}

	// Token: 0x06008AAE RID: 35502 RVA: 0x0034DA84 File Offset: 0x0034BC84
	public static int GetRecipeCraftOutputCount(XUi xui, Recipe _recipe)
	{
		float num = XUiM_Recipes.CraftingOutputModifier;
		if (_recipe.tags.Test_AnySet(XUiM_Recipes.SandboxIgnoreTag))
		{
			num = 1f;
		}
		return (int)Mathf.Max(EffectManager.GetValue(PassiveEffects.CraftingOutputCount, null, (float)_recipe.count, xui.playerUI.entityPlayer, _recipe, _recipe.tags, true, true, true, true, true, 1, true, false) * num, 1f);
	}

	// Token: 0x06008AAF RID: 35503 RVA: 0x0034DAE5 File Offset: 0x0034BCE5
	public static ItemStack GetRecipeOutput(Recipe _recipe)
	{
		return new ItemStack(new ItemValue(_recipe.itemValueType, false), _recipe.count);
	}

	// Token: 0x06008AB0 RID: 35504 RVA: 0x0034DAFE File Offset: 0x0034BCFE
	public static List<ItemStack> GetRecipeIngredients(Recipe _recipe)
	{
		return _recipe.GetIngredientsSummedUp();
	}

	// Token: 0x06008AB1 RID: 35505 RVA: 0x000027FC File Offset: 0x000009FC
	public static void GetCurrentSlots()
	{
	}

	// Token: 0x06008AB2 RID: 35506 RVA: 0x0034DB06 File Offset: 0x0034BD06
	public static bool HasIngredientsForRecipe(IList<ItemStack> allItems, Recipe _recipe, EntityAlive _ea = null)
	{
		return _recipe.CanCraftAny(allItems, _ea);
	}

	// Token: 0x06008AB3 RID: 35507 RVA: 0x0034DB10 File Offset: 0x0034BD10
	public static float GetCraftingInputModifier(Recipe recipe)
	{
		if (recipe.tags.Test_AnySet(XUiM_Recipes.SandboxIgnoreTag))
		{
			return 1f;
		}
		return XUiM_Recipes.CraftingInputModifier;
	}

	// Token: 0x04006658 RID: 26200
	public static float CraftingTimeModifier = 1f;

	// Token: 0x04006659 RID: 26201
	public static float CraftingInputModifier = 1f;

	// Token: 0x0400665A RID: 26202
	public static float CraftingOutputModifier = 1f;

	// Token: 0x0400665B RID: 26203
	public static float SmeltingTimeModifier = 1f;

	// Token: 0x0400665C RID: 26204
	public static bool DisableSmelter = false;

	// Token: 0x0400665D RID: 26205
	public static float ScrappingOutputModifier = 1f;

	// Token: 0x0400665E RID: 26206
	public static float SmeltingOutputModifier = 1f;

	// Token: 0x0400665F RID: 26207
	public static float DewCollectorTimeModifier = 1f;

	// Token: 0x04006660 RID: 26208
	public static float DewCollectorOutput = 1f;

	// Token: 0x04006661 RID: 26209
	public static float DewCollectorInput = 1f;

	// Token: 0x04006662 RID: 26210
	public static float ApiaryTimeModifier = 1f;

	// Token: 0x04006663 RID: 26211
	public static float ApiaryOutput = 1f;

	// Token: 0x04006664 RID: 26212
	public static float ApiaryInput = 1f;

	// Token: 0x04006665 RID: 26213
	public static float CollectorTimeModifier = 1f;

	// Token: 0x04006666 RID: 26214
	public static int CollectorOutput = 1;

	// Token: 0x04006667 RID: 26215
	public static float MiningOutputModifier = 1f;

	// Token: 0x04006668 RID: 26216
	public static float HarvestingOutputModifier = 1f;

	// Token: 0x04006669 RID: 26217
	public static float CropOutputModifier = 1f;

	// Token: 0x0400666A RID: 26218
	public static float SeedDropOutputModifier = 1f;

	// Token: 0x0400666B RID: 26219
	public static int CraftingMaxTier = 6;

	// Token: 0x0400666C RID: 26220
	public static bool CraftingProgression = true;

	// Token: 0x0400666D RID: 26221
	public static BackpackCraftingOptions BackpackCrafting = BackpackCraftingOptions.Enabled;

	// Token: 0x0400666E RID: 26222
	public static bool WorkstationCrafting = true;

	// Token: 0x04006670 RID: 26224
	[PublicizedFrom(EAccessModifier.Private)]
	public static Quest sPreviouslyTrackedQuest = null;

	// Token: 0x04006671 RID: 26225
	[PublicizedFrom(EAccessModifier.Private)]
	public static Challenge sPreviouslyTrackedChallenge = null;

	// Token: 0x04006672 RID: 26226
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> SandboxIgnoreTag = FastTags<TagGroup.Global>.Parse("sandboxIgnore");

	// Token: 0x04006673 RID: 26227
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> workbenchTag = FastTags<TagGroup.Global>.Parse("workbench");

	// Token: 0x04006674 RID: 26228
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe trackedRecipe;

	// Token: 0x04006675 RID: 26229
	public int TrackedRecipeQuality = 1;

	// Token: 0x04006676 RID: 26230
	public int TrackedRecipeCount = 1;

	// Token: 0x04006677 RID: 26231
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> workbenchOnlyTag = FastTags<TagGroup.Global>.Parse("isworkbench");

	// Token: 0x04006678 RID: 26232
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> biomeprogressionTag = FastTags<TagGroup.Global>.Parse("biomeProgression");

	// Token: 0x04006679 RID: 26233
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> useSmelterTag = FastTags<TagGroup.Global>.Parse("use_smelter");

	// Token: 0x0400667A RID: 26234
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> replaceSmelterTag = FastTags<TagGroup.Global>.Parse("replace_smelter");
}
