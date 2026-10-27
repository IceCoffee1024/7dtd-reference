using System;

// Token: 0x02000390 RID: 912
public class DynamicMeshUpdateData
{
	// Token: 0x06001B19 RID: 6937 RVA: 0x000A12E7 File Offset: 0x0009F4E7
	public string ToDebugLocation()
	{
		return this.ChunkPosition.x.ToString() + "," + this.ChunkPosition.z.ToString();
	}

	// Token: 0x04001170 RID: 4464
	public Vector3i ChunkPosition;

	// Token: 0x04001171 RID: 4465
	public long Key;

	// Token: 0x04001172 RID: 4466
	public float MaxTime;

	// Token: 0x04001173 RID: 4467
	public float UpdateTime;

	// Token: 0x04001174 RID: 4468
	public bool IsUrgent;

	// Token: 0x04001175 RID: 4469
	public bool AddToThread;
}
