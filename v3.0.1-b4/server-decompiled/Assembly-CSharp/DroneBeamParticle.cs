using System;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class DroneBeamParticle : MonoBehaviour
{
	// Token: 0x06001DE2 RID: 7650 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x000B5B70 File Offset: 0x000B3D70
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

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000B5C47 File Offset: 0x000B3E47
	public void SetDisplayTime(float time)
	{
		this.displayTime = time;
	}

	// Token: 0x040013F2 RID: 5106
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone drone;

	// Token: 0x040013F3 RID: 5107
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float displayTime;
}
