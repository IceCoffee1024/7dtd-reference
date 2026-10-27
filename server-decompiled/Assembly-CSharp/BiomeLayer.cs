using System;
using System.Collections.Generic;

// Token: 0x02000C19 RID: 3097
public class BiomeLayer
{
	// Token: 0x06005DE7 RID: 24039 RVA: 0x0024A19C File Offset: 0x0024839C
	public BiomeLayer(int _depth, BiomeBlockDecoration _bb)
	{
		this.m_Block = _bb;
		this.m_Depth = _depth;
		this.m_Resources = new List<BiomeBlockDecoration>();
		this.SumResourceProbs = new List<float>();
		this.MaxResourceProb = 0f;
	}

	// Token: 0x06005DE8 RID: 24040 RVA: 0x0024A1D4 File Offset: 0x002483D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BiomeLayer()
	{
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x0024A1FC File Offset: 0x002483FC
	public void AddResource(BiomeBlockDecoration _res)
	{
		this.m_Resources.Add(_res);
		this.MaxResourceProb = Utils.FastMax(_res.prob, this.MaxResourceProb);
		int count = this.SumResourceProbs.Count;
		this.SumResourceProbs.Add((count > 0) ? (this.SumResourceProbs[count - 1] + _res.prob) : _res.prob);
	}

	// Token: 0x040048DA RID: 18650
	public BiomeBlockDecoration m_Block;

	// Token: 0x040048DB RID: 18651
	public int m_Depth;

	// Token: 0x040048DC RID: 18652
	public List<BiomeBlockDecoration> m_Resources;

	// Token: 0x040048DD RID: 18653
	public List<float> SumResourceProbs;

	// Token: 0x040048DE RID: 18654
	public float MaxResourceProb;
}
