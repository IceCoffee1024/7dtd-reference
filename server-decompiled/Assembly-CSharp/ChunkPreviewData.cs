using System;

// Token: 0x02000359 RID: 857
public class ChunkPreviewData
{
	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060018CA RID: 6346 RVA: 0x0008BFBD File Offset: 0x0008A1BD
	// (set) Token: 0x060018CB RID: 6347 RVA: 0x0008BFC5 File Offset: 0x0008A1C5
	public Vector3i WorldPosition { get; set; }

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060018CC RID: 6348 RVA: 0x0008BFCE File Offset: 0x0008A1CE
	// (set) Token: 0x060018CD RID: 6349 RVA: 0x0008BFD6 File Offset: 0x0008A1D6
	public Prefab PrefabData { get; set; }

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060018CE RID: 6350 RVA: 0x0008BFDF File Offset: 0x0008A1DF
	// (set) Token: 0x060018CF RID: 6351 RVA: 0x0008BFE7 File Offset: 0x0008A1E7
	public PrefabInstance PrefabInstance { get; set; }
}
