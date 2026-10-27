using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200145A RID: 5210
public class LightFlicker : MonoBehaviour
{
	// Token: 0x0600A327 RID: 41767 RVA: 0x003D3429 File Offset: 0x003D1629
	public void Start()
	{
		this.Init();
	}

	// Token: 0x0600A328 RID: 41768 RVA: 0x003D3434 File Offset: 0x003D1634
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

	// Token: 0x0600A329 RID: 41769 RVA: 0x003D34E7 File Offset: 0x003D16E7
	public void Reset()
	{
		this.MinLight = 0.1f;
		this.MaxLight = 0.5f;
	}

	// Token: 0x0600A32A RID: 41770 RVA: 0x003D3500 File Offset: 0x003D1700
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

	// Token: 0x04007A98 RID: 31384
	public float MinLight;

	// Token: 0x04007A99 RID: 31385
	public float MaxLight;

	// Token: 0x04007A9A RID: 31386
	public float IntervalMin;

	// Token: 0x04007A9B RID: 31387
	public float IntervalMax;

	// Token: 0x04007A9C RID: 31388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_intervalIdx;

	// Token: 0x04007A9D RID: 31389
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_time;

	// Token: 0x04007A9E RID: 31390
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_baseLight;

	// Token: 0x04007A9F RID: 31391
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LightFlicker.Interval> m_steps;

	// Token: 0x0200145B RID: 5211
	[PublicizedFrom(EAccessModifier.Private)]
	public class Interval
	{
		// Token: 0x04007AA0 RID: 31392
		public float Time;

		// Token: 0x04007AA1 RID: 31393
		public float Value;
	}
}
