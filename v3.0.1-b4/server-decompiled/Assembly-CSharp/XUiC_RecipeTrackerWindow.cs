using System;
using UnityEngine.Scripting;

// Token: 0x02000FFB RID: 4091
[Preserve]
public class XUiC_RecipeTrackerWindow : XUiController
{
	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x06008138 RID: 33080 RVA: 0x00312673 File Offset: 0x00310873
	// (set) Token: 0x06008139 RID: 33081 RVA: 0x0031267B File Offset: 0x0031087B
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

	// Token: 0x0600813A RID: 33082 RVA: 0x00312691 File Offset: 0x00310891
	public override void Init()
	{
		base.Init();
		XUiC_RecipeTrackerWindow.ID = base.WindowGroup.Id;
		this.ingredientList = base.GetChildByType<XUiC_RecipeTrackerIngredientsList>();
	}

	// Token: 0x0600813B RID: 33083 RVA: 0x003126B8 File Offset: 0x003108B8
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

	// Token: 0x0600813C RID: 33084 RVA: 0x003127AC File Offset: 0x003109AC
	public override void OnOpen()
	{
		base.OnOpen();
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.xui.Recipes.OnTrackedRecipeChanged += this.RecipeTracker_OnTrackedRecipeChanged;
		this.IsDirty = true;
	}

	// Token: 0x0600813D RID: 33085 RVA: 0x003127F8 File Offset: 0x003109F8
	public override void OnClose()
	{
		base.OnClose();
		if (XUi.IsGameRunning())
		{
			this.xui.Recipes.OnTrackedRecipeChanged -= this.RecipeTracker_OnTrackedRecipeChanged;
		}
	}

	// Token: 0x0600813E RID: 33086 RVA: 0x00312823 File Offset: 0x00310A23
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeTracker_OnTrackedRecipeChanged()
	{
		this.CurrentRecipe = this.xui.Recipes.TrackedRecipe;
		this.Count = this.xui.Recipes.TrackedRecipeCount;
		this.IsDirty = true;
	}

	// Token: 0x0600813F RID: 33087 RVA: 0x00312858 File Offset: 0x00310A58
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

	// Token: 0x04005FA3 RID: 24483
	public static string ID = "";

	// Token: 0x04005FA4 RID: 24484
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeTrackerIngredientsList ingredientList;

	// Token: 0x04005FA5 RID: 24485
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04005FA6 RID: 24486
	public int Count = 1;

	// Token: 0x04005FA7 RID: 24487
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe currentRecipe;

	// Token: 0x04005FA8 RID: 24488
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}
