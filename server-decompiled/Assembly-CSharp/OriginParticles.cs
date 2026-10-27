using System;
using UnityEngine;

// Token: 0x020012D5 RID: 4821
public class OriginParticles : MonoBehaviour
{
	// Token: 0x06009887 RID: 39047 RVA: 0x00398C2C File Offset: 0x00396E2C
	public void OnEnable()
	{
		Origin.particleSystemTs.Add(base.transform);
	}

	// Token: 0x06009888 RID: 39048 RVA: 0x00398C3E File Offset: 0x00396E3E
	public void OnDisable()
	{
		Origin.particleSystemTs.Remove(base.transform);
	}
}
