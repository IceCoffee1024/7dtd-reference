using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200102C RID: 4140
[Preserve]
public class XUiC_RecipeList : XUiController
{
	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x06008211 RID: 33297 RVA: 0x003185AB File Offset: 0x003167AB
	// (set) Token: 0x06008212 RID: 33298 RVA: 0x003185B4 File Offset: 0x003167B4
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

	// Token: 0x17000F4C RID: 3916
	// (get) Token: 0x06008213 RID: 33299 RVA: 0x00318697 File Offset: 0x00316897
	// (set) Token: 0x06008214 RID: 33300 RVA: 0x0031869F File Offset: 0x0031689F
	public Recipe CurrentRecipe
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F4D RID: 3917
	// (get) Token: 0x06008215 RID: 33301 RVA: 0x003186A8 File Offset: 0x003168A8
	// (set) Token: 0x06008216 RID: 33302 RVA: 0x003186B0 File Offset: 0x003168B0
	public XUiC_RecipeCraftCount CraftCount { get; set; }

	// Token: 0x17000F4E RID: 3918
	// (get) Token: 0x06008217 RID: 33303 RVA: 0x003186B9 File Offset: 0x003168B9
	// (set) Token: 0x06008218 RID: 33304 RVA: 0x003186C4 File Offset: 0x003168C4
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

	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x06008219 RID: 33305 RVA: 0x00318726 File Offset: 0x00316926
	// (set) Token: 0x0600821A RID: 33306 RVA: 0x0031872E File Offset: 0x0031692E
	public XUiC_CraftingInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x0600821B RID: 33307 RVA: 0x00318737 File Offset: 0x00316937
	// (set) Token: 0x0600821C RID: 33308 RVA: 0x00318740 File Offset: 0x00316940
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
	// (add) Token: 0x0600821D RID: 33309 RVA: 0x003187E8 File Offset: 0x003169E8
	// (remove) Token: 0x0600821E RID: 33310 RVA: 0x00318820 File Offset: 0x00316A20
	public event XUiEvent_RecipeChangedEventHandler RecipeChanged;

	// Token: 0x140000D9 RID: 217
	// (add) Token: 0x0600821F RID: 33311 RVA: 0x00318858 File Offset: 0x00316A58
	// (remove) Token: 0x06008220 RID: 33312 RVA: 0x00318890 File Offset: 0x00316A90
	public event XUiEvent_PageNumberChangedEventHandler PageNumberChanged;

	// Token: 0x06008221 RID: 33313 RVA: 0x003188C8 File Offset: 0x00316AC8
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

	// Token: 0x06008222 RID: 33314 RVA: 0x00318A56 File Offset: 0x00316C56
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

	// Token: 0x06008223 RID: 33315 RVA: 0x00318A93 File Offset: 0x00316C93
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

	// Token: 0x06008224 RID: 33316 RVA: 0x00318AC0 File Offset: 0x00316CC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnSubmitHandler(XUiController _sender, string _text)
	{
		this.GetRecipeData();
	}

