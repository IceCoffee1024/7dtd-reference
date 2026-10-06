using System;
using UnityEngine;

// Token: 0x02001421 RID: 5153
public class FPS
{
	// Token: 0x0600A17F RID: 41343 RVA: 0x003CAC15 File Offset: 0x003C8E15
	public FPS(float _restartTime)
	{
		this.restartTime = _restartTime;
		this.timeleft = _restartTime;
	}

	// Token: 0x0600A180 RID: 41344 RVA: 0x003CAC38 File Offset: 0x003C8E38
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

	// Token: 0x040079AA RID: 31146
	[PublicizedFrom(EAccessModifier.Private)]
	public float accum;

	// Token: 0x040079AB RID: 31147
	[PublicizedFrom(EAccessModifier.Private)]
	public int frames;

	// Token: 0x040079AC RID: 31148
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeleft;

	// Token: 0x040079AD RID: 31149
	[PublicizedFrom(EAccessModifier.Private)]
	public float restartTime = 0.5f;

	// Token: 0x040079AE RID: 31150
	public float Counter;
}
