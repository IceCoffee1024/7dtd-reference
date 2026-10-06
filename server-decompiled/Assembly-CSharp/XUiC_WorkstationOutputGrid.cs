using System;
using UnityEngine.Scripting;

// Token: 0x020010E5 RID: 4325
[Preserve]
public class XUiC_WorkstationOutputGrid : XUiC_WorkstationGrid
{
	// Token: 0x06008987 RID: 35207 RVA: 0x003490AC File Offset: 0x003472AC
	public void UpdateData(ItemStack[] stackList)
	{
		this.UpdateBackend(stackList);
	}

	// Token: 0x06008988 RID: 35208 RVA: 0x003490B5 File Offset: 0x003472B5
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationOutputGrid = this;
	}

	// Token: 0x06008989 RID: 35209 RVA: 0x003490C9 File Offset: 0x003472C9
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationOutputGrid = null;
	}

	// Token: 0x0600898A RID: 35210 RVA: 0x003490DD File Offset: 0x003472DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetOutputStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}
}
