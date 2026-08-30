using System;

// Token: 0x0200120E RID: 4622
public class GUIBlinker
{
	// Token: 0x0600937D RID: 37757 RVA: 0x0037CC75 File Offset: 0x0037AE75
	public GUIBlinker(float _ms)
	{
		this.ms = _ms;
	}

	// Token: 0x0600937E RID: 37758 RVA: 0x0037CC8B File Offset: 0x0037AE8B
	public bool Draw(float _curTime)
	{
		if (_curTime - this.lastBlinkTime > this.ms)
		{
			this.lastBlinkTime = _curTime;
			this.bResult = !this.bResult;
		}
		return this.bResult;
	}

	// Token: 0x04006E66 RID: 28262
	[PublicizedFrom(EAccessModifier.Private)]
	public float ms;

	// Token: 0x04006E67 RID: 28263
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastBlinkTime;

	// Token: 0x04006E68 RID: 28264
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bResult = true;
}
