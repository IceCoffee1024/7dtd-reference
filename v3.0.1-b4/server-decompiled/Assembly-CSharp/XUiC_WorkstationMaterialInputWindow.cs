using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010BA RID: 4282
[Preserve]
public class XUiC_WorkstationMaterialInputWindow : XUiController
{
	// Token: 0x140000E0 RID: 224
	// (add) Token: 0x06008838 RID: 34872 RVA: 0x00340518 File Offset: 0x0033E718
	// (remove) Token: 0x06008839 RID: 34873 RVA: 0x00340550 File Offset: 0x0033E750
	public event XuiEvent_WorkstationItemsChanged OnWorkstationMaterialWeightsChanged;

	// Token: 0x0600883A RID: 34874 RVA: 0x00340588 File Offset: 0x0033E788
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

	// Token: 0x0600883B RID: 34875 RVA: 0x00340604 File Offset: 0x0033E804
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

	// Token: 0x0600883C RID: 34876 RVA: 0x003406CA File Offset: 0x0033E8CA
	[PublicizedFrom(EAccessModifier.Private)]
	public void onForgeValuesChanged()
	{
		if (this.OnWorkstationMaterialWeightsChanged != null)
		{
			this.OnWorkstationMaterialWeightsChanged();
		}
	}

	// Token: 0x0600883D RID: 34877 RVA: 0x003406E0 File Offset: 0x0033E8E0
	public override void OnClose()
	{
		base.OnClose();
		XUiC_RecipeList childByType = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (childByType != null)
		{
			childByType.RecipeChanged -= this.RecipeList_RecipeChanged;
		}
	}

	// Token: 0x0600883E RID: 34878 RVA: 0x0034071C File Offset: 0x0033E91C
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

	// Token: 0x0600883F RID: 34879 RVA: 0x003407B4 File Offset: 0x0033E9B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecipeList_RecipeChanged(Recipe _recipe, XUiC_RecipeEntry recipeEntry)
	{
		if (this.weights != null)
		{
			this.ResetWeightColors();
		}
	}

	// Token: 0x06008840 RID: 34880 RVA: 0x003407C4 File Offset: 0x0033E9C4
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

	// Token: 0x06008841 RID: 34881 RVA: 0x00340968 File Offset: 0x0033EB68
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetWeightColors()
	{
		for (int i = 0; i < this.weights.Length; i++)
		{
			((XUiV_Label)this.materialWeights[i].ViewComponent).Color = this.baseTextColor;
		}
	}

	// Token: 0x06008842 RID: 34882 RVA: 0x003409A8 File Offset: 0x0033EBA8
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

	// Token: 0x06008843 RID: 34883 RVA: 0x003409E8 File Offset: 0x0033EBE8
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

	// Token: 0x06008844 RID: 34884 RVA: 0x00340AA0 File Offset: 0x0033ECA0
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightOunces(int materialIndex)
	{
		return (float)this.weights[materialIndex];
	}

	// Token: 0x06008845 RID: 34885 RVA: 0x00340AAB File Offset: 0x0033ECAB
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculateWeightPounds(int materialIndex)
	{
		return this.calculateWeightOunces(materialIndex) / 16f;
	}

	// Token: 0x0400653B RID: 25915
	[PublicizedFrom(EAccessModifier.Private)]
	public const float OUNCES_IN_POUND = 16f;

	// Token: 0x0400653C RID: 25916
	public string[] MaterialNames;

	// Token: 0x0400653D RID: 25917
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] weights;

	// Token: 0x0400653E RID: 25918
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialTitles;

	// Token: 0x0400653F RID: 25919
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] materialWeights;

	// Token: 0x04006540 RID: 25920
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputGrid inputGrid;

	// Token: 0x04006542 RID: 25922
	[PublicizedFrom(EAccessModifier.Private)]
	public Color baseTextColor;

	// Token: 0x04006543 RID: 25923
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor = Color.green;

	// Token: 0x04006544 RID: 25924
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor = Color.red;
}
