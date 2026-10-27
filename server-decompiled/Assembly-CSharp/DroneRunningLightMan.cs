using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020003EA RID: 1002
[Preserve]
public class DroneRunningLightMan
{
	// Token: 0x06001E3E RID: 7742 RVA: 0x000B8379 File Offset: 0x000B6579
	public DroneRunningLightMan()
	{
		DroneRunningLightMan.instance = this;
		this.runningLights = new List<DroneRunningLight>();
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000B8392 File Offset: 0x000B6592
	public void AddLight(DroneRunningLight _light)
	{
		this.runningLights.Add(_light);
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x000027FC File Offset: 0x000009FC
	public void QueueLight(DroneRunningLight _light)
	{
	}

	// Token: 0x0400143C RID: 5180
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DroneRunningLight> runningLights;

	// Token: 0x0400143D RID: 5181
	public static DroneRunningLightMan instance;
}
