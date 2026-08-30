using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C92 RID: 3218
[Preserve]
public class VPSeat : VehiclePart
{
	// Token: 0x060062B9 RID: 25273 RVA: 0x0026CFBC File Offset: 0x0026B1BC
	public override void InitPrefabConnections()
	{
		base.InitIKTarget(AvatarIKGoal.LeftHand, null);
		base.InitIKTarget(AvatarIKGoal.RightHand, null);
		base.InitIKTarget(AvatarIKGoal.LeftFoot, null);
		base.InitIKTarget(AvatarIKGoal.RightFoot, null);
	}
}
