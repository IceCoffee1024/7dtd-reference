using System;

// Token: 0x02000C12 RID: 3090
public class PoiMapDecal
{
	// Token: 0x06005DC9 RID: 24009 RVA: 0x00248E84 File Offset: 0x00247084
	public PoiMapDecal(int _texIndex, BlockFace _face, float _prob)
	{
		this.textureIndex = _texIndex;
		this.face = _face;
		this.m_Prob = _prob;
	}

	// Token: 0x040048C7 RID: 18631
	public int textureIndex;

	// Token: 0x040048C8 RID: 18632
	public BlockFace face;

	// Token: 0x040048C9 RID: 18633
	public float m_Prob;
}
