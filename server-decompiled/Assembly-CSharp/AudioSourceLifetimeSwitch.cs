using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class AudioSourceLifetimeSwitch : MonoBehaviour
{
	// Token: 0x060029C4 RID: 10692 RVA: 0x00107D7A File Offset: 0x00105F7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.entityRoot = RootTransformRefEntity.FindEntityUpwards(base.transform);
		this.audio = base.transform.GetComponent<AudioSource>();
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x00107DA0 File Offset: 0x00105FA0
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

	// Token: 0x04001FE7 RID: 8167
	public float TurnOffDelayAfterEntityDies = 5f;

	// Token: 0x04001FE8 RID: 8168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform entityRoot;

	// Token: 0x04001FE9 RID: 8169
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04001FEA RID: 8170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource audio;

	// Token: 0x04001FEB RID: 8171
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float delay;

	// Token: 0x04001FEC RID: 8172
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFirstUpdate = true;
}
