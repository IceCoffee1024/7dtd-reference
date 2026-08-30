using System;
using UnityEngine.Scripting;

// Token: 0x02001014 RID: 4116
[Preserve]
public class XUiC_RecipeTrackerWindow : XUiController
{
	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x0600822A RID: 33322 RVA: 0x00316EA3 File Offset: 0x003150A3
	// (set) Token: 0x0600822B RID: 33323 RVA: 0x00316EAB File Offset: 0x003150AB
	public Recipe CurrentRecipe
	{
		get
		{
			return this.currentRecipe;
		}
		set
		{
			this.currentRecipe = value;
			this.IsDirty = true;
			base.RefreshBindings();
		}
	}

	// Token: 0x0600822C RID: 33324 RVA: 0x00316EC1 File Offset: 0x003150C1
	public override void Init()
	{
		base.Init();
		XUiC_RecipeTrackerWindow.ID = base.WindowGroup.Id;
		this.ingredientList = base.GetChildByType<XUiC_RecipeTrackerIngredientsList>();
	}

	// Token: 0x0600822D RID: 33325 RVA: 0x00316EE8 File Offset: 0x003150E8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		if (base.ViewComponent.IsVisible && this.localPlayer.IsDead())
		{
			this.IsDirty = true;
		}
		if (this.IsDirty)
		{
			this.ingredientList.Count = this.Count;
			if (this.currentRecipe != null)
			{
				int craftingTier = this.currentRecipe.GetCraftingTier(this.xui.playerUI.entityPlayer);
				if (this.currentRecipe.GetOutputItemClass().HasQuality)
				{
					this.currentRecipe.craftingTier = this.xui.Recipes.TrackedRecipeQuality;
				}
				else
				{
					this.currentRecipe.craftingTier = craftingTier;
				}
			}
			this.ingredientList.Recipe = this.currentRecipe;
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600822E RID: 33326 RVA: 0x00316FDC File Offset: 0x003151DC
	public override void OnOpen()
	{
		base.OnOpen();
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.xui.Recipes.OnTrackedRecipeChanged += this.RecipeTracker_OnTrackedRecipeChanged;
		this.IsDirty = true;
	}

	// Token: 0x0600822F RID: 33327 RVA: 0x00317028 File Offset: 0x00315228
	public override void OnClose()
	{
		base.OnClose();
		if (XUi.IsGameRunning())
		{
			this.xui.Recipes.OnTrackedRecipeChanged -= this.RecipeTracker_OnTrackedRecipeChanged;
		}
	}

	// Token: 0x06008230 RID: 33328 RVA: 0x00317053 File Offset: 0x00315253
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeTracker_OnTrackedRecipeChanged()
	{
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.Count = this.xui.Recipes.TrackedRecipeCount;
		this.IsDirty = true;
	}

	// Token: 0x06008231 RID: 33329 RVA: 0x00317088 File Offset: 0x00315288
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "recipename")
		{
			value = ((this.currentRecipe != null) ? Localization.Get(this.currentRecipe.GetName(), false, null) : "");
			return true;
		}
		if (bindingName == "recipetitle")
		{
			value = ((this.currentRecipe != null) ? (Localization.Get(this.currentRecipe.GetName(), false, null) + ((this.Count > 1) ? string.Format(" (x{0})", this.Count) : "")) : "");
			return true;
		}
		if (bindingName == "recipeicon")
		{
			value = ((this.currentRecipe != null) ? this.currentRecipe.GetIcon() : "");
			return true;
		}
		if (bindingName == "showrecipe")
		{
			value = (this.currentRecipe != null && XUi.IsGameRunning() && this.localPlayer != null && !this.localPlayer.IsDead()).ToString();
			return true;
		}
		if (bindingName == "showempty")
		{
			value = (this.currentRecipe == null).ToString();
			return true;
		}
		if (!(bindingName == "trackerheight"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		if (this.currentRecipe == null)
		{
			value = "0";
		}
		else
		{
			value = this.trackerheightFormatter.Format(this.ingredientList.GetActiveIngredientCount() * 35);
		}
		return true;
	}

	// Token: 0x0400606A RID: 24682
	public static string ID = "";

	// Token: 0x0400606B RID: 24683
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeTrackerIngredientsList ingredientList;

	// Token: 0x0400606C RID: 24684
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400606D RID: 24685
	public int Count = 1;

	// Token: 0x0400606E RID: 24686
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe currentRecipe;

	// Token: 0x0400606F RID: 24687
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}
