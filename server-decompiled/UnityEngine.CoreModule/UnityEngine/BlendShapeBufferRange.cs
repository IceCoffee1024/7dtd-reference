using System;

namespace UnityEngine
{
	// Token: 0x020001CA RID: 458
	[Serializable]
	public struct BlendShapeBufferRange
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x00018EB4 File Offset: 0x000170B4
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x00018ECC File Offset: 0x000170CC
		public uint startIndex
		{
			get
			{
				return this.m_StartIndex;
			}
			internal set
			{
				this.m_StartIndex = value;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x00018ED8 File Offset: 0x000170D8
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x00018EF0 File Offset: 0x000170F0
		public uint endIndex
		{
			get
			{
				return this.m_EndIndex;
			}
			internal set
			{
				this.m_EndIndex = value;
			}
		}

		// Token: 0x04000644 RID: 1604
		[SerializeField]
		private uint m_StartIndex;

		// Token: 0x04000645 RID: 1605
		[SerializeField]
		private uint m_EndIndex;
	}
}
