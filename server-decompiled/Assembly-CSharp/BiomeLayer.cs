using System;
using System.Collections.Generic;

// Token: 0x02000C0E RID: 3086
public class BiomeLayer
{
	// Token: 0x06005DC2 RID: 24002 RVA: 0x00248C74 File Offset: 0x00246E74
	public BiomeLayer(int _depth, BiomeBlockDecoration _bb)
	{
		this.m_Block = _bb;
		this.m_Depth = _depth;
		this.m_Resources = new List<BiomeBlockDecoration>();
		this.SumResourceProbs = new List<float>();
		this.MaxResourceProb = 0f;
	}

	// Token: 0x06005DC3 RID: 24003 RVA: 0x00248CAC File Offset: 0x00246EAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BiomeLayer()
	{
	}

	// Token: 0x06005DC4 RID: 24004 RVA: 0x00248CD4 File Offset: 0x00246ED4
	public void AddResource(BiomeBlockDecoration _res)
	{
		this.m_Resources.Add(_res);
		this.MaxResourceProb = Utils.FastMax(_res.prob, this.MaxResourceProb);
		int count = this.SumResourceProbs.Count;
		this.SumResourceProbs.Add((count > 0) ? (this.SumResourceProbs[count - 1] + _res.prob) : _res.prob);
	}

	// Token: 0x040048B5 RID: 18613
	public BiomeBlockDecoration m_Block;

	// Token: 0x040048B6 RID: 18614
	public int m_Depth;

	// Token: 0x040048B7 RID: 18615
	public List<BiomeBlockDecoration> m_Resources;

	// Token: 0x040048B8 RID: 18616
	public List<float> SumResourceProbs;

	// Token: 0x040048B9 RID: 18617
	public float MaxResourceProb;
}
