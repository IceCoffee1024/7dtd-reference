using System;
using UnityEngine;

// Token: 0x02001278 RID: 4728
public class LightAnim : MonoBehaviour
{
	// Token: 0x060095FF RID: 38399 RVA: 0x0038AA90 File Offset: 0x00388C90
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

	// Token: 0x06009600 RID: 38400 RVA: 0x0038AAFC File Offset: 0x00388CFC
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

	// Token: 0x04007068 RID: 28776
	public float Duration = 0.5f;

	// Token: 0x04007069 RID: 28777
	public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.1f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x0400706A RID: 28778
	public bool DestroyAtEnd;

	// Token: 0x0400706B RID: 28779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light LightRef;

	// Token: 0x0400706C RID: 28780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float intensityStart;

	// Token: 0x0400706D RID: 28781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float time;

	// Token: 0x0400706E RID: 28782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeEnd;
}
