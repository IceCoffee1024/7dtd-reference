using System;

// Token: 0x02000BEC RID: 3052
public class BiomePrefabDecoration
{
	// Token: 0x06005D24 RID: 23844 RVA: 0x00242D9A File Offset: 0x00240F9A
	public BiomePrefabDecoration(string _prefabName, float _prob, bool _isDecorateOnSlopes, int _checkResource = 2147483647)
	{
		this.prefabName = _prefabName;
		this.prob = _prob;
		this.checkResourceOffsetY = _checkResource;
		this.isDecorateOnSlopes = _isDecorateOnSlopes;
	}

	// Token: 0x040047E4 RID: 18404
	public string prefabName;

	// Token: 0x040047E5 RID: 18405
	public float prob;

	// Token: 0x040047E6 RID: 18406
	public int checkResourceOffsetY;

	// Token: 0x040047E7 RID: 18407
	public bool isDecorateOnSlopes;
}
