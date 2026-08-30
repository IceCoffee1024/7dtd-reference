using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x0200007C RID: 124
	public readonly struct FileHandle
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000413C File Offset: 0x0000233C
		public FileStatus Status
		{
			get
			{
				bool flag = !FileHandle.IsFileHandleValid(in this);
				if (flag)
				{
					throw new InvalidOperationException("FileHandle.Status cannot be called on a closed FileHandle");
				}
				return FileHandle.GetFileStatus_Internal(in this);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000416C File Offset: 0x0000236C
		public JobHandle JobHandle
		{
			get
			{
				bool flag = !FileHandle.IsFileHandleValid(in this);
				if (flag)
				{
					throw new InvalidOperationException("FileHandle.JobHandle cannot be called on a closed FileHandle");
				}
				return FileHandle.GetJobHandle_Internal(in this);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000419C File Offset: 0x0000239C
		public bool IsValid()
		{
			return FileHandle.IsFileHandleValid(in this);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000041B4 File Offset: 0x000023B4
		public JobHandle Close(JobHandle dependency = default(JobHandle))
		{
			bool flag = !FileHandle.IsFileHandleValid(in this);
			if (flag)
			{
				throw new InvalidOperationException("FileHandle.Close cannot be called twice on the same FileHandle");
			}
			return AsyncReadManager.CloseFileAsync(in this, dependency);
		}

		// Token: 0x060001ED RID: 493
		[FreeFunction("AsyncReadManagerManaged::IsFileHandleValid")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsFileHandleValid(in FileHandle handle);

		// Token: 0x060001EE RID: 494
		[FreeFunction("AsyncReadManagerManaged::GetFileStatusFromManagedHandle")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FileStatus GetFileStatus_Internal(in FileHandle handle);

		// Token: 0x060001EF RID: 495 RVA: 0x000041E8 File Offset: 0x000023E8
		[FreeFunction("AsyncReadManagerManaged::GetJobFenceFromManagedHandle")]
		private static JobHandle GetJobHandle_Internal(in FileHandle handle)
		{
			JobHandle jobHandle;
			FileHandle.GetJobHandle_Internal_Injected(in handle, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060001F0 RID: 496
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetJobHandle_Internal_Injected(in FileHandle handle, out JobHandle ret);

		// Token: 0x040001CC RID: 460
		[NativeDisableUnsafePtrRestriction]
		internal readonly IntPtr fileCommandPtr;

		// Token: 0x040001CD RID: 461
		internal readonly int version;
	}
}
