using System;

// Token: 0x02001242 RID: 4674
public class GUIBlinker
{
	// Token: 0x0600950F RID: 38159 RVA: 0x00386799 File Offset: 0x00384999
	public GUIBlinker(float _ms)
	{
		this.ms = _ms;
	}

	// Token: 0x06009510 RID: 38160 RVA: 0x003867AF File Offset: 0x003849AF
	public bool Draw(float _curTime)
	{
		if (_curTime - this.lastBlinkTime > this.ms)
		{
			this.lastBlinkTime = _curTime;
			this.bResult = !this.bResult;
		}
		return this.bResult;
	}

	// Token: 0x04006FA9 RID: 28585
	[PublicizedFrom(EAccessModifier.Private)]
	public float ms;

	// Token: 0x04006FAA RID: 28586
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastBlinkTime;

	// Token: 0x04006FAB RID: 28587
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bResult = true;
}
