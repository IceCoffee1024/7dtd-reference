using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.Jobs
{
	// Token: 0x020002BE RID: 702
	[JobProducerType(typeof(IJobParallelForTransformExtensions.TransformParallelForLoopStruct<>))]
	public interface IJobParallelForTransform
	{
		// Token: 0x06001E0E RID: 7694
		void Execute(int index, TransformAccess transform);
	}
}
