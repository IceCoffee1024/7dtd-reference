using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010D3 RID: 4307
[Preserve]
public class XUiC_WorkstationMaterialInputWindow : XUiController
{
	// Token: 0x140000E1 RID: 225
	// (add) Token: 0x0600892A RID: 35114 RVA: 0x00344D3C File Offset: 0x00342F3C
	// (remove) Token: 0x0600892B RID: 35115 RVA: 0x00344D74 File Offset: 0x00342F74
	public event XuiEvent_WorkstationItemsChanged OnWorkstationMaterialWeightsChanged;

	// Token: 0x0600892C RID: 35116 RVA: 0x00344DAC File Offset: 0x00342FAC
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

	// Token: 0x0600892D RID: 35117 RVA: 0x00344E28 File Offset: 0x00343028
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

	// Token: 0x0600892E RID: 35118 RVA: 0x00344EEE File Offset: 0x003430EE
	[PublicizedFrom(EAccessModifier.Private)]
	public void onForgeValuesChanged()
	{
		if (this.OnWorkstationMaterialWeightsChanged != null)
		{
			this.OnWorkstationMaterialWeightsChanged();
		}
	}

	// Token: 0x0600892F RID: 35119 RVA: 0x00344F04 File Offset: 0x00343104
	public override void OnClose()
	{
		base.OnClose();
		XUiC_RecipeList childByType = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (childByType != null)
		{
			childByType.RecipeChanged -= this.RecipeList_RecipeChanged;
		}
	}

	// Token: 0x06008930 RID: 35120 RVA: 0x00344F40 File Offset: 0x00343140
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

	// Token: 0x06008931 RID: 35121 RVA: 0x00344FD8 File Offset: 0x003431D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeList_RecipeChanged(Recipe _recipe, XUiC_RecipeEntry recipeEntry)
	{
		if (this.weights != null)
		{
			this.ResetWeightColors();
		}
	}

	// Token: 0x06008932 RID: 35122 RVA: 0x00344FE8 File Offset: 0x003431E8
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

	// Token: 0x06008933 RID: 35123 RVA: 0x0034518C File Offset: 0x0034338C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetWeightColors()
	{
		for (int i = 0; i < this.weights.Length; i++)
		{
			((XUiV_Label)this.materialWeights[i].ViewComponent).Color = this.baseTextColor;
		}
	}

	// Token: 0x06008934 RID: 35124 RVA: 0x003451CC File Offset: 0x003433CC
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

	// Token: 0x06008935 RID: 35125 RVA: 0x0034520C File Offset: 0x0034340C
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

	// Token: 0x06008936 RID: 35126 RVA: 0x003452C4 File Offset: 0x003434C4
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightOunces(int materialIndex)
	{
		return (float)this.weights[materialIndex];
	}

	// Token: 0x06008937 RID: 35127 RVA: 0x003452CF File Offset: 0x003434CF
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightPounds(int materialIndex)
	{
		return this.calculateWeightOunces(materialIndex) / 16f;
	}

	// Token: 0x04006602 RID: 26114
	[PublicizedFrom(EAccessModifier.Private)]
	public const float OUNCES_IN_POUND = 16f;

	// Token: 0x04006603 RID: 26115
	public string[] MaterialNames;

	// Token: 0x04006604 RID: 26116
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] weights;

	// Token: 0x04006605 RID: 26117
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialTitles;

	// Token: 0x04006606 RID: 26118
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialWeights;

	// Token: 0x04006607 RID: 26119
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputGrid inputGrid;

	// Token: 0x04006609 RID: 26121
	[PublicizedFrom(EAccessModifier.Private)]
	public Color baseTextColor;

	// Token: 0x0400660A RID: 26122
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor = Color.green;

	// Token: 0x0400660B RID: 26123
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor = Color.red;
}
