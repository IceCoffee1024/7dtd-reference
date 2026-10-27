using System;

// Token: 0x02000C1C RID: 3100
public class BiomeBluffDecoration
{
	// Token: 0x06005DED RID: 24045 RVA: 0x0024A387 File Offset: 0x00248587
	public BiomeBluffDecoration(string _name, float _prob, float _minScale, float _maxScale)
	{
		this.m_sName = _name;
		this.m_Prob = _prob;
		this.m_MinScale = _minScale;
		this.m_MaxScale = _maxScale;
	}

	// Token: 0x040048E8 RID: 18664
	public string m_sName;

	// Token: 0x040048E9 RID: 18665
	public float m_Prob;

	// Token: 0x040048EA RID: 18666
	public float m_MinScale;

	// Token: 0x040048EB RID: 18667
	public float m_MaxScale;
}
