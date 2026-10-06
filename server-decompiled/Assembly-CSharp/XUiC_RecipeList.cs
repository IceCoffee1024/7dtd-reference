using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101C RID: 4124
[Preserve]
public class XUiC_RecipeList : XUiController
{
	// Token: 0x17000F40 RID: 3904
	// (get) Token: 0x060081BF RID: 33215 RVA: 0x003168A7 File Offset: 0x00314AA7
	// (set) Token: 0x060081C0 RID: 33216 RVA: 0x003168B0 File Offset: 0x00314AB0
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

	// Token: 0x17000F41 RID: 3905
	// (get) Token: 0x060081C1 RID: 33217 RVA: 0x00316993 File Offset: 0x00314B93
	// (set) Token: 0x060081C2 RID: 33218 RVA: 0x0031699B File Offset: 0x00314B9B
	public Recipe CurrentRecipe
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F42 RID: 3906
	// (get) Token: 0x060081C3 RID: 33219 RVA: 0x003169A4 File Offset: 0x00314BA4
	// (set) Token: 0x060081C4 RID: 33220 RVA: 0x003169AC File Offset: 0x00314BAC
	public XUiC_RecipeCraftCount CraftCount { get; set; }

	// Token: 0x17000F43 RID: 3907
	// (get) Token: 0x060081C5 RID: 33221 RVA: 0x003169B5 File Offset: 0x00314BB5
	// (set) Token: 0x060081C6 RID: 33222 RVA: 0x003169C0 File Offset: 0x00314BC0
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

	// Token: 0x17000F44 RID: 3908
	// (get) Token: 0x060081C7 RID: 33223 RVA: 0x00316A22 File Offset: 0x00314C22
	// (set) Token: 0x060081C8 RID: 33224 RVA: 0x00316A2A File Offset: 0x00314C2A
	public XUiC_CraftingInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F45 RID: 3909
	// (get) Token: 0x060081C9 RID: 33225 RVA: 0x00316A33 File Offset: 0x00314C33
	// (set) Token: 0x060081CA RID: 33226 RVA: 0x00316A3C File Offset: 0x00314C3C
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

	// Token: 0x140000D8 RID: 216
	// (add) Token: 0x060081CB RID: 33227 RVA: 0x00316AE4 File Offset: 0x00314CE4
	// (remove) Token: 0x060081CC RID: 33228 RVA: 0x00316B1C File Offset: 0x00314D1C
	public event XUiEvent_RecipeChangedEventHandler RecipeChanged;

	// Token: 0x140000D9 RID: 217
	// (add) Token: 0x060081CD RID: 33229 RVA: 0x00316B54 File Offset: 0x00314D54
	// (remove) Token: 0x060081CE RID: 33230 RVA: 0x00316B8C File Offset: 0x00314D8C
	public event XUiEvent_PageNumberChangedEventHandler PageNumberChanged;

	// Token: 0x060081CF RID: 33231 RVA: 0x00316BC4 File Offset: 0x00314DC4
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

	// Token: 0x060081D0 RID: 33232 RVA: 0x00316D52 File Offset: 0x00314F52
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

	// Token: 0x060081D1 RID: 33233 RVA: 0x00316D8F File Offset: 0x00314F8F
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

	// Token: 0x060081D2 RID: 33234 RVA: 0x00316DBC File Offset: 0x00314FBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnSubmitHandler(XUiController _sender, string _text)
	{
		this.GetRecipeData();
	}

