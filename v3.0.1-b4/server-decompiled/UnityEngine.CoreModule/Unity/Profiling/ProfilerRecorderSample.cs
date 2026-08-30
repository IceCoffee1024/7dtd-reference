using System;
using System.Diagnostics;
using UnityEngine.Scripting;

namespace Unity.Profiling
{
	// Token: 0x02000063 RID: 99
	[DebuggerDisplay("Value = {Value}; Count = {Count}")]
	[UsedByNativeCode]
	public struct ProfilerRecorderSample
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00003418 File Offset: 0x00001618
		public long Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00003420 File Offset: 0x00001620
		public long Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x04000148 RID: 328
		private long value;

		// Token: 0x04000149 RID: 329
		private long count;

		// Token: 0x0400014A RID: 330
		private long refValue;
	}
}
