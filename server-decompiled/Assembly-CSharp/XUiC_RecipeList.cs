using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200100D RID: 4109
[Preserve]
public class XUiC_RecipeList : XUiController
{
	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x060081B0 RID: 33200 RVA: 0x00313877 File Offset: 0x00311A77
	// (set) Token: 0x060081B1 RID: 33201 RVA: 0x00313880 File Offset: 0x00311A80
	public string Workstation
	{
		get
		{
			return this.workStation;
		}
		set
		{
			this.workStation = value;
			Block blockByName = Block.GetBlockByName(this.workStation, false);
			if (blockByName != null && blockByName.Properties.Contains("Workstation", "CraftingAreaRecipes"))
			{
				string @string = blockByName.Properties.GetString("Workstation", "CraftingAreaRecipes");
				this.craftingArea = new string[] { @string };
				if (@string.Contains(","))
				{
					this.craftingArea = @string.Replace("player", "").Replace(", ", ",").Replace(" ,", ",")
						.Replace(" , ", ",")
						.Split(',', StringSplitOptions.None);
				}
			}
			else
			{
				this.craftingArea = new string[] { this.workStation };
			}
			this.GetRecipeData();
			this.IsDirty = true;
		}
	}

	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x060081B2 RID: 33202 RVA: 0x00313963 File Offset: 0x00311B63
	// (set) Token: 0x060081B3 RID: 33203 RVA: 0x0031396B File Offset: 0x00311B6B
	public Recipe CurrentRecipe
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x060081B4 RID: 33204 RVA: 0x00313974 File Offset: 0x00311B74
	// (set) Token: 0x060081B5 RID: 33205 RVA: 0x0031397C File Offset: 0x00311B7C
	public XUiC_RecipeCraftCount CraftCount { get; set; }

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x060081B6 RID: 33206 RVA: 0x00313985 File Offset: 0x00311B85
	// (set) Token: 0x060081B7 RID: 33207 RVA: 0x00313990 File Offset: 0x00311B90
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetPage(this.page);
				}
				if (this.PageNumberChanged != null)
				{
					this.PageNumberChanged(this.page);
				}
				this.IsDirty = true;
				this.pageChanged = true;
				this.CurrentRecipe = null;
			}
		}
	}

	// Token: 0x17000F1D RID: 3869
	// (get) Token: 0x060081B8 RID: 33208 RVA: 0x003139F2 File Offset: 0x00311BF2
	// (set) Token: 0x060081B9 RID: 33209 RVA: 0x003139FA File Offset: 0x00311BFA
	public XUiC_CraftingInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x060081BA RID: 33210 RVA: 0x00313A03 File Offset: 0x00311C03
	// (set) Token: 0x060081BB RID: 33211 RVA: 0x00313A0C File Offset: 0x00311C0C
	public XUiC_RecipeEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = true;
				this.InfoWindow.ViewComponent.IsVisible = true;
				this.RecipeChanged(this.selectedEntry.Recipe, this.selectedEntry);
				this.InfoWindow.SetRecipe(this.selectedEntry);
				this.CurrentRecipe = this.selectedEntry.Recipe;
			}
			else
			{
				this.InfoWindow.SetRecipe(null);
			}
			this.IsDirty = true;
			this.pageChanged = true;
		}
	}

	// Token: 0x140000D9 RID: 217
	// (add) Token: 0x060081BC RID: 33212 RVA: 0x00313AB4 File Offset: 0x00311CB4
	// (remove) Token: 0x060081BD RID: 33213 RVA: 0x00313AEC File Offset: 0x00311CEC
	public event XUiEvent_RecipeChangedEventHandler RecipeChanged;

	// Token: 0x140000DA RID: 218
	// (add) Token: 0x060081BE RID: 33214 RVA: 0x00313B24 File Offset: 0x00311D24
	// (remove) Token: 0x060081BF RID: 33215 RVA: 0x00313B5C File Offset: 0x00311D5C
	public event XUiEvent_PageNumberChangedEventHandler PageNumberChanged;

	// Token: 0x060081C0 RID: 33216 RVA: 0x00313B94 File Offset: 0x00311D94
	public override void Init()
	{
		base.Init();
		this.windowGroup.Controller.GetChildByType<XUiC_CategoryList>().CategoryChanged += this.HandleCategoryChanged;
		this.pager = base.Parent.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.Page = this.pager.CurrentPageNumber;
			};
		}
		this.recipeControls = base.GetChildrenByType<XUiC_RecipeEntry>(null);
		for (int i = 0; i < this.recipeControls.Length; i++)
		{
			XUiC_RecipeEntry xuiC_RecipeEntry = this.recipeControls[i];
			xuiC_RecipeEntry.OnScroll += this.HandleOnScroll;
			xuiC_RecipeEntry.OnPress += this.OnPressRecipe;
			xuiC_RecipeEntry.RecipeList = this;
		}
		this.parent.OnScroll += this.HandleOnScroll;
		XUiController childById = base.Parent.GetChildById("favorites");
		childById.OnPress += this.HandleFavoritesChanged;
		this.favorites = (XUiV_Button)childById.ViewComponent;
		XUiV_Grid xuiV_Grid = (XUiV_Grid)base.ViewComponent;
		if (xuiV_Grid != null)
		{
			this.length = xuiV_Grid.Columns * xuiV_Grid.Rows;
		}
		this.txtInput = (XUiC_TextInput)this.windowGroup.Controller.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangeHandler;
			this.txtInput.OnSubmitHandler += this.HandleOnSubmitHandler;
		}
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081C1 RID: 33217 RVA: 0x00313D22 File Offset: 0x00311F22
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFavoritesChanged(XUiController _sender, int _mouseButton)
	{
		this.showFavorites = !this.showFavorites;
		this.favorites.Selected = this.showFavorites;
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081C2 RID: 33218 RVA: 0x00313D5F File Offset: 0x00311F5F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x060081C3 RID: 33219 RVA: 0x00313D8C File Offset: 0x00311F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnSubmitHandler(XUiController _sender, string _text)
	{
		this.GetRecipeData();
	}

	// Token: 0x060081C4 RID: 33220 RVA: 0x00313D8C File Offset: 0x00311F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.GetRecipeData();
	}

	// Token: 0x060081C5 RID: 33221 RVA: 0x00313D94 File Offset: 0x00311F94
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressRecipe(XUiController _sender, int _mouseButton)
	{
		XUiC_RecipeEntry xuiC_RecipeEntry = _sender as XUiC_RecipeEntry;
		if (xuiC_RecipeEntry != null && this.RecipeChanged != null)
		{
			this.SelectedEntry = xuiC_RecipeEntry;
			if (InputUtils.ShiftKeyPressed)
			{
				this.CraftCount.SetToMaxCount();
			}
		}
	}

	// Token: 0x060081C6 RID: 33222 RVA: 0x00313DCC File Offset: 0x00311FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.SetCategory(_categoryEntry.CategoryName);
		this.IsDirty = true;
	}

	// Token: 0x060081C7 RID: 33223 RVA: 0x00313DE1 File Offset: 0x00311FE1
	public void SetCategory(string _category)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.category = _category;
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081C8 RID: 33224 RVA: 0x00313E1D File Offset: 0x0031201D
	public string GetCategory()
	{
		return this.category;
	}

	// Token: 0x060081C9 RID: 33225 RVA: 0x00313D8C File Offset: 0x00311F8C
	public void RefreshRecipes()
	{
		this.GetRecipeData();
	}

	// Token: 0x060081CA RID: 33226 RVA: 0x00313E25 File Offset: 0x00312025
	public void RefreshCurrentRecipes()
	{
		this.IsDirty = true;
		this.pageChanged = true;
		if (this.showFavorites)
		{
			CraftingManager.GetFavoriteRecipesFromList(ref this.recipes);
		}
	}

	// Token: 0x060081CB RID: 33227 RVA: 0x00313E48 File Offset: 0x00312048
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetRecipeData()
	{
		ReadOnlyCollection<Recipe> readOnlyCollection = XUiM_Recipes.GetRecipes();
		List<string> questRecipes = this.xui.playerUI.entityPlayer.QuestJournal.GetQuestRecipes();
		List<Recipe> list = ((this.xui.QuestTracker.TrackedChallenge != null) ? this.xui.QuestTracker.TrackedChallenge.CraftedRecipes() : null);
		if (questRecipes.Count > 0 || (list != null && list.Count > 0))
		{
			for (int i = 0; i < readOnlyCollection.Count; i++)
			{
				if (list != null && list.Contains(readOnlyCollection[i]))
				{
					readOnlyCollection[i].isChallenge = true;
					readOnlyCollection[i].isQuest = false;
				}
				else if (questRecipes.Contains(readOnlyCollection[i].GetName()))
				{
					readOnlyCollection[i].isQuest = true;
					readOnlyCollection[i].isChallenge = false;
				}
				else
				{
					readOnlyCollection[i].isQuest = false;
					readOnlyCollection[i].isChallenge = false;
				}
			}
		}
		else
		{
			for (int j = 0; j < readOnlyCollection.Count; j++)
			{
				readOnlyCollection[j].isQuest = false;
				readOnlyCollection[j].isChallenge = false;
			}
		}
		if (this.txtInput != null && this.txtInput.Text.Length > 0)
		{
			this.recipes = XUiM_Recipes.FilterRecipesByName(this.txtInput.Text, XUiM_Recipes.GetRecipes());
		}
		else
		{
			this.recipes = XUiM_Recipes.FilterRecipesByWorkstation(this.workStation, readOnlyCollection);
			if (this.showFavorites)
			{
				CraftingManager.GetFavoriteRecipesFromList(ref this.recipes);
			}
			else if (this.category != "")
			{
				XUiM_Recipes.FilterRecipesByCategory(this.category, ref this.recipes);
			}
		}
		this.Page = 0;
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081CC RID: 33228 RVA: 0x0031401C File Offset: 0x0031221C
	public void SetRecipeDataByIngredientStack(ItemStack stack)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.CurrentRecipe = null;
		this.recipes = XUiM_Recipes.FilterRecipesByIngredient(stack, XUiM_Recipes.GetRecipes());
		this.Page = 0;
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081CD RID: 33229 RVA: 0x00314078 File Offset: 0x00312278
	public void SetRecipeDataByItems(List<int> items)
	{
		if (items.Count == 0)
		{
			return;
		}
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.CurrentRecipe = null;
		this.recipes = XUiM_Recipes.FilterRecipesByItem(items, XUiM_Recipes.GetRecipes());
		this.Page = 0;
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081CE RID: 33230 RVA: 0x003140DC File Offset: 0x003122DC
	public void SetRecipeDataByItem(int itemID)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.CurrentRecipe = null;
		this.recipes = XUiM_Recipes.FilterRecipesByID(itemID, XUiM_Recipes.GetRecipes());
		if (this.recipes != null && this.recipes.Count > 0)
		{
			this.CurrentRecipe = this.recipes[0];
		}
		this.Page = 0;
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081CF RID: 33231 RVA: 0x00314160 File Offset: 0x00312360
	public override void Update(float _dt)
	{
		if (this.IsDirty && this.xui.PlayerInventory != null)
		{
			this.FindShowingWindow();
			if (this.resortRecipes)
			{
				List<ItemStack> list = this.updateStackList;
				list.Clear();
				list.AddRange(this.xui.PlayerInventory.GetBackpackItemStacks());
				list.AddRange(this.xui.PlayerInventory.GetToolbeltItemStacks());
				XUiC_WorkstationInputGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
				if (childByType != null && (!XUiM_Recipes.DisableSmelter || !(childByType is XUiC_WorkstationMaterialInputGrid)))
				{
					list.Clear();
					list.AddRange(childByType.GetSlots());
				}
				this.BuildRecipeInfosList(list);
				this.recipeInfos.Sort(new Comparison<XUiC_RecipeList.RecipeInfo>(this.CompareRecipeInfos));
				this.UpdateRecipes();
				this.resortRecipes = false;
			}
			if (this.pageChanged)
			{
				for (int i = 0; i < this.length; i++)
				{
					int num = i + this.length * this.page;
					XUiC_RecipeEntry xuiC_RecipeEntry = ((i < this.recipeControls.Length) ? this.recipeControls[i] : null);
					if (xuiC_RecipeEntry != null)
					{
						if (num < this.recipeInfos.Count)
						{
							XUiC_RecipeList.RecipeInfo recipeInfo = this.recipeInfos[num];
							xuiC_RecipeEntry.SetRecipeAndHasIngredients(recipeInfo.recipe, recipeInfo.hasIngredients);
							xuiC_RecipeEntry.ViewComponent.Enabled = true;
						}
						else
						{
							xuiC_RecipeEntry.SetRecipeAndHasIngredients(null, false);
							xuiC_RecipeEntry.ViewComponent.Enabled = false;
							if (xuiC_RecipeEntry.IsSelected)
							{
								xuiC_RecipeEntry.IsSelected = false;
							}
						}
						if (this.CurrentRecipe != null && this.CurrentRecipe == xuiC_RecipeEntry.Recipe && this.SelectedEntry != xuiC_RecipeEntry)
						{
							this.SelectedEntry = xuiC_RecipeEntry;
							this.CraftCount.IsDirty = true;
						}
						if (this.SelectedEntry != null && this.SelectedEntry.Recipe != this.CurrentRecipe)
						{
							this.ClearSelection();
						}
					}
				}
				this.pageChanged = false;
			}
			if (this.pager != null)
			{
				this.pager.SetLastPageByElementsAndPageLength(this.recipeInfos.Count, this.recipeControls.Length);
				this.pager.CurrentPageNumber = this.page;
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
		if (this.xui.playerUI.playerInput.GUIActions.Inspect.WasPressed && this.xui.playerUI.CursorController.navigationTarget != null)
		{
			this.OnPressRecipe(this.xui.playerUI.CursorController.navigationTarget.Controller, 0);
		}
	}

	// Token: 0x060081D0 RID: 33232 RVA: 0x003143E8 File Offset: 0x003125E8
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
		}
		this.xui.playerUI.entityPlayer.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.playerUI.entityPlayer.QuestRemoved += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedChallengeChanged += this.QuestTracker_OnTrackedChallengeChanged;
		this.xui.Recipes.OnTrackedRecipeChanged += this.Recipes_OnTrackedRecipeChanged;
		XUiC_WorkstationMaterialInputWindow childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputWindow>();
		if (childByType != null)
		{
			childByType.OnWorkstationMaterialWeightsChanged += this.WorkstationMaterial_OnWeightsChanged;
		}
		XUiC_WorkstationFuelGrid childByType2 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationFuelGrid>();
		if (childByType2 != null)
		{
			childByType2.OnWorkstationFuelChanged += this.WorkStation_OnToolsOrFuelChanged;
		}
		XUiC_WorkstationToolGrid childByType3 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
		if (childByType3 != null)
		{
			childByType3.OnWorkstationToolsChanged += this.WorkStation_OnToolsOrFuelChanged;
		}
		this.ClearSelection();
		if (this.xui.playerUI.entityPlayer.QuestJournal.HasCraftingQuest() && (this.txtInput == null || this.txtInput.Text == ""))
		{
			this.GetRecipeData();
			this.pageChanged = true;
		}
		if (this.xui.QuestTracker.TrackedChallenge != null)
		{
			this.GetRecipeData();
			this.pageChanged = true;
		}
		this.IsDirty = true;
		this.resortRecipes = true;
	}

	// Token: 0x060081D1 RID: 33233 RVA: 0x003145C4 File Offset: 0x003127C4
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
		this.xui.playerUI.entityPlayer.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.playerUI.entityPlayer.QuestRemoved -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedChallengeChanged -= this.QuestTracker_OnTrackedChallengeChanged;
		this.xui.Recipes.OnTrackedRecipeChanged -= this.Recipes_OnTrackedRecipeChanged;
		XUiC_WorkstationMaterialInputWindow childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputWindow>();
		if (childByType != null)
		{
			childByType.OnWorkstationMaterialWeightsChanged -= this.WorkstationMaterial_OnWeightsChanged;
		}
		XUiC_WorkstationFuelGrid childByType2 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationFuelGrid>();
		if (childByType2 != null)
		{
			childByType2.OnWorkstationFuelChanged -= this.WorkStation_OnToolsOrFuelChanged;
		}
		XUiC_WorkstationToolGrid childByType3 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
		if (childByType3 != null)
		{
			childByType3.OnWorkstationToolsChanged -= this.WorkStation_OnToolsOrFuelChanged;
		}
		this.SelectedEntry = null;
	}

	// Token: 0x060081D2 RID: 33234 RVA: 0x0031471D File Offset: 0x0031291D
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorkStation_OnToolsOrFuelChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D3 RID: 33235 RVA: 0x0031471D File Offset: 0x0031291D
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D4 RID: 33236 RVA: 0x0031471D File Offset: 0x0031291D
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D5 RID: 33237 RVA: 0x00314734 File Offset: 0x00312934
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D6 RID: 33238 RVA: 0x00314734 File Offset: 0x00312934
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedChallengeChanged()
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D7 RID: 33239 RVA: 0x0031471D File Offset: 0x0031291D
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorkstationMaterial_OnWeightsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D8 RID: 33240 RVA: 0x00314734 File Offset: 0x00312934
	[PublicizedFrom(EAccessModifier.Private)]
	public void Recipes_OnTrackedRecipeChanged()
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081D9 RID: 33241 RVA: 0x00314751 File Offset: 0x00312951
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x060081DA RID: 33242 RVA: 0x0031475C File Offset: 0x0031295C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildRecipeInfosList(List<ItemStack> _items)
	{
		this.recipeInfos.Clear();
		for (int i = 0; i < this.recipes.Count; i++)
		{
			XUiC_RecipeList.RecipeInfo recipeInfo;
			recipeInfo.recipe = this.recipes[i];
			recipeInfo.unlocked = XUiM_Recipes.GetRecipeIsUnlocked(this.xui, recipeInfo.recipe);
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			bool flag = XUiM_Recipes.HasIngredientsForRecipe(_items, recipeInfo.recipe, entityPlayer);
			recipeInfo.hasIngredients = flag && (this.craftingWindow == null || this.craftingWindow.CraftingRequirementsValid(recipeInfo.recipe));
			recipeInfo.name = Localization.Get(recipeInfo.recipe.GetName(), false, null);
			this.recipeInfos.Add(recipeInfo);
		}
	}

	// Token: 0x060081DB RID: 33243 RVA: 0x00314828 File Offset: 0x00312A28
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRecipes()
	{
		this.recipes.Clear();
		for (int i = 0; i < this.recipeInfos.Count; i++)
		{
			this.recipes.Add(this.recipeInfos[i].recipe);
		}
	}

	// Token: 0x060081DC RID: 33244 RVA: 0x00314874 File Offset: 0x00312A74
	[PublicizedFrom(EAccessModifier.Private)]
	public int CompareRecipeInfos(XUiC_RecipeList.RecipeInfo lhs, XUiC_RecipeList.RecipeInfo rhs)
	{
		if (lhs.recipe.IsTracked != rhs.recipe.IsTracked)
		{
			if (!lhs.recipe.IsTracked)
			{
				return 1;
			}
			return -1;
		}
		else if (lhs.recipe.isChallenge != rhs.recipe.isChallenge)
		{
			if (!lhs.recipe.isChallenge)
			{
				return 1;
			}
			return -1;
		}
		else if (lhs.recipe.isQuest != rhs.recipe.isQuest)
		{
			if (!lhs.recipe.isQuest)
			{
				return 1;
			}
			return -1;
		}
		else if (lhs.unlocked != rhs.unlocked)
		{
			if (!lhs.unlocked)
			{
				return 1;
			}
			return -1;
		}
		else if (lhs.hasIngredients != rhs.hasIngredients)
		{
			if (!lhs.hasIngredients)
			{
				return 1;
			}
			return -1;
		}
		else
		{
			if (!(lhs.name == rhs.name))
			{
				return string.Compare(lhs.name, rhs.name, StringComparison.Ordinal);
			}
			if (lhs.recipe.count > rhs.recipe.count)
			{
				return 1;
			}
			if (lhs.recipe.count < rhs.recipe.count)
			{
				return -1;
			}
			if (lhs.recipe.itemValueType > rhs.recipe.itemValueType)
			{
				return 1;
			}
			if (lhs.recipe.itemValueType < rhs.recipe.itemValueType)
			{
				return -1;
			}
			return this.CompareRecipeIngredients(lhs.recipe.ingredients, rhs.recipe.ingredients);
		}
	}

	// Token: 0x060081DD RID: 33245 RVA: 0x003149E0 File Offset: 0x00312BE0
	[PublicizedFrom(EAccessModifier.Private)]
	public int CompareRecipeIngredients(List<ItemStack> lhs, List<ItemStack> rhs)
	{
		if (lhs.Count > rhs.Count)
		{
			return 1;
		}
		if (lhs.Count < rhs.Count)
		{
			return -1;
		}
		for (int i = 0; i < lhs.Count; i++)
		{
			int itemId = lhs[i].itemValue.GetItemId();
			int itemId2 = rhs[i].itemValue.GetItemId();
			if (itemId > itemId2)
			{
				return 1;
			}
			if (itemId < itemId2)
			{
				return -1;
			}
			if (lhs[i].count > rhs[i].count)
			{
				return 1;
			}
			if (lhs[i].count < rhs[i].count)
			{
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x060081DE RID: 33246 RVA: 0x00314A88 File Offset: 0x00312C88
	[PublicizedFrom(EAccessModifier.Private)]
	public void FindShowingWindow()
	{
		List<XUiC_CraftingWindowGroup> childrenByType = this.xui.GetChildrenByType<XUiC_CraftingWindowGroup>();
		for (int i = 0; i < childrenByType.Count; i++)
		{
			if (childrenByType[i].WindowGroup != null && childrenByType[i].WindowGroup.isShowing)
			{
				this.craftingWindow = childrenByType[i];
				return;
			}
		}
	}

	// Token: 0x04006019 RID: 24601
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeList.RecipeInfo> recipeInfos = new List<XUiC_RecipeList.RecipeInfo>();

	// Token: 0x0400601A RID: 24602
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry[] recipeControls;

	// Token: 0x0400601B RID: 24603
	[PublicizedFrom(EAccessModifier.Private)]
	public string workStation = "";

	// Token: 0x0400601C RID: 24604
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400601D RID: 24605
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry selectedEntry;

	// Token: 0x0400601E RID: 24606
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favorites;

	// Token: 0x0400601F RID: 24607
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "Basics";

	// Token: 0x04006020 RID: 24608
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Recipe> recipes = new List<Recipe>();

	// Token: 0x04006021 RID: 24609
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006022 RID: 24610
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04006023 RID: 24611
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x04006024 RID: 24612
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006025 RID: 24613
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006026 RID: 24614
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resortRecipes;

	// Token: 0x04006027 RID: 24615
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageChanged;

	// Token: 0x04006028 RID: 24616
	public string[] craftingArea = new string[] { "" };

	// Token: 0x0400602E RID: 24622
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> updateStackList = new List<ItemStack>();

	// Token: 0x0400602F RID: 24623
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CraftingWindowGroup craftingWindow;

	// Token: 0x0200100E RID: 4110
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RecipeInfo
	{
		// Token: 0x04006030 RID: 24624
		public Recipe recipe;

		// Token: 0x04006031 RID: 24625
		public bool unlocked;

		// Token: 0x04006032 RID: 24626
		public bool hasIngredients;

		// Token: 0x04006033 RID: 24627
		public string name;
	}
}
