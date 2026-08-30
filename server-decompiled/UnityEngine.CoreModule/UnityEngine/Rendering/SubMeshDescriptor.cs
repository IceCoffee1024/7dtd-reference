using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000408 RID: 1032
	public struct SubMeshDescriptor
	{
		// Token: 0x060021AE RID: 8622 RVA: 0x00037FE8 File Offset: 0x000361E8
		public SubMeshDescriptor(int indexStart, int indexCount, MeshTopology topology = MeshTopology.Triangles)
		{
			this.indexStart = indexStart;
			this.indexCount = indexCount;
			this.topology = topology;
			this.bounds = default(Bounds);
			this.baseVertex = 0;
			this.firstVertex = 0;
			this.vertexCount = 0;
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x00038036 File Offset: 0x00036236
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x0003803E File Offset: 0x0003623E
		public Bounds bounds { readonly get; set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x00038047 File Offset: 0x00036247
		// (set) Token: 0x060021B2 RID: 8626 RVA: 0x0003804F File Offset: 0x0003624F
		public MeshTopology topology { readonly get; set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x00038058 File Offset: 0x00036258
		// (set) Token: 0x060021B4 RID: 8628 RVA: 0x00038060 File Offset: 0x00036260
		public int indexStart { readonly get; set; }

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x00038069 File Offset: 0x00036269
		// (set) Token: 0x060021B6 RID: 8630 RVA: 0x00038071 File Offset: 0x00036271
		public int indexCount { readonly get; set; }

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x0003807A File Offset: 0x0003627A
		// (set) Token: 0x060021B8 RID: 8632 RVA: 0x00038082 File Offset: 0x00036282
		public int baseVertex { readonly get; set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x0003808B File Offset: 0x0003628B
		// (set) Token: 0x060021BA RID: 8634 RVA: 0x00038093 File Offset: 0x00036293
		public int firstVertex { readonly get; set; }

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x0003809C File Offset: 0x0003629C
		// (set) Token: 0x060021BC RID: 8636 RVA: 0x000380A4 File Offset: 0x000362A4
		public int vertexCount { readonly get; set; }

		// Token: 0x060021BD RID: 8637 RVA: 0x000380B0 File Offset: 0x000362B0
		public override string ToString()
		{
			return string.Format("(topo={0} indices={1},{2} vertices={3},{4} basevtx={5} bounds={6})", new object[] { this.topology, this.indexStart, this.indexCount, this.firstVertex, this.vertexCount, this.baseVertex, this.bounds });
		}
	}
}
