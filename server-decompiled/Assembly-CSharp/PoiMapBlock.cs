using System;

// Token: 0x02000BEF RID: 3055
public class PoiMapBlock
{
	// Token: 0x06005D27 RID: 23847 RVA: 0x00242E01 File Offset: 0x00241001
	public PoiMapBlock(BlockValue _blockValue, float _prob, int _offset)
	{
		this.blockValue = _blockValue;
		this.m_Prob = _prob;
		this.offset = _offset;
	}

	// Token: 0x040047EF RID: 18415
	public BlockValue blockValue;

	// Token: 0x040047F0 RID: 18416
	public float m_Prob;

	// Token: 0x040047F1 RID: 18417
	public int offset;
}
