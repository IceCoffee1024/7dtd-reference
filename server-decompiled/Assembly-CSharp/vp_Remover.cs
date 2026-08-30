using System;
using UnityEngine;

// Token: 0x020015D2 RID: 5586
public class vp_Remover : MonoBehaviour
{
	// Token: 0x0600AE83 RID: 44675 RVA: 0x004106FD File Offset: 0x0040E8FD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		vp_Timer.In(Mathf.Max(this.LifeTime, 0.1f), delegate
		{
			vp_Utility.Destroy(base.gameObject);
		}, this.m_DestroyTimer);
	}

	// Token: 0x0600AE84 RID: 44676 RVA: 0x00410726 File Offset: 0x0040E926
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_DestroyTimer.Cancel();
	}

	// Token: 0x0400821A RID: 33306
	public float LifeTime = 10f;

	// Token: 0x0400821B RID: 33307
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DestroyTimer = new vp_Timer.Handle();
}
