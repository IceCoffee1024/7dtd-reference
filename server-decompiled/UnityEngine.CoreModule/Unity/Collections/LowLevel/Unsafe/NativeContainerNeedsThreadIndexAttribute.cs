using System;
using UnityEngine.Scripting;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000B0 RID: 176
	[Obsolete("Use NativeSetThreadIndexAttribute instead")]
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Struct)]
	public sealed class NativeContainerNeedsThreadIndexAttribute : Attribute
	{
	}
}
