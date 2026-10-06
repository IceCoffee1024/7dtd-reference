using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020003E8 RID: 1000
[Preserve]
public class DroneRunningLightMan
{
	// Token: 0x06001E39 RID: 7737 RVA: 0x000B7FCD File Offset: 0x000B61CD
	public DroneRunningLightMan()
	{
		DroneRunningLightMan.instance = this;
		this.runningLights = new List<DroneRunningLight>();
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x000B7FE6 File Offset: 0x000B61E6
	public void AddLight(DroneRunningLight _light)
	{
		this.runningLights.Add(_light);
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x000027FC File Offset: 0x000009FC
	public void QueueLight(DroneRunningLight _light)
	{
	}

	// Token: 0x0400142C RID: 5164
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DroneRunningLight> runningLights;

	// Token: 0x0400142D RID: 5165
	public static DroneRunningLightMan instance;
}
