using System;

// Token: 0x0200038E RID: 910
public class DynamicMeshUpdateData
{
	// Token: 0x06001B15 RID: 6933 RVA: 0x000A10B3 File Offset: 0x0009F2B3
	public string ToDebugLocation()
	{
		return this.ChunkPosition.x.ToString() + "," + this.ChunkPosition.z.ToString();
	}

	// Token: 0x04001160 RID: 4448
	public Vector3i ChunkPosition;

	// Token: 0x04001161 RID: 4449
	public long Key;

	// Token: 0x04001162 RID: 4450
	public float MaxTime;

	// Token: 0x04001163 RID: 4451
	public float UpdateTime;

	// Token: 0x04001164 RID: 4452
	public bool IsUrgent;

	// Token: 0x04001165 RID: 4453
	public bool AddToThread;
}
