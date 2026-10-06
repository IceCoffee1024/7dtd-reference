using System;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public class ColliderHitCallForward : MonoBehaviour
{
	// Token: 0x06002A0F RID: 10767 RVA: 0x001084DB File Offset: 0x001066DB
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (this.Entity != null)
		{
			this.Entity.OnControllerColliderHit(hit);
		}
	}

	// Token: 0x04001FF7 RID: 8183
	public Entity Entity;
}
