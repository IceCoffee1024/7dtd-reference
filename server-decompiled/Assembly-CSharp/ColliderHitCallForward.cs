using System;
using UnityEngine;

// Token: 0x02000503 RID: 1283
public class ColliderHitCallForward : MonoBehaviour
{
	// Token: 0x06002A1A RID: 10778 RVA: 0x00107BA3 File Offset: 0x00105DA3
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (this.Entity != null)
		{
			this.Entity.OnControllerColliderHit(hit);
		}
	}

	// Token: 0x04001FEF RID: 8175
	public Entity Entity;
}
