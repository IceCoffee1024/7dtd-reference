using System;
using UnityEngine;

// Token: 0x02001389 RID: 5001
public class BlendTimer
{
	// Token: 0x06009D93 RID: 40339 RVA: 0x003B7E80 File Offset: 0x003B6080
	public BlendTimer()
		: this(1f)
	{
	}

	// Token: 0x06009D94 RID: 40340 RVA: 0x003B7E90 File Offset: 0x003B6090
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

	// Token: 0x06009D95 RID: 40341 RVA: 0x003B7EF4 File Offset: 0x003B60F4
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

	// Token: 0x06009D96 RID: 40342 RVA: 0x003B7F80 File Offset: 0x003B6180
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

	// Token: 0x06009D97 RID: 40343 RVA: 0x003B7FF1 File Offset: 0x003B61F1
	public void BlendToRate(float value, float unitsPerSecond)
	{
		this.BlendTo(value, Mathf.Abs(value - this.m_value[0]) / unitsPerSecond);
	}

	// Token: 0x17001289 RID: 4745
	// (get) Token: 0x06009D98 RID: 40344 RVA: 0x003B800B File Offset: 0x003B620B
	public float Value
	{
		get
		{
			return this.m_value[0];
		}
	}

	// Token: 0x1700128A RID: 4746
	// (get) Token: 0x06009D99 RID: 40345 RVA: 0x003B8015 File Offset: 0x003B6215
	public float Target
	{
		get
		{
			return this.m_value[2];
		}
	}

	// Token: 0x04007747 RID: 30535
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_value = new float[3];

	// Token: 0x04007748 RID: 30536
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_time = new float[2];
}
