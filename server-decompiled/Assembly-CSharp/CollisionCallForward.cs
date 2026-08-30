using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class CollisionCallForward : MonoBehaviour
{
	// Token: 0x06002A1C RID: 10780 RVA: 0x00107BBF File Offset: 0x00105DBF
	public void OnCollisionEnter(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, false);
		}
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x00107BE2 File Offset: 0x00105DE2
	public void OnCollisionStay(Collision collision)
	{
		if (this.Entity != null)
		{
			this.Entity.OnCollisionForward(base.transform, collision, true);
		}
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x00107C08 File Offset: 0x00105E08
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

	// Token: 0x04001FF0 RID: 8176
	public Entity Entity;
}
