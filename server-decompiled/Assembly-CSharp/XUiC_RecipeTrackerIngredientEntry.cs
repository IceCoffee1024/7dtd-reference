using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001030 RID: 4144
[Preserve]
public class XUiC_RecipeTrackerIngredientEntry : XUiController
{
	// Token: 0x17000F59 RID: 3929
	// (get) Token: 0x06008273 RID: 33395 RVA: 0x0031B2FF File Offset: 0x003194FF
	// (set) Token: 0x06008274 RID: 33396 RVA: 0x0031B307 File Offset: 0x00319507
	public XUiC_RecipeTrackerIngredientsList Owner { get; set; }

	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x06008275 RID: 33397 RVA: 0x0031B310 File Offset: 0x00319510
	// (set) Token: 0x06008276 RID: 33398 RVA: 0x0031B318 File Offset: 0x00319518
	public ItemStack Ingredient
	{
		get
		{
			return this.ingredient;
		}
		set
		{
			this.ingredient = value;
			if (this.ingredient != null && this.Owner.Recipe.materialBasedRecipe)
			{
				this.ingredient = null;
			}
			if (this.ingredient != null)
			{
				this.currentCount = this.xui.PlayerInventory.GetItemCount(this.ingredient.itemValue);
				this.IsComplete = this.currentCount >= this.ingredient.count * this.Owner.Count;
			}
			else
			{
				this.currentCount = 0;
				this.IsComplete = false;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x06008277 RID: 33399 RVA: 0x0031B3B8 File Offset: 0x003195B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.ingredient != null;
		if (bindingName == "hasingredient")
		{
			value = flag.ToString();
			return true;
		}
		if (bindingName == "itemname")
		{
			value = (flag ? this.ingredient.itemValue.ItemClass.GetLocalizedItemName() : "");
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
		if (bindingName == "itemcount")
		{
			if (flag)
			{
				int num = this.ingredient.count * this.Owner.Count;
				value = this.countFormatter.Format(this.currentCount, num);
			}
			return true;
		}
		if (!(bindingName == "ingredientcompletehexcolor"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		if (flag)
		{
			value = ((this.currentCount >= this.ingredient.count * this.Owner.Count) ? this.Owner.completeHexColor : this.Owner.incompleteHexColor);
		}
		else
		{
			value = "FFFFFF";
		}
		return true;
	}

	// Token: 0x06008278 RID: 33400 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008279 RID: 33401 RVA: 0x002EAC62 File Offset: 0x002E8E62
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

	// Token: 0x040060B9 RID: 24761
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x040060BA RID: 24762
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x040060BB RID: 24763
	public bool IsComplete;

	// Token: 0x040060BD RID: 24765
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040060BE RID: 24766
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> countFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => _i1.ToString() + "/" + _i2.ToString());
}
