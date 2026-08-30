using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;

namespace Unity.Profiling
{
	// Token: 0x02000060 RID: 96
	internal struct ProfilerMarkerWithStringData
	{
		// Token: 0x0600014A RID: 330 RVA: 0x000032E8 File Offset: 0x000014E8
		public static ProfilerMarkerWithStringData Create(string name, string parameterName)
		{
			IntPtr intPtr = ProfilerUnsafeUtility.CreateMarker(name, 16, MarkerFlags.Default, 1);
			ProfilerUnsafeUtility.SetMarkerMetadata(intPtr, 0, parameterName, 9, 0);
			return new ProfilerMarkerWithStringData
			{
				_marker = intPtr
			};
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00003324 File Offset: 0x00001524
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarkerWithStringData.AutoScope Auto(bool enabled, Func<string> parameterValue)
		{
			ProfilerMarkerWithStringData.AutoScope autoScope;
			if (enabled)
			{
				autoScope = this.Auto(parameterValue());
			}
			else
			{
				autoScope = new ProfilerMarkerWithStringData.AutoScope(IntPtr.Zero);
			}
			return autoScope;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00003358 File Offset: 0x00001558
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ProfilerMarkerWithStringData.AutoScope Auto(string value)
		{
			bool flag = value == null;
			if (flag)
			{
				throw new ArgumentNullException("value");
			}
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
				{
					Type = 9,
					Size = (uint)(value.Length * 2 + 2)
				};
				profilerMarkerData.Ptr = (void*)ptr;
				ProfilerUnsafeUtility.BeginSampleWithMetadata(this._marker, 1, (void*)(&profilerMarkerData));
			}
			return new ProfilerMarkerWithStringData.AutoScope(this._marker);
		}

		// Token: 0x0400013C RID: 316
		private const MethodImplOptions AggressiveInlining = MethodImplOptions.AggressiveInlining;

		// Token: 0x0400013D RID: 317
		private IntPtr _marker;

		// Token: 0x02000061 RID: 97
		public struct AutoScope : IDisposable
		{
			// Token: 0x0600014D RID: 333 RVA: 0x000033DE File Offset: 0x000015DE
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal AutoScope(IntPtr marker)
			{
				this._marker = marker;
			}

			// Token: 0x0600014E RID: 334 RVA: 0x000033E8 File Offset: 0x000015E8
			[Pure]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
				bool flag = this._marker != IntPtr.Zero;
				if (flag)
				{
					ProfilerUnsafeUtility.EndSample(this._marker);
				}
			}

			// Token: 0x0400013E RID: 318
			private IntPtr _marker;
		}
	}
}
