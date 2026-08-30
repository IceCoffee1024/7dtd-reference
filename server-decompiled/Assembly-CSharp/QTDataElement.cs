using System;

// Token: 0x02000C39 RID: 3129
public class QTDataElement
{
	// Token: 0x06005F49 RID: 24393 RVA: 0x0025ADAC File Offset: 0x00258FAC
	public QTDataElement()
	{
		this.Data = null;
		this.Key = 0L;
		this.LLPosX = 0;
		this.LLPosY = 0;
	}

	// Token: 0x06005F4A RID: 24394 RVA: 0x0025ADD1 File Offset: 0x00258FD1
	public QTDataElement(int _LLPosX, int _LLPosY, byte[] _Data)
	{
		this.LLPosX = _LLPosX;
		this.LLPosY = _LLPosY;
		this.Data = _Data;
	}

	// Token: 0x04004AB8 RID: 19128
	public byte[] Data;

	// Token: 0x04004AB9 RID: 19129
	public long Key;

	// Token: 0x04004ABA RID: 19130
	public int LLPosX;

	// Token: 0x04004ABB RID: 19131
	public int LLPosY;
}
