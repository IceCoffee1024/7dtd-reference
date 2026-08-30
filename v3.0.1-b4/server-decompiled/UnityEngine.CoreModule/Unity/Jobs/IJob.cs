using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x0200003F RID: 63
	[JobProducerType(typeof(IJobExtensions.JobStruct<>))]
	public interface IJob
	{
		// Token: 0x060000AD RID: 173
		void Execute();
	}
}
