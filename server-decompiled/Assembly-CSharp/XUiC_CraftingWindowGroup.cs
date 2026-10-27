using System;
using UnityEngine.Scripting;

// Token: 0x02000F2B RID: 3883
[Preserve]
public class XUiC_CraftingWindowGroup : XUiController
{
	// Token: 0x17000E49 RID: 3657
	// (get) Token: 0x06007914 RID: 30996 RVA: 0x002DE7D7 File Offset: 0x002DC9D7
	// (set) Token: 0x06007915 RID: 30997 RVA: 0x002DE7DF File Offset: 0x002DC9DF
	public string Workstation
	{
		get
		{
			return this.workstation;
		}
		set
		{
			this.workstation = value;
			if (this.recipeList != null)
			{
				this.recipeList.Workstation = this.workstation;
			}
		}
	}

	// Token: 0x06007916 RID: 30998 RVA: 0x002DE804 File Offset: 0x002DCA04
	public override void Init()
	{
		base.Init();
		this.recipeList = base.GetChildByType<XUiC_RecipeList>();
		this.craftingQueue = base.GetChildByType<XUiC_CraftingQueue>();
		this.craftCountControl = base.GetChildByType<XUiC_RecipeCraftCount>();
		this.categoryList = base.GetChildByType<XUiC_CategoryList>();
		this.craftInfoWindow = base.GetChildByType<XUiC_CraftingInfoWindow>();
		this.recipeList.InfoWindow = this.craftInfoWindow;
	}

	// Token: 0x06007917 RID: 30999 RVA: 0x002DE864 File Offset: 0x002DCA64
	public virtual bool AddItemToQueue(Recipe _recipe)
	{
		if (this.craftCountControl != null)
		{
			return this.craftingQueue.AddRecipeToCraft(_recipe, this.craftCountControl.Count, -1f, true, -1f);
		}
		return this.craftingQueue.AddRecipeToCraft(_recipe, 1, -1f, true, -1f);
	}

	// Token: 0x06007918 RID: 31000 RVA: 0x002DE8B4 File Offset: 0x002DCAB4
	public virtual bool AddItemToQueue(Recipe _recipe, int _count)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, -1f, true, -1f);
	}

	// Token: 0x06007919 RID: 31001 RVA: 0x002DE8CE File Offset: 0x002DCACE
	public virtual bool AddItemToQueue(Recipe _recipe, int _count, float _craftTime)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, _craftTime, true, -1f);
	}

	// Token: 0x0600791A RID: 31002 RVA: 0x002DE8E4 File Offset: 0x002DCAE4
	public virtual bool AddRepairItemToQueue(float _repairTime, ItemValue _itemToRepair, int _amountToRepair)
	{
		return this.craftingQueue.AddItemToRepair(_repairTime, _itemToRepair, _amountToRepair);
	}

	// Token: 0x0600791B RID: 31003 RVA: 0x002DE8F4 File Offset: 0x002DCAF4
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.recipeList != null && this.categoryList != null && this.categoryList.SetupCategoriesByWorkstation(""))
		{
			this.recipeList.Workstation = "";
			this.recipeList.SetCategory("Basics");
			this.categoryList.SetCategory("Basics");
		}
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		XUiC_WindowSelector childByType = this.xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>();
		if (childByType != null)
		{
			childByType.SetSelected("crafting");
		}
	}

	// Token: 0x0600791C RID: 31004 RVA: 0x002DE998 File Offset: 0x002DCB98
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x0600791D RID: 31005 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CraftingRequirementsValid(Recipe _recipe)
	{
		return true;
	}

	// Token: 0x0600791E RID: 31006 RVA: 0x000321FF File Offset: 0x000303FF
	public virtual string CraftingRequirementsInvalidMessage(Recipe _recipe)
	{
		return "";
	}

	// Token: 0x040059D4 RID: 22996
	[PublicizedFrom(EAccessModifier.Protected)]
	public string workstation = "";

	// Token: 0x040059D5 RID: 22997
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool firstRun = true;

	// Token: 0x040059D6 RID: 22998
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040059D7 RID: 22999
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeList recipeList;

	// Token: 0x040059D8 RID: 23000
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingQueue craftingQueue;

	// Token: 0x040059D9 RID: 23001
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x040059DA RID: 23002
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingInfoWindow craftInfoWindow;
}
