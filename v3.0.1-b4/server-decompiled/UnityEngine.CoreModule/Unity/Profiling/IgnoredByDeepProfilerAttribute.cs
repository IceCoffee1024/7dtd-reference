using System;
using UnityEngine.Scripting;

namespace Unity.Profiling
{
	// Token: 0x02000057 RID: 87
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = false)]
	[RequiredByNativeCode]
	public sealed class IgnoredByDeepProfilerAttribute : Attribute
	{
	}
}
