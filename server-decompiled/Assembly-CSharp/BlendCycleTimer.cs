using System;

// Token: 0x020013D7 RID: 5079
public class BlendCycleTimer
{
	// Token: 0x06009F9A RID: 40858 RVA: 0x003C4303 File Offset: 0x003C2503
	public BlendCycleTimer(float inTime, float holdTime, float outTime)
	{
		this.m_inTime = inTime;
		this.m_outTime = outTime;
		this.m_holdTime = holdTime;
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.Done;
	}

	// Token: 0x06009F9B RID: 40859 RVA: 0x003C4344 File Offset: 0x003C2544
	public void Tick(float dt)
	{
		this.m_blendTimer.Tick(dt);
		switch (this.m_dir)
		{
		case BlendCycleTimer.Dir.In:
			this.m_time += dt;
			if (this.m_time >= this.m_inTime)
			{
				this.m_dir = BlendCycleTimer.Dir.Hold;
				this.m_time = 0f;
				return;
			}
			break;
		case BlendCycleTimer.Dir.Hold:
			if (this.m_holdTime != -1f)
			{
				this.m_time += dt;
				if (this.m_time >= this.m_holdTime)
				{
					this.m_dir = BlendCycleTimer.Dir.Out;
					this.m_time = 0f;
					this.m_blendTimer.BlendTo(0f, this.m_outTime);
				}
			}
			break;
		case BlendCycleTimer.Dir.Out:
			this.m_time += dt;
			if (this.m_time >= this.m_outTime)
			{
				this.m_dir = BlendCycleTimer.Dir.Done;
				return;
			}
			break;
		case BlendCycleTimer.Dir.Done:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F9C RID: 40860 RVA: 0x003C4422 File Offset: 0x003C2622
	public void FadeIn()
	{
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_time = this.Value * this.m_inTime;
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x06009F9D RID: 40861 RVA: 0x003C4454 File Offset: 0x003C2654
	public void FadeOut()
	{
		this.m_dir = BlendCycleTimer.Dir.Out;
		this.m_time = (1f - this.Value) * this.m_outTime;
		this.m_blendTimer.BlendTo(0f, this.m_outTime);
	}

	// Token: 0x06009F9E RID: 40862 RVA: 0x003C448C File Offset: 0x003C268C
	public void Restart()
	{
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_blendTimer.BlendTo(0f, 0f);
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x06009F9F RID: 40863 RVA: 0x003C44CB File Offset: 0x003C26CB
	public BlendCycleTimer.Dir Direction
	{
		get
		{
			return this.m_dir;
		}
	}

	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x06009FA0 RID: 40864 RVA: 0x003C44D3 File Offset: 0x003C26D3
	public float Value
	{
		get
		{
			return this.m_blendTimer.Value;
		}
	}

	// Token: 0x040078DE RID: 30942
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendTimer m_blendTimer = new BlendTimer(0f);

	// Token: 0x040078DF RID: 30943
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_inTime;

	// Token: 0x040078E0 RID: 30944
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_outTime;

	// Token: 0x040078E1 RID: 30945
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_holdTime;

	// Token: 0x040078E2 RID: 30946
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_time;

	// Token: 0x040078E3 RID: 30947
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendCycleTimer.Dir m_dir;

	// Token: 0x020013D8 RID: 5080
	public enum Dir
	{
		// Token: 0x040078E5 RID: 30949
		In,
		// Token: 0x040078E6 RID: 30950
		Hold,
		// Token: 0x040078E7 RID: 30951
		Out,
		// Token: 0x040078E8 RID: 30952
		Done
	}
}
