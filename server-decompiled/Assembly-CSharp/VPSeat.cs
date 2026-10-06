using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CB2 RID: 3250
[Preserve]
public class VPSeat : VehiclePart
{
	// Token: 0x06006337 RID: 25399 RVA: 0x002732D8 File Offset: 0x002714D8
	public override void InitPrefabConnections()
	{
		base.InitIKTarget(AvatarIKGoal.LeftHand, null);
		base.InitIKTarget(AvatarIKGoal.RightHand, null);
		base.InitIKTarget(AvatarIKGoal.LeftFoot, null);
		base.InitIKTarget(AvatarIKGoal.RightFoot, null);
	}
}
