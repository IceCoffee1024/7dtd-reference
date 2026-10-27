using System;

// Token: 0x0200035E RID: 862
public class ChunkPreviewData
{
	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0008C471 File Offset: 0x0008A671
	// (set) Token: 0x060018CA RID: 6346 RVA: 0x0008C479 File Offset: 0x0008A679
	public Vector3i WorldPosition { get; set; }

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060018CB RID: 6347 RVA: 0x0008C482 File Offset: 0x0008A682
	// (set) Token: 0x060018CC RID: 6348 RVA: 0x0008C48A File Offset: 0x0008A68A
	public Prefab PrefabData { get; set; }

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060018CD RID: 6349 RVA: 0x0008C493 File Offset: 0x0008A693
	// (set) Token: 0x060018CE RID: 6350 RVA: 0x0008C49B File Offset: 0x0008A69B
	public PrefabInstance PrefabInstance { get; set; }
}
