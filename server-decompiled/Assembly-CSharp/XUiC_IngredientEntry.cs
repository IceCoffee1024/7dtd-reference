using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F59 RID: 3929
[Preserve]
public class XUiC_IngredientEntry : XUiController
{
	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x06007AEF RID: 31471 RVA: 0x002E8E22 File Offset: 0x002E7022
	// (set) Token: 0x06007AF0 RID: 31472 RVA: 0x002E8E2A File Offset: 0x002E702A
	public Recipe Recipe { get; set; }

	// Token: 0x17000E73 RID: 3699
	// (get) Token: 0x06007AF1 RID: 31473 RVA: 0x002E8E33 File Offset: 0x002E7033
	// (set) Token: 0x06007AF2 RID: 31474 RVA: 0x002E8E3C File Offset: 0x002E703C
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

	// Token: 0x06007AF3 RID: 31475 RVA: 0x002E8EC0 File Offset: 0x002E70C0
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

	// Token: 0x06007AF4 RID: 31476 RVA: 0x002E8F88 File Offset: 0x002E7188
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

	// Token: 0x06007AF5 RID: 31477 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007AF6 RID: 31478 RVA: 0x002E9366 File Offset: 0x002E7566
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

	// Token: 0x04005B3B RID: 23355
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x04005B3C RID: 23356
	[PublicizedFrom(EAccessModifier.Private)]
	public bool materialBased;

	// Token: 0x04005B3D RID: 23357
	[PublicizedFrom(EAccessModifier.Private)]
	public string material = "";

	// Token: 0x04005B3E RID: 23358
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoItem;

	// Token: 0x04005B3F RID: 23359
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x04005B40 RID: 23360
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblHaveCount;

	// Token: 0x04005B41 RID: 23361
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblNeedCount;

	// Token: 0x04005B42 RID: 23362
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x04005B44 RID: 23364
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B45 RID: 23365
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005B46 RID: 23366
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
