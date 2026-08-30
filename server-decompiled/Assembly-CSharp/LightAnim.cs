using System;
using UnityEngine;

// Token: 0x0200124E RID: 4686
public class LightAnim : MonoBehaviour
{
	// Token: 0x0600951A RID: 38170 RVA: 0x00383E4C File Offset: 0x0038204C
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

	// Token: 0x0600951B RID: 38171 RVA: 0x00383EB8 File Offset: 0x003820B8
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

	// Token: 0x04006FBD RID: 28605
	public float Duration = 0.5f;

	// Token: 0x04006FBE RID: 28606
	public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.1f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04006FBF RID: 28607
	public bool DestroyAtEnd;

	// Token: 0x04006FC0 RID: 28608
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light LightRef;

	// Token: 0x04006FC1 RID: 28609
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float intensityStart;

	// Token: 0x04006FC2 RID: 28610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float time;

	// Token: 0x04006FC3 RID: 28611
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeEnd;
}
