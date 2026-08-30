using System;
using UnityEngine;

// Token: 0x020013E6 RID: 5094
public class FPS
{
	// Token: 0x06009FD7 RID: 40919 RVA: 0x003C0D35 File Offset: 0x003BEF35
	public FPS(float _restartTime)
	{
		this.restartTime = _restartTime;
		this.timeleft = _restartTime;
	}

	// Token: 0x06009FD8 RID: 40920 RVA: 0x003C0D58 File Offset: 0x003BEF58
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

	// Token: 0x04007856 RID: 30806
	[PublicizedFrom(EAccessModifier.Private)]
	public float accum;

	// Token: 0x04007857 RID: 30807
	[PublicizedFrom(EAccessModifier.Private)]
	public int frames;

	// Token: 0x04007858 RID: 30808
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeleft;

	// Token: 0x04007859 RID: 30809
	[PublicizedFrom(EAccessModifier.Private)]
	public float restartTime = 0.5f;

	// Token: 0x0400785A RID: 30810
	public float Counter;
}
