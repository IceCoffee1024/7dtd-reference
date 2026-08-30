using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001011 RID: 4113
[Preserve]
public class XUiC_RecipeTrackerIngredientEntry : XUiController
{
	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x06008212 RID: 33298 RVA: 0x003165CB File Offset: 0x003147CB
	// (set) Token: 0x06008213 RID: 33299 RVA: 0x003165D3 File Offset: 0x003147D3
	public XUiC_RecipeTrackerIngredientsList Owner { get; set; }

	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x06008214 RID: 33300 RVA: 0x003165DC File Offset: 0x003147DC
	// (set) Token: 0x06008215 RID: 33301 RVA: 0x003165E4 File Offset: 0x003147E4
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

	// Token: 0x06008216 RID: 33302 RVA: 0x00316684 File Offset: 0x00314884
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

	// Token: 0x06008217 RID: 33303 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008218 RID: 33304 RVA: 0x002E5FE6 File Offset: 0x002E41E6
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

	// Token: 0x04006052 RID: 24658
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x04006053 RID: 24659
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x04006054 RID: 24660
	public bool IsComplete;

	// Token: 0x04006056 RID: 24662
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006057 RID: 24663
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> countFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => _i1.ToString() + "/" + _i2.ToString());
}
