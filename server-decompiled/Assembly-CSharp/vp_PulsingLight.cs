using System;
using UnityEngine;

// Token: 0x020015A7 RID: 5543
public class vp_PulsingLight : MonoBehaviour
{
	// Token: 0x0600AD75 RID: 44405 RVA: 0x0040A977 File Offset: 0x00408B77
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Light = base.GetComponent<Light>();
	}

	// Token: 0x0600AD76 RID: 44406 RVA: 0x0040A988 File Offset: 0x00408B88
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Light == null)
		{
			return;
		}
		this.m_Light.intensity = this.m_MinIntensity + Mathf.Abs(Mathf.Cos(Time.time * this.m_Rate) * (this.m_MaxIntensity - this.m_MinIntensity));
	}

	// Token: 0x040080F4 RID: 33012
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x040080F5 RID: 33013
	public float m_MinIntensity = 2f;

	// Token: 0x040080F6 RID: 33014
	public float m_MaxIntensity = 5f;

	// Token: 0x040080F7 RID: 33015
	public float m_Rate = 1f;
}
