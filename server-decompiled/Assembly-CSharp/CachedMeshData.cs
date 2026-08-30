using System;
using UnityEngine;

// Token: 0x020013F1 RID: 5105
[Serializable]
public class CachedMeshData
{
	// Token: 0x0600A056 RID: 41046 RVA: 0x003C42A8 File Offset: 0x003C24A8
	public bool ApproximatelyEquals(CachedMeshData other)
	{
		return this.name.Equals(other.name) && Mathf.Abs(this.vertexCount - other.vertexCount) < 10 && Mathf.Abs(this.triCount - other.triCount) < 10;
	}

	// Token: 0x04007910 RID: 30992
	public string name;

	// Token: 0x04007911 RID: 30993
	public int vertexCount;

	// Token: 0x04007912 RID: 30994
	public int triCount;
}
