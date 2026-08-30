using System;
using UnityEngine;

// Token: 0x020004FF RID: 1279
public class CollisionCallForward : MonoBehaviour
{
	// Token: 0x060029F2 RID: 10738 RVA: 0x00106B83 File Offset: 0x00104D83
	public void OnCollisionEnter(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, false);
		}
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x00106BA6 File Offset: 0x00104DA6
	public void OnCollisionStay(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, true);
		}
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x00106BCC File Offset: 0x00104DCC
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

	// Token: 0x04001FC1 RID: 8129
	public Entity Entity;
}
