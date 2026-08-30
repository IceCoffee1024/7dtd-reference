using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000044 RID: 68
	public static class IJobForExtensions
	{
		// Token: 0x060000BC RID: 188 RVA: 0x0000280C File Offset: 0x00000A0C
		public static void EarlyJobInit<T>() where T : struct, IJobFor
		{
			IJobForExtensions.ForJobStruct<T>.Initialize();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002818 File Offset: 0x00000A18
		private unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobFor
		{
			IJobForExtensions.ForJobStruct<T>.Initialize();
			return *IJobForExtensions.ForJobStruct<T>.jobReflectionData.Data;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002840 File Offset: 0x00000A40
		public static JobHandle Schedule<T>(this T jobData, int arrayLength, JobHandle dependency) where T : struct, IJobFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobForExtensions.GetReflectionData<T>(), dependency, ScheduleMode.Single);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002870 File Offset: 0x00000A70
		public static JobHandle ScheduleParallel<T>(this T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependency) where T : struct, IJobFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobForExtensions.GetReflectionData<T>(), dependency, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, innerloopBatchCount);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000028A0 File Offset: 0x00000AA0
		public static void Run<T>(this T jobData, int arrayLength) where T : struct, IJobFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobForExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000028D8 File Offset: 0x00000AD8
		public static JobHandle ScheduleByRef<T>(this T jobData, int arrayLength, JobHandle dependency) where T : struct, IJobFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobForExtensions.GetReflectionData<T>(), dependency, ScheduleMode.Single);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002908 File Offset: 0x00000B08
		public static JobHandle ScheduleParallelByRef<T>(this T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependency) where T : struct, IJobFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobForExtensions.GetReflectionData<T>(), dependency, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, innerloopBatchCount);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002938 File Offset: 0x00000B38
		public static void RunByRef<T>(this T jobData, int arrayLength) where T : struct, IJobFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobForExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x02000045 RID: 69
		internal struct ForJobStruct<T> where T : struct, IJobFor
		{
			// Token: 0x060000C4 RID: 196 RVA: 0x0000296C File Offset: 0x00000B6C
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				bool flag = *IJobForExtensions.ForJobStruct<T>.jobReflectionData.Data == IntPtr.Zero;
				if (flag)
				{
					*IJobForExtensions.ForJobStruct<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new IJobForExtensions.ForJobStruct<T>.ExecuteJobFunction(IJobForExtensions.ForJobStruct<T>.Execute), null, null);
				}
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x000029BC File Offset: 0x00000BBC
			public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				for (;;)
				{
					int num;
					int num2;
					bool flag = !JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2);
					if (flag)
					{
						break;
					}
					int num3 = num2;
					for (int i = num; i < num3; i++)
					{
						jobData.Execute(i);
					}
				}
			}

			// Token: 0x040000F9 RID: 249
			internal static readonly BurstLike.SharedStatic<IntPtr> jobReflectionData = BurstLike.SharedStatic<IntPtr>.GetOrCreate<IJobForExtensions.ForJobStruct<T>>(0U);

			// Token: 0x02000046 RID: 70
			// (Invoke) Token: 0x060000C8 RID: 200
			public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
