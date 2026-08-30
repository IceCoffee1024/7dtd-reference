using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200028F RID: 655
	[VisibleToOtherModules]
	internal class SystemClock
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x0002E168 File Offset: 0x0002C368
		public static DateTime now
		{
			get
			{
				return DateTime.Now;
			}
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0002E180 File Offset: 0x0002C380
		public static long ToUnixTimeMilliseconds(DateTime date)
		{
			return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalMilliseconds);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
		public static long ToUnixTimeSeconds(DateTime date)
		{
			return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalSeconds);
		}

		// Token: 0x04000954 RID: 2388
		private static readonly DateTime s_Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}
}
