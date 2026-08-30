using System;
using UnityEngine;

// Token: 0x02001232 RID: 4658
public class LightAnim : MonoBehaviour
{
	// Token: 0x06009416 RID: 37910 RVA: 0x0037EB3C File Offset: 0x0037CD3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Duration = Utils.FastMax(this.Duration, 0.001f);
		this.LightRef = base.GetComponent<Light>();
		this.intensityStart = this.LightRef.intensity;
		int length = this.IntensityCurve.length;
		if (length > 0)
		{
			this.timeEnd = this.IntensityCurve[length - 1].time;
		}
	}

	// Token: 0x06009417 RID: 37911 RVA: 0x0037EBA8 File Offset: 0x0037CDA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		float num = Time.deltaTime / this.Duration;
		this.time += num;
		if (this.time < this.timeEnd)
		{
			this.LightRef.intensity = this.intensityStart * this.IntensityCurve.Evaluate(this.time);
			return;
		}
		if (this.DestroyAtEnd)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04006EE4 RID: 28388
	public float Duration = 0.5f;

	// Token: 0x04006EE5 RID: 28389
	public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.1f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04006EE6 RID: 28390
	public bool DestroyAtEnd;

	// Token: 0x04006EE7 RID: 28391
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light LightRef;

	// Token: 0x04006EE8 RID: 28392
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float intensityStart;

	// Token: 0x04006EE9 RID: 28393
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float time;

	// Token: 0x04006EEA RID: 28394
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeEnd;
}
