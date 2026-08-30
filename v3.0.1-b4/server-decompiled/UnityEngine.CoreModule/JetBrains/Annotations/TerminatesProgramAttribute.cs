using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000DD RID: 221
	[Obsolete("Use [ContractAnnotation('=> halt')] instead")]
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class TerminatesProgramAttribute : Attribute
	{
	}
}
