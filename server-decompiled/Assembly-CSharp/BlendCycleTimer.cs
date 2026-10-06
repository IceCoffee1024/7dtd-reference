using System;

// Token: 0x020013C5 RID: 5061
public class BlendCycleTimer
{
	// Token: 0x06009F42 RID: 40770 RVA: 0x003C1EFF File Offset: 0x003C00FF
	public BlendCycleTimer(float inTime, float holdTime, float outTime)
	{
		this.m_inTime = inTime;
		this.m_outTime = outTime;
		this.m_holdTime = holdTime;
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.Done;
	}

	// Token: 0x06009F43 RID: 40771 RVA: 0x003C1F40 File Offset: 0x003C0140
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

	// Token: 0x06009F44 RID: 40772 RVA: 0x003C201E File Offset: 0x003C021E
	public void FadeIn()
	{
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_time = this.Value * this.m_inTime;
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x06009F45 RID: 40773 RVA: 0x003C2050 File Offset: 0x003C0250
	public void FadeOut()
	{
		this.m_dir = BlendCycleTimer.Dir.Out;
		this.m_time = (1f - this.Value) * this.m_outTime;
		this.m_blendTimer.BlendTo(0f, this.m_outTime);
	}

	// Token: 0x06009F46 RID: 40774 RVA: 0x003C2088 File Offset: 0x003C0288
	public void Restart()
	{
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_blendTimer.BlendTo(0f, 0f);
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x17001309 RID: 4873
	// (get) Token: 0x06009F47 RID: 40775 RVA: 0x003C20C7 File Offset: 0x003C02C7
	public BlendCycleTimer.Dir Direction
	{
		get
		{
			return this.m_dir;
		}
	}

	// Token: 0x1700130A RID: 4874
	// (get) Token: 0x06009F48 RID: 40776 RVA: 0x003C20CF File Offset: 0x003C02CF
	public float Value
	{
		get
		{
			return this.m_blendTimer.Value;
		}
	}

	// Token: 0x0400789D RID: 30877
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendTimer m_blendTimer = new BlendTimer(0f);

	// Token: 0x0400789E RID: 30878
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_inTime;

	// Token: 0x0400789F RID: 30879
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_outTime;

	// Token: 0x040078A0 RID: 30880
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_holdTime;

	// Token: 0x040078A1 RID: 30881
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_time;

	// Token: 0x040078A2 RID: 30882
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendCycleTimer.Dir m_dir;

	// Token: 0x020013C6 RID: 5062
	public enum Dir
	{
		// Token: 0x040078A4 RID: 30884
		In,
		// Token: 0x040078A5 RID: 30885
		Hold,
		// Token: 0x040078A6 RID: 30886
		Out,
		// Token: 0x040078A7 RID: 30887
		Done
	}
}
