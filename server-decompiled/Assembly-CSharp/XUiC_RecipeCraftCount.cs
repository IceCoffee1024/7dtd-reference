using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001028 RID: 4136
[Preserve]
public class XUiC_RecipeCraftCount : XUiC_Counter
{
	// Token: 0x060081EF RID: 33263 RVA: 0x0031778C File Offset: 0x0031598C
	public override void Init()
	{
		base.Init();
		XUiC_RecipeList childByType = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (childByType != null)
		{
			childByType.CraftCount = this;
			childByType.RecipeChanged += this.HandleRecipeChanged;
		}
	}

	// Token: 0x060081F0 RID: 33264 RVA: 0x003177CC File Offset: 0x003159CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRecipeChanged(Recipe _recipe, XUiC_RecipeEntry recipeEntry)
	{
		if (this.recipe != _recipe)
		{
			this.recipe = _recipe;
			base.Count = 1;
			this.CalculateMaxCount();
		}
	}

	// Token: 0x060081F1 RID: 33265 RVA: 0x003177EB File Offset: 0x003159EB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleMaxCountOnPress(XUiController _sender, int _mouseButton)
	{
		base.Count = this.calcMaxCraftable();
		base.HandleCountChangedEvent();
	}

	// Token: 0x060081F2 RID: 33266 RVA: 0x003177FF File Offset: 0x003159FF
	public void CalculateMaxCount()
	{
		this.MaxCount = this.calcMaxCraftable();
		if (base.Count > this.MaxCount)
		{
			base.Count = this.MaxCount;
		}
		base.RefreshBindings();
	}

	// Token: 0x060081F3 RID: 33267 RVA: 0x00317830 File Offset: 0x00315A30
	[PublicizedFrom(EAccessModifier.Private)]
	public int calcMaxCraftable()
	{
		if (this.recipe == null)
		{
			return 1;
		}
		XUiC_WorkstationInputGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
		ItemStack[] array;
		if (childByType != null)
		{
			array = childByType.GetSlots();
		}
		else
		{
			array = this.xui.PlayerInventory.GetAllItemStacks().ToArray();
		}
		for (int i = 0; i < this.recipe.ingredients.Count; i++)
		{
			ItemStack itemStack = this.recipe.ingredients[i];
			if (itemStack != null && itemStack.itemValue.HasQuality)
			{
				return 1;
			}
		}
		int num = int.MaxValue;
		int num2 = ((this.recipe.craftingTier == -1) ? this.recipe.GetCraftingTier(this.xui.playerUI.entityPlayer) : this.recipe.craftingTier);
		for (int j = 0; j < this.recipe.ingredients.Count; j++)
		{
			ItemStack itemStack2 = this.recipe.ingredients[j];
			if (itemStack2 != null && itemStack2.itemValue.type != 0)
			{
				int count = itemStack2.count;
				float num3;
				if (this.recipe.UseIngredientModifier)
				{
					num3 = (float)((int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)count, this.xui.playerUI.entityPlayer, this.recipe, FastTags<TagGroup.Global>.Parse(itemStack2.itemValue.ItemClass.GetItemName()), true, true, true, true, true, num2, true, false));
					if (num3 > 0f)
					{
						num3 = (float)((int)(num3 * XUiM_Recipes.GetCraftingInputModifier(this.recipe)));
						if (XUiM_Recipes.CraftingInputModifier > 0f)
						{
							num3 = Utils.FastMax(1f, num3);
						}
					}
				}
				else
				{
					num3 = (float)count;
				}
				if (num3 >= 1f)
				{
					int num4 = 0;
					for (int k = 0; k < array.Length; k++)
					{
						if (array[k] != null && array[k].itemValue.type != 0 && itemStack2.itemValue.type == array[k].itemValue.type)
						{
							num4 += array[k].count;
						}
					}
					int num5 = Mathf.CeilToInt((float)num4 / num3);
					if (Mathf.FloorToInt(num3 * (float)num5) > num4)
					{
						num5--;
					}
					num = Mathf.Min(num5, num);
					if (num == 0)
					{
						break;
					}
				}
			}
		}
		if (XUiM_Recipes.CraftingInputModifier == 0f)
		{
			return 10000;
		}
		return Mathf.Clamp(num, 1, 10000);
	}

	// Token: 0x060081F4 RID: 33268 RVA: 0x00317A94 File Offset: 0x00315C94
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.CalculateMaxCount();
		}
	}

	// Token: 0x060081F5 RID: 33269 RVA: 0x00317AAC File Offset: 0x00315CAC
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
			this.CalculateMaxCount();
		}
		((XUiV_Label)this.counter.ViewComponent).Text = (this.textInput.Text = base.Count.ToString());
	}

	// Token: 0x060081F6 RID: 33270 RVA: 0x00317B3B File Offset: 0x00315D3B
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x060081F7 RID: 33271 RVA: 0x00317B7B File Offset: 0x00315D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.CalculateMaxCount();
	}

	// Token: 0x060081F8 RID: 33272 RVA: 0x00317B7B File Offset: 0x00315D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.CalculateMaxCount();
	}

	// Token: 0x060081F9 RID: 33273 RVA: 0x00317B83 File Offset: 0x00315D83
	public void RefreshCounts()
	{
		this.CalculateMaxCount();
		this.IsDirty = true;
	}

	// Token: 0x060081FA RID: 33274 RVA: 0x00317B94 File Offset: 0x00315D94
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "enablecountdown")
		{
			value = (base.Count > 1).ToString();
			return true;
		}
		if (!(bindingName == "enablecountup"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = (base.Count < this.MaxCount).ToString();
		return true;
	}

	// Token: 0x04006070 RID: 24688
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxAllowed = 10000;

	// Token: 0x04006071 RID: 24689
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;
}
