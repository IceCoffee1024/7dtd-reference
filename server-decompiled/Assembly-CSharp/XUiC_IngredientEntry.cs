using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F68 RID: 3944
[Preserve]
public class XUiC_IngredientEntry : XUiController
{
	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x06007B32 RID: 31538 RVA: 0x002EA71E File Offset: 0x002E891E
	// (set) Token: 0x06007B33 RID: 31539 RVA: 0x002EA726 File Offset: 0x002E8926
	public Recipe Recipe { get; set; }

	// Token: 0x17000E7D RID: 3709
	// (get) Token: 0x06007B34 RID: 31540 RVA: 0x002EA72F File Offset: 0x002E892F
	// (set) Token: 0x06007B35 RID: 31541 RVA: 0x002EA738 File Offset: 0x002E8938
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

	// Token: 0x06007B36 RID: 31542 RVA: 0x002EA7BC File Offset: 0x002E89BC
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

	// Token: 0x06007B37 RID: 31543 RVA: 0x002EA884 File Offset: 0x002E8A84
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

	// Token: 0x06007B38 RID: 31544 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007B39 RID: 31545 RVA: 0x002EAC62 File Offset: 0x002E8E62
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

	// Token: 0x04005B6E RID: 23406
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x04005B6F RID: 23407
	[PublicizedFrom(EAccessModifier.Private)]
	public bool materialBased;

	// Token: 0x04005B70 RID: 23408
	[PublicizedFrom(EAccessModifier.Private)]
	public string material = "";

	// Token: 0x04005B71 RID: 23409
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoItem;

	// Token: 0x04005B72 RID: 23410
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x04005B73 RID: 23411
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblHaveCount;

	// Token: 0x04005B74 RID: 23412
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblNeedCount;

	// Token: 0x04005B75 RID: 23413
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x04005B77 RID: 23415
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B78 RID: 23416
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005B79 RID: 23417
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
