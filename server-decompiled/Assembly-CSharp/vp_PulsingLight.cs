using System;
using UnityEngine;

// Token: 0x020015C9 RID: 5577
public class vp_PulsingLight : MonoBehaviour
{
	// Token: 0x0600AE28 RID: 44584 RVA: 0x0040F7CB File Offset: 0x0040D9CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Light = base.GetComponent<Light>();
	}

	// Token: 0x0600AE29 RID: 44585 RVA: 0x0040F7DC File Offset: 0x0040D9DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Light == null)
		{
			return;
		}
		this.m_Light.intensity = this.m_MinIntensity + Mathf.Abs(Mathf.Cos(Time.time * this.m_Rate) * (this.m_MaxIntensity - this.m_MinIntensity));
	}

	// Token: 0x04008177 RID: 33143
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x04008178 RID: 33144
	public float m_MinIntensity = 2f;

	// Token: 0x04008179 RID: 33145
	public float m_MaxIntensity = 5f;

	// Token: 0x0400817A RID: 33146
	public float m_Rate = 1f;
}
