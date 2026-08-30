using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F4A RID: 3914
[Preserve]
public class XUiC_IngredientEntry : XUiController
{
	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x06007AF2 RID: 31474 RVA: 0x002E5AA2 File Offset: 0x002E3CA2
	// (set) Token: 0x06007AF3 RID: 31475 RVA: 0x002E5AAA File Offset: 0x002E3CAA
	public Recipe Recipe { get; set; }

	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x06007AF4 RID: 31476 RVA: 0x002E5AB3 File Offset: 0x002E3CB3
	// (set) Token: 0x06007AF5 RID: 31477 RVA: 0x002E5ABC File Offset: 0x002E3CBC
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

	// Token: 0x06007AF6 RID: 31478 RVA: 0x002E5B40 File Offset: 0x002E3D40
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

	// Token: 0x06007AF7 RID: 31479 RVA: 0x002E5C08 File Offset: 0x002E3E08
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

	// Token: 0x06007AF8 RID: 31480 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007AF9 RID: 31481 RVA: 0x002E5FE6 File Offset: 0x002E41E6
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

	// Token: 0x04005B03 RID: 23299
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x04005B04 RID: 23300
	[PublicizedFrom(EAccessModifier.Private)]
	public bool materialBased;

	// Token: 0x04005B05 RID: 23301
	[PublicizedFrom(EAccessModifier.Private)]
	public string material = "";

	// Token: 0x04005B06 RID: 23302
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoItem;

	// Token: 0x04005B07 RID: 23303
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x04005B08 RID: 23304
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblHaveCount;

	// Token: 0x04005B09 RID: 23305
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblNeedCount;

	// Token: 0x04005B0A RID: 23306
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x04005B0C RID: 23308
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B0D RID: 23309
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005B0E RID: 23310
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