	// Token: 0x060081D3 RID: 33235 RVA: 0x00316DBC File Offset: 0x00314FBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.GetRecipeData();
	}

	// Token: 0x060081D4 RID: 33236 RVA: 0x00316DC4 File Offset: 0x00314FC4
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

	// Token: 0x060081D5 RID: 33237 RVA: 0x00316DFC File Offset: 0x00314FFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.SetCategory(_categoryEntry.CategoryName);
		this.IsDirty = true;
	}

	// Token: 0x060081D6 RID: 33238 RVA: 0x00316E11 File Offset: 0x00315011
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

	// Token: 0x060081D7 RID: 33239 RVA: 0x00316E4D File Offset: 0x0031504D
	public string GetCategory()
	{
		return this.category;
	}

	// Token: 0x060081D8 RID: 33240 RVA: 0x00316DBC File Offset: 0x00314FBC
	public void RefreshRecipes()
	{
		this.GetRecipeData();
	}

	// Token: 0x060081D9 RID: 33241 RVA: 0x00316E55 File Offset: 0x00315055
	public void RefreshCurrentRecipes()
	{
		this.IsDirty = true;
		this.pageChanged = true;
		if (this.showFavorites)
		{
			CraftingManager.GetFavoriteRecipesFromList(ref this.recipes);
		}
	}

	// Token: 0x060081DA RID: 33242 RVA: 0x00316E78 File Offset: 0x00315078
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

	// Token: 0x060081DB RID: 33243 RVA: 0x0031704C File Offset: 0x0031524C
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

	// Token: 0x060081DC RID: 33244 RVA: 0x003170A8 File Offset: 0x003152A8
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

	// Token: 0x060081DD RID: 33245 RVA: 0x0031710C File Offset: 0x0031530C
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

	// Token: 0x060081DE RID: 33246 RVA: 0x00317190 File Offset: 0x00315390
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

	// Token: 0x060081DF RID: 33247 RVA: 0x00317418 File Offset: 0x00315618
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

	// Token: 0x060081E0 RID: 33248 RVA: 0x003175F4 File Offset: 0x003157F4
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

	// Token: 0x060081E1 RID: 33249 RVA: 0x0031774D File Offset: 0x0031594D
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorkStation_OnToolsOrFuelChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E2 RID: 33250 RVA: 0x0031774D File Offset: 0x0031594D
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E3 RID: 33251 RVA: 0x0031774D File Offset: 0x0031594D
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E4 RID: 33252 RVA: 0x00317764 File Offset: 0x00315964
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E5 RID: 33253 RVA: 0x00317764 File Offset: 0x00315964
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedChallengeChanged()
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E6 RID: 33254 RVA: 0x0031774D File Offset: 0x0031594D
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorkstationMaterial_OnWeightsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E7 RID: 33255 RVA: 0x00317764 File Offset: 0x00315964
	[PublicizedFrom(EAccessModifier.Private)]
	public void Recipes_OnTrackedRecipeChanged()
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x060081E8 RID: 33256 RVA: 0x00317781 File Offset: 0x00315981
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x060081E9 RID: 33257 RVA: 0x0031778C File Offset: 0x0031598C
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

	// Token: 0x060081EA RID: 33258 RVA: 0x00317858 File Offset: 0x00315A58
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRecipes()
	{
		this.recipes.Clear();
		for (int i = 0; i < this.recipeInfos.Count; i++)
		{
			this.recipes.Add(this.recipeInfos[i].recipe);
		}
	}

	// Token: 0x060081EB RID: 33259 RVA: 0x003178A4 File Offset: 0x00315AA4
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

	// Token: 0x060081EC RID: 33260 RVA: 0x00317A10 File Offset: 0x00315C10
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

	// Token: 0x060081ED RID: 33261 RVA: 0x00317AB8 File Offset: 0x00315CB8
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

	// Token: 0x04006042 RID: 24642
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeList.RecipeInfo> recipeInfos = new List<XUiC_RecipeList.RecipeInfo>();

	// Token: 0x04006043 RID: 24643
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry[] recipeControls;

	// Token: 0x04006044 RID: 24644
	[PublicizedFrom(EAccessModifier.Private)]
	public string workStation = "";

	// Token: 0x04006045 RID: 24645
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006046 RID: 24646
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry selectedEntry;

	// Token: 0x04006047 RID: 24647
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favorites;

	// Token: 0x04006048 RID: 24648
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "Basics";

	// Token: 0x04006049 RID: 24649
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Recipe> recipes = new List<Recipe>();

	// Token: 0x0400604A RID: 24650
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400604B RID: 24651
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400604C RID: 24652
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x0400604D RID: 24653
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400604E RID: 24654
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400604F RID: 24655
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resortRecipes;

	// Token: 0x04006050 RID: 24656
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageChanged;

	// Token: 0x04006051 RID: 24657
	public string[] craftingArea = new string[] { "" };

	// Token: 0x04006057 RID: 24663
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> updateStackList = new List<ItemStack>();

	// Token: 0x04006058 RID: 24664
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CraftingWindowGroup craftingWindow;

	// Token: 0x0200101D RID: 4125
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RecipeInfo
	{
		// Token: 0x04006059 RID: 24665
		public Recipe recipe;

		// Token: 0x0400605A RID: 24666
		public bool unlocked;

		// Token: 0x0400605B RID: 24667
		public bool hasIngredients;

		// Token: 0x0400605C RID: 24668
		public string name;
	}
}
