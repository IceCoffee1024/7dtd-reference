using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020010B0 RID: 4272
public class XUiC_VariableHeightGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x0600883C RID: 34876 RVA: 0x000817FC File Offset: 0x0007F9FC
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x0600883D RID: 34877 RVA: 0x003408D1 File Offset: 0x0033EAD1
	public void SetHeight(int height)
	{
		this.gridHeight = height;
		this.heightChanged = true;
	}

	// Token: 0x0600883E RID: 34878 RVA: 0x003408E1 File Offset: 0x0033EAE1
	public void InitVariableHeightGrids(XUiV_Grid[] _grids)
	{
		this.grids = _grids;
	}

	// Token: 0x0600883F RID: 34879 RVA: 0x003408EA File Offset: 0x0033EAEA
	public override void OnOpen()
	{
		base.OnOpen();
	}

	// Token: 0x06008840 RID: 34880 RVA: 0x003408F4 File Offset: 0x0033EAF4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.heightChanged)
		{
			this.heightChanged = false;
			foreach (XUiV_Grid xuiV_Grid in this.grids)
			{
				this.updateGridHeight(xuiV_Grid, this.gridHeight);
			}
		}
	}

	// Token: 0x06008841 RID: 34881 RVA: 0x00340940 File Offset: 0x0033EB40
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateGridHeight(XUiV_Grid grid, int height)
	{
		grid.Rows = height;
		List<XUiController> children = grid.Controller.Children;
		for (int i = 0; i < children.Count; i++)
		{
			int num = i / 3;
			children[i].ViewComponent.IsVisible = num < height;
		}
	}

	// Token: 0x06008842 RID: 34882 RVA: 0x0034098C File Offset: 0x0033EB8C
	public override XUiC_ItemStack[] GetItemStackControllers()
	{
		int num = 3 * this.gridHeight;
		return this.itemControllers.Take<XUiC_ItemStack>(num).ToArray<XUiC_ItemStack>();
	}

	// Token: 0x0400657E RID: 25982
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x0400657F RID: 25983
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRowWidth = 3;

	// Token: 0x04006580 RID: 25984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid[] grids;

	// Token: 0x04006581 RID: 25985
	[PublicizedFrom(EAccessModifier.Private)]
	public int gridHeight = 1;

	// Token: 0x04006582 RID: 25986
	[PublicizedFrom(EAccessModifier.Private)]
	public bool heightChanged = true;
}
