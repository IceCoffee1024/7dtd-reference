using System;
using UnityEngine;

// Token: 0x020013D6 RID: 5078
[Serializable]
public class CachedMeshData
{
	// Token: 0x06009F59 RID: 40793 RVA: 0x003BEF64 File Offset: 0x003BD164
	public bool ApproximatelyEquals(CachedMeshData other)
	{
		return this.name.Equals(other.name) && Mathf.Abs(this.vertexCount - other.vertexCount) < 10 && Mathf.Abs(this.triCount - other.triCount) < 10;
	}

	// Token: 0x04007837 RID: 30775
	public string name;

	// Token: 0x04007838 RID: 30776
	public int vertexCount;

	// Token: 0x04007839 RID: 30777
	public int triCount;
}
