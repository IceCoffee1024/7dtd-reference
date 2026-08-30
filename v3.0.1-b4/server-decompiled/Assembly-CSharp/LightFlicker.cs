using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200140D RID: 5133
public class LightFlicker : MonoBehaviour
{
	// Token: 0x0600A127 RID: 41255 RVA: 0x003C7145 File Offset: 0x003C5345
	public void Start()
	{
		this.Init();
	}

	// Token: 0x0600A128 RID: 41256 RVA: 0x003C7150 File Offset: 0x003C5350
	public void Update()
	{
		this.m_time += Time.deltaTime;
		LightFlicker.Interval interval;
		for (;;)
		{
			interval = this.m_steps[this.m_intervalIdx];
			if (this.m_time < interval.Time)
			{
				break;
			}
			this.m_time -= interval.Time;
			this.m_baseLight = interval.Value;
			this.m_intervalIdx++;
			if (this.m_intervalIdx >= this.m_steps.Count)
			{
				this.m_intervalIdx = 0;
			}
		}
		base.GetComponent<Light>().intensity = Mathf.Lerp(this.m_baseLight, interval.Value, this.m_time / interval.Time);
	}

	// Token: 0x0600A129 RID: 41257 RVA: 0x003C7203 File Offset: 0x003C5403
	public void Reset()
	{
		this.MinLight = 0.1f;
		this.MaxLight = 0.5f;
	}

	// Token: 0x0600A12A RID: 41258 RVA: 0x003C721C File Offset: 0x003C541C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		this.m_intervalIdx = 0;
		this.m_time = 0f;
		this.m_steps = new List<LightFlicker.Interval>();
		this.m_baseLight = this.MinLight;
		base.GetComponent<Light>().intensity = this.m_baseLight;
		float num = 0f;
		do
		{
			LightFlicker.Interval interval = new LightFlicker.Interval();
			interval.Time = global::UnityEngine.Random.Range(Mathf.Max(0.001f, this.IntervalMin), Mathf.Max(0.001f, this.IntervalMax));
			interval.Value = global::UnityEngine.Random.Range(this.MinLight, this.MaxLight);
			this.m_steps.Add(interval);
			num += interval.Time;
		}
		while (num < 5f);
	}

	// Token: 0x04007903 RID: 30979
	public float MinLight;

	// Token: 0x04007904 RID: 30980
	public float MaxLight;

	// Token: 0x04007905 RID: 30981
	public float IntervalMin;

	// Token: 0x04007906 RID: 30982
	public float IntervalMax;

	// Token: 0x04007907 RID: 30983
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_intervalIdx;

	// Token: 0x04007908 RID: 30984
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_time;

	// Token: 0x04007909 RID: 30985
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_baseLight;

	// Token: 0x0400790A RID: 30986
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LightFlicker.Interval> m_steps;

	// Token: 0x0200140E RID: 5134
	[PublicizedFrom(EAccessModifier.Private)]
	public class Interval
	{
		// Token: 0x0400790B RID: 30987
		public float Time;

		// Token: 0x0400790C RID: 30988
		public float Value;
	}
}
