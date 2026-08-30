using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000040 RID: 64
	public static class IJobExtensions
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000026A6 File Offset: 0x000008A6
		public static void EarlyJobInit<T>() where T : struct, IJob
		{
			IJobExtensions.JobStruct<T>.Initialize();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000026B0 File Offset: 0x000008B0
		private unsafe static IntPtr GetReflectionData<T>() where T : struct, IJob
		{
			IJobExtensions.JobStruct<T>.Initialize();
			return *IJobExtensions.JobStruct<T>.jobReflectionData.Data;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000026D8 File Offset: 0x000008D8
		public static JobHandle Schedule<T>(this T jobData, JobHandle dependsOn = default(JobHandle)) where T : struct, IJob
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Single);
			return JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002708 File Offset: 0x00000908
		public static void Run<T>(this T jobData) where T : struct, IJob
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000273C File Offset: 0x0000093C
		public static JobHandle ScheduleByRef<T>(this T jobData, JobHandle dependsOn = default(JobHandle)) where T : struct, IJob
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Single);
			return JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000276C File Offset: 0x0000096C
		public static void RunByRef<T>(this T jobData) where T : struct, IJob
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x02000041 RID: 65
		internal struct JobStruct<T> where T : struct, IJob
		{
			// Token: 0x060000B4 RID: 180 RVA: 0x000027A0 File Offset: 0x000009A0
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				bool flag = *IJobExtensions.JobStruct<T>.jobReflectionData.Data == IntPtr.Zero;
				if (flag)
				{
					*IJobExtensions.JobStruct<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new IJobExtensions.JobStruct<T>.ExecuteJobFunction(IJobExtensions.JobStruct<T>.Execute), null, null);
				}
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x000027EF File Offset: 0x000009EF
			public static void Execute(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				data.Execute();
			}

			// Token: 0x040000F8 RID: 248
			internal static readonly BurstLike.SharedStatic<IntPtr> jobReflectionData = BurstLike.SharedStatic<IntPtr>.GetOrCreate<IJobExtensions.JobStruct<T>>(0U);

			// Token: 0x02000042 RID: 66
			// (Invoke) Token: 0x060000B8 RID: 184
			internal delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
