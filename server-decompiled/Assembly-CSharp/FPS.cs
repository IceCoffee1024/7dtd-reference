using System;
using UnityEngine;

// Token: 0x02001433 RID: 5171
public class FPS
{
	// Token: 0x0600A1D7 RID: 41431 RVA: 0x003CD019 File Offset: 0x003CB219
	public FPS(float _restartTime)
	{
		this.restartTime = _restartTime;
		this.timeleft = _restartTime;
	}

	// Token: 0x0600A1D8 RID: 41432 RVA: 0x003CD03C File Offset: 0x003CB23C
	public bool Update()
	{
		this.timeleft -= Time.unscaledDeltaTime;
		this.accum += Time.unscaledDeltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			this.Counter = (float)this.frames / this.accum;
			this.timeleft = this.restartTime;
			this.accum = 0f;
			this.frames = 0;
			return true;
		}
		return false;
	}

	// Token: 0x040079EB RID: 31211
	[PublicizedFrom(EAccessModifier.Private)]
	public float accum;

	// Token: 0x040079EC RID: 31212
	[PublicizedFrom(EAccessModifier.Private)]
	public int frames;

	// Token: 0x040079ED RID: 31213
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeleft;

	// Token: 0x040079EE RID: 31214
	[PublicizedFrom(EAccessModifier.Private)]
	public float restartTime = 0.5f;

	// Token: 0x040079EF RID: 31215
	public float Counter;
}
