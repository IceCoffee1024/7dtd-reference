using System;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
public class AudioSourceLifetimeSwitch : MonoBehaviour
{
	// Token: 0x060029A5 RID: 10661 RVA: 0x00106408 File Offset: 0x00104608
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.entityRoot = RootTransformRefEntity.FindEntityUpwards(base.transform);
		this.audio = base.transform.GetComponent<AudioSource>();
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x0010642C File Offset: 0x0010462C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.bFirstUpdate)
		{
			this.bFirstUpdate = false;
			this.entity = ((this.entityRoot != null) ? this.entityRoot.GetComponent<Entity>() : null);
			if (this.entity && this.entity.IsDead())
			{
				if (this.audio)
				{
					this.audio.enabled = false;
					this.audio = null;
				}
				this.entity = null;
			}
		}
		if (this.audio && !this.entity)
		{
			this.delay -= Time.deltaTime;
			if (this.delay <= 0f)
			{
				this.audio.enabled = false;
				this.audio = null;
			}
		}
		if (this.entity && this.entity.IsDead())
		{
			this.delay = this.TurnOffDelayAfterEntityDies;
			this.entity = null;
		}
	}

	// Token: 0x04001FB0 RID: 8112
	public float TurnOffDelayAfterEntityDies = 5f;

	// Token: 0x04001FB1 RID: 8113
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform entityRoot;

	// Token: 0x04001FB2 RID: 8114
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04001FB3 RID: 8115
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource audio;

	// Token: 0x04001FB4 RID: 8116
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float delay;

	// Token: 0x04001FB5 RID: 8117
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFirstUpdate = true;
}
