using System;
using UnityEngine.Scripting;

// Token: 0x020004EB RID: 1259
[Preserve]
public class EntityVGyroCopter : EntityDriveable
{
	// Token: 0x06002944 RID: 10564 RVA: 0x000FE248 File Offset: 0x000FC448
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
	}
}
