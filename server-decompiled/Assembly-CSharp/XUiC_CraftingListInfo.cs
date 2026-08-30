using System;
using UnityEngine.Scripting;

// Token: 0x02000F0B RID: 3851
[Preserve]
public class XUiC_CraftingListInfo : XUiController
{
	// Token: 0x17000E21 RID: 3617
	// (get) Token: 0x060078CC RID: 30924 RVA: 0x002D9975 File Offset: 0x002D7B75
	// (set) Token: 0x060078CD RID: 30925 RVA: 0x002D997D File Offset: 0x002D7B7D
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

	// Token: 0x060078CE RID: 30926 RVA: 0x002D99A0 File Offset: 0x002D7BA0
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

	// Token: 0x060078CF RID: 30927 RVA: 0x002D9A4C File Offset: 0x002D7C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.CategoryName = _categoryEntry.CategoryDisplayName;
		this.spriteName = _categoryEntry.SpriteName;
	}

	// Token: 0x060078D0 RID: 30928 RVA: 0x002D9A68 File Offset: 0x002D7C68
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

	// Token: 0x060078D1 RID: 30929 RVA: 0x002D9AFC File Offset: 0x002D7CFC
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		CraftingManager.RecipeUnlocked += this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x060078D2 RID: 30930 RVA: 0x002D9B1C File Offset: 0x002D7D1C
	public override void OnClose()
	{
		base.OnClose();
		CraftingManager.RecipeUnlocked -= this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x060078D3 RID: 30931 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void CraftingManager_RecipeUnlocked(string recipeName)
	{
		this.IsDirty = true;
	}

	// Token: 0x04005962 RID: 22882
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "Basics";

	// Token: 0x04005963 RID: 22883
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "Craft_Icon_Basics";

	// Token: 0x04005964 RID: 22884
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockedCount;

	// Token: 0x04005965 RID: 22885
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x04005966 RID: 22886
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoCategoryIcon;

	// Token: 0x04005967 RID: 22887
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblUnlockedCount;

	// Token: 0x04005968 RID: 22888
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;
}
