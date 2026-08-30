using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.IO
{
	// Token: 0x0200048F RID: 1167
	[NativeHeader("Runtime/VirtualFileSystem/VirtualFileSystem.h")]
	[StaticAccessor("FileAccessor", StaticAccessorType.DoubleColon)]
	[NativeConditional("ENABLE_PROFILER")]
	internal static class File
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x00044E64 File Offset: 0x00043064
		internal static ulong totalOpenCalls
		{
			get
			{
				return File.GetTotalOpenCalls();
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x00044E7C File Offset: 0x0004307C
		internal static ulong totalCloseCalls
		{
			get
			{
				return File.GetTotalCloseCalls();
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x00044E94 File Offset: 0x00043094
		internal static ulong totalReadCalls
		{
			get
			{
				return File.GetTotalReadCalls();
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x00044EAC File Offset: 0x000430AC
		internal static ulong totalWriteCalls
		{
			get
			{
				return File.GetTotalWriteCalls();
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x00044EC4 File Offset: 0x000430C4
		internal static ulong totalSeekCalls
		{
			get
			{
				return File.GetTotalSeekCalls();
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x00044EDC File Offset: 0x000430DC
		internal static ulong totalZeroSeekCalls
		{
			get
			{
				return File.GetTotalZeroSeekCalls();
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x00044EF4 File Offset: 0x000430F4
		internal static ulong totalFilesOpened
		{
			get
			{
				return File.GetTotalFilesOpened();
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x00044F0C File Offset: 0x0004310C
		internal static ulong totalFilesClosed
		{
			get
			{
				return File.GetTotalFilesClosed();
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x00044F24 File Offset: 0x00043124
		internal static ulong totalBytesRead
		{
			get
			{
				return File.GetTotalBytesRead();
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x00044F3C File Offset: 0x0004313C
		internal static ulong totalBytesWritten
		{
			get
			{
				return File.GetTotalBytesWritten();
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x00044F60 File Offset: 0x00043160
		// (set) Token: 0x0600282D RID: 10285 RVA: 0x00044F53 File Offset: 0x00043153
		internal static bool recordZeroSeeks
		{
			get
			{
				return File.GetRecordZeroSeeks();
			}
			set
			{
				File.SetRecordZeroSeeks(value);
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x00044F78 File Offset: 0x00043178
		// (set) Token: 0x06002830 RID: 10288 RVA: 0x00044F8F File Offset: 0x0004318F
		internal static ThreadIORestrictionMode MainThreadIORestrictionMode
		{
			get
			{
				return File.GetMainThreadFileIORestriction();
			}
			set
			{
				File.SetMainThreadFileIORestriction(value);
			}
		}

		// Token: 0x06002831 RID: 10289
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetRecordZeroSeeks(bool enable);

		// Token: 0x06002832 RID: 10290
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetRecordZeroSeeks();

		// Token: 0x06002833 RID: 10291
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalOpenCalls();

		// Token: 0x06002834 RID: 10292
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalCloseCalls();

		// Token: 0x06002835 RID: 10293
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalReadCalls();

		// Token: 0x06002836 RID: 10294
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalWriteCalls();

		// Token: 0x06002837 RID: 10295
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalSeekCalls();

		// Token: 0x06002838 RID: 10296
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalZeroSeekCalls();

		// Token: 0x06002839 RID: 10297
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalFilesOpened();

		// Token: 0x0600283A RID: 10298
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalFilesClosed();

		// Token: 0x0600283B RID: 10299
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalBytesRead();

		// Token: 0x0600283C RID: 10300
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetTotalBytesWritten();

		// Token: 0x0600283D RID: 10301
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMainThreadFileIORestriction(ThreadIORestrictionMode mode);

		// Token: 0x0600283E RID: 10302
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ThreadIORestrictionMode GetMainThreadFileIORestriction();
	}
}
