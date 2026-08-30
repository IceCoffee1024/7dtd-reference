using System;
using UnityEngine;

// Token: 0x020012AB RID: 4779
public class OriginParticles : MonoBehaviour
{
	// Token: 0x060097A2 RID: 38818 RVA: 0x00391FC0 File Offset: 0x003901C0
	public void OnEnable()
	{
		Origin.particleSystemTs.Add(base.transform);
	}

	// Token: 0x060097A3 RID: 38819 RVA: 0x00391FD2 File Offset: 0x003901D2
	public void OnDisable()
	{
		Origin.particleSystemTs.Remove(base.transform);
	}
}
