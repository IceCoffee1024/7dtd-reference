using System;
using UnityEngine.Bindings;

namespace UnityEngine.U2D
{
	// Token: 0x020002B4 RID: 692
	[VisibleToOtherModules]
	internal struct SpriteChannelInfo
	{
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x000308E8 File Offset: 0x0002EAE8
		// (set) Token: 0x06001D78 RID: 7544 RVA: 0x00030905 File Offset: 0x0002EB05
		public unsafe void* buffer
		{
			get
			{
				return (void*)this.m_Buffer;
			}
			set
			{
				this.m_Buffer = (IntPtr)value;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x00030914 File Offset: 0x0002EB14
		// (set) Token: 0x06001D7A RID: 7546 RVA: 0x0003092C File Offset: 0x0002EB2C
		public int count
		{
			get
			{
				return this.m_Count;
			}
			set
			{
				this.m_Count = value;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x00030938 File Offset: 0x0002EB38
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x00030950 File Offset: 0x0002EB50
		public int offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x0003095C File Offset: 0x0002EB5C
		// (set) Token: 0x06001D7E RID: 7550 RVA: 0x00030974 File Offset: 0x0002EB74
		public int stride
		{
			get
			{
				return this.m_Stride;
			}
			set
			{
				this.m_Stride = value;
			}
		}

		// Token: 0x040009D9 RID: 2521
		[NativeName("buffer")]
		private IntPtr m_Buffer;

		// Token: 0x040009DA RID: 2522
		[NativeName("count")]
		private int m_Count;

		// Token: 0x040009DB RID: 2523
		[NativeName("offset")]
		private int m_Offset;

		// Token: 0x040009DC RID: 2524
		[NativeName("stride")]
		private int m_Stride;
	}
}
