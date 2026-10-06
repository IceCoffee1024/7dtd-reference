using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public class ParticleLifetimeSwitch : MonoBehaviour
{
	// Token: 0x06002A51 RID: 10833 RVA: 0x0010B6A3 File Offset: 0x001098A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.entityRoot = RootTransformRefEntity.FindEntityUpwards(base.transform);
		this.particles = base.transform.GetComponent<ParticleSystem>();
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x0010B6C8 File Offset: 0x001098C8
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

	// Token: 0x04002053 RID: 8275
	public float TurnOffDelayAfterEntityDies = 5f;

	// Token: 0x04002054 RID: 8276
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform entityRoot;

	// Token: 0x04002055 RID: 8277
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04002056 RID: 8278
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ParticleSystem particles;

	// Token: 0x04002057 RID: 8279
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float delay;

	// Token: 0x04002058 RID: 8280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFirstUpdate = true;
}
