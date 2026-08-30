using System;

namespace UnityEngine
{
	// Token: 0x020001CD RID: 461
	public struct CombineInstance
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x00019390 File Offset: 0x00017590
		// (set) Token: 0x06001212 RID: 4626 RVA: 0x000193AD File Offset: 0x000175AD
		public Mesh mesh
		{
			get
			{
				return Mesh.FromInstanceID(this.m_MeshInstanceID);
			}
			set
			{
				this.m_MeshInstanceID = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x000193C8 File Offset: 0x000175C8
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x000193E0 File Offset: 0x000175E0
		public int subMeshIndex
		{
			get
			{
				return this.m_SubMeshIndex;
			}
			set
			{
				this.m_SubMeshIndex = value;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x000193EC File Offset: 0x000175EC
		// (set) Token: 0x06001216 RID: 4630 RVA: 0x00019404 File Offset: 0x00017604
		public Matrix4x4 transform
		{
			get
			{
				return this.m_Transform;
			}
			set
			{
				this.m_Transform = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00019410 File Offset: 0x00017610
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x00019428 File Offset: 0x00017628
		public Vector4 lightmapScaleOffset
		{
			get
			{
				return this.m_LightmapScaleOffset;
			}
			set
			{
				this.m_LightmapScaleOffset = value;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00019434 File Offset: 0x00017634
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x0001944C File Offset: 0x0001764C
		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				return this.m_RealtimeLightmapScaleOffset;
			}
			set
			{
				this.m_RealtimeLightmapScaleOffset = value;
			}
		}

		// Token: 0x04000650 RID: 1616
		private int m_MeshInstanceID;

		// Token: 0x04000651 RID: 1617
		private int m_SubMeshIndex;

		// Token: 0x04000652 RID: 1618
		private Matrix4x4 m_Transform;

		// Token: 0x04000653 RID: 1619
		private Vector4 m_LightmapScaleOffset;

		// Token: 0x04000654 RID: 1620
		private Vector4 m_RealtimeLightmapScaleOffset;
	}
}
