using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x020002D5 RID: 725
public class CraftingManager
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060014E8 RID: 5352 RVA: 0x0007E740 File Offset: 0x0007C940
	// (remove) Token: 0x060014E9 RID: 5353 RVA: 0x0007E774 File Offset: 0x0007C974
	public static event CraftingManager.OnRecipeUnlocked RecipeUnlocked;

	// Token: 0x060014EB RID: 5355 RVA: 0x0007E814 File Offset: 0x0007CA14
	public static void InitForNewGame()
	{
		CraftingManager.ClearAllRecipes();
		CraftingManager.UnlockedRecipeList.Clear();
		CraftingManager.AlreadyCraftedList.Clear();
		CraftingManager.FavoriteRecipeList.Clear();
		CraftingManager.craftingAreaData.Clear();
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x0007E843 File Offset: 0x0007CA43
	public static void PostInit()
	{
		CraftingManager.cacheNonScrapableRecipes();
		if (XUiM_Recipes.BackpackCrafting != BackpackCraftingOptions.Enabled)
		{
			XUiM_Recipes.UpdateRecipesforBackpackCrafting();
		}
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x0007E858 File Offset: 0x0007CA58
	[PublicizedFrom(EAccessModifier.Private)]
	public static void cacheNonScrapableRecipes()
	{
		CraftingManager.nonScrapableRecipes.Clear();
		for (int i = 0; i < CraftingManager.recipes.Count; i++)
		{
			if (!CraftingManager.recipes[i].wildcardForgeCategory)
			{
				CraftingManager.nonScrapableRecipes.Add(CraftingManager.recipes[i]);
			}
		}
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x0007E8AB File Offset: 0x0007CAAB
	public static void ClearAllRecipes()
	{
		CraftingManager.recipes.Clear();
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x0007E8B7 File Offset: 0x0007CAB7
	public static void ClearLockedData()
	{
		CraftingManager.lockedRecipeNames.Clear();
		CraftingManager.lockedRecipeTypes.Clear();
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x0007E8D0 File Offset: 0x0007CAD0
	public static void ClearAllGeneralRecipes()
	{
		List<Recipe> list = new List<Recipe>(CraftingManager.recipes);
		for (int i = 0; i < list.Count; i++)
		{
			Recipe recipe = list[i];
			if (recipe.craftingArea == null || recipe.craftingArea.Length == 0)
			{
				CraftingManager.recipes.Remove(recipe);
			}
		}
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x0007E922 File Offset: 0x0007CB22
	public static void ClearRecipe(Recipe _r)
	{
		CraftingManager.recipes.Remove(_r);
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x0007E930 File Offset: 0x0007CB30
	public static void ClearCraftAreaRecipes(string _craftArea, ItemValue _craftTool)
	{
		List<Recipe> list = new List<Recipe>(CraftingManager.recipes);
		for (int i = 0; i < list.Count; i++)
		{
			Recipe recipe = list[i];
			if (recipe.craftingArea != null && recipe.craftingArea.Equals(_craftArea) && recipe.craftingToolType == _craftTool.type)
			{
				CraftingManager.recipes.Remove(recipe);
			}
		}
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x0007E991 File Offset: 0x0007CB91
	public static void AddRecipe(Recipe _recipe)
	{
		CraftingManager.recipes.Add(_recipe);
		CraftingManager.bSorted = false;
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x0007E9A4 File Offset: 0x0007CBA4
	public static bool RecipeIsFavorite(Recipe _recipe)
	{
		return CraftingManager.FavoriteRecipeList.Contains(_recipe.GetName());
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x0007E9B8 File Offset: 0x0007CBB8
	public static void LockRecipe(string _recipeName, CraftingManager.RecipeLockTypes locktype = CraftingManager.RecipeLockTypes.Item)
	{
		for (int i = 0; i < CraftingManager.lockedRecipeNames.list.Count; i++)
		{
			if (CraftingManager.lockedRecipeNames.list[i].EqualsCaseInsensitive(_recipeName))
			{
				List<CraftingManager.RecipeLockTypes> list = CraftingManager.lockedRecipeTypes;
				int num = i;
				list[num] |= locktype;
				return;
			}
		}
		CraftingManager.lockedRecipeNames.Add(_recipeName, 0);
		CraftingManager.lockedRecipeTypes.Add(locktype);
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x0007EA28 File Offset: 0x0007CC28
	public static void UnlockRecipe(Recipe _recipe, EntityPlayer _entity)
	{
		CraftingManager.UnlockedRecipeList.Add(_recipe.GetName());
		if (CraftingManager.RecipeUnlocked != null)
		{
			CraftingManager.RecipeUnlocked(_recipe.GetName());
		}
		if (_entity != null)
		{
			_entity.SetCVar(_recipe.GetName(), 1f);
		}
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x0007EA77 File Offset: 0x0007CC77
	public static void UnlockRecipe(string _recipeName, EntityPlayer _entity)
	{
		CraftingManager.UnlockedRecipeList.Add(_recipeName);
		if (CraftingManager.RecipeUnlocked != null)
		{
			CraftingManager.RecipeUnlocked(_recipeName);
		}
		if (_entity != null)
		{
			_entity.SetCVar(_recipeName, 1f);
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x0007EAAC File Offset: 0x0007CCAC
	public static void ToggleFavoriteRecipe(Recipe _recipe)
	{
		string name = _recipe.GetName();
		if (CraftingManager.FavoriteRecipeList.Contains(name))
		{
			CraftingManager.FavoriteRecipeList.Remove(name);
			return;
		}
		CraftingManager.FavoriteRecipeList.Add(name);
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0007EAE6 File Offset: 0x0007CCE6
	public static int GetLockedRecipeCount()
	{
		return CraftingManager.lockedRecipeNames.list.Count;
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x0007EAF7 File Offset: 0x0007CCF7
	public static int GetUnlockedRecipeCount()
	{
		return CraftingManager.UnlockedRecipeList.Count;
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x0007EB03 File Offset: 0x0007CD03
	public static List<Recipe> GetRecipes()
	{
		return new List<Recipe>(CraftingManager.recipes);
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x0007EB10 File Offset: 0x0007CD10
	public static Recipe GetRecipe(string _itemName)
	{
		for (int i = 0; i < CraftingManager.recipes.Count; i++)
		{
			if (CraftingManager.recipes[i].GetName() == _itemName)
			{
				return CraftingManager.recipes[i];
			}
		}
		return null;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x0007EB58 File Offset: 0x0007CD58
	public static List<Recipe> GetRecipes(string _itemName)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < CraftingManager.recipes.Count; i++)
		{
			if (_itemName == CraftingManager.recipes[i].GetName())
			{
				list.Add(CraftingManager.recipes[i]);
			}
		}
		return list;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x0007EBAA File Offset: 0x0007CDAA
	public static List<Recipe> GetAllRecipes()
	{
		return CraftingManager.recipes;
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x0007EBB4 File Offset: 0x0007CDB4
	public static List<Recipe> GetNonScrapableRecipes(string _itemName)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < CraftingManager.nonScrapableRecipes.Count; i++)
		{
			Recipe recipe = CraftingManager.nonScrapableRecipes[i];
			if (recipe.GetName() == _itemName)
			{
				list.Add(recipe);
			}
		}
		return list;
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x0007EC00 File Offset: 0x0007CE00
	public static List<Recipe> GetAllRecipes(string _itemName)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < CraftingManager.recipes.Count; i++)
		{
			Recipe recipe = CraftingManager.recipes[i];
			if (recipe.GetName() == _itemName)
			{
				list.Add(recipe);
			}
		}
		return list;
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x0007EC4C File Offset: 0x0007CE4C
	public static void GetFavoriteRecipesFromList(ref List<Recipe> recipeList)
	{
		List<Recipe> list = new List<Recipe>();
		for (int i = 0; i < recipeList.Count; i++)
		{
			Recipe recipe = recipeList[i];
			if (CraftingManager.FavoriteRecipeList.Contains(recipe.GetName()))
			{
				list.Add(recipe);
			}
		}
		recipeList = list;
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x0007EC98 File Offset: 0x0007CE98
	public static Recipe GetScrapableRecipe(ItemValue _itemValue, int _count = 1)
	{
		MaterialBlock madeOfMaterial = _itemValue.ItemClass.MadeOfMaterial;
		if (madeOfMaterial == null || madeOfMaterial.ForgeCategory == null)
		{
			return null;
		}
		ItemClass itemClass = _itemValue.ItemClass;
		if (itemClass == null)
		{
			return null;
		}
		if (itemClass.NoScrapping)
		{
			return null;
		}
		for (int i = 0; i < CraftingManager.recipes.Count; i++)
		{
			Recipe recipe = CraftingManager.recipes[i];
			if (recipe.wildcardForgeCategory)
			{
				ItemClass forId = ItemClass.GetForId(recipe.itemValueType);
				MaterialBlock madeOfMaterial2 = forId.MadeOfMaterial;
				if (madeOfMaterial2 != null && madeOfMaterial2.ForgeCategory != null && recipe.itemValueType != _itemValue.type && madeOfMaterial2.ForgeCategory.Equals(madeOfMaterial.ForgeCategory) && itemClass.GetWeight() * _count >= forId.GetWeight())
				{
					return recipe;
				}
			}
		}
		return null;
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x0007ED59 File Offset: 0x0007CF59
	public static void AddWorkstationData(WorkstationData workstationData)
	{
		if (CraftingManager.craftingAreaData.ContainsKey(workstationData.WorkstationName))
		{
			CraftingManager.craftingAreaData[workstationData.WorkstationName] = workstationData;
			return;
		}
		CraftingManager.craftingAreaData.Add(workstationData.WorkstationName, workstationData);
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x0007ED90 File Offset: 0x0007CF90
	public static WorkstationData GetWorkstationData(string workstationName)
	{
		if (workstationName != null && CraftingManager.craftingAreaData.ContainsKey(workstationName))
		{
			return CraftingManager.craftingAreaData[workstationName];
		}
		return null;
	}

	// Token: 0x04000DF9 RID: 3577
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Recipe> recipes = new List<Recipe>();

	// Token: 0x04000DFA RID: 3578
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool bSorted;

	// Token: 0x04000DFB RID: 3579
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryKeyList<string, int> lockedRecipeNames = new DictionaryKeyList<string, int>();

	// Token: 0x04000DFC RID: 3580
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<CraftingManager.RecipeLockTypes> lockedRecipeTypes = new List<CraftingManager.RecipeLockTypes>();

	// Token: 0x04000DFD RID: 3581
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, WorkstationData> craftingAreaData = new CaseInsensitiveStringDictionary<WorkstationData>();

	// Token: 0x04000DFE RID: 3582
	public static HashSet<string> UnlockedRecipeList = new HashSet<string>();

	// Token: 0x04000DFF RID: 3583
	public static HashSet<string> FavoriteRecipeList = new HashSet<string>();

	// Token: 0x04000E00 RID: 3584
	public static HashSet<string> AlreadyCraftedList = new HashSet<string>();

	// Token: 0x04000E01 RID: 3585
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Recipe> nonScrapableRecipes = new List<Recipe>();

	// Token: 0x04000E02 RID: 3586
	public static readonly ReadOnlyCollection<Recipe> NonScrapableRecipes = CraftingManager.nonScrapableRecipes.AsReadOnly();

	// Token: 0x020002D6 RID: 726
	// (Invoke) Token: 0x06001507 RID: 5383
	public delegate void OnRecipeUnlocked(string recipeName);

	// Token: 0x020002D7 RID: 727
	[Flags]
	public enum RecipeLockTypes
	{
		// Token: 0x04000E04 RID: 3588
		None = 0,
		// Token: 0x04000E05 RID: 3589
		Item = 1,
		// Token: 0x04000E06 RID: 3590
		Skill = 2,
		// Token: 0x04000E07 RID: 3591
		Quest = 4
	}
}
