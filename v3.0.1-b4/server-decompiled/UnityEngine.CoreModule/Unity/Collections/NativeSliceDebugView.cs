using System;

namespace Unity.Collections
{
	// Token: 0x020000A3 RID: 163
	internal sealed class NativeSliceDebugView<T> where T : struct
	{
		// Token: 0x0600033E RID: 830 RVA: 0x000063A1 File Offset: 0x000045A1
		public NativeSliceDebugView(NativeSlice<T> array)
		{
			this.m_Array = array;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000063B4 File Offset: 0x000045B4
		public T[] Items
		{
			get
			{
				return this.m_Array.ToArray();
			}
		}

		// Token: 0x04000241 RID: 577
		private NativeSlice<T> m_Array;
	}
}
