using System;
using UnityEngine;

// Token: 0x02001423 RID: 5155
[Serializable]
public class CachedMeshData
{
	// Token: 0x0600A159 RID: 41305 RVA: 0x003CB248 File Offset: 0x003C9448
	public bool ApproximatelyEquals(CachedMeshData other)
	{
		return this.name.Equals(other.name) && Mathf.Abs(this.vertexCount - other.vertexCount) < 10 && Mathf.Abs(this.triCount - other.triCount) < 10;
	}

	// Token: 0x040079CC RID: 31180
	public string name;

	// Token: 0x040079CD RID: 31181
	public int vertexCount;

	// Token: 0x040079CE RID: 31182
	public int triCount;
}
