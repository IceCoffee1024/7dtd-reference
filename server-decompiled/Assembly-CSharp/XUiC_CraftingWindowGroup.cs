using System;
using UnityEngine.Scripting;

// Token: 0x02000F1C RID: 3868
[Preserve]
public class XUiC_CraftingWindowGroup : XUiController
{
	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x060078D1 RID: 30929 RVA: 0x002DCEE7 File Offset: 0x002DB0E7
	// (set) Token: 0x060078D2 RID: 30930 RVA: 0x002DCEEF File Offset: 0x002DB0EF
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

	// Token: 0x060078D3 RID: 30931 RVA: 0x002DCF14 File Offset: 0x002DB114
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

	// Token: 0x060078D4 RID: 30932 RVA: 0x002DCF74 File Offset: 0x002DB174
	public virtual bool AddItemToQueue(Recipe _recipe)
	{
		if (this.craftCountControl != null)
		{
			return this.craftingQueue.AddRecipeToCraft(_recipe, this.craftCountControl.Count, -1f, true, -1f);
		}
		return this.craftingQueue.AddRecipeToCraft(_recipe, 1, -1f, true, -1f);
	}

	// Token: 0x060078D5 RID: 30933 RVA: 0x002DCFC4 File Offset: 0x002DB1C4
	public virtual bool AddItemToQueue(Recipe _recipe, int _count)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, -1f, true, -1f);
	}

	// Token: 0x060078D6 RID: 30934 RVA: 0x002DCFDE File Offset: 0x002DB1DE
	public virtual bool AddItemToQueue(Recipe _recipe, int _count, float _craftTime)
	{
		return this.craftingQueue.AddRecipeToCraft(_recipe, _count, _craftTime, true, -1f);
	}

	// Token: 0x060078D7 RID: 30935 RVA: 0x002DCFF4 File Offset: 0x002DB1F4
	public virtual bool AddRepairItemToQueue(float _repairTime, ItemValue _itemToRepair, int _amountToRepair)
	{
		return this.craftingQueue.AddItemToRepair(_repairTime, _itemToRepair, _amountToRepair);
	}

	// Token: 0x060078D8 RID: 30936 RVA: 0x002DD004 File Offset: 0x002DB204
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

	// Token: 0x060078D9 RID: 30937 RVA: 0x002DD0A8 File Offset: 0x002DB2A8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x060078DA RID: 30938 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CraftingRequirementsValid(Recipe _recipe)
	{
		return true;
	}

	// Token: 0x060078DB RID: 30939 RVA: 0x0003223F File Offset: 0x0003043F
	public virtual string CraftingRequirementsInvalidMessage(Recipe _recipe)
	{
		return "";
	}

	// Token: 0x040059A1 RID: 22945
	[PublicizedFrom(EAccessModifier.Protected)]
	public string workstation = "";

	// Token: 0x040059A2 RID: 22946
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool firstRun = true;

	// Token: 0x040059A3 RID: 22947
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040059A4 RID: 22948
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeList recipeList;

	// Token: 0x040059A5 RID: 22949
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingQueue craftingQueue;

	// Token: 0x040059A6 RID: 22950
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x040059A7 RID: 22951
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CraftingInfoWindow craftInfoWindow;
}
