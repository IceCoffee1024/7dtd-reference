using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000047 RID: 71
	[JobProducerType(typeof(IJobParallelForExtensions.ParallelForJobStruct<>))]
	public interface IJobParallelFor
	{
		// Token: 0x060000CB RID: 203
		void Execute(int index);
	}
}
