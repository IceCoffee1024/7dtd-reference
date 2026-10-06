using System;
using UnityEngine.Scripting;

// Token: 0x02000F1A RID: 3866
[Preserve]
public class XUiC_CraftingListInfo : XUiController
{
	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x060078C7 RID: 30919 RVA: 0x002DCD09 File Offset: 0x002DAF09
	// (set) Token: 0x060078C8 RID: 30920 RVA: 0x002DCD11 File Offset: 0x002DAF11
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

	// Token: 0x060078C9 RID: 30921 RVA: 0x002DCD34 File Offset: 0x002DAF34
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

	// Token: 0x060078CA RID: 30922 RVA: 0x002DCDE0 File Offset: 0x002DAFE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.CategoryName = _categoryEntry.CategoryDisplayName;
		this.spriteName = _categoryEntry.SpriteName;
	}

	// Token: 0x060078CB RID: 30923 RVA: 0x002DCDFC File Offset: 0x002DAFFC
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

	// Token: 0x060078CC RID: 30924 RVA: 0x002DCE90 File Offset: 0x002DB090
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		CraftingManager.RecipeUnlocked += this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x060078CD RID: 30925 RVA: 0x002DCEB0 File Offset: 0x002DB0B0
	public override void OnClose()
	{
		base.OnClose();
		CraftingManager.RecipeUnlocked -= this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x060078CE RID: 30926 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void CraftingManager_RecipeUnlocked(string recipeName)
	{
		this.IsDirty = true;
	}

	// Token: 0x0400599A RID: 22938
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "Basics";

	// Token: 0x0400599B RID: 22939
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "Craft_Icon_Basics";

	// Token: 0x0400599C RID: 22940
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockedCount;

	// Token: 0x0400599D RID: 22941
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x0400599E RID: 22942
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoCategoryIcon;

	// Token: 0x0400599F RID: 22943
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblUnlockedCount;

	// Token: 0x040059A0 RID: 22944
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;
}
