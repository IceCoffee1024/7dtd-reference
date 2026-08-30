using System;
using UnityEngine;

// Token: 0x020015B7 RID: 5559
public class vp_Remover : MonoBehaviour
{
	// Token: 0x0600AD86 RID: 44422 RVA: 0x0040B3BD File Offset: 0x004095BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		vp_Timer.In(Mathf.Max(this.LifeTime, 0.1f), delegate
		{
			vp_Utility.Destroy(base.gameObject);
		}, this.m_DestroyTimer);
	}

	// Token: 0x0600AD87 RID: 44423 RVA: 0x0040B3E6 File Offset: 0x004095E6
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_DestroyTimer.Cancel();
	}

	// Token: 0x04008141 RID: 33089
	public float LifeTime = 10f;

	// Token: 0x04008142 RID: 33090
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DestroyTimer = new vp_Timer.Handle();
}
