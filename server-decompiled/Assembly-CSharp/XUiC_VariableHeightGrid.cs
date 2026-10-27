using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020010D1 RID: 4305
public class XUiC_VariableHeightGrid : XUiC_ItemStackGrid
{
	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x060088DE RID: 35038 RVA: 0x00060491 File Offset: 0x0005E691
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x060088DF RID: 35039 RVA: 0x0034676D File Offset: 0x0034496D
	public void SetHeight(int height)
	{
		this.gridHeight = height;
		this.heightChanged = true;
	}

	// Token: 0x060088E0 RID: 35040 RVA: 0x0034677D File Offset: 0x0034497D
	public void InitVariableHeightGrids(XUiV_Grid[] _grids)
	{
		this.grids = _grids;
	}

	// Token: 0x060088E1 RID: 35041 RVA: 0x00346786 File Offset: 0x00344986
	public override void OnOpen()
	{
		base.OnOpen();
	}

	// Token: 0x060088E2 RID: 35042 RVA: 0x00346790 File Offset: 0x00344990
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

	// Token: 0x060088E3 RID: 35043 RVA: 0x003467DC File Offset: 0x003449DC
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

	// Token: 0x060088E4 RID: 35044 RVA: 0x00346828 File Offset: 0x00344A28
	public override XUiC_ItemStack[] GetItemStackControllers()
	{
		int num = 3 * this.gridHeight;
		return this.itemControllers.Take<XUiC_ItemStack>(num).ToArray<XUiC_ItemStack>();
	}

	// Token: 0x040065FF RID: 26111
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x04006600 RID: 26112
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRowWidth = 3;

	// Token: 0x04006601 RID: 26113
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid[] grids;

	// Token: 0x04006602 RID: 26114
	[PublicizedFrom(EAccessModifier.Private)]
	public int gridHeight = 1;

	// Token: 0x04006603 RID: 26115
	[PublicizedFrom(EAccessModifier.Private)]
	public bool heightChanged = true;
}
