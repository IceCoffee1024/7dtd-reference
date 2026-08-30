using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000048 RID: 72
	public static class IJobParallelForExtensions
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00002A1C File Offset: 0x00000C1C
		public static void EarlyJobInit<T>() where T : struct, IJobParallelFor
		{
			IJobParallelForExtensions.ParallelForJobStruct<T>.Initialize();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00002A28 File Offset: 0x00000C28
		private unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobParallelFor
		{
			IJobParallelForExtensions.ParallelForJobStruct<T>.Initialize();
			return *IJobParallelForExtensions.ParallelForJobStruct<T>.jobReflectionData.Data;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002A50 File Offset: 0x00000C50
		public static JobHandle Schedule<T>(this T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, innerloopBatchCount);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002A80 File Offset: 0x00000C80
		public static void Run<T>(this T jobData, int arrayLength) where T : struct, IJobParallelFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public static JobHandle ScheduleByRef<T>(this T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, innerloopBatchCount);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public static void RunByRef<T>(this T jobData, int arrayLength) where T : struct, IJobParallelFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x02000049 RID: 73
		internal struct ParallelForJobStruct<T> where T : struct, IJobParallelFor
		{
			// Token: 0x060000D2 RID: 210 RVA: 0x00002B1C File Offset: 0x00000D1C
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				bool flag = *IJobParallelForExtensions.ParallelForJobStruct<T>.jobReflectionData.Data == IntPtr.Zero;
				if (flag)
				{
					*IJobParallelForExtensions.ParallelForJobStruct<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new IJobParallelForExtensions.ParallelForJobStruct<T>.ExecuteJobFunction(IJobParallelForExtensions.ParallelForJobStruct<T>.Execute), null, null);
				}
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00002B6C File Offset: 0x00000D6C
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

			// Token: 0x040000FA RID: 250
			internal static readonly BurstLike.SharedStatic<IntPtr> jobReflectionData = BurstLike.SharedStatic<IntPtr>.GetOrCreate<IJobParallelForExtensions.ParallelForJobStruct<T>>(0U);

			// Token: 0x0200004A RID: 74
			// (Invoke) Token: 0x060000D6 RID: 214
			public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
