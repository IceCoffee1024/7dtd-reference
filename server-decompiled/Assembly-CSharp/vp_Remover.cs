using System;
using UnityEngine;

// Token: 0x020015F4 RID: 5620
public class vp_Remover : MonoBehaviour
{
	// Token: 0x0600AF36 RID: 44854 RVA: 0x00415551 File Offset: 0x00413751
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		vp_Timer.In(Mathf.Max(this.LifeTime, 0.1f), delegate
		{
			vp_Utility.Destroy(base.gameObject);
		}, this.m_DestroyTimer);
	}

	// Token: 0x0600AF37 RID: 44855 RVA: 0x0041557A File Offset: 0x0041377A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_DestroyTimer.Cancel();
	}

	// Token: 0x0400829D RID: 33437
	public float LifeTime = 10f;

	// Token: 0x0400829E RID: 33438
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DestroyTimer = new vp_Timer.Handle();
}
