using System;

// Token: 0x02000C00 RID: 3072
public class PoiMapDecal
{
	// Token: 0x06005DB0 RID: 23984 RVA: 0x00245B88 File Offset: 0x00243D88
	public PoiMapDecal(int _texIndex, BlockFace _face, float _prob)
	{
		this.textureIndex = _texIndex;
		this.face = _face;
		this.m_Prob = _prob;
	}

	// Token: 0x04004888 RID: 18568
	public int textureIndex;

	// Token: 0x04004889 RID: 18569
	public BlockFace face;

	// Token: 0x0400488A RID: 18570
	public float m_Prob;
}
