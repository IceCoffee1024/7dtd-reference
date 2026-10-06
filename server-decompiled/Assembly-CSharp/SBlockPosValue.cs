using System;

// Token: 0x02000197 RID: 407
public struct SBlockPosValue
{
	// Token: 0x06000C54 RID: 3156 RVA: 0x0004CB13 File Offset: 0x0004AD13
	public SBlockPosValue(Vector3i _blockPosition, BlockValue _blockValue)
	{
		this.blockPos = _blockPosition;
		this.blockValue = _blockValue;
	}

	// Token: 0x04000B14 RID: 2836
	public Vector3i blockPos;

	// Token: 0x04000B15 RID: 2837
	public BlockValue blockValue;
}
