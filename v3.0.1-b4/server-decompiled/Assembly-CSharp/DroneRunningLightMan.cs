using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020003E1 RID: 993
[Preserve]
public class DroneRunningLightMan
{
	// Token: 0x06001E24 RID: 7716 RVA: 0x000B767D File Offset: 0x000B587D
	public DroneRunningLightMan()
	{
		DroneRunningLightMan.instance = this;
		this.runningLights = new List<DroneRunningLight>();
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000B7696 File Offset: 0x000B5896
	public void AddLight(DroneRunningLight _light)
	{
		this.runningLights.Add(_light);
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000027FC File Offset: 0x000009FC
	public void QueueLight(DroneRunningLight _light)
	{
	}

	// Token: 0x0400141E RID: 5150
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DroneRunningLight> runningLights;

	// Token: 0x0400141F RID: 5151
	public static DroneRunningLightMan instance;
}
