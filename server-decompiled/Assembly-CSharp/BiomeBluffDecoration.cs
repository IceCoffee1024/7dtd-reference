using System;

// Token: 0x02000C11 RID: 3089
public class BiomeBluffDecoration
{
	// Token: 0x06005DC8 RID: 24008 RVA: 0x00248E5F File Offset: 0x0024705F
	public BiomeBluffDecoration(string _name, float _prob, float _minScale, float _maxScale)
	{
		this.m_sName = _name;
		this.m_Prob = _prob;
		this.m_MinScale = _minScale;
		this.m_MaxScale = _maxScale;
	}

	// Token: 0x040048C3 RID: 18627
	public string m_sName;

	// Token: 0x040048C4 RID: 18628
	public float m_Prob;

	// Token: 0x040048C5 RID: 18629
	public float m_MinScale;

	// Token: 0x040048C6 RID: 18630
	public float m_MaxScale;
}
