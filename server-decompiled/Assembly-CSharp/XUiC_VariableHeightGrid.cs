using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020010C1 RID: 4289
public class XUiC_VariableHeightGrid : XUiC_ItemStackGrid
{
	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x0600888B RID: 34955 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x0600888C RID: 34956 RVA: 0x003446E9 File Offset: 0x003428E9
	public void SetHeight(int height)
	{
		this.gridHeight = height;
		this.heightChanged = true;
	}

	// Token: 0x0600888D RID: 34957 RVA: 0x003446F9 File Offset: 0x003428F9
	public void InitVariableHeightGrids(XUiV_Grid[] _grids)
	{
		this.grids = _grids;
	}

	// Token: 0x0600888E RID: 34958 RVA: 0x00344702 File Offset: 0x00342902
	public override void OnOpen()
	{
		base.OnOpen();
	}

	// Token: 0x0600888F RID: 34959 RVA: 0x0034470C File Offset: 0x0034290C
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

	// Token: 0x06008890 RID: 34960 RVA: 0x00344758 File Offset: 0x00342958
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

	// Token: 0x06008891 RID: 34961 RVA: 0x003447A4 File Offset: 0x003429A4
	public override XUiC_ItemStack[] GetItemStackControllers()
	{
		int num = 3 * this.gridHeight;
		return this.itemControllers.Take<XUiC_ItemStack>(num).ToArray<XUiC_ItemStack>();
	}

	// Token: 0x040065C0 RID: 26048
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x040065C1 RID: 26049
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRowWidth = 3;

	// Token: 0x040065C2 RID: 26050
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid[] grids;

	// Token: 0x040065C3 RID: 26051
	[PublicizedFrom(EAccessModifier.Private)]
	public int gridHeight = 1;

	// Token: 0x040065C4 RID: 26052
	[PublicizedFrom(EAccessModifier.Private)]
	public bool heightChanged = true;
}
