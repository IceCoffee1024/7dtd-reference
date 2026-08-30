using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000043 RID: 67
	[JobProducerType(typeof(IJobForExtensions.ForJobStruct<>))]
	public interface IJobFor
	{
		// Token: 0x060000BB RID: 187
		void Execute(int index);
	}
}
