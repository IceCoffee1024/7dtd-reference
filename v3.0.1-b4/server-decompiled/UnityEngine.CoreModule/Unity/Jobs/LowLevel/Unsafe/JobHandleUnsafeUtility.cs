using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x0200004E RID: 78
	public static class JobHandleUnsafeUtility
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00002E30 File Offset: 0x00001030
		public unsafe static JobHandle CombineDependencies(JobHandle* jobs, int count)
		{
			return JobHandle.CombineDependenciesInternalPtr((void*)jobs, count);
		}
	}
}
