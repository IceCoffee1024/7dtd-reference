using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FF8 RID: 4088
[Preserve]
public class XUiC_RecipeTrackerIngredientEntry : XUiController
{
	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x06008120 RID: 33056 RVA: 0x00311D9B File Offset: 0x0030FF9B
	// (set) Token: 0x06008121 RID: 33057 RVA: 0x00311DA3 File Offset: 0x0030FFA3
	public XUiC_RecipeTrackerIngredientsList Owner { get; set; }

	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x06008122 RID: 33058 RVA: 0x00311DAC File Offset: 0x0030FFAC
	// (set) Token: 0x06008123 RID: 33059 RVA: 0x00311DB4 File Offset: 0x0030FFB4
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

	// Token: 0x06008124 RID: 33060 RVA: 0x00311E54 File Offset: 0x00310054
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

	// Token: 0x06008125 RID: 33061 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008126 RID: 33062 RVA: 0x002E2032 File Offset: 0x002E0232
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

	// Token: 0x04005F8B RID: 24459
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x04005F8C RID: 24460
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x04005F8D RID: 24461
	public bool IsComplete;

	// Token: 0x04005F8F RID: 24463
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005F90 RID: 24464
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> countFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => _i1.ToString() + "/" + _i2.ToString());
}
