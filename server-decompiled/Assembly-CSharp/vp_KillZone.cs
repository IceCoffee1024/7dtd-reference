using System;
using UnityEngine;

// Token: 0x020015F9 RID: 5625
public class vp_KillZone : MonoBehaviour
{
	// Token: 0x0600AF53 RID: 44883 RVA: 0x00415D90 File Offset: 0x00413F90
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

	// Token: 0x040082BE RID: 33470
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_DamageHandler m_TargetDamageHandler;

	// Token: 0x040082BF RID: 33471
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Respawner m_TargetRespawner;
}
