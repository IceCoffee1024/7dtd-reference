using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054F RID: 1359
public class ExplosionDamageArea : MonoBehaviour
{
	// Token: 0x06002C4D RID: 11341 RVA: 0x00119609 File Offset: 0x00117809
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x00119620 File Offset: 0x00117820
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

	// Token: 0x06002C4F RID: 11343 RVA: 0x001196AC File Offset: 0x001178AC
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

	// Token: 0x0400221B RID: 8731
	public List<string> BuffActions;

	// Token: 0x0400221C RID: 8732
	public int InitiatorEntityId = -1;
}
