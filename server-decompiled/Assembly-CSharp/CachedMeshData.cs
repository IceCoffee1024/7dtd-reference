using System;
using UnityEngine;

// Token: 0x02001411 RID: 5137
[Serializable]
public class CachedMeshData
{
	// Token: 0x0600A101 RID: 41217 RVA: 0x003C8E44 File Offset: 0x003C7044
	public bool ApproximatelyEquals(CachedMeshData other)
	{
		return this.name.Equals(other.name) && Mathf.Abs(this.vertexCount - other.vertexCount) < 10 && Mathf.Abs(this.triCount - other.triCount) < 10;
	}

	// Token: 0x0400798B RID: 31115
	public string name;

	// Token: 0x0400798C RID: 31116
	public int vertexCount;

	// Token: 0x0400798D RID: 31117
	public int triCount;
}
