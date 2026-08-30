using System;
using System.Diagnostics;
using Unity.Burst;

namespace Unity.Jobs
{
	// Token: 0x0200003E RID: 62
	internal static class JobValidationInternal
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckReflectionDataCorrect<T>(IntPtr reflectionData)
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000266C File Offset: 0x0000086C
		[BurstDiscard]
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckReflectionDataCorrectInternal<T>(IntPtr reflectionData, ref bool burstCompiled)
		{
			bool flag = reflectionData == IntPtr.Zero;
			if (flag)
			{
				throw new InvalidOperationException(string.Format("Reflection data was not set up by an Initialize() call. Support for burst compiled calls to Schedule depends on the Collections package.\n\nFor generic job types, please include [assembly: RegisterGenericJobType(typeof({0}))] in your source file.", typeof(T)));
			}
			burstCompiled = false;
		}
	}
}
