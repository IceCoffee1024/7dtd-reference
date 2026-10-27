using System;
using UnityEngine;

// Token: 0x020013D6 RID: 5078
public class BlendTimer
{
	// Token: 0x06009F93 RID: 40851 RVA: 0x003C4164 File Offset: 0x003C2364
	public BlendTimer()
		: this(1f)
	{
	}

	// Token: 0x06009F94 RID: 40852 RVA: 0x003C4174 File Offset: 0x003C2374
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

	// Token: 0x06009F95 RID: 40853 RVA: 0x003C41D8 File Offset: 0x003C23D8
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

	// Token: 0x06009F96 RID: 40854 RVA: 0x003C4264 File Offset: 0x003C2464
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

	// Token: 0x06009F97 RID: 40855 RVA: 0x003C42D5 File Offset: 0x003C24D5
	public void BlendToRate(float value, float unitsPerSecond)
	{
		this.BlendTo(value, Mathf.Abs(value - this.m_value[0]) / unitsPerSecond);
	}

	// Token: 0x17001313 RID: 4883
	// (get) Token: 0x06009F98 RID: 40856 RVA: 0x003C42EF File Offset: 0x003C24EF
	public float Value
	{
		get
		{
			return this.m_value[0];
		}
	}

	// Token: 0x17001314 RID: 4884
	// (get) Token: 0x06009F99 RID: 40857 RVA: 0x003C42F9 File Offset: 0x003C24F9
	public float Target
	{
		get
		{
			return this.m_value[2];
		}
	}

	// Token: 0x040078DC RID: 30940
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_value = new float[3];

	// Token: 0x040078DD RID: 30941
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_time = new float[2];
}
