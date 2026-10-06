using System;
using UnityEngine;

// Token: 0x02001266 RID: 4710
public class LightAnim : MonoBehaviour
{
	// Token: 0x060095A8 RID: 38312 RVA: 0x00388694 File Offset: 0x00386894
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

	// Token: 0x060095A9 RID: 38313 RVA: 0x00388700 File Offset: 0x00386900
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

	// Token: 0x04007027 RID: 28711
	public float Duration = 0.5f;

	// Token: 0x04007028 RID: 28712
	public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.1f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04007029 RID: 28713
	public bool DestroyAtEnd;

	// Token: 0x0400702A RID: 28714
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light LightRef;

	// Token: 0x0400702B RID: 28715
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float intensityStart;

	// Token: 0x0400702C RID: 28716
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float time;

	// Token: 0x0400702D RID: 28717
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeEnd;
}
