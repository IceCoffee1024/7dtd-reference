using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001022 RID: 4130
[Preserve]
public class XUiC_RecipeTrackerIngredientsList : XUiController
{
	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x0600822C RID: 33324 RVA: 0x003198A1 File Offset: 0x00317AA1
	// (set) Token: 0x0600822D RID: 33325 RVA: 0x003198A9 File Offset: 0x00317AA9
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

	// Token: 0x17000F51 RID: 3921
	// (get) Token: 0x0600822E RID: 33326 RVA: 0x003198D8 File Offset: 0x00317AD8
	// (set) Token: 0x0600822F RID: 33327 RVA: 0x003198E0 File Offset: 0x00317AE0
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

	// Token: 0x06008230 RID: 33328 RVA: 0x003198F0 File Offset: 0x00317AF0
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

	// Token: 0x06008231 RID: 33329 RVA: 0x00319935 File Offset: 0x00317B35
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008232 RID: 33330 RVA: 0x00319975 File Offset: 0x00317B75
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008233 RID: 33331 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008234 RID: 33332 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008235 RID: 33333 RVA: 0x003199B8 File Offset: 0x00317BB8
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

	// Token: 0x06008236 RID: 33334 RVA: 0x00319C08 File Offset: 0x00317E08
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

	// Token: 0x06008237 RID: 33335 RVA: 0x00319D10 File Offset: 0x00317F10
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

	// Token: 0x04006083 RID: 24707
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04006084 RID: 24708
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier;

	// Token: 0x04006085 RID: 24709
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x04006086 RID: 24710
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeTrackerIngredientEntry> ingredientEntries = new List<XUiC_RecipeTrackerIngredientEntry>();

	// Token: 0x04006087 RID: 24711
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstSetup;

	// Token: 0x04006088 RID: 24712
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastComplete;

	// Token: 0x04006089 RID: 24713
	public string completeIconName = "";

	// Token: 0x0400608A RID: 24714
	public string incompleteIconName = "";

	// Token: 0x0400608B RID: 24715
	public string completeHexColor = "FF00FF00";

	// Token: 0x0400608C RID: 24716
	public string incompleteHexColor = "FFB400";

	// Token: 0x0400608D RID: 24717
	public string warningHexColor = "FFFF00FF";

	// Token: 0x0400608E RID: 24718
	public string inactiveHexColor = "888888FF";

	// Token: 0x0400608F RID: 24719
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x04006090 RID: 24720
	public string completeColor = "0,255,0,255";

	// Token: 0x04006091 RID: 24721
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x04006092 RID: 24722
	public string warningColor = "255,255,0,255";
}
