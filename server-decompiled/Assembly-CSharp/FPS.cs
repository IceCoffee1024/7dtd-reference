using System;
using UnityEngine;

// Token: 0x02001401 RID: 5121
public class FPS
{
	// Token: 0x0600A0D4 RID: 41172 RVA: 0x003C6079 File Offset: 0x003C4279
	public FPS(float _restartTime)
	{
		this.restartTime = _restartTime;
		this.timeleft = _restartTime;
	}

	// Token: 0x0600A0D5 RID: 41173 RVA: 0x003C609C File Offset: 0x003C429C
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

	// Token: 0x0400792F RID: 31023
	[PublicizedFrom(EAccessModifier.Private)]
	public float accum;

	// Token: 0x04007930 RID: 31024
	[PublicizedFrom(EAccessModifier.Private)]
	public int frames;

	// Token: 0x04007931 RID: 31025
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeleft;

	// Token: 0x04007932 RID: 31026
	[PublicizedFrom(EAccessModifier.Private)]
	public float restartTime = 0.5f;

	// Token: 0x04007933 RID: 31027
	public float Counter;
}
