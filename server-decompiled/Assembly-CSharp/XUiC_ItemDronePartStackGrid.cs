using System;
using UnityEngine.Scripting;

// Token: 0x02000F39 RID: 3897
[Preserve]
public class XUiC_ItemDronePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17000E5B RID: 3675
	// (get) Token: 0x060079D7 RID: 31191 RVA: 0x002E17D6 File Offset: 0x002DF9D6
	// (set) Token: 0x060079D8 RID: 31192 RVA: 0x002E17DE File Offset: 0x002DF9DE
	public EntityDrone CurrentVehicle { get; set; }

	// Token: 0x060079D9 RID: 31193 RVA: 0x002E17E8 File Offset: 0x002DF9E8
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemDronePartStack>(null);
		this.itemControllers = childrenByType;
	}
}
