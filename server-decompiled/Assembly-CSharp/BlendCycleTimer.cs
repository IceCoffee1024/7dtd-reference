using System;

// Token: 0x0200138A RID: 5002
public class BlendCycleTimer
{
	// Token: 0x06009D9A RID: 40346 RVA: 0x003B801F File Offset: 0x003B621F
	public BlendCycleTimer(float inTime, float holdTime, float outTime)
	{
		this.m_inTime = inTime;
		this.m_outTime = outTime;
		this.m_holdTime = holdTime;
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.Done;
	}

	// Token: 0x06009D9B RID: 40347 RVA: 0x003B8060 File Offset: 0x003B6260
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

	// Token: 0x06009D9C RID: 40348 RVA: 0x003B813E File Offset: 0x003B633E
	public void FadeIn()
	{
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_time = this.Value * this.m_inTime;
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x06009D9D RID: 40349 RVA: 0x003B8170 File Offset: 0x003B6370
	public void FadeOut()
	{
		this.m_dir = BlendCycleTimer.Dir.Out;
		this.m_time = (1f - this.Value) * this.m_outTime;
		this.m_blendTimer.BlendTo(0f, this.m_outTime);
	}

	// Token: 0x06009D9E RID: 40350 RVA: 0x003B81A8 File Offset: 0x003B63A8
	public void Restart()
	{
		this.m_time = 0f;
		this.m_dir = BlendCycleTimer.Dir.In;
		this.m_blendTimer.BlendTo(0f, 0f);
		this.m_blendTimer.BlendTo(1f, this.m_inTime);
	}

	// Token: 0x1700128B RID: 4747
	// (get) Token: 0x06009D9F RID: 40351 RVA: 0x003B81E7 File Offset: 0x003B63E7
	public BlendCycleTimer.Dir Direction
	{
		get
		{
			return this.m_dir;
		}
	}

	// Token: 0x1700128C RID: 4748
	// (get) Token: 0x06009DA0 RID: 40352 RVA: 0x003B81EF File Offset: 0x003B63EF
	public float Value
	{
		get
		{
			return this.m_blendTimer.Value;
		}
	}

	// Token: 0x04007749 RID: 30537
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendTimer m_blendTimer = new BlendTimer(0f);

	// Token: 0x0400774A RID: 30538
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_inTime;

	// Token: 0x0400774B RID: 30539
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_outTime;

	// Token: 0x0400774C RID: 30540
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_holdTime;

	// Token: 0x0400774D RID: 30541
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_time;

	// Token: 0x0400774E RID: 30542
	[PublicizedFrom(EAccessModifier.Private)]
	public BlendCycleTimer.Dir m_dir;

	// Token: 0x0200138B RID: 5003
	public enum Dir
	{
		// Token: 0x04007750 RID: 30544
		In,
		// Token: 0x04007751 RID: 30545
		Hold,
		// Token: 0x04007752 RID: 30546
		Out,
		// Token: 0x04007753 RID: 30547
		Done
	}
}
