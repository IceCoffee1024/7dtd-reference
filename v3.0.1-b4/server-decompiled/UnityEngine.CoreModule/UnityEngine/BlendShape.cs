using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001C9 RID: 457
	[UsedByNativeCode]
	[Serializable]
	internal struct BlendShape
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00018E24 File Offset: 0x00017024
		// (set) Token: 0x060011E8 RID: 4584 RVA: 0x00018E3C File Offset: 0x0001703C
		public uint firstVertex
		{
			get
			{
				return this.m_FirstVertex;
			}
			set
			{
				this.m_FirstVertex = value;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00018E48 File Offset: 0x00017048
		// (set) Token: 0x060011EA RID: 4586 RVA: 0x00018E60 File Offset: 0x00017060
		public uint vertexCount
		{
			get
			{
				return this.m_VertexCount;
			}
			set
			{
				this.m_VertexCount = value;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x00018E6C File Offset: 0x0001706C
		// (set) Token: 0x060011EC RID: 4588 RVA: 0x00018E84 File Offset: 0x00017084
		public bool hasNormals
		{
			get
			{
				return this.m_HasNormals;
			}
			set
			{
				this.m_HasNormals = value;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00018E90 File Offset: 0x00017090
		// (set) Token: 0x060011EE RID: 4590 RVA: 0x00018EA8 File Offset: 0x000170A8
		public bool hasTangents
		{
			get
			{
				return this.m_HasTangents;
			}
			set
			{
				this.m_HasTangents = value;
			}
		}

		// Token: 0x04000640 RID: 1600
		[SerializeField]
		private uint m_FirstVertex;

		// Token: 0x04000641 RID: 1601
		[SerializeField]
		private uint m_VertexCount;

		// Token: 0x04000642 RID: 1602
		[SerializeField]
		private bool m_HasNormals;

		// Token: 0x04000643 RID: 1603
		[SerializeField]
		private bool m_HasTangents;
	}
}
