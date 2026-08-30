using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CA4 RID: 3236
[Preserve]
public class VPSeat : VehiclePart
{
	// Token: 0x06006344 RID: 25412 RVA: 0x0026FE94 File Offset: 0x0026E094
	public override void InitPrefabConnections()
	{
		base.InitIKTarget(AvatarIKGoal.LeftHand, null);
		base.InitIKTarget(AvatarIKGoal.RightHand, null);
		base.InitIKTarget(AvatarIKGoal.LeftFoot, null);
		base.InitIKTarget(AvatarIKGoal.RightFoot, null);
	}
}
