using System;
using UnityEngine.Scripting;

// Token: 0x020010BB RID: 4283
[Preserve]
public class XUiC_WorkstationOutputGrid : XUiC_WorkstationGrid
{
	// Token: 0x06008847 RID: 34887 RVA: 0x00340AD8 File Offset: 0x0033ECD8
	public void UpdateData(ItemStack[] stackList)
	{
		this.UpdateBackend(stackList);
	}

	// Token: 0x06008848 RID: 34888 RVA: 0x00340AE1 File Offset: 0x0033ECE1
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationOutputGrid = this;
	}

	// Token: 0x06008849 RID: 34889 RVA: 0x00340AF5 File Offset: 0x0033ECF5
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationOutputGrid = null;
	}

	// Token: 0x0600884A RID: 34890 RVA: 0x00340B09 File Offset: 0x0033ED09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetOutputStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}
}
