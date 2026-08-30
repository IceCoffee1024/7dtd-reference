using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Profiling
{
	// Token: 0x02000064 RID: 100
	[DebuggerDisplay("Count = {Count}")]
	[NativeHeader("Runtime/Profiler/ScriptBindings/ProfilerRecorder.bindings.h")]
	[DebuggerTypeProxy(typeof(ProfilerRecorderDebugView))]
	[UsedByNativeCode]
	public struct ProfilerRecorder : IDisposable
	{
		// Token: 0x06000151 RID: 337 RVA: 0x00003428 File Offset: 0x00001628
		internal ProfilerRecorder(ProfilerRecorderOptions options)
		{
			this = ProfilerRecorder.Create(default(ProfilerRecorderHandle), 0, options);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000344C File Offset: 0x0000164C
		public ProfilerRecorder(string statName, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			this = new ProfilerRecorder(ProfilerCategory.Any, statName, capacity, options);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000345E File Offset: 0x0000165E
		public ProfilerRecorder(string categoryName, string statName, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			this = new ProfilerRecorder(new ProfilerCategory(categoryName), statName, capacity, options);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00003474 File Offset: 0x00001674
		public ProfilerRecorder(ProfilerCategory category, string statName, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			ProfilerRecorderHandle byName = ProfilerRecorderHandle.GetByName(category, statName);
			this = ProfilerRecorder.Create(byName, capacity, options);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000349C File Offset: 0x0000169C
		public unsafe ProfilerRecorder(ProfilerCategory category, char* statName, int statNameLen, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			ProfilerRecorderHandle byName = ProfilerRecorderHandle.GetByName(category, statName, statNameLen);
			this = ProfilerRecorder.Create(byName, capacity, options);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000034C3 File Offset: 0x000016C3
		public ProfilerRecorder(ProfilerMarker marker, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			this = ProfilerRecorder.Create(ProfilerRecorderHandle.Get(marker), capacity, options);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000034D9 File Offset: 0x000016D9
		public ProfilerRecorder(ProfilerRecorderHandle statHandle, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			this = ProfilerRecorder.Create(statHandle, capacity, options);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000034EC File Offset: 0x000016EC
		public unsafe static ProfilerRecorder StartNew(ProfilerCategory category, string statName, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			char* ptr = statName;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return new ProfilerRecorder(category, ptr, statName.Length, capacity, options | ProfilerRecorderOptions.StartImmediately);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003520 File Offset: 0x00001720
		public static ProfilerRecorder StartNew(ProfilerMarker marker, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
		{
			return new ProfilerRecorder(marker, capacity, options | ProfilerRecorderOptions.StartImmediately);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000353C File Offset: 0x0000173C
		internal static ProfilerRecorder StartNew()
		{
			return ProfilerRecorder.Create(default(ProfilerRecorderHandle), 0, ProfilerRecorderOptions.StartImmediately);
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000355E File Offset: 0x0000175E
		public bool Valid
		{
			get
			{
				return this.handle != 0UL && ProfilerRecorder.GetValid(this);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00003578 File Offset: 0x00001778
		public ProfilerMarkerDataType DataType
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetValueDataType(this);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000359C File Offset: 0x0000179C
		public ProfilerMarkerDataUnit UnitType
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetValueUnitType(this);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000035C0 File Offset: 0x000017C0
		public void Start()
		{
			this.CheckInitializedAndThrow();
			ProfilerRecorder.Control(this, ProfilerRecorder.ControlOptions.Start);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000035D7 File Offset: 0x000017D7
		public void Stop()
		{
			this.CheckInitializedAndThrow();
			ProfilerRecorder.Control(this, ProfilerRecorder.ControlOptions.Stop);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000035EE File Offset: 0x000017EE
		public void Reset()
		{
			this.CheckInitializedAndThrow();
			ProfilerRecorder.Control(this, ProfilerRecorder.ControlOptions.Reset);
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00003608 File Offset: 0x00001808
		public long CurrentValue
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetCurrentValue(this);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000362C File Offset: 0x0000182C
		public double CurrentValueAsDouble
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetCurrentValueAsDouble(this);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00003650 File Offset: 0x00001850
		public long LastValue
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetLastValue(this);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00003674 File Offset: 0x00001874
		public double LastValueAsDouble
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetLastValueAsDouble(this);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00003698 File Offset: 0x00001898
		public int Capacity
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetCount(this, ProfilerRecorder.CountOptions.MaxCount);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000036C0 File Offset: 0x000018C0
		public int Count
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetCount(this, ProfilerRecorder.CountOptions.Count);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000036E8 File Offset: 0x000018E8
		public bool IsRunning
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetRunning(this);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000370C File Offset: 0x0000190C
		public bool WrappedAround
		{
			get
			{
				this.CheckInitializedAndThrow();
				return ProfilerRecorder.GetWrapped(this);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003730 File Offset: 0x00001930
		public ProfilerRecorderSample GetSample(int index)
		{
			this.CheckInitializedAndThrow();
			return ProfilerRecorder.GetSampleInternal(this, index);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003758 File Offset: 0x00001958
		public void CopyTo(List<ProfilerRecorderSample> outSamples, bool reset = false)
		{
			bool flag = outSamples == null;
			if (flag)
			{
				throw new ArgumentNullException("outSamples");
			}
			this.CheckInitializedAndThrow();
			ProfilerRecorder.CopyTo_List(this, outSamples, reset);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003790 File Offset: 0x00001990
		public unsafe int CopyTo(ProfilerRecorderSample* dest, int destSize, bool reset = false)
		{
			this.CheckInitializedWithParamsAndThrow(dest);
			return ProfilerRecorder.CopyTo_Pointer(this, dest, destSize, reset);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000037B8 File Offset: 0x000019B8
		public unsafe ProfilerRecorderSample[] ToArray()
		{
			this.CheckInitializedAndThrow();
			int count = this.Count;
			ProfilerRecorderSample[] array = new ProfilerRecorderSample[count];
			ProfilerRecorderSample[] array2;
			ProfilerRecorderSample* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			ProfilerRecorder.CopyTo_Pointer(this, ptr, count, false);
			array2 = null;
			return array;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000380D File Offset: 0x00001A0D
		internal void FilterToCurrentThread()
		{
			this.CheckInitializedAndThrow();
			ProfilerRecorder.Control(this, ProfilerRecorder.ControlOptions.SetFilterToCurrentThread);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003824 File Offset: 0x00001A24
		internal void CollectFromAllThreads()
		{
			this.CheckInitializedAndThrow();
			ProfilerRecorder.Control(this, ProfilerRecorder.ControlOptions.SetToCollectFromAllThreads);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000383C File Offset: 0x00001A3C
		[NativeMethod(IsThreadSafe = true, ThrowsException = true)]
		private static ProfilerRecorder Create(ProfilerRecorderHandle statHandle, int maxSampleCount, ProfilerRecorderOptions options)
		{
			ProfilerRecorder profilerRecorder;
			ProfilerRecorder.Create_Injected(ref statHandle, maxSampleCount, options, out profilerRecorder);
			return profilerRecorder;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00003855 File Offset: 0x00001A55
		[NativeMethod(IsThreadSafe = true)]
		private static void Control(ProfilerRecorder handle, ProfilerRecorder.ControlOptions options)
		{
			ProfilerRecorder.Control_Injected(ref handle, options);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000385F File Offset: 0x00001A5F
		[NativeMethod(IsThreadSafe = true)]
		private static ProfilerMarkerDataUnit GetValueUnitType(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetValueUnitType_Injected(ref handle);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003868 File Offset: 0x00001A68
		[NativeMethod(IsThreadSafe = true)]
		private static ProfilerMarkerDataType GetValueDataType(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetValueDataType_Injected(ref handle);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003871 File Offset: 0x00001A71
		[NativeMethod(IsThreadSafe = true)]
		private static long GetCurrentValue(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetCurrentValue_Injected(ref handle);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000387A File Offset: 0x00001A7A
		[NativeMethod(IsThreadSafe = true)]
		private static double GetCurrentValueAsDouble(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetCurrentValueAsDouble_Injected(ref handle);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00003883 File Offset: 0x00001A83
		[NativeMethod(IsThreadSafe = true)]
		private static long GetLastValue(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetLastValue_Injected(ref handle);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000388C File Offset: 0x00001A8C
		[NativeMethod(IsThreadSafe = true)]
		private static double GetLastValueAsDouble(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetLastValueAsDouble_Injected(ref handle);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00003895 File Offset: 0x00001A95
		[NativeMethod(IsThreadSafe = true)]
		private static int GetCount(ProfilerRecorder handle, ProfilerRecorder.CountOptions countOptions)
		{
			return ProfilerRecorder.GetCount_Injected(ref handle, countOptions);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000389F File Offset: 0x00001A9F
		[NativeMethod(IsThreadSafe = true)]
		private static bool GetValid(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetValid_Injected(ref handle);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000038A8 File Offset: 0x00001AA8
		[NativeMethod(IsThreadSafe = true)]
		private static bool GetWrapped(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetWrapped_Injected(ref handle);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000038B1 File Offset: 0x00001AB1
		[NativeMethod(IsThreadSafe = true)]
		private static bool GetRunning(ProfilerRecorder handle)
		{
			return ProfilerRecorder.GetRunning_Injected(ref handle);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000038BC File Offset: 0x00001ABC
		[NativeMethod(IsThreadSafe = true, ThrowsException = true)]
		private static ProfilerRecorderSample GetSampleInternal(ProfilerRecorder handle, int index)
		{
			ProfilerRecorderSample profilerRecorderSample;
			ProfilerRecorder.GetSampleInternal_Injected(ref handle, index, out profilerRecorderSample);
			return profilerRecorderSample;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000038D4 File Offset: 0x00001AD4
		[NativeMethod(IsThreadSafe = true)]
		private static void CopyTo_List(ProfilerRecorder handle, List<ProfilerRecorderSample> outSamples, bool reset)
		{
			ProfilerRecorder.CopyTo_List_Injected(ref handle, outSamples, reset);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000038DF File Offset: 0x00001ADF
		[NativeMethod(IsThreadSafe = true)]
		private unsafe static int CopyTo_Pointer(ProfilerRecorder handle, ProfilerRecorderSample* outSamples, int outSamplesSize, bool reset)
		{
			return ProfilerRecorder.CopyTo_Pointer_Injected(ref handle, outSamples, outSamplesSize, reset);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000038EC File Offset: 0x00001AEC
		public void Dispose()
		{
			bool flag = this.handle == 0UL;
			if (!flag)
			{
				ProfilerRecorder.Control(this, ProfilerRecorder.ControlOptions.Release);
				this.handle = 0UL;
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00003920 File Offset: 0x00001B20
		[BurstDiscard]
		private unsafe void CheckInitializedWithParamsAndThrow(ProfilerRecorderSample* dest)
		{
			bool flag = this.handle == 0UL;
			if (flag)
			{
				throw new InvalidOperationException("ProfilerRecorder object is not initialized or has been disposed.");
			}
			bool flag2 = dest == null;
			if (flag2)
			{
				throw new ArgumentNullException("dest");
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000395C File Offset: 0x00001B5C
		[BurstDiscard]
		private void CheckInitializedAndThrow()
		{
			bool flag = this.handle == 0UL;
			if (flag)
			{
				throw new InvalidOperationException("ProfilerRecorder object is not initialized or has been disposed.");
			}
		}

		// Token: 0x06000181 RID: 385
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Create_Injected(ref ProfilerRecorderHandle statHandle, int maxSampleCount, ProfilerRecorderOptions options, out ProfilerRecorder ret);

		// Token: 0x06000182 RID: 386
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Control_Injected(ref ProfilerRecorder handle, ProfilerRecorder.ControlOptions options);

		// Token: 0x06000183 RID: 387
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ProfilerMarkerDataUnit GetValueUnitType_Injected(ref ProfilerRecorder handle);

		// Token: 0x06000184 RID: 388
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ProfilerMarkerDataType GetValueDataType_Injected(ref ProfilerRecorder handle);

		// Token: 0x06000185 RID: 389
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetCurrentValue_Injected(ref ProfilerRecorder handle);

		// Token: 0x06000186 RID: 390
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetCurrentValueAsDouble_Injected(ref ProfilerRecorder handle);

		// Token: 0x06000187 RID: 391
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetLastValue_Injected(ref ProfilerRecorder handle);

		// Token: 0x06000188 RID: 392
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetLastValueAsDouble_Injected(ref ProfilerRecorder handle);

		// Token: 0x06000189 RID: 393
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetCount_Injected(ref ProfilerRecorder handle, ProfilerRecorder.CountOptions countOptions);

		// Token: 0x0600018A RID: 394
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetValid_Injected(ref ProfilerRecorder handle);

		// Token: 0x0600018B RID: 395
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetWrapped_Injected(ref ProfilerRecorder handle);

		// Token: 0x0600018C RID: 396
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetRunning_Injected(ref ProfilerRecorder handle);

		// Token: 0x0600018D RID: 397
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSampleInternal_Injected(ref ProfilerRecorder handle, int index, out ProfilerRecorderSample ret);

		// Token: 0x0600018E RID: 398
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyTo_List_Injected(ref ProfilerRecorder handle, List<ProfilerRecorderSample> outSamples, bool reset);

		// Token: 0x0600018F RID: 399
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int CopyTo_Pointer_Injected(ref ProfilerRecorder handle, ProfilerRecorderSample* outSamples, int outSamplesSize, bool reset);

		// Token: 0x0400014B RID: 331
		internal ulong handle;

		// Token: 0x0400014C RID: 332
		internal const ProfilerRecorderOptions SharedRecorder = (ProfilerRecorderOptions)128;

		// Token: 0x02000065 RID: 101
		internal enum ControlOptions
		{
			// Token: 0x0400014E RID: 334
			Start,
			// Token: 0x0400014F RID: 335
			Stop,
			// Token: 0x04000150 RID: 336
			Reset,
			// Token: 0x04000151 RID: 337
			Release = 4,
			// Token: 0x04000152 RID: 338
			SetFilterToCurrentThread,
			// Token: 0x04000153 RID: 339
			SetToCollectFromAllThreads
		}

		// Token: 0x02000066 RID: 102
		internal enum CountOptions
		{
			// Token: 0x04000155 RID: 341
			Count,
			// Token: 0x04000156 RID: 342
			MaxCount
		}
	}
}
