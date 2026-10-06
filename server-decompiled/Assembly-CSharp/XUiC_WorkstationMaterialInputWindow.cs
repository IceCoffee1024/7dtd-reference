using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010E4 RID: 4324
[Preserve]
public class XUiC_WorkstationMaterialInputWindow : XUiController
{
	// Token: 0x140000E0 RID: 224
	// (add) Token: 0x06008978 RID: 35192 RVA: 0x00348AEC File Offset: 0x00346CEC
	// (remove) Token: 0x06008979 RID: 35193 RVA: 0x00348B24 File Offset: 0x00346D24
	public event XuiEvent_WorkstationItemsChanged OnWorkstationMaterialWeightsChanged;

	// Token: 0x0600897A RID: 35194 RVA: 0x00348B5C File Offset: 0x00346D5C
	public override void Init()
	{
		base.Init();
		this.materialTitles = base.GetChildrenById("material", null);
		this.materialWeights = base.GetChildrenById("weight", null);
		this.inputGrid = base.GetChildByType<XUiC_WorkstationMaterialInputGrid>();
		if (this.inputGrid == null)
		{
			Log.Error("Input Grid not found!");
		}
		if (this.materialWeights[0] != null)
		{
			this.baseTextColor = ((XUiV_Label)this.materialWeights[0].ViewComponent).Color;
		}
	}

	// Token: 0x0600897B RID: 35195 RVA: 0x00348BD8 File Offset: 0x00346DD8
	public override void OnOpen()
	{
		base.OnOpen();
		this.MaterialNames = this.inputGrid.WorkstationData.GetMaterialNames();
		for (int i = 0; i < this.MaterialNames.Length; i++)
		{
			string text = this.MaterialNames[i].UppercaseFirst();
			if (Localization.Exists("lbl" + this.MaterialNames[i], false))
			{
				text = Localization.Get("lbl" + this.MaterialNames[i], false, null);
			}
			((XUiV_Label)this.materialTitles[i].ViewComponent).Text = text + ":";
		}
		XUiC_RecipeList childByType = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (childByType != null)
		{
			childByType.RecipeChanged += this.RecipeList_RecipeChanged;
		}
	}

	// Token: 0x0600897C RID: 35196 RVA: 0x00348C9E File Offset: 0x00346E9E
	[PublicizedFrom(EAccessModifier.Private)]
	public void onForgeValuesChanged()
	{
		if (this.OnWorkstationMaterialWeightsChanged != null)
		{
			this.OnWorkstationMaterialWeightsChanged();
		}
	}

	// Token: 0x0600897D RID: 35197 RVA: 0x00348CB4 File Offset: 0x00346EB4
	public override void OnClose()
	{
		base.OnClose();
		XUiC_RecipeList childByType = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (childByType != null)
		{
			childByType.RecipeChanged -= this.RecipeList_RecipeChanged;
		}
	}

	// Token: 0x0600897E RID: 35198 RVA: 0x00348CF0 File Offset: 0x00346EF0
	public override void Update(float _dt)
	{
		if (!this.windowGroup.isShowing)
		{
			return;
		}
		if (this.weights == null)
		{
			this.weights = new int[this.MaterialNames.Length];
			this.SetMaterialWeights(this.inputGrid.WorkstationData.GetInputStacks());
		}
		for (int i = 0; i < this.weights.Length; i++)
		{
			((XUiV_Label)this.materialWeights[i].ViewComponent).Text = string.Format("{0}", this.weights[i].ToString());
		}
		base.Update(_dt);
	}

	// Token: 0x0600897F RID: 35199 RVA: 0x00348D88 File Offset: 0x00346F88
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeList_RecipeChanged(Recipe _recipe, XUiC_RecipeEntry recipeEntry)
	{
		if (this.weights != null)
		{
			this.ResetWeightColors();
		}
	}

