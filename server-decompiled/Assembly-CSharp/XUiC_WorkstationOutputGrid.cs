using System;
using UnityEngine.Scripting;

// Token: 0x020010D4 RID: 4308
[Preserve]
public class XUiC_WorkstationOutputGrid : XUiC_WorkstationGrid
{
	// Token: 0x06008939 RID: 35129 RVA: 0x003452FC File Offset: 0x003434FC
	public void UpdateData(ItemStack[] stackList)
	{
		this.UpdateBackend(stackList);
	}

	// Token: 0x0600893A RID: 35130 RVA: 0x00345305 File Offset: 0x00343505
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationOutputGrid = this;
	}

	// Token: 0x0600893B RID: 35131 RVA: 0x00345319 File Offset: 0x00343519
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationOutputGrid = null;
	}

	// Token: 0x0600893C RID: 35132 RVA: 0x0034532D File Offset: 0x0034352D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetOutputStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}
}
