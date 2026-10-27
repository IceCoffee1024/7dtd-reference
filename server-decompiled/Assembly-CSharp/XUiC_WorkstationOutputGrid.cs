using System;
using UnityEngine.Scripting;

// Token: 0x020010F5 RID: 4341
[Preserve]
public class XUiC_WorkstationOutputGrid : XUiC_WorkstationGrid
{
	// Token: 0x060089DA RID: 35290 RVA: 0x0034B130 File Offset: 0x00349330
	public void UpdateData(ItemStack[] stackList)
	{
		this.UpdateBackend(stackList);
	}

	// Token: 0x060089DB RID: 35291 RVA: 0x0034B139 File Offset: 0x00349339
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationOutputGrid = this;
	}

	// Token: 0x060089DC RID: 35292 RVA: 0x0034B14D File Offset: 0x0034934D
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationOutputGrid = null;
	}

	// Token: 0x060089DD RID: 35293 RVA: 0x0034B161 File Offset: 0x00349361
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetOutputStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}
}