	// Token: 0x06008980 RID: 35200 RVA: 0x00348D98 File Offset: 0x00346F98
	public bool HasRequirement(Recipe recipe)
	{
		if (this.weights == null)
		{
			return true;
		}
		if (recipe == null)
		{
			this.ResetWeightColors();
			return true;
		}
		for (int i = 0; i < this.weights.Length; i++)
		{
			((XUiV_Label)this.materialWeights[i].ViewComponent).Color = this.baseTextColor;
			for (int j = 0; j < recipe.ingredients.Count; j++)
			{
				int num = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)recipe.ingredients[j].count, this.xui.playerUI.entityPlayer, recipe, FastTags<TagGroup.Global>.Parse(recipe.ingredients[j].itemValue.ItemClass.GetItemName()), true, true, true, true, true, recipe.GetCraftingTier(this.xui.playerUI.entityPlayer), true, false);
				int type = recipe.ingredients[j].itemValue.type;
				if (num > 0)
				{
					num = (int)((float)num * XUiM_Recipes.GetCraftingInputModifier(recipe));
					if (XUiM_Recipes.CraftingInputModifier > 0f)
					{
						num = Utils.FastMax(1, num);
					}
				}
				ItemClass forId = ItemClass.GetForId(type);
				if (forId != null)
				{
					if (forId.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(this.MaterialNames[i]))
					{
						if (num <= this.weights[i])
						{
							((XUiV_Label)this.materialWeights[i].ViewComponent).Color = Color.green;
							break;
						}
						((XUiV_Label)this.materialWeights[i].ViewComponent).Color = Color.red;
						break;
					}
					else
					{
						((XUiV_Label)this.materialWeights[i].ViewComponent).Color = this.baseTextColor;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06008981 RID: 35201 RVA: 0x00348F3C File Offset: 0x0034713C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetWeightColors()
	{
		for (int i = 0; i < this.weights.Length; i++)
		{
			((XUiV_Label)this.materialWeights[i].ViewComponent).Color = this.baseTextColor;
		}
	}

	// Token: 0x06008982 RID: 35202 RVA: 0x00348F7C File Offset: 0x0034717C
	public void SetMaterialWeights(ItemStack[] stackList)
	{
		for (int i = 3; i < stackList.Length; i++)
		{
			if (this.weights != null && stackList[i] != null)
			{
				this.weights[i - 3] = stackList[i].count;
			}
		}
		this.onForgeValuesChanged();
	}

	// Token: 0x06008983 RID: 35203 RVA: 0x00348FBC File Offset: 0x003471BC
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (flag)
		{
			return flag;
		}
		if (name == "materials")
		{
			if (value.Contains(","))
			{
				this.MaterialNames = value.Replace(" ", "").Split(',', StringSplitOptions.None);
				this.weights = new int[this.MaterialNames.Length];
			}
			else
			{
				this.MaterialNames = new string[] { value };
			}
			return true;
		}
		if (name == "valid_materials_color")
		{
			this.validColor = StringParsers.ParseColor32(value);
			return true;
		}
		if (!(name == "invalid_materials_color"))
		{
			return false;
		}
		this.invalidColor = StringParsers.ParseColor32(value);
		return true;
	}

	// Token: 0x06008984 RID: 35204 RVA: 0x00349074 File Offset: 0x00347274
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightOunces(int materialIndex)
	{
		return (float)this.weights[materialIndex];
	}

	// Token: 0x06008985 RID: 35205 RVA: 0x0034907F File Offset: 0x0034727F
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightPounds(int materialIndex)
	{
		return this.calculateWeightOunces(materialIndex) / 16f;
	}

	// Token: 0x04006644 RID: 26180
	[PublicizedFrom(EAccessModifier.Private)]
	public const float OUNCES_IN_POUND = 16f;

	// Token: 0x04006645 RID: 26181
	public string[] MaterialNames;

	// Token: 0x04006646 RID: 26182
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] weights;

	// Token: 0x04006647 RID: 26183
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialTitles;

	// Token: 0x04006648 RID: 26184
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialWeights;

	// Token: 0x04006649 RID: 26185
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputGrid inputGrid;

	// Token: 0x0400664B RID: 26187
	[PublicizedFrom(EAccessModifier.Private)]
	public Color baseTextColor;

	// Token: 0x0400664C RID: 26188
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor = Color.green;

	// Token: 0x0400664D RID: 26189
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor = Color.red;
}
