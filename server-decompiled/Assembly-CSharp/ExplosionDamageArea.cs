using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class ExplosionDamageArea : MonoBehaviour
{
	// Token: 0x06002C25 RID: 11301 RVA: 0x00117239 File Offset: 0x00115439
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x00117250 File Offset: 0x00115450
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity getEntityFromCollider(Collider col)
	{
		Transform transform = col.transform;
		if (!transform.tag.StartsWith("E_") && !transform.CompareTag("Item"))
		{
			return null;
		}
		if (transform.CompareTag("Item"))
		{
			return null;
		}
		Transform transform2 = null;
		if (transform.tag.StartsWith("E_BP_"))
		{
			transform2 = GameUtils.GetHitRootTransform(transform.tag, transform);
		}
		EntityAlive entityAlive = ((transform2 != null) ? transform2.GetComponent<EntityAlive>() : null);
		if (entityAlive == null || entityAlive.IsDead())
		{
			return null;
		}
		return entityAlive;
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x001172DC File Offset: 0x001154DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		EntityAlive entityAlive = this.getEntityFromCollider(other) as EntityAlive;
		if (entityAlive == null)
		{
			return;
		}
		if (this.BuffActions != null)
		{
			for (int i = 0; i < this.BuffActions.Count; i++)
			{
				entityAlive.Buffs.AddBuff(this.BuffActions[i], this.InitiatorEntityId, entityAlive.isEntityRemote, false, -1f);
			}
		}
	}

	// Token: 0x040021CA RID: 8650
	public List<string> BuffActions;

	// Token: 0x040021CB RID: 8651
	public int InitiatorEntityId = -1;
}
