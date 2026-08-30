using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000B6 RID: 182
	public static class NativeSliceUnsafeUtility
	{
		// Token: 0x0600035D RID: 861 RVA: 0x000065CC File Offset: 0x000047CC
		public unsafe static NativeSlice<T> ConvertExistingDataToNativeSlice<T>(void* dataPointer, int stride, int length) where T : struct
		{
			bool flag = length < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid length of '{0}'. It must be greater than 0.", length), "length");
			}
			bool flag2 = stride < 0;
			if (flag2)
			{
				throw new ArgumentException(string.Format("Invalid stride '{0}'. It must be greater than 0.", stride), "stride");
			}
			return new NativeSlice<T>
			{
				m_Stride = stride,
				m_Buffer = (byte*)dataPointer,
				m_Length = length
			};
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000664C File Offset: 0x0000484C
		public unsafe static void* GetUnsafePtr<T>(this NativeSlice<T> nativeSlice) where T : struct
		{
			return (void*)nativeSlice.m_Buffer;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00006664 File Offset: 0x00004864
		public unsafe static void* GetUnsafeReadOnlyPtr<T>(this NativeSlice<T> nativeSlice) where T : struct
		{
			return (void*)nativeSlice.m_Buffer;
		}
	}
}
