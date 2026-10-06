using System;

// Token: 0x02000C4B RID: 3147
public class QTDataElement
{
	// Token: 0x06005F62 RID: 24418 RVA: 0x0025E0A8 File Offset: 0x0025C2A8
	public QTDataElement()
	{
		this.Data = null;
		this.Key = 0L;
		this.LLPosX = 0;
		this.LLPosY = 0;
	}

	// Token: 0x06005F63 RID: 24419 RVA: 0x0025E0CD File Offset: 0x0025C2CD
	public QTDataElement(int _LLPosX, int _LLPosY, byte[] _Data)
	{
		this.LLPosX = _LLPosX;
		this.LLPosY = _LLPosY;
		this.Data = _Data;
	}

	// Token: 0x04004AF7 RID: 19191
	public byte[] Data;

	// Token: 0x04004AF8 RID: 19192
	public long Key;

	// Token: 0x04004AF9 RID: 19193
	public int LLPosX;

	// Token: 0x04004AFA RID: 19194
	public int LLPosY;
}
