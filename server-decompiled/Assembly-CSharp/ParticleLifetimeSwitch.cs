using System;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class ParticleLifetimeSwitch : MonoBehaviour
{
	// Token: 0x06002A58 RID: 10840 RVA: 0x0010BBF7 File Offset: 0x00109DF7
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.entityRoot = RootTransformRefEntity.FindEntityUpwards(base.transform);
		this.particles = base.transform.GetComponent<ParticleSystem>();
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x0010BC1C File Offset: 0x00109E1C
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

	// Token: 0x04002063 RID: 8291
	public float TurnOffDelayAfterEntityDies = 5f;

	// Token: 0x04002064 RID: 8292
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform entityRoot;

	// Token: 0x04002065 RID: 8293
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04002066 RID: 8294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ParticleSystem particles;

	// Token: 0x04002067 RID: 8295
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float delay;

	// Token: 0x04002068 RID: 8296
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFirstUpdate = true;
}
