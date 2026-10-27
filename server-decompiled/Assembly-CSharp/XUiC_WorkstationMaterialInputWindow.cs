using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010F4 RID: 4340
[Preserve]
public class XUiC_WorkstationMaterialInputWindow : XUiController
{
	// Token: 0x140000E0 RID: 224
	// (add) Token: 0x060089CB RID: 35275 RVA: 0x0034AB70 File Offset: 0x00348D70
	// (remove) Token: 0x060089CC RID: 35276 RVA: 0x0034ABA8 File Offset: 0x00348DA8
	public event XuiEvent_WorkstationItemsChanged OnWorkstationMaterialWeightsChanged;

	// Token: 0x060089CD RID: 35277 RVA: 0x0034ABE0 File Offset: 0x00348DE0
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

	// Token: 0x060089CE RID: 35278 RVA: 0x0034AC5C File Offset: 0x00348E5C
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

	// Token: 0x060089CF RID: 35279 RVA: 0x0034AD22 File Offset: 0x00348F22
	[PublicizedFrom(EAccessModifier.Private)]
	public void onForgeValuesChanged()
	{
		if (this.OnWorkstationMaterialWeightsChanged != null)
		{
			this.OnWorkstationMaterialWeightsChanged();
		}
	}

	// Token: 0x060089D0 RID: 35280 RVA: 0x0034AD38 File Offset: 0x00348F38
	public override void OnClose()
	{
		base.OnClose();
		XUiC_RecipeList childByType = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (childByType != null)
		{
			childByType.RecipeChanged -= this.RecipeList_RecipeChanged;
		}
	}

	// Token: 0x060089D1 RID: 35281 RVA: 0x0034AD74 File Offset: 0x00348F74
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

	// Token: 0x060089D2 RID: 35282 RVA: 0x0034AE0C File Offset: 0x0034900C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeList_RecipeChanged(Recipe _recipe, XUiC_RecipeEntry recipeEntry)
	{
		if (this.weights != null)
		{
			this.ResetWeightColors();
		}
	}

	// Token: 0x060089D3 RID: 35283 RVA: 0x0034AE1C File Offset: 0x0034901C
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

	// Token: 0x060089D4 RID: 35284 RVA: 0x0034AFC0 File Offset: 0x003491C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetWeightColors()
	{
		for (int i = 0; i < this.weights.Length; i++)
		{
			((XUiV_Label)this.materialWeights[i].ViewComponent).Color = this.baseTextColor;
		}
	}

	// Token: 0x060089D5 RID: 35285 RVA: 0x0034B000 File Offset: 0x00349200
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

	// Token: 0x060089D6 RID: 35286 RVA: 0x0034B040 File Offset: 0x00349240
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

	// Token: 0x060089D7 RID: 35287 RVA: 0x0034B0F8 File Offset: 0x003492F8
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightOunces(int materialIndex)
	{
		return (float)this.weights[materialIndex];
	}

	// Token: 0x060089D8 RID: 35288 RVA: 0x0034B103 File Offset: 0x00349303
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightPounds(int materialIndex)
	{
		return this.calculateWeightOunces(materialIndex) / 16f;
	}

	// Token: 0x04006683 RID: 26243
	[PublicizedFrom(EAccessModifier.Private)]
	public const float OUNCES_IN_POUND = 16f;

	// Token: 0x04006684 RID: 26244
	public string[] MaterialNames;

	// Token: 0x04006685 RID: 26245
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] weights;

	// Token: 0x04006686 RID: 26246
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialTitles;

	// Token: 0x04006687 RID: 26247
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialWeights;

	// Token: 0x04006688 RID: 26248
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputGrid inputGrid;

	// Token: 0x0400668A RID: 26250
	[PublicizedFrom(EAccessModifier.Private)]
	public Color baseTextColor;

	// Token: 0x0400668B RID: 26251
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor = Color.green;

	// Token: 0x0400668C RID: 26252
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor = Color.red;
}
