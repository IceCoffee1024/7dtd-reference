using System;
using UnityEngine.Scripting;

// Token: 0x02000F2A RID: 3882
[Preserve]
public class XUiC_ItemDronePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x060079DA RID: 31194 RVA: 0x002DE40A File Offset: 0x002DC60A
	// (set) Token: 0x060079DB RID: 31195 RVA: 0x002DE412 File Offset: 0x002DC612
	public EntityDrone CurrentVehicle { get; set; }

	// Token: 0x060079DC RID: 31196 RVA: 0x002DE41C File Offset: 0x002DC61C
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemDronePartStack>(null);
		this.itemControllers = childrenByType;
	}
}
