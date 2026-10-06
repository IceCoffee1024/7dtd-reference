using System;
using UnityEngine;

// Token: 0x020012C3 RID: 4803
public class OriginParticles : MonoBehaviour
{
	// Token: 0x0600982F RID: 38959 RVA: 0x00396828 File Offset: 0x00394A28
	public void OnEnable()
	{
		Origin.particleSystemTs.Add(base.transform);
	}

	// Token: 0x06009830 RID: 38960 RVA: 0x0039683A File Offset: 0x00394A3A
	public void OnDisable()
	{
		Origin.particleSystemTs.Remove(base.transform);
	}
}
