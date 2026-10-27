using System;
using UnityEngine.Scripting;

// Token: 0x02000F48 RID: 3912
[Preserve]
public class XUiC_ItemDronePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x06007A1A RID: 31258 RVA: 0x002E30C6 File Offset: 0x002E12C6
	// (set) Token: 0x06007A1B RID: 31259 RVA: 0x002E30CE File Offset: 0x002E12CE
	public EntityDrone CurrentVehicle { get; set; }

	// Token: 0x06007A1C RID: 31260 RVA: 0x002E30D8 File Offset: 0x002E12D8
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemDronePartStack>(null);
		this.itemControllers = childrenByType;
	}
}
