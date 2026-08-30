using System;
using UnityEngine.Scripting;

// Token: 0x02000EF6 RID: 3830
[Preserve]
public class XUiC_CraftingWindowGroup : XUiController
{
	// Token: 0x17000E08 RID: 3592
	// (get) Token: 0x0600781E RID: 30750 RVA: 0x002D5BC7 File Offset: 0x002D3DC7
	// (set) Token: 0x0600781F RID: 30751 RVA: 0x002D5BCF File Offset: 0x002D3DCF
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

	// Token: 0x06007820 RID: 30752 RVA: 0x002D5BF4 File Offset: 0x002D3DF4
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

	// Token: 0x06007821 RID: 30753 RVA: 0x002D5C54 File Offset: 0x002D3E54
	public virtual bool AddItemToQueue(Recipe _recipe)
	{
		if (this.craftCountControl != null)
		{
			return this.craftingQueue.AddRecipeToCraft(_recipe, this.craftCountControl.Count, -1f, true, -1f);
		}
		return this.craftingQueue.AddRecipeToCraft(_recipe, 1, -1f, true, -1f);
	}

	// Token: 0x06007822 RID: 30754 RVA: 0x002D5CA4 File Offset: 0x002D3EA4
	public virtual bool AddItemToQueue(Recipe _recipe, int _count)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, -1f, true, -1f);
	}

	// Token: 0x06007823 RID: 30755 RVA: 0x002D5CBE File Offset: 0x002D3EBE
	public virtual bool AddItemToQueue(Recipe _recipe, int _count, float _craftTime)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, _craftTime, true, -1f);
	}

	// Token: 0x06007824 RID: 30756 RVA: 0x002D5CD4 File Offset: 0x002D3ED4
	public virtual bool AddRepairItemToQueue(float _repairTime, ItemValue _itemToRepair, int _amountToRepair)
	{
		return this.craftingQueue.AddItemToRepair(_repairTime, _itemToRepair, _amountToRepair);
	}

	// Token: 0x06007825 RID: 30757 RVA: 0x002D5CE4 File Offset: 0x002D3EE4
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

	// Token: 0x06007826 RID: 30758 RVA: 0x002D5D88 File Offset: 0x002D3F88
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x06007827 RID: 30759 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CraftingRequirementsValid(Recipe _recipe)
	{
		return true;
	}

	// Token: 0x06007828 RID: 30760 RVA: 0x00031E8B File Offset: 0x0003008B
	public virtual string CraftingRequirementsInvalidMessage(Recipe _recipe)
	{
		return "";
	}

	// Token: 0x040058B7 RID: 22711
	[PublicizedFrom(EAccessModifier.Protected)]
	public string workstation = "";

	// Token: 0x040058B8 RID: 22712
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool firstRun = true;

	// Token: 0x040058B9 RID: 22713
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040058BA RID: 22714
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeList recipeList;

	// Token: 0x040058BB RID: 22715
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingQueue craftingQueue;

	// Token: 0x040058BC RID: 22716
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x040058BD RID: 22717
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingInfoWindow craftInfoWindow;
}
