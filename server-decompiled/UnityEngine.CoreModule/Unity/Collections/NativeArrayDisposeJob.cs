using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Scripting;

namespace Unity.Collections
{
	// Token: 0x0200009D RID: 157
	[NativeClass(null)]
	internal struct NativeArrayDisposeJob : IJob
	{
		// Token: 0x06000310 RID: 784 RVA: 0x00005D71 File Offset: 0x00003F71
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00005D80 File Offset: 0x00003F80
		[RequiredByNativeCode]
		internal static void RegisterNativeArrayDisposeJobReflectionData()
		{
			IJobExtensions.EarlyJobInit<NativeArrayDisposeJob>();
		}

		// Token: 0x04000239 RID: 569
		internal NativeArrayDispose Data;
	}
}
