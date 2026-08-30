using System;
using System.Collections.Generic;

// Token: 0x02000BEA RID: 3050
public class BiomeLayer
{
	// Token: 0x06005D1F RID: 23839 RVA: 0x00242BD4 File Offset: 0x00240DD4
	public BiomeLayer(int _depth, BiomeBlockDecoration _bb)
	{
		this.m_Block = _bb;
		this.m_Depth = _depth;
		this.m_Resources = new List<BiomeBlockDecoration>();
		this.SumResourceProbs = new List<float>();
		this.MaxResourceProb = 0f;
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x00242C0C File Offset: 0x00240E0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BiomeLayer()
	{
	}

	// Token: 0x06005D21 RID: 23841 RVA: 0x00242C34 File Offset: 0x00240E34
	public void AddResource(BiomeBlockDecoration _res)
	{
		this.m_Resources.Add(_res);
		this.MaxResourceProb = Utils.FastMax(_res.prob, this.MaxResourceProb);
		int count = this.SumResourceProbs.Count;
		this.SumResourceProbs.Add((count > 0) ? (this.SumResourceProbs[count - 1] + _res.prob) : _res.prob);
	}

	// Token: 0x040047DA RID: 18394
	public BiomeBlockDecoration m_Block;

	// Token: 0x040047DB RID: 18395
	public int m_Depth;

	// Token: 0x040047DC RID: 18396
	public List<BiomeBlockDecoration> m_Resources;

	// Token: 0x040047DD RID: 18397
	public List<float> SumResourceProbs;

	// Token: 0x040047DE RID: 18398
	public float MaxResourceProb;
}
