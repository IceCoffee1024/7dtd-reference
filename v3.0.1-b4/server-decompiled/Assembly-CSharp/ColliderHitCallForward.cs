using System;
using UnityEngine;

// Token: 0x020004FE RID: 1278
public class ColliderHitCallForward : MonoBehaviour
{
	// Token: 0x060029F0 RID: 10736 RVA: 0x00106B67 File Offset: 0x00104D67
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (this.Entity != null)
		{
			this.Entity.OnControllerColliderHit(hit);
		}
	}

	// Token: 0x04001FC0 RID: 8128
	public Entity Entity;
}
