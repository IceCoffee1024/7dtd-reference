using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02001097 RID: 4247
public class XUiC_VariableHeightGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x0600874A RID: 34634 RVA: 0x00081148 File Offset: 0x0007F348
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x0600874B RID: 34635 RVA: 0x0033C0A9 File Offset: 0x0033A2A9
	public void SetHeight(int height)
	{
		this.gridHeight = height;
		this.heightChanged = true;
	}

	// Token: 0x0600874C RID: 34636 RVA: 0x0033C0B9 File Offset: 0x0033A2B9
	public void InitVariableHeightGrids(XUiV_Grid[] _grids)
	{
		this.grids = _grids;
	}

	// Token: 0x0600874D RID: 34637 RVA: 0x0033C0C2 File Offset: 0x0033A2C2
	public override void OnOpen()
	{
		base.OnOpen();
	}

	// Token: 0x0600874E RID: 34638 RVA: 0x0033C0CC File Offset: 0x0033A2CC
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

	// Token: 0x0600874F RID: 34639 RVA: 0x0033C118 File Offset: 0x0033A318
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

	// Token: 0x06008750 RID: 34640 RVA: 0x0033C164 File Offset: 0x0033A364
	public override XUiC_ItemStack[] GetItemStackControllers()
	{
		int num = 3 * this.gridHeight;
		return this.itemControllers.Take<XUiC_ItemStack>(num).ToArray<XUiC_ItemStack>();
	}

	// Token: 0x040064B7 RID: 25783
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x040064B8 RID: 25784
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRowWidth = 3;

	// Token: 0x040064B9 RID: 25785
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid[] grids;

	// Token: 0x040064BA RID: 25786
	[PublicizedFrom(EAccessModifier.Private)]
	public int gridHeight = 1;

	// Token: 0x040064BB RID: 25787
	[PublicizedFrom(EAccessModifier.Private)]
	public bool heightChanged = true;
}
