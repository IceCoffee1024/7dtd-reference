using System;

namespace Unity.Collections
{
	// Token: 0x0200009F RID: 159
	internal sealed class NativeArrayReadOnlyDebugView<T> where T : struct
	{
		// Token: 0x06000314 RID: 788 RVA: 0x00005E15 File Offset: 0x00004015
		public NativeArrayReadOnlyDebugView(NativeArray<T>.ReadOnly array)
		{
			this.m_Array = array;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00005E28 File Offset: 0x00004028
		public T[] Items
		{
			get
			{
				bool flag = !this.m_Array.IsCreated;
				T[] array;
				if (flag)
				{
					array = null;
				}
				else
				{
					array = this.m_Array.ToArray();
				}
				return array;
			}
		}

		// Token: 0x0400023B RID: 571
		private NativeArray<T>.ReadOnly m_Array;
	}
}
