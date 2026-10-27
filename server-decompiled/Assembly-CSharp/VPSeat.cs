using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CBD RID: 3261
[Preserve]
public class VPSeat : VehiclePart
{
	// Token: 0x0600635C RID: 25436 RVA: 0x00274800 File Offset: 0x00272A00
	public override void InitPrefabConnections()
	{
		base.InitIKTarget(AvatarIKGoal.LeftHand, null);
		base.InitIKTarget(AvatarIKGoal.RightHand, null);
		base.InitIKTarget(AvatarIKGoal.LeftFoot, null);
		base.InitIKTarget(AvatarIKGoal.RightFoot, null);
	}
}
