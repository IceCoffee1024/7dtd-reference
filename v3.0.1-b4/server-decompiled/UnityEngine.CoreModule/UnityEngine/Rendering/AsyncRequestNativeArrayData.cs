using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E2 RID: 994
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/AsyncGPUReadbackManaged.h")]
	internal struct AsyncRequestNativeArrayData
	{
		// Token: 0x06002176 RID: 8566 RVA: 0x00037828 File Offset: 0x00035A28
		public static AsyncRequestNativeArrayData CreateAndCheckAccess<T>(NativeArray<T> array) where T : struct
		{
			bool flag = array.m_AllocatorLabel == Allocator.Temp || array.m_AllocatorLabel == Allocator.TempJob;
			if (flag)
			{
				throw new ArgumentException("AsyncGPUReadback cannot use Temp memory as input since the result may only become available at an unspecified point in the future.");
			}
			return new AsyncRequestNativeArrayData
			{
				nativeArrayBuffer = array.GetUnsafePtr<T>(),
				lengthInBytes = (long)array.Length * (long)UnsafeUtility.SizeOf<T>()
			};
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x0003788C File Offset: 0x00035A8C
		public static AsyncRequestNativeArrayData CreateAndCheckAccess<T>(NativeSlice<T> array) where T : struct
		{
			return new AsyncRequestNativeArrayData
			{
				nativeArrayBuffer = array.GetUnsafePtr<T>(),
				lengthInBytes = (long)array.Length * (long)UnsafeUtility.SizeOf<T>()
			};
		}

		// Token: 0x04000B15 RID: 2837
		public unsafe void* nativeArrayBuffer;

		// Token: 0x04000B16 RID: 2838
		public long lengthInBytes;
	}
}
