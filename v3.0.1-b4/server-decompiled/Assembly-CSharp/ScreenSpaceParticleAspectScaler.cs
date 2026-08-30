using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class ScreenSpaceParticleAspectScaler : MonoBehaviour
{
	// Token: 0x06000241 RID: 577 RVA: 0x00012CA4 File Offset: 0x00010EA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		float num = (float)Screen.width / (float)Screen.height / this.designTimeAspectRatio;
		base.transform.localScale = new Vector3(num, 1f, 1f);
	}

	// Token: 0x040002F9 RID: 761
	public float designTimeAspectRatio;
}
