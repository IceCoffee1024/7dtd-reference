using System;
using UnityEngine.Scripting;

// Token: 0x02000F0D RID: 3853
[Preserve]
public class XUiC_CraftingWindowGroup : XUiController
{
	// Token: 0x17000E22 RID: 3618
	// (get) Token: 0x060078D6 RID: 30934 RVA: 0x002D9B53 File Offset: 0x002D7D53
	// (set) Token: 0x060078D7 RID: 30935 RVA: 0x002D9B5B File Offset: 0x002D7D5B
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

	// Token: 0x060078D8 RID: 30936 RVA: 0x002D9B80 File Offset: 0x002D7D80
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

	// Token: 0x060078D9 RID: 30937 RVA: 0x002D9BE0 File Offset: 0x002D7DE0
	public virtual bool AddItemToQueue(Recipe _recipe)
	{
		if (this.craftCountControl != null)
		{
			return this.craftingQueue.AddRecipeToCraft(_recipe, this.craftCountControl.Count, -1f, true, -1f);
		}
		return this.craftingQueue.AddRecipeToCraft(_recipe, 1, -1f, true, -1f);
	}

	// Token: 0x060078DA RID: 30938 RVA: 0x002D9C30 File Offset: 0x002D7E30
	public virtual bool AddItemToQueue(Recipe _recipe, int _count)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, -1f, true, -1f);
	}

	// Token: 0x060078DB RID: 30939 RVA: 0x002D9C4A File Offset: 0x002D7E4A
	public virtual bool AddItemToQueue(Recipe _recipe, int _count, float _craftTime)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, _craftTime, true, -1f);
	}

	// Token: 0x060078DC RID: 30940 RVA: 0x002D9C60 File Offset: 0x002D7E60
	public virtual bool AddRepairItemToQueue(float _repairTime, ItemValue _itemToRepair, int _amountToRepair)
	{
		return this.craftingQueue.AddItemToRepair(_repairTime, _itemToRepair, _amountToRepair);
	}

	// Token: 0x060078DD RID: 30941 RVA: 0x002D9C70 File Offset: 0x002D7E70
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

	// Token: 0x060078DE RID: 30942 RVA: 0x002D9D14 File Offset: 0x002D7F14
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x060078DF RID: 30943 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool CraftingRequirementsValid(Recipe _recipe)
	{
		return true;
	}

	// Token: 0x060078E0 RID: 30944 RVA: 0x00032147 File Offset: 0x00030347
	public virtual string CraftingRequirementsInvalidMessage(Recipe _recipe)
	{
		return "";
	}

	// Token: 0x04005969 RID: 22889
	[PublicizedFrom(EAccessModifier.Protected)]
	public string workstation = "";

	// Token: 0x0400596A RID: 22890
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool firstRun = true;

	// Token: 0x0400596B RID: 22891
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x0400596C RID: 22892
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeList recipeList;

	// Token: 0x0400596D RID: 22893
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingQueue craftingQueue;

	// Token: 0x0400596E RID: 22894
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x0400596F RID: 22895
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingInfoWindow craftInfoWindow;
}
