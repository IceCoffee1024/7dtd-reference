using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x02000053 RID: 83
	[NativeType(Header = "Runtime/Jobs/ScriptBindings/JobsBindings.h")]
	[NativeHeader("Runtime/Jobs/JobSystem.h")]
	public static class JobsUtility
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00002E64 File Offset: 0x00001064
		public unsafe static void GetJobRange(ref JobRanges ranges, int jobIndex, out int beginIndex, out int endIndex)
		{
			int* ptr = (int*)(void*)ranges.StartEndIndex;
			beginIndex = ptr[jobIndex * 2];
			endIndex = ptr[jobIndex * 2 + 1];
		}

		// Token: 0x060000F5 RID: 245
		[NativeMethod(IsFreeFunction = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetWorkStealingRange(ref JobRanges ranges, int jobIndex, out int beginIndex, out int endIndex);

		// Token: 0x060000F6 RID: 246 RVA: 0x00002E98 File Offset: 0x00001098
		[FreeFunction("ScheduleManagedJob", ThrowsException = true, IsThreadSafe = true)]
		public static JobHandle Schedule(ref JobsUtility.JobScheduleParameters parameters)
		{
			JobHandle jobHandle;
			JobsUtility.Schedule_Injected(ref parameters, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00002EB0 File Offset: 0x000010B0
		[FreeFunction("ScheduleManagedJobParallelFor", ThrowsException = true, IsThreadSafe = true)]
		public static JobHandle ScheduleParallelFor(ref JobsUtility.JobScheduleParameters parameters, int arrayLength, int innerloopBatchCount)
		{
			JobHandle jobHandle;
			JobsUtility.ScheduleParallelFor_Injected(ref parameters, arrayLength, innerloopBatchCount, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00002EC8 File Offset: 0x000010C8
		[FreeFunction("ScheduleManagedJobParallelForDeferArraySize", ThrowsException = true, IsThreadSafe = true)]
		public unsafe static JobHandle ScheduleParallelForDeferArraySize(ref JobsUtility.JobScheduleParameters parameters, int innerloopBatchCount, void* listData, void* listDataAtomicSafetyHandle)
		{
			JobHandle jobHandle;
			JobsUtility.ScheduleParallelForDeferArraySize_Injected(ref parameters, innerloopBatchCount, listData, listDataAtomicSafetyHandle, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00002EE4 File Offset: 0x000010E4
		[FreeFunction("ScheduleManagedJobParallelForTransform", ThrowsException = true)]
		public static JobHandle ScheduleParallelForTransform(ref JobsUtility.JobScheduleParameters parameters, IntPtr transfromAccesssArray)
		{
			JobHandle jobHandle;
			JobsUtility.ScheduleParallelForTransform_Injected(ref parameters, transfromAccesssArray, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002EFC File Offset: 0x000010FC
		[FreeFunction("ScheduleManagedJobParallelForTransformReadOnly", ThrowsException = true)]
		public static JobHandle ScheduleParallelForTransformReadOnly(ref JobsUtility.JobScheduleParameters parameters, IntPtr transfromAccesssArray, int innerloopBatchCount)
		{
			JobHandle jobHandle;
			JobsUtility.ScheduleParallelForTransformReadOnly_Injected(ref parameters, transfromAccesssArray, innerloopBatchCount, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060000FB RID: 251
		[NativeMethod(IsThreadSafe = true, IsFreeFunction = true)]
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void PatchBufferMinMaxRanges(IntPtr bufferRangePatchData, void* jobdata, int startIndex, int rangeSize);

		// Token: 0x060000FC RID: 252
		[FreeFunction(ThrowsException = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, object managedJobFunction0, object managedJobFunction1, object managedJobFunction2);

		// Token: 0x060000FD RID: 253 RVA: 0x00002F14 File Offset: 0x00001114
		[Obsolete("JobType is obsolete. The parameter should be removed. (UnityUpgradable) -> !1")]
		public static IntPtr CreateJobReflectionData(Type type, JobType jobType, object managedJobFunction0, object managedJobFunction1 = null, object managedJobFunction2 = null)
		{
			return JobsUtility.CreateJobReflectionData(type, type, managedJobFunction0, managedJobFunction1, managedJobFunction2);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00002F34 File Offset: 0x00001134
		public static IntPtr CreateJobReflectionData(Type type, object managedJobFunction0, object managedJobFunction1 = null, object managedJobFunction2 = null)
		{
			return JobsUtility.CreateJobReflectionData(type, type, managedJobFunction0, managedJobFunction1, managedJobFunction2);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00002F50 File Offset: 0x00001150
		[Obsolete("JobType is obsolete. The parameter should be removed. (UnityUpgradable) -> !2")]
		public static IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, JobType jobType, object managedJobFunction0)
		{
			return JobsUtility.CreateJobReflectionData(wrapperJobType, userJobType, managedJobFunction0, null, null);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00002F6C File Offset: 0x0000116C
		public static IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, object managedJobFunction0)
		{
			return JobsUtility.CreateJobReflectionData(wrapperJobType, userJobType, managedJobFunction0, null, null);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000101 RID: 257
		public static extern bool IsExecutingJob
		{
			[NativeMethod(IsFreeFunction = true, IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000102 RID: 258
		// (set) Token: 0x06000103 RID: 259
		public static extern bool JobDebuggerEnabled
		{
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000104 RID: 260
		// (set) Token: 0x06000105 RID: 261
		public static extern bool JobCompilerEnabled
		{
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000106 RID: 262
		[FreeFunction("JobSystem::GetJobQueueWorkerThreadCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetJobQueueWorkerThreadCount();

		// Token: 0x06000107 RID: 263
		[FreeFunction("JobSystem::ForceSetJobQueueWorkerThreadCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetJobQueueMaximumActiveThreadCount(int count);

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000108 RID: 264
		public static extern int JobWorkerMaximumCount
		{
			[FreeFunction("JobSystem::GetJobQueueMaximumThreadCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000109 RID: 265
		[FreeFunction("JobSystem::ResetJobQueueWorkerThreadCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetJobWorkerCount();

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00002F88 File Offset: 0x00001188
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00002FA0 File Offset: 0x000011A0
		public static int JobWorkerCount
		{
			get
			{
				return JobsUtility.GetJobQueueWorkerThreadCount();
			}
			set
			{
				bool flag = value < 0 || value > JobsUtility.JobWorkerMaximumCount;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("JobWorkerCount", string.Format("Invalid JobWorkerCount {0} must be in the range 0 -> {1}", value, JobsUtility.JobWorkerMaximumCount));
				}
				JobsUtility.SetJobQueueMaximumActiveThreadCount(value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600010C RID: 268
		public static extern int ThreadIndex
		{
			[FreeFunction("GetJobWorkerIndex", IsThreadSafe = true)]
			[BurstAuthorizedExternalMethod]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600010D RID: 269
		public static extern int ThreadIndexCount
		{
			[FreeFunction("GetJobWorkerIndexCount", IsThreadSafe = true)]
			[BurstAuthorizedExternalMethod]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600010E RID: 270
		[FreeFunction("IsJobQueueBatchingEnabled")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetJobBatchingEnabled();

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00002FEE File Offset: 0x000011EE
		internal static bool JobBatchingEnabled
		{
			get
			{
				return JobsUtility.GetJobBatchingEnabled();
			}
		}

		// Token: 0x06000110 RID: 272
		[FreeFunction("JobDebuggerGetSystemIdCellPtr")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetSystemIdCellPtr();

		// Token: 0x06000111 RID: 273
		[FreeFunction("JobDebuggerClearSystemIds")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearSystemIds();

		// Token: 0x06000112 RID: 274
		[FreeFunction("JobDebuggerGetSystemIdMappings")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int GetSystemIdMappings(JobHandle* handles, int* systemIds, int maxCount);

		// Token: 0x06000113 RID: 275 RVA: 0x00002FF8 File Offset: 0x000011F8
		[RequiredByNativeCode]
		private static void InvokePanicFunction()
		{
			JobsUtility.PanicFunction_ panicFunction = JobsUtility.PanicFunction;
			bool flag = panicFunction == null;
			if (!flag)
			{
				panicFunction();
			}
		}

		// Token: 0x06000114 RID: 276
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Schedule_Injected(ref JobsUtility.JobScheduleParameters parameters, out JobHandle ret);

		// Token: 0x06000115 RID: 277
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleParallelFor_Injected(ref JobsUtility.JobScheduleParameters parameters, int arrayLength, int innerloopBatchCount, out JobHandle ret);

		// Token: 0x06000116 RID: 278
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleParallelForDeferArraySize_Injected(ref JobsUtility.JobScheduleParameters parameters, int innerloopBatchCount, void* listData, void* listDataAtomicSafetyHandle, out JobHandle ret);

		// Token: 0x06000117 RID: 279
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleParallelForTransform_Injected(ref JobsUtility.JobScheduleParameters parameters, IntPtr transfromAccesssArray, out JobHandle ret);

		// Token: 0x06000118 RID: 280
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleParallelForTransformReadOnly_Injected(ref JobsUtility.JobScheduleParameters parameters, IntPtr transfromAccesssArray, int innerloopBatchCount, out JobHandle ret);

		// Token: 0x0400010D RID: 269
		public const int MaxJobThreadCount = 128;

		// Token: 0x0400010E RID: 270
		public const int CacheLineSize = 64;

		// Token: 0x0400010F RID: 271
		internal static JobsUtility.PanicFunction_ PanicFunction;

		// Token: 0x02000054 RID: 84
		public struct JobScheduleParameters
		{
			// Token: 0x06000119 RID: 281 RVA: 0x0000301D File Offset: 0x0000121D
			public unsafe JobScheduleParameters(void* i_jobData, IntPtr i_reflectionData, JobHandle i_dependency, ScheduleMode i_scheduleMode)
			{
				this.Dependency = i_dependency;
				this.JobDataPtr = (IntPtr)i_jobData;
				this.ReflectionData = i_reflectionData;
				this.ScheduleMode = (int)i_scheduleMode;
			}

			// Token: 0x04000110 RID: 272
			public JobHandle Dependency;

			// Token: 0x04000111 RID: 273
			public int ScheduleMode;

			// Token: 0x04000112 RID: 274
			public IntPtr ReflectionData;

			// Token: 0x04000113 RID: 275
			public IntPtr JobDataPtr;
		}

		// Token: 0x02000055 RID: 85
		// (Invoke) Token: 0x0600011B RID: 283
		internal delegate void PanicFunction_();
	}
}
