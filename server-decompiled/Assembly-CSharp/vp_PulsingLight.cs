using System;
using UnityEngine;

// Token: 0x020015DB RID: 5595
public class vp_PulsingLight : MonoBehaviour
{
	// Token: 0x0600AE80 RID: 44672 RVA: 0x00411BCF File Offset: 0x0040FDCF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Light = base.GetComponent<Light>();
	}

	// Token: 0x0600AE81 RID: 44673 RVA: 0x00411BE0 File Offset: 0x0040FDE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Light == null)
		{
			return;
		}
		this.m_Light.intensity = this.m_MinIntensity + Mathf.Abs(Mathf.Cos(Time.time * this.m_Rate) * (this.m_MaxIntensity - this.m_MinIntensity));
	}

	// Token: 0x040081B8 RID: 33208
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x040081B9 RID: 33209
	public float m_MinIntensity = 2f;

	// Token: 0x040081BA RID: 33210
	public float m_MaxIntensity = 5f;

	// Token: 0x040081BB RID: 33211
	public float m_Rate = 1f;
}
