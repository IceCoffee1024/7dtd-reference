using System;
using UnityEngine.Scripting;

// Token: 0x020004ED RID: 1261
[Preserve]
public class EntityVJeep : EntityDriveable
{
	// Token: 0x0600294C RID: 10572 RVA: 0x001030E9 File Offset: 0x001012E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
		this.wheels[1].wheelC.steerAngle = this.wheelDir;
	}
}
