using System;
using System.Diagnostics;
using Unity.Profiling;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	// Token: 0x020002BD RID: 701
	[NativeHeader("Runtime/Profiler/ScriptBindings/Sampler.bindings.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Profiler/Marker.h")]
	public sealed class CustomSampler : Sampler
	{
		// Token: 0x06001E07 RID: 7687 RVA: 0x0003189D File Offset: 0x0002FA9D
		internal CustomSampler()
		{
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000318A7 File Offset: 0x0002FAA7
		internal CustomSampler(IntPtr ptr)
		{
			this.m_Ptr = ptr;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000318B8 File Offset: 0x0002FAB8
		public static CustomSampler Create(string name, bool collectGpuData = false)
		{
			IntPtr intPtr = ProfilerUnsafeUtility.CreateMarker(name, 1, MarkerFlags.AvailabilityNonDevelopment | (collectGpuData ? MarkerFlags.SampleGPU : MarkerFlags.Default), 0);
			bool flag = intPtr == IntPtr.Zero;
			CustomSampler customSampler;
			if (flag)
			{
				customSampler = CustomSampler.s_InvalidCustomSampler;
			}
			else
			{
				customSampler = new CustomSampler(intPtr);
			}
			return customSampler;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000318FD File Offset: 0x0002FAFD
		[Conditional("ENABLE_PROFILER")]
		[IgnoredByDeepProfiler]
		public void Begin()
		{
			ProfilerUnsafeUtility.BeginSample(this.m_Ptr);
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x0003190C File Offset: 0x0002FB0C
		[IgnoredByDeepProfiler]
		[Conditional("ENABLE_PROFILER")]
		public void Begin(Object targetObject)
		{
			ProfilerUnsafeUtility.Internal_BeginWithObject(this.m_Ptr, targetObject);
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0003191C File Offset: 0x0002FB1C
		[IgnoredByDeepProfiler]
		[Conditional("ENABLE_PROFILER")]
		public void End()
		{
			ProfilerUnsafeUtility.EndSample(this.m_Ptr);
		}

		// Token: 0x040009F5 RID: 2549
		internal static CustomSampler s_InvalidCustomSampler = new CustomSampler();
	}
}
