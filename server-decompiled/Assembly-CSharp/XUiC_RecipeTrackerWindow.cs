using System;
using UnityEngine.Scripting;

// Token: 0x02001033 RID: 4147
[Preserve]
public class XUiC_RecipeTrackerWindow : XUiController
{
	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x0600828B RID: 33419 RVA: 0x0031BBD7 File Offset: 0x00319DD7
	// (set) Token: 0x0600828C RID: 33420 RVA: 0x0031BBDF File Offset: 0x00319DDF
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

	// Token: 0x0600828D RID: 33421 RVA: 0x0031BBF5 File Offset: 0x00319DF5
	public override void Init()
	{
		base.Init();
		XUiC_RecipeTrackerWindow.ID = base.WindowGroup.Id;
		this.ingredientList = base.GetChildByType<XUiC_RecipeTrackerIngredientsList>();
	}

	// Token: 0x0600828E RID: 33422 RVA: 0x0031BC1C File Offset: 0x00319E1C
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

	// Token: 0x0600828F RID: 33423 RVA: 0x0031BD10 File Offset: 0x00319F10
	public override void OnOpen()
	{
		base.OnOpen();
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.xui.Recipes.OnTrackedRecipeChanged += this.RecipeTracker_OnTrackedRecipeChanged;
		this.IsDirty = true;
	}

	// Token: 0x06008290 RID: 33424 RVA: 0x0031BD5C File Offset: 0x00319F5C
	public override void OnClose()
	{
		base.OnClose();
		if (XUi.IsGameRunning())
		{
			this.xui.Recipes.OnTrackedRecipeChanged -= this.RecipeTracker_OnTrackedRecipeChanged;
		}
	}

	// Token: 0x06008291 RID: 33425 RVA: 0x0031BD87 File Offset: 0x00319F87
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeTracker_OnTrackedRecipeChanged()
	{
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.Count = this.xui.Recipes.TrackedRecipeCount;
		this.IsDirty = true;
	}

	// Token: 0x06008292 RID: 33426 RVA: 0x0031BDBC File Offset: 0x00319FBC
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

	// Token: 0x040060D1 RID: 24785
	public static string ID = "";

	// Token: 0x040060D2 RID: 24786
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeTrackerIngredientsList ingredientList;

	// Token: 0x040060D3 RID: 24787
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x040060D4 RID: 24788
	public int Count = 1;

	// Token: 0x040060D5 RID: 24789
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe currentRecipe;

	// Token: 0x040060D6 RID: 24790
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}
