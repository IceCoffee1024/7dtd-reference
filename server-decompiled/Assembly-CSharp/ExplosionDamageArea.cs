using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class ExplosionDamageArea : MonoBehaviour
{
	// Token: 0x06002C45 RID: 11333 RVA: 0x0011907D File Offset: 0x0011727D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x00119094 File Offset: 0x00117294
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

	// Token: 0x06002C47 RID: 11335 RVA: 0x00119120 File Offset: 0x00117320
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

	// Token: 0x04002204 RID: 8708
	public List<string> BuffActions;

	// Token: 0x04002205 RID: 8709
	public int InitiatorEntityId = -1;
}
