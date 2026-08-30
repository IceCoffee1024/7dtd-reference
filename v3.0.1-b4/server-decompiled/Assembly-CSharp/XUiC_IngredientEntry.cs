using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F33 RID: 3891
[Preserve]
public class XUiC_IngredientEntry : XUiController
{
	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x06007A39 RID: 31289 RVA: 0x002E1AEE File Offset: 0x002DFCEE
	// (set) Token: 0x06007A3A RID: 31290 RVA: 0x002E1AF6 File Offset: 0x002DFCF6
	public Recipe Recipe { get; set; }

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x06007A3B RID: 31291 RVA: 0x002E1AFF File Offset: 0x002DFCFF
	// (set) Token: 0x06007A3C RID: 31292 RVA: 0x002E1B08 File Offset: 0x002DFD08
	public ItemStack Ingredient
	{
		get
		{
			return this.ingredient;
		}
		set
		{
			this.ingredient = value;
			if (this.ingredient != null)
			{
				this.materialBased = ((XUiC_IngredientList)this.parent).Recipe.materialBasedRecipe;
				if (this.ingredient.itemValue.ItemClass != null)
				{
					this.material = this.ingredient.itemValue.ItemClass.MadeOfMaterial.ForgeCategory;
					if (this.material == null)
					{
						this.material = "";
					}
				}
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x06007A3D RID: 31293 RVA: 0x002E1B8C File Offset: 0x002DFD8C
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("icon");
		if (childById != null)
		{
			this.icoItem = childById.ViewComponent as XUiV_Sprite;
		}
		XUiController xuiController = base.GetChildById("name");
		if (xuiController != null)
		{
			this.lblName = xuiController.ViewComponent as XUiV_Label;
		}
		xuiController = base.GetChildById("havecount");
		if (xuiController != null)
		{
			this.lblHaveCount = xuiController.ViewComponent as XUiV_Label;
		}
		xuiController = base.GetChildById("needcount");
		if (xuiController != null)
		{
			this.lblNeedCount = xuiController.ViewComponent as XUiV_Label;
		}
		this.craftCountControl = this.windowGroup.Controller.GetChildByType<XUiC_RecipeCraftCount>();
		this.craftCountControl.OnCountChanged += this.HandleOnCountChanged;
		this.IsDirty = false;
	}

	// Token: 0x06007A3E RID: 31294 RVA: 0x002E1C54 File Offset: 0x002DFE54
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.ingredient != null;
		if (bindingName == "itemname")
		{
			string text = "";
			if (flag && this.materialBased)
			{
				if (Localization.Exists("lbl" + this.material, false))
				{
					text = Localization.Get("lbl" + this.material, false, null);
				}
				else
				{
					text = this.material.UppercaseFirst();
				}
			}
			value = (flag ? (this.materialBased ? text : this.ingredient.itemValue.ItemClass.GetLocalizedItemName()) : "");
			return true;
		}
		if (bindingName == "itemicon")
		{
			value = (flag ? this.ingredient.itemValue.ItemClass.GetIconName() : "");
			return true;
		}
		if (bindingName == "itemicontint")
		{
			Color32 color = Color.white;
			if (flag)
			{
				ItemClass itemClass = this.ingredient.itemValue.ItemClass;
				if (itemClass != null)
				{
					color = itemClass.GetIconTint(this.ingredient.itemValue);
				}
			}
			value = this.itemicontintcolorFormatter.Format(color);
			return true;
		}
		if (bindingName == "havecount")
		{
			XUiC_WorkstationMaterialInputGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputGrid>();
			if (childByType != null)
			{
				if (this.materialBased)
				{
					value = (flag ? this.havecountFormatter.Format(childByType.GetWeight(this.material)) : "");
				}
				else
				{
					value = (flag ? this.havecountFormatter.Format(this.xui.PlayerInventory.GetItemCount(this.ingredient.itemValue)) : "");
				}
			}
			else
			{
				XUiC_WorkstationInputGrid childByType2 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
				if (childByType2 != null)
				{
					value = (flag ? this.havecountFormatter.Format(childByType2.GetItemCount(this.ingredient.itemValue)) : "");
				}
				else
				{
					value = (flag ? this.havecountFormatter.Format(this.xui.PlayerInventory.GetItemCount(this.ingredient.itemValue)) : "");
				}
			}
			return true;
		}
		if (bindingName == "needcount")
		{
			value = (flag ? this.needcountFormatter.Format(this.ingredient.count * this.craftCountControl.Count) : "");
			return true;
		}
		if (!(bindingName == "haveneedcount"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		string text2 = (flag ? this.needcountFormatter.Format(this.ingredient.count * this.craftCountControl.Count) : "");
		XUiC_WorkstationMaterialInputGrid childByType3 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputGrid>();
		if (childByType3 != null)
		{
			if (this.materialBased)
			{
				value = (flag ? (this.havecountFormatter.Format(childByType3.GetWeight(this.material)) + "/" + text2) : "");
			}
			else
			{
				value = (flag ? (this.havecountFormatter.Format(this.xui.PlayerInventory.GetItemCount(this.ingredient.itemValue)) + "/" + text2) : "");
			}
		}
		else
		{
			XUiC_WorkstationInputGrid childByType4 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
			if (childByType4 != null)
			{
				value = (flag ? (this.havecountFormatter.Format(childByType4.GetItemCount(this.ingredient.itemValue)) + "/" + text2) : "");
			}
			else
			{
				value = (flag ? (this.havecountFormatter.Format(this.xui.PlayerInventory.GetItemCount(this.ingredient.itemValue)) + "/" + text2) : "");
			}
		}
		return true;
	}

	// Token: 0x06007A3F RID: 31295 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007A40 RID: 31296 RVA: 0x002E2032 File Offset: 0x002E0232
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			base.RefreshBindings();
			base.ViewComponent.IsVisible = true;
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005A50 RID: 23120
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x04005A51 RID: 23121
	[PublicizedFrom(EAccessModifier.Private)]
	public bool materialBased;

	// Token: 0x04005A52 RID: 23122
	[PublicizedFrom(EAccessModifier.Private)]
	public string material = "";

	// Token: 0x04005A53 RID: 23123
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoItem;

	// Token: 0x04005A54 RID: 23124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x04005A55 RID: 23125
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblHaveCount;

	// Token: 0x04005A56 RID: 23126
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblNeedCount;

	// Token: 0x04005A57 RID: 23127
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x04005A59 RID: 23129
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005A5A RID: 23130
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A5B RID: 23131
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
