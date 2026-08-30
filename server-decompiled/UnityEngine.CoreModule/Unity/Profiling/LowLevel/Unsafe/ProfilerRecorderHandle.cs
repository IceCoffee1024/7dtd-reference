using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Profiling.LowLevel.Unsafe
{
	// Token: 0x0200006C RID: 108
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public readonly struct ProfilerRecorderHandle
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00003A28 File Offset: 0x00001C28
		internal ProfilerRecorderHandle(ulong handle)
		{
			this.handle = handle;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00003A32 File Offset: 0x00001C32
		public bool Valid
		{
			get
			{
				return this.handle != 0UL && this.handle != ulong.MaxValue;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00003A4C File Offset: 0x00001C4C
		internal static ProfilerRecorderHandle Get(ProfilerMarker marker)
		{
			return new ProfilerRecorderHandle((ulong)marker.Handle.ToInt64());
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00003A74 File Offset: 0x00001C74
		internal static ProfilerRecorderHandle Get(ProfilerCategory category, string statName)
		{
			bool flag = string.IsNullOrEmpty(statName);
			if (flag)
			{
				throw new ArgumentException("String must be not null or empty", "statName");
			}
			return ProfilerRecorderHandle.GetByName(category, statName);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00003AA8 File Offset: 0x00001CA8
		public static ProfilerRecorderDescription GetDescription(ProfilerRecorderHandle handle)
		{
			bool flag = !handle.Valid;
			if (flag)
			{
				throw new ArgumentException("ProfilerRecorderHandle is not initialized or is not available", "handle");
			}
			return ProfilerRecorderHandle.GetDescriptionInternal(handle);
		}

		// Token: 0x060001A6 RID: 422
		[NativeMethod(IsThreadSafe = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetAvailable(List<ProfilerRecorderHandle> outRecorderHandleList);

		// Token: 0x060001A7 RID: 423 RVA: 0x00003AE0 File Offset: 0x00001CE0
		[NativeMethod(IsThreadSafe = true)]
		internal static ProfilerRecorderHandle GetByName(ProfilerCategory category, string name)
		{
			ProfilerRecorderHandle profilerRecorderHandle;
			ProfilerRecorderHandle.GetByName_Injected(ref category, name, out profilerRecorderHandle);
			return profilerRecorderHandle;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00003AF8 File Offset: 0x00001CF8
		[RequiredMember]
		[NativeMethod(IsThreadSafe = true)]
		internal unsafe static ProfilerRecorderHandle GetByName__Unmanaged(ProfilerCategory category, byte* name, int nameLen)
		{
			ProfilerRecorderHandle profilerRecorderHandle;
			ProfilerRecorderHandle.GetByName__Unmanaged_Injected(ref category, name, nameLen, out profilerRecorderHandle);
			return profilerRecorderHandle;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00003B14 File Offset: 0x00001D14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static ProfilerRecorderHandle GetByName(ProfilerCategory category, char* name, int nameLen)
		{
			return ProfilerRecorderHandle.GetByName_Unsafe(category, name, nameLen);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00003B30 File Offset: 0x00001D30
		[NativeMethod(IsThreadSafe = true)]
		private unsafe static ProfilerRecorderHandle GetByName_Unsafe(ProfilerCategory category, char* name, int nameLen)
		{
			ProfilerRecorderHandle profilerRecorderHandle;
			ProfilerRecorderHandle.GetByName_Unsafe_Injected(ref category, name, nameLen, out profilerRecorderHandle);
			return profilerRecorderHandle;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00003B4C File Offset: 0x00001D4C
		[NativeMethod(IsThreadSafe = true)]
		private static ProfilerRecorderDescription GetDescriptionInternal(ProfilerRecorderHandle handle)
		{
			ProfilerRecorderDescription profilerRecorderDescription;
			ProfilerRecorderHandle.GetDescriptionInternal_Injected(ref handle, out profilerRecorderDescription);
			return profilerRecorderDescription;
		}

		// Token: 0x060001AC RID: 428
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetByName_Injected(ref ProfilerCategory category, string name, out ProfilerRecorderHandle ret);

		// Token: 0x060001AD RID: 429
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void GetByName__Unmanaged_Injected(ref ProfilerCategory category, byte* name, int nameLen, out ProfilerRecorderHandle ret);

		// Token: 0x060001AE RID: 430
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void GetByName_Unsafe_Injected(ref ProfilerCategory category, char* name, int nameLen, out ProfilerRecorderHandle ret);

		// Token: 0x060001AF RID: 431
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDescriptionInternal_Injected(ref ProfilerRecorderHandle handle, out ProfilerRecorderDescription ret);

		// Token: 0x04000178 RID: 376
		private const ulong k_InvalidHandle = 18446744073709551615UL;

		// Token: 0x04000179 RID: 377
		[FieldOffset(0)]
		internal readonly ulong handle;
	}
}
