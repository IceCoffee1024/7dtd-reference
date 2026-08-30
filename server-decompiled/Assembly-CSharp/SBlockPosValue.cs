using System;

// Token: 0x02000194 RID: 404
public struct SBlockPosValue
{
	// Token: 0x06000C52 RID: 3154 RVA: 0x0004C6F7 File Offset: 0x0004A8F7
	public SBlockPosValue(Vector3i _blockPosition, BlockValue _blockValue)
	{
		this.blockPos = _blockPosition;
		this.blockValue = _blockValue;
	}

	// Token: 0x04000B0E RID: 2830
	public Vector3i blockPos;

	// Token: 0x04000B0F RID: 2831
	public BlockValue blockValue;
}
