using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FFA RID: 4090
[Preserve]
public class XUiC_RecipeTrackerIngredientsList : XUiController
{
	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x0600812B RID: 33067 RVA: 0x00312041 File Offset: 0x00310241
	// (set) Token: 0x0600812C RID: 33068 RVA: 0x00312049 File Offset: 0x00310249
	public Recipe Recipe
	{
		get
		{
			return this.recipe;
		}
		set
		{
			this.recipe = value;
			Recipe recipe = this.recipe;
			this.selectedCraftingTier = ((recipe != null) ? recipe.craftingTier : (-1));
			this.IsDirty = true;
			this.firstSetup = true;
		}
	}

	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x0600812D RID: 33069 RVA: 0x00312078 File Offset: 0x00310278
	// (set) Token: 0x0600812E RID: 33070 RVA: 0x00312080 File Offset: 0x00310280
	public int Count
	{
		get
		{
			return this.count;
		}
		set
		{
			this.count = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x0600812F RID: 33071 RVA: 0x00312090 File Offset: 0x00310290
	public override void Init()
	{
		base.Init();
		XUiC_RecipeTrackerIngredientEntry[] childrenByType = base.GetChildrenByType<XUiC_RecipeTrackerIngredientEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			if (childrenByType[i] != null)
			{
				childrenByType[i].Owner = this;
				this.ingredientEntries.Add(childrenByType[i]);
			}
		}
	}

	// Token: 0x06008130 RID: 33072 RVA: 0x003120D5 File Offset: 0x003102D5
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008131 RID: 33073 RVA: 0x00312115 File Offset: 0x00310315
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008132 RID: 33074 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008133 RID: 33075 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008134 RID: 33076 RVA: 0x00312158 File Offset: 0x00310358
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			if (this.recipe != null)
			{
				bool flag = true;
				int num = this.ingredientEntries.Count;
				int num2 = this.recipe.ingredients.Count;
				int num3 = ((this.selectedCraftingTier == -1) ? this.recipe.GetCraftingTier(this.xui.playerUI.entityPlayer) : this.selectedCraftingTier);
				for (int i = 0; i < num; i++)
				{
					XUiC_RecipeTrackerIngredientEntry xuiC_RecipeTrackerIngredientEntry = this.ingredientEntries[i];
					ItemStack itemStack = ((i < num2) ? this.recipe.ingredients[i].Clone() : null);
					if (itemStack != null && this.recipe.UseIngredientModifier)
					{
						itemStack.count = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)itemStack.count, this.xui.playerUI.entityPlayer, this.recipe, FastTags<TagGroup.Global>.Parse(itemStack.itemValue.ItemClass.GetItemName()), true, true, true, true, true, num3, true, false);
						if (itemStack.count > 0)
						{
							itemStack.count = (int)((float)itemStack.count * XUiM_Recipes.GetCraftingInputModifier(this.recipe));
							if (XUiM_Recipes.CraftingInputModifier > 0f)
							{
								itemStack.count = Utils.FastMax(1, itemStack.count);
							}
						}
					}
					if (itemStack == null || (itemStack != null && itemStack.count > 0))
					{
						xuiC_RecipeTrackerIngredientEntry.Ingredient = itemStack;
					}
					else
					{
						xuiC_RecipeTrackerIngredientEntry.Ingredient = null;
					}
					if (xuiC_RecipeTrackerIngredientEntry.Ingredient != null && !xuiC_RecipeTrackerIngredientEntry.IsComplete)
					{
						flag = false;
					}
				}
				if (this.firstSetup)
				{
					this.lastComplete = flag;
					this.firstSetup = false;
				}
				if (flag && !this.lastComplete)
				{
					GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "ttAllIngredientsFound", Localization.Get(this.recipe.GetName(), false, null), null, null, false, false, 0f);
					this.lastComplete = flag;
					this.IsDirty = false;
					base.Update(_dt);
					return;
				}
				this.lastComplete = flag;
			}
			else
			{
				int num4 = this.ingredientEntries.Count;
				for (int j = 0; j < num4; j++)
				{
					this.ingredientEntries[j].Ingredient = null;
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06008135 RID: 33077 RVA: 0x003123A8 File Offset: 0x003105A8
	public int GetActiveIngredientCount()
	{
		if (this.recipe == null)
		{
			return 0;
		}
		int num = ((this.selectedCraftingTier == -1) ? this.recipe.GetCraftingTier(this.xui.playerUI.entityPlayer) : this.selectedCraftingTier);
		int num2 = 0;
		for (int i = 0; i < this.recipe.ingredients.Count; i++)
		{
			ItemStack itemStack = this.recipe.ingredients[i];
			if (this.recipe.UseIngredientModifier)
			{
				int num3 = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)itemStack.count, this.xui.playerUI.entityPlayer, this.recipe, FastTags<TagGroup.Global>.Parse(itemStack.itemValue.ItemClass.GetItemName()), true, true, true, true, true, num, true, false);
				if (num3 > 0)
				{
					num3 = (int)((float)num3 * XUiM_Recipes.GetCraftingInputModifier(this.recipe));
					if (XUiM_Recipes.CraftingInputModifier > 0f)
					{
						num3 = Utils.FastMax(1, num3);
					}
				}
				if (num3 > 0)
				{
					num2++;
				}
			}
		}
		return num2;
	}

	// Token: 0x06008136 RID: 33078 RVA: 0x003124B0 File Offset: 0x003106B0
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "complete_icon")
		{
			this.completeIconName = value;
			return true;
		}
		if (name == "incomplete_icon")
		{
			this.incompleteIconName = value;
			return true;
		}
		if (name == "complete_color")
		{
			Color32 color = StringParsers.ParseColor(value);
			this.completeColor = string.Format("{0},{1},{2},{3}", new object[] { color.r, color.g, color.b, color.a });
			this.completeHexColor = Utils.ColorToHex(color);
			return true;
		}
		if (!(name == "incomplete_color"))
		{
			return base.ParseAttribute(name, value);
		}
		Color32 color2 = StringParsers.ParseColor(value);
		this.incompleteColor = string.Format("{0},{1},{2},{3}", new object[] { color2.r, color2.g, color2.b, color2.a });
		this.incompleteHexColor = Utils.ColorToHex(color2);
		return true;
	}

	// Token: 0x04005F93 RID: 24467
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04005F94 RID: 24468
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier;

	// Token: 0x04005F95 RID: 24469
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x04005F96 RID: 24470
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeTrackerIngredientEntry> ingredientEntries = new List<XUiC_RecipeTrackerIngredientEntry>();

	// Token: 0x04005F97 RID: 24471
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstSetup;

	// Token: 0x04005F98 RID: 24472
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastComplete;

	// Token: 0x04005F99 RID: 24473
	public string completeIconName = "";

	// Token: 0x04005F9A RID: 24474
	public string incompleteIconName = "";

	// Token: 0x04005F9B RID: 24475
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005F9C RID: 24476
	public string incompleteHexColor = "FFB400";

	// Token: 0x04005F9D RID: 24477
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005F9E RID: 24478
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005F9F RID: 24479
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x04005FA0 RID: 24480
	public string completeColor = "0,255,0,255";

	// Token: 0x04005FA1 RID: 24481
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x04005FA2 RID: 24482
	public string warningColor = "255,255,0,255";
}
