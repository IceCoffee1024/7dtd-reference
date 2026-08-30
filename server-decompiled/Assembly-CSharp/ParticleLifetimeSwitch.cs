using System;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class ParticleLifetimeSwitch : MonoBehaviour
{
	// Token: 0x06002A5C RID: 10844 RVA: 0x0010AD6B File Offset: 0x00108F6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.entityRoot = RootTransformRefEntity.FindEntityUpwards(base.transform);
		this.particles = base.transform.GetComponent<ParticleSystem>();
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0010AD90 File Offset: 0x00108F90
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.bFirstUpdate)
		{
			this.bFirstUpdate = false;
			this.entity = ((this.entityRoot != null) ? this.entityRoot.GetComponent<Entity>() : null);
			if (this.entity && this.entity.IsDead())
			{
				if (this.particles != null)
				{
					this.particles.emission.enabled = false;
					this.particles = null;
				}
				this.entity = null;
			}
		}
		if (this.particles && !this.entity)
		{
			this.delay -= Time.deltaTime;
			if (this.delay <= 0f)
			{
				this.particles.emission.enabled = false;
				this.particles = null;
			}
		}
		if (this.entity && this.entity.IsDead())
		{
			this.delay = this.TurnOffDelayAfterEntityDies;
			this.entity = null;
		}
	}

	// Token: 0x0400204B RID: 8267
	public float TurnOffDelayAfterEntityDies = 5f;

	// Token: 0x0400204C RID: 8268
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform entityRoot;

	// Token: 0x0400204D RID: 8269
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x0400204E RID: 8270
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ParticleSystem particles;

	// Token: 0x0400204F RID: 8271
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float delay;

	// Token: 0x04002050 RID: 8272
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFirstUpdate = true;
}
