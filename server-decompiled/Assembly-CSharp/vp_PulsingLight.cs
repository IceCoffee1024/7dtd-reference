using System;
using UnityEngine;

// Token: 0x0200158C RID: 5516
public class vp_PulsingLight : MonoBehaviour
{
	// Token: 0x0600AC78 RID: 44152 RVA: 0x00405637 File Offset: 0x00403837
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Light = base.GetComponent<Light>();
	}

	// Token: 0x0600AC79 RID: 44153 RVA: 0x00405648 File Offset: 0x00403848
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Light == null)
		{
			return;
		}
		this.m_Light.intensity = this.m_MinIntensity + Mathf.Abs(Mathf.Cos(Time.time * this.m_Rate) * (this.m_MaxIntensity - this.m_MinIntensity));
	}

	// Token: 0x0400801B RID: 32795
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x0400801C RID: 32796
	public float m_MinIntensity = 2f;

	// Token: 0x0400801D RID: 32797
	public float m_MaxIntensity = 5f;

	// Token: 0x0400801E RID: 32798
	public float m_Rate = 1f;
}
