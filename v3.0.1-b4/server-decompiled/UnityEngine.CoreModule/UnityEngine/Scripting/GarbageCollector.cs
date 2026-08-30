using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Scripting
{
	// Token: 0x02000315 RID: 789
	[NativeHeader("Runtime/Scripting/GarbageCollector.h")]
	public static class GarbageCollector
	{
		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002035 RID: 8245 RVA: 0x00035908 File Offset: 0x00033B08
		// (remove) Token: 0x06002036 RID: 8246 RVA: 0x0003593C File Offset: 0x00033B3C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<GarbageCollector.Mode> GCModeChanged;

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002037 RID: 8247 RVA: 0x00035970 File Offset: 0x00033B70
		// (set) Token: 0x06002038 RID: 8248 RVA: 0x00035988 File Offset: 0x00033B88
		public static GarbageCollector.Mode GCMode
		{
			get
			{
				return GarbageCollector.GetMode();
			}
			set
			{
				bool flag = value == GarbageCollector.GetMode();
				if (!flag)
				{
					GarbageCollector.SetMode(value);
					bool flag2 = GarbageCollector.GCModeChanged != null;
					if (flag2)
					{
						GarbageCollector.GCModeChanged(value);
					}
				}
			}
		}

		// Token: 0x06002039 RID: 8249
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMode(GarbageCollector.Mode mode);

		// Token: 0x0600203A RID: 8250
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GarbageCollector.Mode GetMode();

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600203B RID: 8251
		public static extern bool isIncremental
		{
			[NativeMethod("GetIncrementalEnabled")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600203C RID: 8252
		// (set) Token: 0x0600203D RID: 8253
		public static extern ulong incrementalTimeSliceNanoseconds
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600203E RID: 8254
		[NativeThrows]
		[NativeMethod("CollectIncrementalWrapper")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CollectIncremental(ulong nanoseconds = 0UL);

		// Token: 0x02000316 RID: 790
		public enum Mode
		{
			// Token: 0x04000AAF RID: 2735
			Disabled,
			// Token: 0x04000AB0 RID: 2736
			Enabled,
			// Token: 0x04000AB1 RID: 2737
			Manual
		}
	}
}
