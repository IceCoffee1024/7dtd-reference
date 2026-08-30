using System;

// Token: 0x02000C01 RID: 3073
public class PoiMapBlock
{
	// Token: 0x06005DB1 RID: 23985 RVA: 0x00245BA5 File Offset: 0x00243DA5
	public PoiMapBlock(BlockValue _blockValue, float _prob, int _offset)
	{
		this.blockValue = _blockValue;
		this.m_Prob = _prob;
		this.offset = _offset;
	}

	// Token: 0x0400488B RID: 18571
	public BlockValue blockValue;

	// Token: 0x0400488C RID: 18572
	public float m_Prob;

	// Token: 0x0400488D RID: 18573
	public int offset;
}
