using System;
using UnityEngine;

// Token: 0x020013A4 RID: 5028
public class BlendTimer
{
	// Token: 0x06009E90 RID: 40592 RVA: 0x003BD1C4 File Offset: 0x003BB3C4
	public BlendTimer()
		: this(1f)
	{
	}

	// Token: 0x06009E91 RID: 40593 RVA: 0x003BD1D4 File Offset: 0x003BB3D4
	public BlendTimer(float initialValue)
	{
		this.m_time[0] = (this.m_time[1] = 0f);
		float[] value = this.m_value;
		int num = 0;
		float[] value2 = this.m_value;
		int num2 = 1;
		this.m_value[2] = initialValue;
		value[num] = (value2[num2] = initialValue);
	}

	// Token: 0x06009E92 RID: 40594 RVA: 0x003BD238 File Offset: 0x003BB438
	public void Tick(float dt)
	{
		if (this.m_time[1] != 0f)
		{
			this.m_time[0] += dt;
			if (this.m_time[0] >= this.m_time[1])
			{
				this.m_value[0] = this.m_value[2];
				this.m_time[1] = 0f;
				return;
			}
			this.m_value[0] = Mathf.Lerp(this.m_value[1], this.m_value[2], this.m_time[0] / this.m_time[1]);
		}
	}

	// Token: 0x06009E93 RID: 40595 RVA: 0x003BD2C4 File Offset: 0x003BB4C4
	public void BlendTo(float value, float time)
	{
		if (time > 0f)
		{
			this.m_value[1] = this.m_value[0];
			this.m_value[2] = value;
			this.m_time[0] = 0f;
			this.m_time[1] = time;
			return;
		}
		float[] value2 = this.m_value;
		int num = 0;
		float[] value3 = this.m_value;
		int num2 = 1;
		this.m_value[2] = value;
		value2[num] = (value3[num2] = value);
		this.m_time[1] = 0f;
	}

	// Token: 0x06009E94 RID: 40596 RVA: 0x003BD335 File Offset: 0x003BB535
	public void BlendToRate(float value, float unitsPerSecond)
	{
		this.BlendTo(value, Mathf.Abs(value - this.m_value[0]) / unitsPerSecond);
	}

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x06009E95 RID: 40597 RVA: 0x003BD34F File Offset: 0x003BB54F
	public float Value
	{
		get
		{
			return this.m_value[0];
		}
	}

	// Token: 0x170012C3 RID: 4803
	// (get) Token: 0x06009E96 RID: 40598 RVA: 0x003BD359 File Offset: 0x003BB559
	public float Target
	{
		get
		{
			return this.m_value[2];
		}
	}

	// Token: 0x04007820 RID: 30752
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_value = new float[3];

	// Token: 0x04007821 RID: 30753
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_time = new float[2];
}
