using System;

// Token: 0x02000196 RID: 406
public struct SBlockPosValue
{
	// Token: 0x06000C5D RID: 3165 RVA: 0x0004CB0F File Offset: 0x0004AD0F
	public SBlockPosValue(Vector3i _blockPosition, BlockValue _blockValue)
	{
		this.blockPos = _blockPosition;
		this.blockValue = _blockValue;
	}

	// Token: 0x04000B1B RID: 2843
	public Vector3i blockPos;

	// Token: 0x04000B1C RID: 2844
	public BlockValue blockValue;
}
