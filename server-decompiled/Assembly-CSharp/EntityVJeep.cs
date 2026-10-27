using System;
using UnityEngine.Scripting;

// Token: 0x020004FA RID: 1274
[Preserve]
public class EntityVJeep : EntityDriveable
{
	// Token: 0x06002972 RID: 10610 RVA: 0x00104F7D File Offset: 0x0010317D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
		this.wheels[1].wheelC.steerAngle = this.wheelDir;
	}
}
