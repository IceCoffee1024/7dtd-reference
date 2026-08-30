using System;
using UnityEngine;

// Token: 0x020015BC RID: 5564
public class vp_KillZone : MonoBehaviour
{
	// Token: 0x0600ADA3 RID: 44451 RVA: 0x0040BBFC File Offset: 0x00409DFC
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

	// Token: 0x04008162 RID: 33122
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_DamageHandler m_TargetDamageHandler;

	// Token: 0x04008163 RID: 33123
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Respawner m_TargetRespawner;
}
