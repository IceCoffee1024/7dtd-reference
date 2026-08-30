using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.IO.Archive
{
	// Token: 0x02000089 RID: 137
	[RequiredByNativeCode]
	[NativeHeader("Runtime/VirtualFileSystem/ArchiveFileSystem/ArchiveFileHandle.h")]
	public struct ArchiveHandle
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00004AB0 File Offset: 0x00002CB0
		public ArchiveStatus Status
		{
			get
			{
				this.ThrowIfInvalid();
				return ArchiveFileInterface.Archive_GetStatus(this);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00004AD4 File Offset: 0x00002CD4
		public JobHandle JobHandle
		{
			get
			{
				this.ThrowIfInvalid();
				return ArchiveFileInterface.Archive_GetJobHandle(this);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00004AF8 File Offset: 0x00002CF8
		public JobHandle Unmount()
		{
			this.ThrowIfInvalid();
			return ArchiveFileInterface.Archive_UnmountAsync(this);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00004B1C File Offset: 0x00002D1C
		private void ThrowIfInvalid()
		{
			bool flag = !ArchiveFileInterface.Archive_IsValid(this);
			if (flag)
			{
				throw new InvalidOperationException("The archive has already been unmounted.");
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00004B48 File Offset: 0x00002D48
		public string GetMountPath()
		{
			this.ThrowIfInvalid();
			return ArchiveFileInterface.Archive_GetMountPath(this);
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00004B6C File Offset: 0x00002D6C
		public CompressionType Compression
		{
			get
			{
				this.ThrowIfInvalid();
				return ArchiveFileInterface.Archive_GetCompression(this);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00004B90 File Offset: 0x00002D90
		public bool IsStreamed
		{
			get
			{
				this.ThrowIfInvalid();
				return ArchiveFileInterface.Archive_IsStreamed(this);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public ArchiveFileInfo[] GetFileInfo()
		{
			this.ThrowIfInvalid();
			return ArchiveFileInterface.Archive_GetFileInfo(this);
		}

		// Token: 0x0400020D RID: 525
		internal ulong Handle;
	}
}
