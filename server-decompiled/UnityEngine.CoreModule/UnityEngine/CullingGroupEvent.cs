using System;

namespace UnityEngine
{
	// Token: 0x02000110 RID: 272
	public struct CullingGroupEvent
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0000904C File Offset: 0x0000724C
		public int index
		{
			get
			{
				return this.m_Index;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00009064 File Offset: 0x00007264
		public bool isVisible
		{
			get
			{
				return (this.m_ThisState & 128) > 0;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00009088 File Offset: 0x00007288
		public bool wasVisible
		{
			get
			{
				return (this.m_PrevState & 128) > 0;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000090AC File Offset: 0x000072AC
		public bool hasBecomeVisible
		{
			get
			{
				return this.isVisible && !this.wasVisible;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x000090D4 File Offset: 0x000072D4
		public bool hasBecomeInvisible
		{
			get
			{
				return !this.isVisible && this.wasVisible;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x000090F8 File Offset: 0x000072F8
		public int currentDistance
		{
			get
			{
				return (int)(this.m_ThisState & 127);
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00009114 File Offset: 0x00007314
		public int previousDistance
		{
			get
			{
				return (int)(this.m_PrevState & 127);
			}
		}

		// Token: 0x0400038D RID: 909
		private int m_Index;

		// Token: 0x0400038E RID: 910
		private byte m_PrevState;

		// Token: 0x0400038F RID: 911
		private byte m_ThisState;

		// Token: 0x04000390 RID: 912
		private const byte kIsVisibleMask = 128;

		// Token: 0x04000391 RID: 913
		private const byte kDistanceMask = 127;
	}
}
