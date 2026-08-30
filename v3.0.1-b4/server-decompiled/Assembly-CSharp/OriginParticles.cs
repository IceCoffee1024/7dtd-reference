using System;
using UnityEngine;

// Token: 0x02001291 RID: 4753
public class OriginParticles : MonoBehaviour
{
	// Token: 0x060096A7 RID: 38567 RVA: 0x0038CE64 File Offset: 0x0038B064
	public void OnEnable()
	{
		Origin.particleSystemTs.Add(base.transform);
	}

	// Token: 0x060096A8 RID: 38568 RVA: 0x0038CE76 File Offset: 0x0038B076
	public void OnDisable()
	{
		Origin.particleSystemTs.Remove(base.transform);
	}
}
