using System;

// Token: 0x0200035C RID: 860
public class ChunkPreviewData
{
	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0008C23D File Offset: 0x0008A43D
	// (set) Token: 0x060018C6 RID: 6342 RVA: 0x0008C245 File Offset: 0x0008A445
	public Vector3i WorldPosition { get; set; }

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060018C7 RID: 6343 RVA: 0x0008C24E File Offset: 0x0008A44E
	// (set) Token: 0x060018C8 RID: 6344 RVA: 0x0008C256 File Offset: 0x0008A456
	public Prefab PrefabData { get; set; }

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0008C25F File Offset: 0x0008A45F
	// (set) Token: 0x060018CA RID: 6346 RVA: 0x0008C267 File Offset: 0x0008A467
	public PrefabInstance PrefabInstance { get; set; }
}
