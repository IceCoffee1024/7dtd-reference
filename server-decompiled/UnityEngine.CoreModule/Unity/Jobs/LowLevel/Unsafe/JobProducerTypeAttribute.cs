using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x0200004F RID: 79
	[AttributeUsage(AttributeTargets.Interface)]
	public sealed class JobProducerTypeAttribute : Attribute
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00002E49 File Offset: 0x00001049
		public Type ProducerType { get; }

		// Token: 0x060000F3 RID: 243 RVA: 0x00002E51 File Offset: 0x00001051
		public JobProducerTypeAttribute(Type producerType)
		{
			this.ProducerType = producerType;
		}
	}
}
