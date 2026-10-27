using System;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class ColliderHitCallForward : MonoBehaviour
{
	// Token: 0x06002A16 RID: 10774 RVA: 0x00108A2F File Offset: 0x00106C2F
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (this.Entity != null)
		{
			this.Entity.OnControllerColliderHit(hit);
		}
	}

	// Token: 0x04002007 RID: 8199
	public Entity Entity;
}
