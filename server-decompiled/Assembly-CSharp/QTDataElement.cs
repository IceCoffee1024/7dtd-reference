using System;

// Token: 0x02000C27 RID: 3111
public class QTDataElement
{
	// Token: 0x06005EBF RID: 24255 RVA: 0x00257EFC File Offset: 0x002560FC
	public QTDataElement()
	{
		this.Data = null;
		this.Key = 0L;
		this.LLPosX = 0;
		this.LLPosY = 0;
	}

	// Token: 0x06005EC0 RID: 24256 RVA: 0x00257F21 File Offset: 0x00256121
	public QTDataElement(int _LLPosX, int _LLPosY, byte[] _Data)
	{
		this.LLPosX = _LLPosX;
		this.LLPosY = _LLPosY;
		this.Data = _Data;
	}

	// Token: 0x04004A1B RID: 18971
	public byte[] Data;

	// Token: 0x04004A1C RID: 18972
	public long Key;

	// Token: 0x04004A1D RID: 18973
	public int LLPosX;

	// Token: 0x04004A1E RID: 18974
	public int LLPosY;
}
