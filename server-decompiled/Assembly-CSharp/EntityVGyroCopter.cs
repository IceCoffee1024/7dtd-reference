using System;
using UnityEngine.Scripting;

// Token: 0x020004F6 RID: 1270
[Preserve]
public class EntityVGyroCopter : EntityDriveable
{
	// Token: 0x06002963 RID: 10595 RVA: 0x000FFB1C File Offset: 0x000FDD1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
	}
}
