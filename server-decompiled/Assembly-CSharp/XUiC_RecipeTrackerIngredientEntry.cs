using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001020 RID: 4128
[Preserve]
public class XUiC_RecipeTrackerIngredientEntry : XUiController
{
	// Token: 0x17000F4E RID: 3918
	// (get) Token: 0x06008221 RID: 33313 RVA: 0x003195FB File Offset: 0x003177FB
	// (set) Token: 0x06008222 RID: 33314 RVA: 0x00319603 File Offset: 0x00317803
	public XUiC_RecipeTrackerIngredientsList Owner { get; set; }

	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x06008223 RID: 33315 RVA: 0x0031960C File Offset: 0x0031780C
	// (set) Token: 0x06008224 RID: 33316 RVA: 0x00319614 File Offset: 0x00317814
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

	// Token: 0x06008225 RID: 33317 RVA: 0x003196B4 File Offset: 0x003178B4
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

	// Token: 0x06008226 RID: 33318 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008227 RID: 33319 RVA: 0x002E9366 File Offset: 0x002E7566
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

	// Token: 0x0400607B RID: 24699
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack ingredient;

	// Token: 0x0400607C RID: 24700
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x0400607D RID: 24701
	public bool IsComplete;

	// Token: 0x0400607F RID: 24703
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006080 RID: 24704
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> countFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => _i1.ToString() + "/" + _i2.ToString());
}
