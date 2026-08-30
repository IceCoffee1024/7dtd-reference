using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x0200004D RID: 77
	public struct BatchQueryJobStruct<T> where T : struct
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00002DEC File Offset: 0x00000FEC
		public static IntPtr Initialize()
		{
			bool flag = BatchQueryJobStruct<T>.jobReflectionData == IntPtr.Zero;
			if (flag)
			{
				BatchQueryJobStruct<T>.jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(T), null, null, null);
			}
			return BatchQueryJobStruct<T>.jobReflectionData;
		}

		// Token: 0x040000FF RID: 255
		internal static IntPtr jobReflectionData;
	}
}
