using System;
using Audio;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class AudioPlayer : MonoBehaviour
{
	// Token: 0x06000385 RID: 901 RVA: 0x00019F04 File Offset: 0x00018104
	public void Play()
	{
		if (!this.refEntity)
		{
			this.refEntity = RootTransformRefEntity.AddIfEntity(base.transform);
			if (this.refEntity)
			{
				this.attachedEntity = this.refEntity.RootTransform.GetComponent<Entity>();
			}
		}
		if (this.attachedEntity)
		{
			Manager.Play(this.attachedEntity, this.soundName, 1f, false);
			this.isPlaying = true;
			this.queuedForPlaying = false;
		}
		else if (this.refEntity)
		{
			Vector3 position = this.refEntity.transform.position;
			if (position == Vector3.zero)
			{
				this.queuedForPlaying = true;
			}
			else
			{
				this.PlayAtPos(position);
			}
		}
		else
		{
			Vector3 position2 = base.transform.position;
			if (position2 == Vector3.zero)
			{
				this.queuedForPlaying = true;
			}
			else
			{
				this.PlayAtPos(position2);
			}
		}
		if (this.isPlaying && this.duration > 0f)
		{
			this.startTime = Time.time;
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001A00D File Offset: 0x0001820D
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayAtPos(Vector3 _pos)
	{
		this.playPos = _pos + Origin.position;
		Manager.Play(this.playPos, this.soundName, -1, false, 1f);
		this.isPlaying = true;
		this.queuedForPlaying = false;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001A048 File Offset: 0x00018248
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.startDelay > 0f)
		{
			this.startDelay -= Time.deltaTime;
			return;
		}
		if (this.queuedForPlaying)
		{
			this.Play();
		}
		if (this.isPlaying && this.duration > 0f && Time.time > this.startTime + this.duration)
		{
			this.StopAudio();
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001A0B4 File Offset: 0x000182B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void StopAudio()
	{
		if (this.isPlaying)
		{
			if (this.attachedEntity)
			{
				Manager.Stop(this.attachedEntity.entityId, this.soundName);
			}
			else
			{
				Manager.Stop(this.playPos, this.soundName);
			}
			this.isPlaying = false;
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001A106 File Offset: 0x00018306
	public void OnEnable()
	{
		if (!this.playOnDemand)
		{
			this.queuedForPlaying = true;
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0001A117 File Offset: 0x00018317
	public void OnDisable()
	{
		this.StopAudio();
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0001A117 File Offset: 0x00018317
	public void OnDestroy()
	{
		this.StopAudio();
	}

	// Token: 0x0400040C RID: 1036
	public string soundName;

	// Token: 0x0400040D RID: 1037
	public float duration = -1f;

	// Token: 0x0400040E RID: 1038
	public bool playOnDemand;

	// Token: 0x0400040F RID: 1039
	public float startDelay;

	// Token: 0x04000410 RID: 1040
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity attachedEntity;

	// Token: 0x04000411 RID: 1041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RootTransformRefEntity refEntity;

	// Token: 0x04000412 RID: 1042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool queuedForPlaying;

	// Token: 0x04000413 RID: 1043
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isPlaying;

	// Token: 0x04000414 RID: 1044
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startTime;

	// Token: 0x04000415 RID: 1045
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 playPos;
}
