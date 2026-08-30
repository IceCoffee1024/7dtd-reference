using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Profiling.LowLevel.Unsafe
{
	// Token: 0x0200006F RID: 111
	[NativeHeader("Runtime/Profiler/ScriptBindings/ProfilerUnsafeUtility.bindings.h")]
	[UsedByNativeCode]
	[IgnoredByDeepProfiler]
	public static class ProfilerUnsafeUtility
	{
		// Token: 0x060001B1 RID: 433
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ushort CreateCategory(string name, ProfilerCategoryColor colorIndex);

		// Token: 0x060001B2 RID: 434
		[RequiredMember]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern ushort CreateCategory__Unmanaged(byte* name, int nameLen, ProfilerCategoryColor colorIndex);

		// Token: 0x060001B3 RID: 435 RVA: 0x00003B78 File Offset: 0x00001D78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ushort CreateCategory(char* name, int nameLen, ProfilerCategoryColor colorIndex)
		{
			return ProfilerUnsafeUtility.CreateCategory_Unsafe(name, nameLen, colorIndex);
		}

		// Token: 0x060001B4 RID: 436
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern ushort CreateCategory_Unsafe(char* name, int nameLen, ProfilerCategoryColor colorIndex);

		// Token: 0x060001B5 RID: 437 RVA: 0x00003B94 File Offset: 0x00001D94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ushort GetCategoryByName(char* name, int nameLen)
		{
			return ProfilerUnsafeUtility.GetCategoryByName_Unsafe(name, nameLen);
		}

		// Token: 0x060001B6 RID: 438
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern ushort GetCategoryByName_Unsafe(char* name, int nameLen);

		// Token: 0x060001B7 RID: 439 RVA: 0x00003BB0 File Offset: 0x00001DB0
		[ThreadSafe]
		public static ProfilerCategoryDescription GetCategoryDescription(ushort categoryId)
		{
			ProfilerCategoryDescription profilerCategoryDescription;
			ProfilerUnsafeUtility.GetCategoryDescription_Injected(categoryId, out profilerCategoryDescription);
			return profilerCategoryDescription;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00003BC8 File Offset: 0x00001DC8
		[ThreadSafe]
		internal static Color32 GetCategoryColor(ProfilerCategoryColor colorIndex)
		{
			Color32 color;
			ProfilerUnsafeUtility.GetCategoryColor_Injected(colorIndex, out color);
			return color;
		}

		// Token: 0x060001B9 RID: 441
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CreateMarker(string name, ushort categoryId, MarkerFlags flags, int metadataCount);

		// Token: 0x060001BA RID: 442
		[RequiredMember]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern IntPtr CreateMarker__Unmanaged(byte* name, int nameLen, ushort categoryId, MarkerFlags flags, int metadataCount);

		// Token: 0x060001BB RID: 443 RVA: 0x00003BE0 File Offset: 0x00001DE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static IntPtr CreateMarker(char* name, int nameLen, ushort categoryId, MarkerFlags flags, int metadataCount)
		{
			return ProfilerUnsafeUtility.CreateMarker_Unsafe(name, nameLen, categoryId, flags, metadataCount);
		}

		// Token: 0x060001BC RID: 444
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr CreateMarker_Unsafe(char* name, int nameLen, ushort categoryId, MarkerFlags flags, int metadataCount);

		// Token: 0x060001BD RID: 445
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetMarker(string name);

		// Token: 0x060001BE RID: 446
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetMarkerMetadata(IntPtr markerPtr, int index, string name, byte type, byte unit);

		// Token: 0x060001BF RID: 447
		[RequiredMember]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void SetMarkerMetadata__Unmanaged(IntPtr markerPtr, int index, byte* name, int nameLen, byte type, byte unit);

		// Token: 0x060001C0 RID: 448 RVA: 0x00003BFD File Offset: 0x00001DFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void SetMarkerMetadata(IntPtr markerPtr, int index, char* name, int nameLen, byte type, byte unit)
		{
			ProfilerUnsafeUtility.SetMarkerMetadata_Unsafe(markerPtr, index, name, nameLen, type, unit);
		}

		// Token: 0x060001C1 RID: 449
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetMarkerMetadata_Unsafe(IntPtr markerPtr, int index, char* name, int nameLen, byte type, byte unit);

		// Token: 0x060001C2 RID: 450
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginSample(IntPtr markerPtr);

		// Token: 0x060001C3 RID: 451
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void BeginSampleWithMetadata(IntPtr markerPtr, int metadataCount, void* metadata);

		// Token: 0x060001C4 RID: 452
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndSample(IntPtr markerPtr);

		// Token: 0x060001C5 RID: 453
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void SingleSampleWithMetadata(IntPtr markerPtr, int metadataCount, void* metadata);

		// Token: 0x060001C6 RID: 454
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void* CreateCounterValue(out IntPtr counterPtr, string name, ushort categoryId, MarkerFlags flags, byte dataType, byte dataUnit, int dataSize, ProfilerCounterOptions counterOptions);

		// Token: 0x060001C7 RID: 455
		[ThreadSafe]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void* CreateCounterValue__Unmanaged(out IntPtr counterPtr, byte* name, int nameLen, ushort categoryId, MarkerFlags flags, byte dataType, byte dataUnit, int dataSize, ProfilerCounterOptions counterOptions);

		// Token: 0x060001C8 RID: 456 RVA: 0x00003C10 File Offset: 0x00001E10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* CreateCounterValue(out IntPtr counterPtr, char* name, int nameLen, ushort categoryId, MarkerFlags flags, byte dataType, byte dataUnit, int dataSize, ProfilerCounterOptions counterOptions)
		{
			return ProfilerUnsafeUtility.CreateCounterValue_Unsafe(out counterPtr, name, nameLen, categoryId, flags, dataType, dataUnit, dataSize, counterOptions);
		}

		// Token: 0x060001C9 RID: 457
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* CreateCounterValue_Unsafe(out IntPtr counterPtr, char* name, int nameLen, ushort categoryId, MarkerFlags flags, byte dataType, byte dataUnit, int dataSize, ProfilerCounterOptions counterOptions);

		// Token: 0x060001CA RID: 458
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void FlushCounterValue(void* counterValuePtr);

		// Token: 0x060001CB RID: 459 RVA: 0x00003C38 File Offset: 0x00001E38
		internal unsafe static string Utf8ToString(byte* chars, int charsLen)
		{
			bool flag = chars == null;
			string text;
			if (flag)
			{
				text = null;
			}
			else
			{
				byte[] array = new byte[charsLen];
				Marshal.Copy((IntPtr)((void*)chars), array, 0, charsLen);
				text = Encoding.UTF8.GetString(array, 0, charsLen);
			}
			return text;
		}

		// Token: 0x060001CC RID: 460
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint CreateFlow(ushort categoryId);

		// Token: 0x060001CD RID: 461
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FlowEvent(uint flowId, ProfilerFlowEventType flowEventType);

		// Token: 0x060001CE RID: 462
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_BeginWithObject(IntPtr markerPtr, Object contextUnityObject);

		// Token: 0x060001CF RID: 463
		[NativeConditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string Internal_GetName(IntPtr markerPtr);

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001D0 RID: 464
		public static extern long Timestamp
		{
			[ThreadSafe]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00003C7C File Offset: 0x00001E7C
		public static ProfilerUnsafeUtility.TimestampConversionRatio TimestampToNanosecondsConversionRatio
		{
			[ThreadSafe]
			get
			{
				ProfilerUnsafeUtility.TimestampConversionRatio timestampConversionRatio;
				ProfilerUnsafeUtility.get_TimestampToNanosecondsConversionRatio_Injected(out timestampConversionRatio);
				return timestampConversionRatio;
			}
		}

		// Token: 0x060001D2 RID: 466
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCategoryDescription_Injected(ushort categoryId, out ProfilerCategoryDescription ret);

		// Token: 0x060001D3 RID: 467
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCategoryColor_Injected(ProfilerCategoryColor colorIndex, out Color32 ret);

		// Token: 0x060001D4 RID: 468
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_TimestampToNanosecondsConversionRatio_Injected(out ProfilerUnsafeUtility.TimestampConversionRatio ret);

		// Token: 0x04000185 RID: 389
		public const ushort CategoryRender = 0;

		// Token: 0x04000186 RID: 390
		public const ushort CategoryScripts = 1;

		// Token: 0x04000187 RID: 391
		public const ushort CategoryGUI = 4;

		// Token: 0x04000188 RID: 392
		public const ushort CategoryPhysics = 5;

		// Token: 0x04000189 RID: 393
		public const ushort CategoryAnimation = 6;

		// Token: 0x0400018A RID: 394
		public const ushort CategoryAi = 7;

		// Token: 0x0400018B RID: 395
		public const ushort CategoryAudio = 8;

		// Token: 0x0400018C RID: 396
		public const ushort CategoryVideo = 11;

		// Token: 0x0400018D RID: 397
		public const ushort CategoryParticles = 12;

		// Token: 0x0400018E RID: 398
		public const ushort CategoryLighting = 13;

		// Token: 0x0400018F RID: 399
		[Obsolete("CategoryLightning has been renamed. Use CategoryLighting instead (UnityUpgradable) -> CategoryLighting", false)]
		public const ushort CategoryLightning = 13;

		// Token: 0x04000190 RID: 400
		public const ushort CategoryNetwork = 14;

		// Token: 0x04000191 RID: 401
		public const ushort CategoryLoading = 15;

		// Token: 0x04000192 RID: 402
		public const ushort CategoryOther = 16;

		// Token: 0x04000193 RID: 403
		public const ushort CategoryVr = 22;

		// Token: 0x04000194 RID: 404
		public const ushort CategoryAllocation = 23;

		// Token: 0x04000195 RID: 405
		public const ushort CategoryInternal = 24;

		// Token: 0x04000196 RID: 406
		public const ushort CategoryFileIO = 25;

		// Token: 0x04000197 RID: 407
		public const ushort CategoryInput = 30;

		// Token: 0x04000198 RID: 408
		public const ushort CategoryVirtualTexturing = 31;

		// Token: 0x04000199 RID: 409
		internal const ushort CategoryGPU = 32;

		// Token: 0x0400019A RID: 410
		public const ushort CategoryPhysics2D = 33;

		// Token: 0x0400019B RID: 411
		internal const ushort CategoryAny = 65535;

		// Token: 0x02000070 RID: 112
		public struct TimestampConversionRatio
		{
			// Token: 0x0400019C RID: 412
			public long Numerator;

			// Token: 0x0400019D RID: 413
			public long Denominator;
		}
	}
}
