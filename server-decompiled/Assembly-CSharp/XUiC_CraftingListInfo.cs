using System;
using UnityEngine.Scripting;

// Token: 0x02000EF4 RID: 3828
[Preserve]
public class XUiC_CraftingListInfo : XUiController
{
	// Token: 0x17000E07 RID: 3591
	// (get) Token: 0x06007814 RID: 30740 RVA: 0x002D59E9 File Offset: 0x002D3BE9
	// (set) Token: 0x06007815 RID: 30741 RVA: 0x002D59F1 File Offset: 0x002D3BF1
	public string CategoryName
	{
		get
		{
			return this.categoryName;
		}
		set
		{
			this.IsDirty |= this.categoryName != value;
			this.categoryName = value;
		}
	}

	// Token: 0x06007816 RID: 30742 RVA: 0x002D5A14 File Offset: 0x002D3C14
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("windowicon");
		if (childById != null)
		{
			this.icoCategoryIcon = childById.ViewComponent as XUiV_Sprite;
		}
		XUiController xuiController = base.GetChildById("windowname");
		if (xuiController != null)
		{
			this.lblName = xuiController.ViewComponent as XUiV_Label;
		}
		xuiController = base.GetChildById("unlockedcount");
		if (xuiController != null)
		{
			this.lblUnlockedCount = xuiController.ViewComponent as XUiV_Label;
		}
		XUiController childById2 = base.GetChildById("categories");
		if (childById2 != null)
		{
			this.categoryList = (XUiC_CategoryList)childById2;
			this.categoryList.CategoryChanged += this.HandleCategoryChanged;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007817 RID: 30743 RVA: 0x002D5AC0 File Offset: 0x002D3CC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.CategoryName = _categoryEntry.CategoryDisplayName;
		this.spriteName = _categoryEntry.SpriteName;
	}

	// Token: 0x06007818 RID: 30744 RVA: 0x002D5ADC File Offset: 0x002D3CDC
	public override void Update(float _dt)
	{
		if (!this.windowGroup.isShowing)
		{
			return;
		}
		if (this.IsDirty)
		{
			if (this.lblName != null)
			{
				this.lblName.Text = this.categoryName;
			}
			if (this.icoCategoryIcon != null)
			{
				this.icoCategoryIcon.SpriteName = this.spriteName;
			}
			if (this.lblUnlockedCount != null)
			{
				this.lblUnlockedCount.Text = string.Format("{0}/{1}", CraftingManager.GetUnlockedRecipeCount(), CraftingManager.GetLockedRecipeCount());
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06007819 RID: 30745 RVA: 0x002D5B70 File Offset: 0x002D3D70
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		CraftingManager.RecipeUnlocked += this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x0600781A RID: 30746 RVA: 0x002D5B90 File Offset: 0x002D3D90
	public override void OnClose()
	{
		base.OnClose();
		CraftingManager.RecipeUnlocked -= this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x0600781B RID: 30747 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void CraftingManager_RecipeUnlocked(string recipeName)
	{
		this.IsDirty = true;
	}

	// Token: 0x040058B0 RID: 22704
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "Basics";

	// Token: 0x040058B1 RID: 22705
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "Craft_Icon_Basics";

	// Token: 0x040058B2 RID: 22706
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockedCount;

	// Token: 0x040058B3 RID: 22707
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x040058B4 RID: 22708
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoCategoryIcon;

	// Token: 0x040058B5 RID: 22709
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblUnlockedCount;

	// Token: 0x040058B6 RID: 22710
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;
}
