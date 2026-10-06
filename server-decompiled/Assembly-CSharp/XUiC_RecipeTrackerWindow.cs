using System;
using UnityEngine.Scripting;

// Token: 0x02001023 RID: 4131
[Preserve]
public class XUiC_RecipeTrackerWindow : XUiController
{
	// Token: 0x17000F52 RID: 3922
	// (get) Token: 0x06008239 RID: 33337 RVA: 0x00319ED3 File Offset: 0x003180D3
	// (set) Token: 0x0600823A RID: 33338 RVA: 0x00319EDB File Offset: 0x003180DB
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

	// Token: 0x0600823B RID: 33339 RVA: 0x00319EF1 File Offset: 0x003180F1
	public override void Init()
	{
		base.Init();
		XUiC_RecipeTrackerWindow.ID = base.WindowGroup.Id;
		this.ingredientList = base.GetChildByType<XUiC_RecipeTrackerIngredientsList>();
	}

	// Token: 0x0600823C RID: 33340 RVA: 0x00319F18 File Offset: 0x00318118
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

	// Token: 0x0600823D RID: 33341 RVA: 0x0031A00C File Offset: 0x0031820C
	public override void OnOpen()
	{
		base.OnOpen();
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.xui.Recipes.OnTrackedRecipeChanged += this.RecipeTracker_OnTrackedRecipeChanged;
		this.IsDirty = true;
	}

	// Token: 0x0600823E RID: 33342 RVA: 0x0031A058 File Offset: 0x00318258
	public override void OnClose()
	{
		base.OnClose();
		if (XUi.IsGameRunning())
		{
			this.xui.Recipes.OnTrackedRecipeChanged -= this.RecipeTracker_OnTrackedRecipeChanged;
		}
	}

	// Token: 0x0600823F RID: 33343 RVA: 0x0031A083 File Offset: 0x00318283
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeTracker_OnTrackedRecipeChanged()
	{
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.Count = this.xui.Recipes.TrackedRecipeCount;
		this.IsDirty = true;
	}

	// Token: 0x06008240 RID: 33344 RVA: 0x0031A0B8 File Offset: 0x003182B8
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

	// Token: 0x04006093 RID: 24723
	public static string ID = "";

	// Token: 0x04006094 RID: 24724
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeTrackerIngredientsList ingredientList;

	// Token: 0x04006095 RID: 24725
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04006096 RID: 24726
	public int Count = 1;

	// Token: 0x04006097 RID: 24727
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe currentRecipe;

	// Token: 0x04006098 RID: 24728
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}
