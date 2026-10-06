using System;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public class CollisionCallForward : MonoBehaviour
{
	// Token: 0x06002A11 RID: 10769 RVA: 0x001084F7 File Offset: 0x001066F7
	public void OnCollisionEnter(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, false);
		}
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x0010851A File Offset: 0x0010671A
	public void OnCollisionStay(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, true);
		}
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x00108540 File Offset: 0x00106740
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

	// Token: 0x04001FF8 RID: 8184
	public Entity Entity;
}
