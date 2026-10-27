using System;
using UnityEngine;

// Token: 0x02001606 RID: 5638
public class vp_Remover : MonoBehaviour
{
	// Token: 0x0600AF8E RID: 44942 RVA: 0x00417955 File Offset: 0x00415B55
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		vp_Timer.In(Mathf.Max(this.LifeTime, 0.1f), delegate
		{
			vp_Utility.Destroy(base.gameObject);
		}, this.m_DestroyTimer);
	}

	// Token: 0x0600AF8F RID: 44943 RVA: 0x0041797E File Offset: 0x00415B7E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_DestroyTimer.Cancel();
	}

	// Token: 0x040082DE RID: 33502
	public float LifeTime = 10f;

	// Token: 0x040082DF RID: 33503
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DestroyTimer = new vp_Timer.Handle();
}
