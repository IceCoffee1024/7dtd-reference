using System;
using UnityEngine;

// Token: 0x0200160B RID: 5643
public class vp_KillZone : MonoBehaviour
{
	// Token: 0x0600AFAB RID: 44971 RVA: 0x00418194 File Offset: 0x00416394
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

	// Token: 0x040082FF RID: 33535
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_DamageHandler m_TargetDamageHandler;

	// Token: 0x04008300 RID: 33536
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Respawner m_TargetRespawner;
}
