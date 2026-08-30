using System;
using UnityEngine.Scripting;

// Token: 0x02000F13 RID: 3859
[Preserve]
public class XUiC_ItemDronePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17000E23 RID: 3619
	// (get) Token: 0x06007921 RID: 31009 RVA: 0x002DA456 File Offset: 0x002D8656
	// (set) Token: 0x06007922 RID: 31010 RVA: 0x002DA45E File Offset: 0x002D865E
	public EntityDrone CurrentVehicle { get; set; }

	// Token: 0x06007923 RID: 31011 RVA: 0x002DA468 File Offset: 0x002D8668
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemDronePartStack>(null);
		this.itemControllers = childrenByType;
	}
}
