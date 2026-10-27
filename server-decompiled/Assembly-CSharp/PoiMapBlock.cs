using System;

// Token: 0x02000C1E RID: 3102
public class PoiMapBlock
{
	// Token: 0x06005DEF RID: 24047 RVA: 0x0024A3C9 File Offset: 0x002485C9
	public PoiMapBlock(BlockValue _blockValue, float _prob, int _offset)
	{
		this.blockValue = _blockValue;
		this.m_Prob = _prob;
		this.offset = _offset;
	}

	// Token: 0x040048EF RID: 18671
	public BlockValue blockValue;

	// Token: 0x040048F0 RID: 18672
	public float m_Prob;

	// Token: 0x040048F1 RID: 18673
	public int offset;
}
