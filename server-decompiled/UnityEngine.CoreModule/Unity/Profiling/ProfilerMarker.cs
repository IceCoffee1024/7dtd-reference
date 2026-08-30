using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Scripting;

namespace Unity.Profiling
{
	// Token: 0x0200005B RID: 91
	[IgnoredByDeepProfiler]
	[UsedByNativeCode]
	public struct ProfilerMarker
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000319C File Offset: 0x0000139C
		public IntPtr Handle
		{
			get
			{
				return this.m_Ptr;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000031A4 File Offset: 0x000013A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(string name)
		{
			this.m_Ptr = ProfilerUnsafeUtility.CreateMarker(name, 1, MarkerFlags.Default, 0);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000031B6 File Offset: 0x000013B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ProfilerMarker(char* name, int nameLen)
		{
			this.m_Ptr = ProfilerUnsafeUtility.CreateMarker(name, nameLen, 1, MarkerFlags.Default, 0);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000031C9 File Offset: 0x000013C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(ProfilerCategory category, string name)
		{
			this.m_Ptr = ProfilerUnsafeUtility.CreateMarker(name, category, MarkerFlags.Default, 0);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000031E0 File Offset: 0x000013E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ProfilerMarker(ProfilerCategory category, char* name, int nameLen)
		{
			this.m_Ptr = ProfilerUnsafeUtility.CreateMarker(name, nameLen, category, MarkerFlags.Default, 0);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000031F8 File Offset: 0x000013F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(ProfilerCategory category, string name, MarkerFlags flags)
		{
			this.m_Ptr = ProfilerUnsafeUtility.CreateMarker(name, category, flags, 0);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000320F File Offset: 0x0000140F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ProfilerMarker(ProfilerCategory category, char* name, int nameLen, MarkerFlags flags)
		{
			this.m_Ptr = ProfilerUnsafeUtility.CreateMarker(name, nameLen, category, flags, 0);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00003228 File Offset: 0x00001428
		[Pure]
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin()
		{
			ProfilerUnsafeUtility.BeginSample(this.m_Ptr);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00003237 File Offset: 0x00001437
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin(Object contextUnityObject)
		{
			ProfilerUnsafeUtility.Internal_BeginWithObject(this.m_Ptr, contextUnityObject);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00003247 File Offset: 0x00001447
		[Conditional("ENABLE_PROFILER")]
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End()
		{
			ProfilerUnsafeUtility.EndSample(this.m_Ptr);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00003256 File Offset: 0x00001456
		[Conditional("ENABLE_PROFILER")]
		internal void GetName(ref string name)
		{
			name = ProfilerUnsafeUtility.Internal_GetName(this.m_Ptr);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00003268 File Offset: 0x00001468
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker.AutoScope Auto()
		{
			return new ProfilerMarker.AutoScope(this.m_Ptr);
		}

		// Token: 0x0400012A RID: 298
		[NativeDisableUnsafePtrRestriction]
		[NonSerialized]
		internal readonly IntPtr m_Ptr;

		// Token: 0x0200005C RID: 92
		[UsedByNativeCode]
		[IgnoredByDeepProfiler]
		public struct AutoScope : IDisposable
		{
			// Token: 0x06000148 RID: 328 RVA: 0x00003288 File Offset: 0x00001488
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal AutoScope(IntPtr markerPtr)
			{
				this.m_Ptr = markerPtr;
				bool flag = this.m_Ptr != IntPtr.Zero;
				if (flag)
				{
					ProfilerUnsafeUtility.BeginSample(markerPtr);
				}
			}

			// Token: 0x06000149 RID: 329 RVA: 0x000032B8 File Offset: 0x000014B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
				bool flag = this.m_Ptr != IntPtr.Zero;
				if (flag)
				{
					ProfilerUnsafeUtility.EndSample(this.m_Ptr);
				}
			}

			// Token: 0x0400012B RID: 299
			[NativeDisableUnsafePtrRestriction]
			internal readonly IntPtr m_Ptr;
		}
	}
}
