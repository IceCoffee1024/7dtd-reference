using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001428 RID: 5160
public class LightFlicker : MonoBehaviour
{
	// Token: 0x0600A224 RID: 41508 RVA: 0x003CC489 File Offset: 0x003CA689
	public void Start()
	{
		this.Init();
	}

	// Token: 0x0600A225 RID: 41509 RVA: 0x003CC494 File Offset: 0x003CA694
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

	// Token: 0x0600A226 RID: 41510 RVA: 0x003CC547 File Offset: 0x003CA747
	public void Reset()
	{
		this.MinLight = 0.1f;
		this.MaxLight = 0.5f;
	}

	// Token: 0x0600A227 RID: 41511 RVA: 0x003CC560 File Offset: 0x003CA760
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

	// Token: 0x040079DC RID: 31196
	public float MinLight;

	// Token: 0x040079DD RID: 31197
	public float MaxLight;

	// Token: 0x040079DE RID: 31198
	public float IntervalMin;

	// Token: 0x040079DF RID: 31199
	public float IntervalMax;

	// Token: 0x040079E0 RID: 31200
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_intervalIdx;

	// Token: 0x040079E1 RID: 31201
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_time;

	// Token: 0x040079E2 RID: 31202
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_baseLight;

	// Token: 0x040079E3 RID: 31203
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LightFlicker.Interval> m_steps;

	// Token: 0x02001429 RID: 5161
	[PublicizedFrom(EAccessModifier.Private)]
	public class Interval
	{
		// Token: 0x040079E4 RID: 31204
		public float Time;

		// Token: 0x040079E5 RID: 31205
		public float Value;
	}
}
