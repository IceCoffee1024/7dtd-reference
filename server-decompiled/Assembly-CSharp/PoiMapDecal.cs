using System;

// Token: 0x02000C1D RID: 3101
public class PoiMapDecal
{
	// Token: 0x06005DEE RID: 24046 RVA: 0x0024A3AC File Offset: 0x002485AC
	public PoiMapDecal(int _texIndex, BlockFace _face, float _prob)
	{
		this.textureIndex = _texIndex;
		this.face = _face;
		this.m_Prob = _prob;
	}

	// Token: 0x040048EC RID: 18668
	public int textureIndex;

	// Token: 0x040048ED RID: 18669
	public BlockFace face;

	// Token: 0x040048EE RID: 18670
	public float m_Prob;
}
