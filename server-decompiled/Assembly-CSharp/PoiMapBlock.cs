using System;

// Token: 0x02000C13 RID: 3091
public class PoiMapBlock
{
	// Token: 0x06005DCA RID: 24010 RVA: 0x00248EA1 File Offset: 0x002470A1
	public PoiMapBlock(BlockValue _blockValue, float _prob, int _offset)
	{
		this.blockValue = _blockValue;
		this.m_Prob = _prob;
		this.offset = _offset;
	}

	// Token: 0x040048CA RID: 18634
	public BlockValue blockValue;

	// Token: 0x040048CB RID: 18635
	public float m_Prob;

	// Token: 0x040048CC RID: 18636
	public int offset;
}
