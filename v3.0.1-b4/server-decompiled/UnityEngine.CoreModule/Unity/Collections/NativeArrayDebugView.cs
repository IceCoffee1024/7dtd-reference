using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200009E RID: 158
	internal sealed class NativeArrayDebugView<T> where T : struct
	{
		// Token: 0x06000312 RID: 786 RVA: 0x00005D89 File Offset: 0x00003F89
		public NativeArrayDebugView(NativeArray<T> array)
		{
			this.m_Array = array;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00005D9C File Offset: 0x00003F9C
		public unsafe T[] Items
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
					int length = this.m_Array.m_Length;
					T[] array2 = new T[length];
					GCHandle gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
					IntPtr intPtr = gchandle.AddrOfPinnedObject();
					UnsafeUtility.MemCpy((void*)intPtr, this.m_Array.m_Buffer, (long)(length * UnsafeUtility.SizeOf<T>()));
					gchandle.Free();
					array = array2;
				}
				return array;
			}
		}

		// Token: 0x0400023A RID: 570
		private NativeArray<T> m_Array;
	}
}
