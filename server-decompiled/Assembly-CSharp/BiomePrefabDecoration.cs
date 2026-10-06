using System;

// Token: 0x02000C10 RID: 3088
public class BiomePrefabDecoration
{
	// Token: 0x06005DC7 RID: 24007 RVA: 0x00248E3A File Offset: 0x0024703A
	public BiomePrefabDecoration(string _prefabName, float _prob, bool _isDecorateOnSlopes, int _checkResource = 2147483647)
	{
		this.prefabName = _prefabName;
		this.prob = _prob;
		this.checkResourceOffsetY = _checkResource;
		this.isDecorateOnSlopes = _isDecorateOnSlopes;
	}

	// Token: 0x040048BF RID: 18623
	public string prefabName;

	// Token: 0x040048C0 RID: 18624
	public float prob;

	// Token: 0x040048C1 RID: 18625
	public int checkResourceOffsetY;

	// Token: 0x040048C2 RID: 18626
	public bool isDecorateOnSlopes;
}
