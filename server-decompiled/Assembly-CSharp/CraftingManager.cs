using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x020002DE RID: 734
public class CraftingManager
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060014FC RID: 5372 RVA: 0x0007F280 File Offset: 0x0007D480
	// (remove) Token: 0x060014FD RID: 5373 RVA: 0x0007F2B4 File Offset: 0x0007D4B4
	public static event CraftingManager.OnRecipeUnlocked RecipeUnlocked;

	// Token: 0x060014FF RID: 5375 RVA: 0x0007F354 File Offset: 0x0007D554
	public static void InitForNewGame()
	{
		CraftingManager.ClearAllRecipes();
		CraftingManager.UnlockedRecipeList.Clear();
		CraftingManager.AlreadyCraftedList.Clear();
		CraftingManager.FavoriteRecipeList.Clear();
		CraftingManager.craftingAreaData.Clear();
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x0007F383 File Offset: 0x0007D583
	public static void PostInit()
	{
		CraftingManager.cacheNonScrapableRecipes();
		if (XUiM_Recipes.BackpackCrafting != BackpackCraftingOptions.Enabled)
		{
			XUiM_Recipes.UpdateRecipesforBackpackCrafting();
		}
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x0007F398 File Offset: 0x0007D598
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

	// Token: 0x06001502 RID: 5378 RVA: 0x0007F3EB File Offset: 0x0007D5EB
	public static void ClearAllRecipes()
	{
		CraftingManager.recipes.Clear();
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x0007F3F7 File Offset: 0x0007D5F7
	public static void ClearLockedData()
	{
		CraftingManager.lockedRecipeNames.Clear();
		CraftingManager.lockedRecipeTypes.Clear();
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x0007F410 File Offset: 0x0007D610
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

	// Token: 0x06001505 RID: 5381 RVA: 0x0007F462 File Offset: 0x0007D662
	public static void ClearRecipe(Recipe _r)
	{
		CraftingManager.recipes.Remove(_r);
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x0007F470 File Offset: 0x0007D670
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

	// Token: 0x06001507 RID: 5383 RVA: 0x0007F4D1 File Offset: 0x0007D6D1
	public static void AddRecipe(Recipe _recipe)
	{
		CraftingManager.recipes.Add(_recipe);
		CraftingManager.bSorted = false;
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x0007F4E4 File Offset: 0x0007D6E4
	public static bool RecipeIsFavorite(Recipe _recipe)
	{
		return CraftingManager.FavoriteRecipeList.Contains(_recipe.GetName());
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x0007F4F8 File Offset: 0x0007D6F8
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

	// Token: 0x0600150A RID: 5386 RVA: 0x0007F568 File Offset: 0x0007D768
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

	// Token: 0x0600150B RID: 5387 RVA: 0x0007F5B7 File Offset: 0x0007D7B7
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

	// Token: 0x0600150C RID: 5388 RVA: 0x0007F5EC File Offset: 0x0007D7EC
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

	// Token: 0x0600150D RID: 5389 RVA: 0x0007F626 File Offset: 0x0007D826
	public static int GetLockedRecipeCount()
	{
		return CraftingManager.lockedRecipeNames.list.Count;
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x0007F637 File Offset: 0x0007D837
	public static int GetUnlockedRecipeCount()
	{
		return CraftingManager.UnlockedRecipeList.Count;
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0007F643 File Offset: 0x0007D843
	public static List<Recipe> GetRecipes()
	{
		return new List<Recipe>(CraftingManager.recipes);
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x0007F650 File Offset: 0x0007D850
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

	// Token: 0x06001511 RID: 5393 RVA: 0x0007F698 File Offset: 0x0007D898
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

	// Token: 0x06001512 RID: 5394 RVA: 0x0007F6EA File Offset: 0x0007D8EA
	public static List<Recipe> GetAllRecipes()
	{
		return CraftingManager.recipes;
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x0007F6F4 File Offset: 0x0007D8F4
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

	// Token: 0x06001514 RID: 5396 RVA: 0x0007F740 File Offset: 0x0007D940
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

	// Token: 0x06001515 RID: 5397 RVA: 0x0007F78C File Offset: 0x0007D98C
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

	// Token: 0x06001516 RID: 5398 RVA: 0x0007F7D8 File Offset: 0x0007D9D8
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

	// Token: 0x06001517 RID: 5399 RVA: 0x0007F899 File Offset: 0x0007DA99
	public static void AddWorkstationData(WorkstationData workstationData)
	{
		if (CraftingManager.craftingAreaData.ContainsKey(workstationData.WorkstationName))
		{
			CraftingManager.craftingAreaData[workstationData.WorkstationName] = workstationData;
			return;
		}
		CraftingManager.craftingAreaData.Add(workstationData.WorkstationName, workstationData);
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x0007F8D0 File Offset: 0x0007DAD0
	public static WorkstationData GetWorkstationData(string workstationName)
	{
		if (workstationName != null && CraftingManager.craftingAreaData.ContainsKey(workstationName))
		{
			return CraftingManager.craftingAreaData[workstationName];
		}
		return null;
	}

	// Token: 0x04000E04 RID: 3588
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Recipe> recipes = new List<Recipe>();

	// Token: 0x04000E05 RID: 3589
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool bSorted;

	// Token: 0x04000E06 RID: 3590
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryKeyList<string, int> lockedRecipeNames = new DictionaryKeyList<string, int>();

	// Token: 0x04000E07 RID: 3591
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<CraftingManager.RecipeLockTypes> lockedRecipeTypes = new List<CraftingManager.RecipeLockTypes>();

	// Token: 0x04000E08 RID: 3592
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, WorkstationData> craftingAreaData = new CaseInsensitiveStringDictionary<WorkstationData>();

	// Token: 0x04000E09 RID: 3593
	public static HashSet<string> UnlockedRecipeList = new HashSet<string>();

	// Token: 0x04000E0A RID: 3594
	public static HashSet<string> FavoriteRecipeList = new HashSet<string>();

	// Token: 0x04000E0B RID: 3595
	public static HashSet<string> AlreadyCraftedList = new HashSet<string>();

	// Token: 0x04000E0C RID: 3596
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Recipe> nonScrapableRecipes = new List<Recipe>();

	// Token: 0x04000E0D RID: 3597
	public static readonly ReadOnlyCollection<Recipe> NonScrapableRecipes = CraftingManager.nonScrapableRecipes.AsReadOnly();

	// Token: 0x020002DF RID: 735
	// (Invoke) Token: 0x0600151B RID: 5403
	public delegate void OnRecipeUnlocked(string recipeName);

	// Token: 0x020002E0 RID: 736
	[Flags]
	public enum RecipeLockTypes
	{
		// Token: 0x04000E0F RID: 3599
		None = 0,
		// Token: 0x04000E10 RID: 3600
		Item = 1,
		// Token: 0x04000E11 RID: 3601
		Skill = 2,
		// Token: 0x04000E12 RID: 3602
		Quest = 4
	}
}
