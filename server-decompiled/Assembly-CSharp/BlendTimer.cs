using System;
using UnityEngine;

// Token: 0x020013C4 RID: 5060
public class BlendTimer
{
	// Token: 0x06009F3B RID: 40763 RVA: 0x003C1D60 File Offset: 0x003BFF60
	public BlendTimer()
		: this(1f)
	{
	}

	// Token: 0x06009F3C RID: 40764 RVA: 0x003C1D70 File Offset: 0x003BFF70
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

	// Token: 0x06009F3D RID: 40765 RVA: 0x003C1DD4 File Offset: 0x003BFFD4
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

	// Token: 0x06009F3E RID: 40766 RVA: 0x003C1E60 File Offset: 0x003C0060
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

	// Token: 0x06009F3F RID: 40767 RVA: 0x003C1ED1 File Offset: 0x003C00D1
	public void BlendToRate(float value, float unitsPerSecond)
	{
		this.BlendTo(value, Mathf.Abs(value - this.m_value[0]) / unitsPerSecond);
	}

	// Token: 0x17001307 RID: 4871
	// (get) Token: 0x06009F40 RID: 40768 RVA: 0x003C1EEB File Offset: 0x003C00EB
	public float Value
	{
		get
		{
			return this.m_value[0];
		}
	}

	// Token: 0x17001308 RID: 4872
	// (get) Token: 0x06009F41 RID: 40769 RVA: 0x003C1EF5 File Offset: 0x003C00F5
	public float Target
	{
		get
		{
			return this.m_value[2];
		}
	}

	// Token: 0x0400789B RID: 30875
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_value = new float[3];

	// Token: 0x0400789C RID: 30876
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_time = new float[2];
}
