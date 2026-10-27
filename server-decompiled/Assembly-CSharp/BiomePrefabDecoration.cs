using System;

// Token: 0x02000C1B RID: 3099
public class BiomePrefabDecoration
{
	// Token: 0x06005DEC RID: 24044 RVA: 0x0024A362 File Offset: 0x00248562
	public BiomePrefabDecoration(string _prefabName, float _prob, bool _isDecorateOnSlopes, int _checkResource = 2147483647)
	{
		this.prefabName = _prefabName;
		this.prob = _prob;
		this.checkResourceOffsetY = _checkResource;
		this.isDecorateOnSlopes = _isDecorateOnSlopes;
	}

	// Token: 0x040048E4 RID: 18660
	public string prefabName;

	// Token: 0x040048E5 RID: 18661
	public float prob;

	// Token: 0x040048E6 RID: 18662
	public int checkResourceOffsetY;

	// Token: 0x040048E7 RID: 18663
	public bool isDecorateOnSlopes;
}
