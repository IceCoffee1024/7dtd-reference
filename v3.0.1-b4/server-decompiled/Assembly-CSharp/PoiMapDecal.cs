using System;

// Token: 0x02000BEE RID: 3054
public class PoiMapDecal
{
	// Token: 0x06005D26 RID: 23846 RVA: 0x00242DE4 File Offset: 0x00240FE4
	public PoiMapDecal(int _texIndex, BlockFace _face, float _prob)
	{
		this.textureIndex = _texIndex;
		this.face = _face;
		this.m_Prob = _prob;
	}

	// Token: 0x040047EC RID: 18412
	public int textureIndex;

	// Token: 0x040047ED RID: 18413
	public BlockFace face;

	// Token: 0x040047EE RID: 18414
	public float m_Prob;
}
