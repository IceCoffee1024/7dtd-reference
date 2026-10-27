using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001032 RID: 4146
[Preserve]
public class XUiC_RecipeTrackerIngredientsList : XUiController
{
	// Token: 0x17000F5B RID: 3931
	// (get) Token: 0x0600827E RID: 33406 RVA: 0x0031B5A5 File Offset: 0x003197A5
	// (set) Token: 0x0600827F RID: 33407 RVA: 0x0031B5AD File Offset: 0x003197AD
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

	// Token: 0x17000F5C RID: 3932
	// (get) Token: 0x06008280 RID: 33408 RVA: 0x0031B5DC File Offset: 0x003197DC
	// (set) Token: 0x06008281 RID: 33409 RVA: 0x0031B5E4 File Offset: 0x003197E4
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

	// Token: 0x06008282 RID: 33410 RVA: 0x0031B5F4 File Offset: 0x003197F4
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

	// Token: 0x06008283 RID: 33411 RVA: 0x0031B639 File Offset: 0x00319839
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008284 RID: 33412 RVA: 0x0031B679 File Offset: 0x00319879
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06008285 RID: 33413 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008286 RID: 33414 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008287 RID: 33415 RVA: 0x0031B6BC File Offset: 0x003198BC
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

	// Token: 0x06008288 RID: 33416 RVA: 0x0031B90C File Offset: 0x00319B0C
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

	// Token: 0x06008289 RID: 33417 RVA: 0x0031BA14 File Offset: 0x00319C14
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

	// Token: 0x040060C1 RID: 24769
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x040060C2 RID: 24770
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier;

	// Token: 0x040060C3 RID: 24771
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x040060C4 RID: 24772
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_RecipeTrackerIngredientEntry> ingredientEntries = new List<XUiC_RecipeTrackerIngredientEntry>();

	// Token: 0x040060C5 RID: 24773
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstSetup;

	// Token: 0x040060C6 RID: 24774
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastComplete;

	// Token: 0x040060C7 RID: 24775
	public string completeIconName = "";

	// Token: 0x040060C8 RID: 24776
	public string incompleteIconName = "";

	// Token: 0x040060C9 RID: 24777
	public string completeHexColor = "FF00FF00";

	// Token: 0x040060CA RID: 24778
	public string incompleteHexColor = "FFB400";

	// Token: 0x040060CB RID: 24779
	public string warningHexColor = "FFFF00FF";

	// Token: 0x040060CC RID: 24780
	public string inactiveHexColor = "888888FF";

	// Token: 0x040060CD RID: 24781
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x040060CE RID: 24782
	public string completeColor = "0,255,0,255";

	// Token: 0x040060CF RID: 24783
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x040060D0 RID: 24784
	public string warningColor = "255,255,0,255";
}
