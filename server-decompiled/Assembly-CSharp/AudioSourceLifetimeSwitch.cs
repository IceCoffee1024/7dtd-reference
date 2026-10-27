using System;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class AudioSourceLifetimeSwitch : MonoBehaviour
{
	// Token: 0x060029CB RID: 10699 RVA: 0x001082CE File Offset: 0x001064CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.entityRoot = RootTransformRefEntity.FindEntityUpwards(base.transform);
		this.audio = base.transform.GetComponent<AudioSource>();
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x001082F4 File Offset: 0x001064F4
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

	// Token: 0x04001FF7 RID: 8183
	public float TurnOffDelayAfterEntityDies = 5f;

	// Token: 0x04001FF8 RID: 8184
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform entityRoot;

	// Token: 0x04001FF9 RID: 8185
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04001FFA RID: 8186
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource audio;

	// Token: 0x04001FFB RID: 8187
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float delay;

	// Token: 0x04001FFC RID: 8188
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFirstUpdate = true;
}
