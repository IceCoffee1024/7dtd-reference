using System;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class DroneBeamParticle : MonoBehaviour
{
	// Token: 0x06001DF7 RID: 7671 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x000B64C0 File Offset: 0x000B46C0
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

	// Token: 0x06001DF9 RID: 7673 RVA: 0x000B6597 File Offset: 0x000B4797
	public void SetDisplayTime(float time)
	{
		this.displayTime = time;
	}

	// Token: 0x04001400 RID: 5120
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone drone;

	// Token: 0x04001401 RID: 5121
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float displayTime;
}
