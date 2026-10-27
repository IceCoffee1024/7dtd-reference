using System;
using UnityEngine.Scripting;

// Token: 0x020004F8 RID: 1272
[Preserve]
public class EntityVGyroCopter : EntityDriveable
{
	// Token: 0x0600296A RID: 10602 RVA: 0x00100070 File Offset: 0x000FE270
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
	}
}
