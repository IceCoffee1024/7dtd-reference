using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000B5 RID: 181
	public static class NativeArrayUnsafeUtility
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00006534 File Offset: 0x00004734
		public unsafe static NativeArray<T> ConvertExistingDataToNativeArray<T>(void* dataPointer, int length, Allocator allocator) where T : struct
		{
			return new NativeArray<T>
			{
				m_Buffer = dataPointer,
				m_Length = length,
				m_AllocatorLabel = allocator
			};
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000656C File Offset: 0x0000476C
		public unsafe static void* GetUnsafePtr<T>(this NativeArray<T> nativeArray) where T : struct
		{
			return nativeArray.m_Buffer;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00006584 File Offset: 0x00004784
		public unsafe static void* GetUnsafeReadOnlyPtr<T>(this NativeArray<T> nativeArray) where T : struct
		{
			return nativeArray.m_Buffer;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000659C File Offset: 0x0000479C
		public unsafe static void* GetUnsafeReadOnlyPtr<T>(this NativeArray<T>.ReadOnly nativeArray) where T : struct
		{
			return nativeArray.m_Buffer;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000065B4 File Offset: 0x000047B4
		public unsafe static void* GetUnsafeBufferPointerWithoutChecks<T>(NativeArray<T> nativeArray) where T : struct
		{
			return nativeArray.m_Buffer;
		}
	}
}
