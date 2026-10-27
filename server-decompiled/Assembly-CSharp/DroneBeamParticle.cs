using System;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class DroneBeamParticle : MonoBehaviour
{
	// Token: 0x06001DFB RID: 7675 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x000B66D0 File Offset: 0x000B48D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (!this.drone)
		{
			this.drone = base.GetComponentInParent<EntityDrone>();
			Transform transform = base.transform.parent.FindInChilds("WristLeft", false);
			if (transform)
			{
				base.transform.SetParent(transform, false);
			}
			return;
		}
		if (this.displayTime > 0f)
		{
			this.displayTime -= Time.deltaTime;
			EntityAlive attackTargetLocal = this.drone.GetAttackTargetLocal();
			if (attackTargetLocal)
			{
				base.transform.rotation = Quaternion.LookRotation(attackTargetLocal.getChestPosition() - this.drone.GetHealArmPosition());
				if (attackTargetLocal.IsDead())
				{
					this.displayTime = 0f;
				}
			}
			if (this.displayTime <= 0f)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x000B67A7 File Offset: 0x000B49A7
	public void SetDisplayTime(float time)
	{
		this.displayTime = time;
	}

	// Token: 0x04001410 RID: 5136
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone drone;

	// Token: 0x04001411 RID: 5137
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float displayTime;
}
