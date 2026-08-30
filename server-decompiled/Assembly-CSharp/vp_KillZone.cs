using System;
using UnityEngine;

// Token: 0x020015D7 RID: 5591
public class vp_KillZone : MonoBehaviour
{
	// Token: 0x0600AEA0 RID: 44704 RVA: 0x00410F3C File Offset: 0x0040F13C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == 29 || col.gameObject.layer == 26)
		{
			return;
		}
		this.m_TargetDamageHandler = vp_DamageHandler.GetDamageHandlerOfCollider(col);
		if (this.m_TargetDamageHandler == null)
		{
			return;
		}
		if (this.m_TargetDamageHandler.CurrentHealth <= 0f)
		{
			return;
		}
		this.m_TargetRespawner = vp_Respawner.GetRespawnerOfCollider(col);
		if (this.m_TargetRespawner != null && Time.time < this.m_TargetRespawner.LastRespawnTime + 1f)
		{
			return;
		}
		this.m_TargetDamageHandler.Damage(new vp_DamageInfo(this.m_TargetDamageHandler.CurrentHealth, this.m_TargetDamageHandler.Transform));
	}

	// Token: 0x0400823B RID: 33339
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_DamageHandler m_TargetDamageHandler;

	// Token: 0x0400823C RID: 33340
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Respawner m_TargetRespawner;
}
