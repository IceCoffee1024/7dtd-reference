using System;
using UnityEngine.Scripting;

// Token: 0x02000F29 RID: 3881
[Preserve]
public class XUiC_CraftingListInfo : XUiController
{
	// Token: 0x17000E48 RID: 3656
	// (get) Token: 0x0600790A RID: 30986 RVA: 0x002DE5F9 File Offset: 0x002DC7F9
	// (set) Token: 0x0600790B RID: 30987 RVA: 0x002DE601 File Offset: 0x002DC801
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

	// Token: 0x0600790C RID: 30988 RVA: 0x002DE624 File Offset: 0x002DC824
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

	// Token: 0x0600790D RID: 30989 RVA: 0x002DE6D0 File Offset: 0x002DC8D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.CategoryName = _categoryEntry.CategoryDisplayName;
		this.spriteName = _categoryEntry.SpriteName;
	}

	// Token: 0x0600790E RID: 30990 RVA: 0x002DE6EC File Offset: 0x002DC8EC
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

	// Token: 0x0600790F RID: 30991 RVA: 0x002DE780 File Offset: 0x002DC980
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		CraftingManager.RecipeUnlocked += this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x06007910 RID: 30992 RVA: 0x002DE7A0 File Offset: 0x002DC9A0
	public override void OnClose()
	{
		base.OnClose();
		CraftingManager.RecipeUnlocked -= this.CraftingManager_RecipeUnlocked;
	}

	// Token: 0x06007911 RID: 30993 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void CraftingManager_RecipeUnlocked(string recipeName)
	{
		this.IsDirty = true;
	}

	// Token: 0x040059CD RID: 22989
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "Basics";

	// Token: 0x040059CE RID: 22990
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "Craft_Icon_Basics";

	// Token: 0x040059CF RID: 22991
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockedCount;

	// Token: 0x040059D0 RID: 22992
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x040059D1 RID: 22993
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoCategoryIcon;

	// Token: 0x040059D2 RID: 22994
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblUnlockedCount;

	// Token: 0x040059D3 RID: 22995
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;
}
