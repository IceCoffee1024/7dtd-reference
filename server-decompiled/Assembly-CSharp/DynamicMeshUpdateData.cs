using System;

// Token: 0x02000387 RID: 903
public class DynamicMeshUpdateData
{
	// Token: 0x06001AFD RID: 6909 RVA: 0x000A06F3 File Offset: 0x0009E8F3
	public string ToDebugLocation()
	{
		return this.ChunkPosition.x.ToString() + "," + this.ChunkPosition.z.ToString();
	}

	// Token: 0x04001152 RID: 4434
	public Vector3i ChunkPosition;

	// Token: 0x04001153 RID: 4435
	public long Key;

	// Token: 0x04001154 RID: 4436
	public float MaxTime;

	// Token: 0x04001155 RID: 4437
	public float UpdateTime;

	// Token: 0x04001156 RID: 4438
	public bool IsUrgent;

	// Token: 0x04001157 RID: 4439
	public bool AddToThread;
}
