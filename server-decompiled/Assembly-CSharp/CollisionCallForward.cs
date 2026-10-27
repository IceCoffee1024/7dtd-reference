using System;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public class CollisionCallForward : MonoBehaviour
{
	// Token: 0x06002A18 RID: 10776 RVA: 0x00108A4B File Offset: 0x00106C4B
	public void OnCollisionEnter(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, false);
		}
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x00108A6E File Offset: 0x00106C6E
	public void OnCollisionStay(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, true);
		}
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x00108A94 File Offset: 0x00106C94
	public static Entity FindEntity(Transform _t)
	{
		Entity component = _t.GetComponent<Entity>();
		if (component)
		{
			return component;
		}
		CollisionCallForward componentInParent = _t.GetComponentInParent<CollisionCallForward>();
		if (componentInParent)
		{
			return componentInParent.Entity;
		}
		return null;
	}

	// Token: 0x04002008 RID: 8200
	public Entity Entity;
}
