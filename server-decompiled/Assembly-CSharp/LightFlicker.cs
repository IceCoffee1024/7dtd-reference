using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001448 RID: 5192
public class LightFlicker : MonoBehaviour
{
	// Token: 0x0600A2CF RID: 41679 RVA: 0x003D1025 File Offset: 0x003CF225
	public void Start()
	{
		this.Init();
	}

	// Token: 0x0600A2D0 RID: 41680 RVA: 0x003D1030 File Offset: 0x003CF230
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

	// Token: 0x0600A2D1 RID: 41681 RVA: 0x003D10E3 File Offset: 0x003CF2E3
	public void Reset()
	{
		this.MinLight = 0.1f;
		this.MaxLight = 0.5f;
	}

	// Token: 0x0600A2D2 RID: 41682 RVA: 0x003D10FC File Offset: 0x003CF2FC
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

	// Token: 0x04007A57 RID: 31319
	public float MinLight;

	// Token: 0x04007A58 RID: 31320
	public float MaxLight;

	// Token: 0x04007A59 RID: 31321
	public float IntervalMin;

	// Token: 0x04007A5A RID: 31322
	public float IntervalMax;

	// Token: 0x04007A5B RID: 31323
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_intervalIdx;

	// Token: 0x04007A5C RID: 31324
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_time;

	// Token: 0x04007A5D RID: 31325
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_baseLight;

	// Token: 0x04007A5E RID: 31326
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LightFlicker.Interval> m_steps;

	// Token: 0x02001449 RID: 5193
	[PublicizedFrom(EAccessModifier.Private)]
	public class Interval
	{
		// Token: 0x04007A5F RID: 31327
		public float Time;

		// Token: 0x04007A60 RID: 31328
		public float Value;
	}
}
