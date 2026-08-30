using System;

// Token: 0x020013A5 RID: 5029
public class BlendCycleTimer
{
	// Token: 0x06009E97 RID: 40599 RVA: 0x003BD363 File Offset: 0x003BB563
	public BlendCycleTimer(float inTime, float holdTime, float outTime)
	{
		this.m_inTime = inTime;
		this.m_outTime = outTime;
		this.m_holdTime = holdTime;
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.Done;
	}

	// Token: 0x06009E98 RID: 40600 RVA: 0x003BD3A4 File Offset: 0x003BB5A4
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

	// Token: 0x06009E99 RID: 40601 RVA: 0x003BD482 File Offset: 0x003BB682
	public void FadeIn()
	{
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_time = this.Value * this.m_inTime;
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x06009E9A RID: 40602 RVA: 0x003BD4B4 File Offset: 0x003BB6B4
	public void FadeOut()
	{
		this.m_dir = BlendCycleTimer.Dir.Out;
		this.m_time = (1f - this.Value) * this.m_outTime;
		this.m_blendTimer.BlendTo(0f, this.m_outTime);
	}

	// Token: 0x06009E9B RID: 40603 RVA: 0x003BD4EC File Offset: 0x003BB6EC
	public void Restart()
	{
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_blendTimer.BlendTo(0f, 0f);
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x170012C4 RID: 4804
	// (get) Token: 0x06009E9C RID: 40604 RVA: 0x003BD52B File Offset: 0x003BB72B
	public BlendCycleTimer.Dir Direction
	{
		get
		{
			return this.m_dir;
		}
	}

	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x06009E9D RID: 40605 RVA: 0x003BD533 File Offset: 0x003BB733
	public float Value
	{
		get
		{
			return this.m_blendTimer.Value;
		}
	}

	// Token: 0x04007822 RID: 30754
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendTimer m_blendTimer = new BlendTimer(0f);

	// Token: 0x04007823 RID: 30755
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_inTime;

	// Token: 0x04007824 RID: 30756
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_outTime;

	// Token: 0x04007825 RID: 30757
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_holdTime;

	// Token: 0x04007826 RID: 30758
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_time;

	// Token: 0x04007827 RID: 30759
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendCycleTimer.Dir m_dir;

	// Token: 0x020013A6 RID: 5030
	public enum Dir
	{
		// Token: 0x04007829 RID: 30761
		In,
		// Token: 0x0400782A RID: 30762
		Hold,
		// Token: 0x0400782B RID: 30763
		Out,
		// Token: 0x0400782C RID: 30764
		Done
	}
}
