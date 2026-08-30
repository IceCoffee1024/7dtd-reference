using System;

// Token: 0x02000BED RID: 3053
public class BiomeBluffDecoration
{
	// Token: 0x06005D25 RID: 23845 RVA: 0x00242DBF File Offset: 0x00240FBF
	public BiomeBluffDecoration(string _name, float _prob, float _minScale, float _maxScale)
	{
		this.m_sName = _name;
		this.m_Prob = _prob;
		this.m_MinScale = _minScale;
		this.m_MaxScale = _maxScale;
	}

	// Token: 0x040047E8 RID: 18408
	public string m_sName;

	// Token: 0x040047E9 RID: 18409
	public float m_Prob;

	// Token: 0x040047EA RID: 18410
	public float m_MinScale;

	// Token: 0x040047EB RID: 18411
	public float m_MaxScale;
}