	// Token: 0x06008225 RID: 33317 RVA: 0x00318AC0 File Offset: 0x00316CC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.GetRecipeData();
	}

	// Token: 0x06008226 RID: 33318 RVA: 0x00318AC8 File Offset: 0x00316CC8
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

	// Token: 0x06008227 RID: 33319 RVA: 0x00318B00 File Offset: 0x00316D00
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.SetCategory(_categoryEntry.CategoryName);
		this.IsDirty = true;
	}

	// Token: 0x06008228 RID: 33320 RVA: 0x00318B15 File Offset: 0x00316D15
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

	// Token: 0x06008229 RID: 33321 RVA: 0x00318B51 File Offset: 0x00316D51
	public string GetCategory()
	{
		return this.category;
	}

	// Token: 0x0600822A RID: 33322 RVA: 0x00318AC0 File Offset: 0x00316CC0
	public void RefreshRecipes()
	{
		this.GetRecipeData();
	}

	// Token: 0x0600822B RID: 33323 RVA: 0x00318B59 File Offset: 0x00316D59
	public void RefreshCurrentRecipes()
	{
		this.IsDirty = true;
		this.pageChanged = true;
		if (this.showFavorites)
		{
			CraftingManager.GetFavoriteRecipesFromList(ref this.recipes);
		}
	}

	// Token: 0x0600822C RID: 33324 RVA: 0x00318B7C File Offset: 0x00316D7C
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

	// Token: 0x0600822D RID: 33325 RVA: 0x00318D50 File Offset: 0x00316F50
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

	// Token: 0x0600822E RID: 33326 RVA: 0x00318DAC File Offset: 0x00316FAC
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

	// Token: 0x0600822F RID: 33327 RVA: 0x00318E10 File Offset: 0x00317010
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

	// Token: 0x06008230 RID: 33328 RVA: 0x00318E94 File Offset: 0x00317094
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

	// Token: 0x06008231 RID: 33329 RVA: 0x0031911C File Offset: 0x0031731C
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

	// Token: 0x06008232 RID: 33330 RVA: 0x003192F8 File Offset: 0x003174F8
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

	// Token: 0x06008233 RID: 33331 RVA: 0x00319451 File Offset: 0x00317651
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorkStation_OnToolsOrFuelChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x06008234 RID: 33332 RVA: 0x00319451 File Offset: 0x00317651
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x06008235 RID: 33333 RVA: 0x00319451 File Offset: 0x00317651
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x06008236 RID: 33334 RVA: 0x00319468 File Offset: 0x00317668
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x06008237 RID: 33335 RVA: 0x00319468 File Offset: 0x00317668
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedChallengeChanged()
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x06008238 RID: 33336 RVA: 0x00319451 File Offset: 0x00317651
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorkstationMaterial_OnWeightsChanged()
	{
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x06008239 RID: 33337 RVA: 0x00319468 File Offset: 0x00317668
	[PublicizedFrom(EAccessModifier.Private)]
	public void Recipes_OnTrackedRecipeChanged()
	{
		this.GetRecipeData();
		this.IsDirty = true;
		this.resortRecipes = true;
		this.pageChanged = true;
	}

	// Token: 0x0600823A RID: 33338 RVA: 0x00319485 File Offset: 0x00317685
	public void ClearSelection()
	{
		this.SelectedEntry = null;
	}

	// Token: 0x0600823B RID: 33339 RVA: 0x00319490 File Offset: 0x00317690
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

	// Token: 0x0600823C RID: 33340 RVA: 0x0031955C File Offset: 0x0031775C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRecipes()
	{
		this.recipes.Clear();
		for (int i = 0; i < this.recipeInfos.Count; i++)
		{
			this.recipes.Add(this.recipeInfos[i].recipe);
		}
	}

	// Token: 0x0600823D RID: 33341 RVA: 0x003195A8 File Offset: 0x003177A8
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

	// Token: 0x0600823E RID: 33342 RVA: 0x00319714 File Offset: 0x00317914
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

	// Token: 0x0600823F RID: 33343 RVA: 0x003197BC File Offset: 0x003179BC
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

	// Token: 0x04006080 RID: 24704
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeList.RecipeInfo> recipeInfos = new List<XUiC_RecipeList.RecipeInfo>();

	// Token: 0x04006081 RID: 24705
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry[] recipeControls;

	// Token: 0x04006082 RID: 24706
	[PublicizedFrom(EAccessModifier.Private)]
	public string workStation = "";

	// Token: 0x04006083 RID: 24707
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006084 RID: 24708
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry selectedEntry;

	// Token: 0x04006085 RID: 24709
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favorites;

	// Token: 0x04006086 RID: 24710
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "Basics";

	// Token: 0x04006087 RID: 24711
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Recipe> recipes = new List<Recipe>();

	// Token: 0x04006088 RID: 24712
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006089 RID: 24713
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400608A RID: 24714
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x0400608B RID: 24715
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400608C RID: 24716
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400608D RID: 24717
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resortRecipes;

	// Token: 0x0400608E RID: 24718
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageChanged;

	// Token: 0x0400608F RID: 24719
	public string[] craftingArea = new string[] { "" };

	// Token: 0x04006095 RID: 24725
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> updateStackList = new List<ItemStack>();

	// Token: 0x04006096 RID: 24726
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CraftingWindowGroup craftingWindow;

	// Token: 0x0200102D RID: 4141
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RecipeInfo
	{
		// Token: 0x04006097 RID: 24727
		public Recipe recipe;

		// Token: 0x04006098 RID: 24728
		public bool unlocked;

		// Token: 0x04006099 RID: 24729
		public bool hasIngredients;

		// Token: 0x0400609A RID: 24730
		public string name;
	}
}
