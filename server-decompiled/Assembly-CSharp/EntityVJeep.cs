using System;
using UnityEngine.Scripting;

// Token: 0x020004F8 RID: 1272
[Preserve]
public class EntityVJeep : EntityDriveable
{
	// Token: 0x0600296B RID: 10603 RVA: 0x00104A29 File Offset: 0x00102C29
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
		this.wheels[1].wheelC.steerAngle = this.wheelDir;
	}
}
