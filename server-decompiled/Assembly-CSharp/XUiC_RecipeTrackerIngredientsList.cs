using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001013 RID: 4115
[Preserve]
public class XUiC_RecipeTrackerIngredientsList : XUiController
{
	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x0600821D RID: 33309 RVA: 0x00316871 File Offset: 0x00314A71
	// (set) Token: 0x0600821E RID: 33310 RVA: 0x00316879 File Offset: 0x00314A79
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

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x0600821F RID: 33311 RVA: 0x003168A8 File Offset: 0x00314AA8
	// (set) Token: 0x06008220 RID: 33312 RVA: 0x003168B0 File Offset: 0x00314AB0
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

	// Token: 0x06008221 RID: 33313 RVA: 0x003168C0 File Offset: 0x00314AC0
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

	// Token: 0x06008222 RID: 33314 RVA: 0x00316905 File Offset: 0x00314B05
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008223 RID: 33315 RVA: 0x00316945 File Offset: 0x00314B45
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008224 RID: 33316 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008225 RID: 33317 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008226 RID: 33318 RVA: 0x00316988 File Offset: 0x00314B88
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

	// Token: 0x06008227 RID: 33319 RVA: 0x00316BD8 File Offset: 0x00314DD8
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

	// Token: 0x06008228 RID: 33320 RVA: 0x00316CE0 File Offset: 0x00314EE0
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

	// Token: 0x0400605A RID: 24666
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x0400605B RID: 24667
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier;

	// Token: 0x0400605C RID: 24668
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x0400605D RID: 24669
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeTrackerIngredientEntry> ingredientEntries = new List<XUiC_RecipeTrackerIngredientEntry>();

	// Token: 0x0400605E RID: 24670
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstSetup;

	// Token: 0x0400605F RID: 24671
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastComplete;

	// Token: 0x04006060 RID: 24672
	public string completeIconName = "";

	// Token: 0x04006061 RID: 24673
	public string incompleteIconName = "";

	// Token: 0x04006062 RID: 24674
	public string completeHexColor = "FF00FF00";

	// Token: 0x04006063 RID: 24675
	public string incompleteHexColor = "FFB400";

	// Token: 0x04006064 RID: 24676
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04006065 RID: 24677
	public string inactiveHexColor = "888888FF";

	// Token: 0x04006066 RID: 24678
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x04006067 RID: 24679
	public string completeColor = "0,255,0,255";

	// Token: 0x04006068 RID: 24680
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x04006069 RID: 24681
	public string warningColor = "255,255,0,255";
}
