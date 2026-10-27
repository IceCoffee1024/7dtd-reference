using System;

// Token: 0x02000C56 RID: 3158
public class QTDataElement
{
	// Token: 0x06005F87 RID: 24455 RVA: 0x0025F5D0 File Offset: 0x0025D7D0
	public QTDataElement()
	{
		this.Data = null;
		this.Key = 0L;
		this.LLPosX = 0;
		this.LLPosY = 0;
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x0025F5F5 File Offset: 0x0025D7F5
	public QTDataElement(int _LLPosX, int _LLPosY, byte[] _Data)
	{
		this.LLPosX = _LLPosX;
		this.LLPosY = _LLPosY;
		this.Data = _Data;
	}

	// Token: 0x04004B1C RID: 19228
	public byte[] Data;

	// Token: 0x04004B1D RID: 19229
	public long Key;

	// Token: 0x04004B1E RID: 19230
	public int LLPosX;

	// Token: 0x04004B1F RID: 19231
	public int LLPosY;
}
