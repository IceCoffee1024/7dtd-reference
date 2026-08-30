using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000096 RID: 150
	public static class NativeLeakDetection
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00004F10 File Offset: 0x00003110
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00004F28 File Offset: 0x00003128
		public static NativeLeakDetectionMode Mode
		{
			get
			{
				return UnsafeUtility.GetLeakDetectionMode();
			}
			set
			{
				bool flag = value < NativeLeakDetectionMode.Disabled || value > NativeLeakDetectionMode.EnabledWithStackTrace;
				if (flag)
				{
					throw new ArgumentException("NativeLeakDetectionMode out of range");
				}
				UnsafeUtility.SetLeakDetectionMode(value);
			}
		}
	}
}
