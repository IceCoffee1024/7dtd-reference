using System;

namespace UnityEngine
{
	// Token: 0x0200017F RID: 383
	[Flags]
	public enum ComputeBufferType
	{
		// Token: 0x040004B7 RID: 1207
		Default = 0,
		// Token: 0x040004B8 RID: 1208
		Raw = 1,
		// Token: 0x040004B9 RID: 1209
		Append = 2,
		// Token: 0x040004BA RID: 1210
		Counter = 4,
		// Token: 0x040004BB RID: 1211
		Constant = 8,
		// Token: 0x040004BC RID: 1212
		Structured = 16,
		// Token: 0x040004BD RID: 1213
		[Obsolete("Enum member DrawIndirect has been deprecated. Use IndirectArguments instead (UnityUpgradable) -> IndirectArguments", false)]
		DrawIndirect = 256,
		// Token: 0x040004BE RID: 1214
		IndirectArguments = 256,
		// Token: 0x040004BF RID: 1215
		[Obsolete("Enum member GPUMemory has been deprecated. All compute buffers now follow the behavior previously defined by this member.", false)]
		GPUMemory = 512
	}
}
