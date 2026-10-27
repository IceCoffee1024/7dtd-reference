using System;

// Token: 0x02001254 RID: 4692
public class GUIBlinker
{
	// Token: 0x06009566 RID: 38246 RVA: 0x00388B95 File Offset: 0x00386D95
	public GUIBlinker(float _ms)
	{
		this.ms = _ms;
	}

	// Token: 0x06009567 RID: 38247 RVA: 0x00388BAB File Offset: 0x00386DAB
	public bool Draw(float _curTime)
	{
		if (_curTime - this.lastBlinkTime > this.ms)
		{
			this.lastBlinkTime = _curTime;
			this.bResult = !this.bResult;
		}
		return this.bResult;
	}

	// Token: 0x04006FEA RID: 28650
	[PublicizedFrom(EAccessModifier.Private)]
	public float ms;

	// Token: 0x04006FEB RID: 28651
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastBlinkTime;

	// Token: 0x04006FEC RID: 28652
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bResult = true;
}
