using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x0200007D RID: 125
	public struct ReadHandle : IDisposable
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x00004200 File Offset: 0x00002400
		public bool IsValid()
		{
			return ReadHandle.IsReadHandleValid(this);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00004220 File Offset: 0x00002420
		public void Dispose()
		{
			bool flag = !ReadHandle.IsReadHandleValid(this);
			if (flag)
			{
				throw new InvalidOperationException("ReadHandle.Dispose cannot be called twice on the same ReadHandle");
			}
			bool flag2 = this.Status == ReadStatus.InProgress;
			if (flag2)
			{
				throw new InvalidOperationException("ReadHandle.Dispose cannot be called until the read operation completes");
			}
			ReadHandle.ReleaseReadHandle(this);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00004270 File Offset: 0x00002470
		public void Cancel()
		{
			bool flag = !ReadHandle.IsReadHandleValid(this);
			if (flag)
			{
				throw new InvalidOperationException("ReadHandle.Cancel cannot be called on a disposed ReadHandle");
			}
			ReadHandle.CancelInternal(this);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000042A7 File Offset: 0x000024A7
		[FreeFunction("AsyncReadManagerManaged::CancelReadRequest")]
		private static void CancelInternal(ReadHandle handle)
		{
			ReadHandle.CancelInternal_Injected(ref handle);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x000042B0 File Offset: 0x000024B0
		public JobHandle JobHandle
		{
			get
			{
				bool flag = !ReadHandle.IsReadHandleValid(this);
				if (flag)
				{
					throw new InvalidOperationException("ReadHandle.JobHandle cannot be called after the ReadHandle has been disposed");
				}
				return ReadHandle.GetJobHandle(this);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000042EC File Offset: 0x000024EC
		public ReadStatus Status
		{
			get
			{
				bool flag = !ReadHandle.IsReadHandleValid(this);
				if (flag)
				{
					throw new InvalidOperationException("Cannot use a ReadHandle that has been disposed");
				}
				return ReadHandle.GetReadStatus(this);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00004328 File Offset: 0x00002528
		public long ReadCount
		{
			get
			{
				bool flag = !ReadHandle.IsReadHandleValid(this);
				if (flag)
				{
					throw new InvalidOperationException("Cannot use a ReadHandle that has been disposed");
				}
				return ReadHandle.GetReadCount(this);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00004364 File Offset: 0x00002564
		public long GetBytesRead()
		{
			bool flag = !ReadHandle.IsReadHandleValid(this);
			if (flag)
			{
				throw new InvalidOperationException("ReadHandle.GetBytesRead cannot be called after the ReadHandle has been disposed");
			}
			return ReadHandle.GetBytesRead(this);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000043A0 File Offset: 0x000025A0
		public long GetBytesRead(uint readCommandIndex)
		{
			bool flag = !ReadHandle.IsReadHandleValid(this);
			if (flag)
			{
				throw new InvalidOperationException("ReadHandle.GetBytesRead cannot be called after the ReadHandle has been disposed");
			}
			return ReadHandle.GetBytesReadForCommand(this, readCommandIndex);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000043DC File Offset: 0x000025DC
		public unsafe ulong* GetBytesReadArray()
		{
			bool flag = !ReadHandle.IsReadHandleValid(this);
			if (flag)
			{
				throw new InvalidOperationException("ReadHandle.GetBytesReadArray cannot be called after the ReadHandle has been disposed");
			}
			return ReadHandle.GetBytesReadArray(this);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00004416 File Offset: 0x00002616
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::GetReadStatus", IsThreadSafe = true)]
		private static ReadStatus GetReadStatus(ReadHandle handle)
		{
			return ReadHandle.GetReadStatus_Injected(ref handle);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000441F File Offset: 0x0000261F
		[FreeFunction("AsyncReadManagerManaged::GetReadCount", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		private static long GetReadCount(ReadHandle handle)
		{
			return ReadHandle.GetReadCount_Injected(ref handle);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00004428 File Offset: 0x00002628
		[FreeFunction("AsyncReadManagerManaged::GetBytesRead", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		private static long GetBytesRead(ReadHandle handle)
		{
			return ReadHandle.GetBytesRead_Injected(ref handle);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00004431 File Offset: 0x00002631
		[FreeFunction("AsyncReadManagerManaged::GetBytesReadForCommand", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		private static long GetBytesReadForCommand(ReadHandle handle, uint readCommandIndex)
		{
			return ReadHandle.GetBytesReadForCommand_Injected(ref handle, readCommandIndex);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000443B File Offset: 0x0000263B
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::GetBytesReadArray", IsThreadSafe = true)]
		private unsafe static ulong* GetBytesReadArray(ReadHandle handle)
		{
			return ReadHandle.GetBytesReadArray_Injected(ref handle);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00004444 File Offset: 0x00002644
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::ReleaseReadHandle", IsThreadSafe = true)]
		private static void ReleaseReadHandle(ReadHandle handle)
		{
			ReadHandle.ReleaseReadHandle_Injected(ref handle);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000444D File Offset: 0x0000264D
		[FreeFunction("AsyncReadManagerManaged::IsReadHandleValid", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		private static bool IsReadHandleValid(ReadHandle handle)
		{
			return ReadHandle.IsReadHandleValid_Injected(ref handle);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00004458 File Offset: 0x00002658
		[FreeFunction("AsyncReadManagerManaged::GetJobHandle", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		private static JobHandle GetJobHandle(ReadHandle handle)
		{
			JobHandle jobHandle;
			ReadHandle.GetJobHandle_Injected(ref handle, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000203 RID: 515
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CancelInternal_Injected(ref ReadHandle handle);

		// Token: 0x06000204 RID: 516
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ReadStatus GetReadStatus_Injected(ref ReadHandle handle);

		// Token: 0x06000205 RID: 517
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetReadCount_Injected(ref ReadHandle handle);

		// Token: 0x06000206 RID: 518
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetBytesRead_Injected(ref ReadHandle handle);

		// Token: 0x06000207 RID: 519
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetBytesReadForCommand_Injected(ref ReadHandle handle, uint readCommandIndex);

		// Token: 0x06000208 RID: 520
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern ulong* GetBytesReadArray_Injected(ref ReadHandle handle);

		// Token: 0x06000209 RID: 521
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReleaseReadHandle_Injected(ref ReadHandle handle);

		// Token: 0x0600020A RID: 522
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsReadHandleValid_Injected(ref ReadHandle handle);

		// Token: 0x0600020B RID: 523
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetJobHandle_Injected(ref ReadHandle handle, out JobHandle ret);

		// Token: 0x040001CE RID: 462
		[NativeDisableUnsafePtrRestriction]
		internal IntPtr ptr;

		// Token: 0x040001CF RID: 463
		internal int version;
	}
}
