using System;

// Token: 0x02000355 RID: 853
public class ChunkPreviewData
{
	// Token: 0x170002EB RID: 747
	// (get) Token: 0x060018AD RID: 6317 RVA: 0x0008B87D File Offset: 0x00089A7D
	// (set) Token: 0x060018AE RID: 6318 RVA: 0x0008B885 File Offset: 0x00089A85
	public Vector3i WorldPosition { get; set; }

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x060018AF RID: 6319 RVA: 0x0008B88E File Offset: 0x00089A8E
	// (set) Token: 0x060018B0 RID: 6320 RVA: 0x0008B896 File Offset: 0x00089A96
	public Prefab PrefabData { get; set; }

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0008B89F File Offset: 0x00089A9F
	// (set) Token: 0x060018B2 RID: 6322 RVA: 0x0008B8A7 File Offset: 0x00089AA7
	public PrefabInstance PrefabInstance { get; set; }
}
