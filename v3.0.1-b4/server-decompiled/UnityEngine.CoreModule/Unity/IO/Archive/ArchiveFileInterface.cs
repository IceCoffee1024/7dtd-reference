using System;
using System.Runtime.CompilerServices;
using Unity.Content;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.IO.Archive
{
	// Token: 0x0200008A RID: 138
	[NativeHeader("Runtime/VirtualFileSystem/ArchiveFileSystem/ArchiveFileHandle.h")]
	[RequiredByNativeCode]
	[StaticAccessor("GetManagedArchiveSystem()", StaticAccessorType.Dot)]
	public static class ArchiveFileInterface
	{
		// Token: 0x06000285 RID: 645 RVA: 0x00004BD8 File Offset: 0x00002DD8
		public static ArchiveHandle MountAsync(ContentNamespace namespaceId, string filePath, string prefix)
		{
			ArchiveHandle archiveHandle;
			ArchiveFileInterface.MountAsync_Injected(ref namespaceId, filePath, prefix, out archiveHandle);
			return archiveHandle;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00004BF1 File Offset: 0x00002DF1
		public static ArchiveHandle[] GetMountedArchives(ContentNamespace namespaceId)
		{
			return ArchiveFileInterface.GetMountedArchives_Injected(ref namespaceId);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00004BFA File Offset: 0x00002DFA
		internal static ArchiveStatus Archive_GetStatus(ArchiveHandle handle)
		{
			return ArchiveFileInterface.Archive_GetStatus_Injected(ref handle);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00004C04 File Offset: 0x00002E04
		internal static JobHandle Archive_GetJobHandle(ArchiveHandle handle)
		{
			JobHandle jobHandle;
			ArchiveFileInterface.Archive_GetJobHandle_Injected(ref handle, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00004C1B File Offset: 0x00002E1B
		internal static bool Archive_IsValid(ArchiveHandle handle)
		{
			return ArchiveFileInterface.Archive_IsValid_Injected(ref handle);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00004C24 File Offset: 0x00002E24
		internal static JobHandle Archive_UnmountAsync(ArchiveHandle handle)
		{
			JobHandle jobHandle;
			ArchiveFileInterface.Archive_UnmountAsync_Injected(ref handle, out jobHandle);
			return jobHandle;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00004C3B File Offset: 0x00002E3B
		internal static string Archive_GetMountPath(ArchiveHandle handle)
		{
			return ArchiveFileInterface.Archive_GetMountPath_Injected(ref handle);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00004C44 File Offset: 0x00002E44
		internal static CompressionType Archive_GetCompression(ArchiveHandle handle)
		{
			return ArchiveFileInterface.Archive_GetCompression_Injected(ref handle);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00004C4D File Offset: 0x00002E4D
		internal static bool Archive_IsStreamed(ArchiveHandle handle)
		{
			return ArchiveFileInterface.Archive_IsStreamed_Injected(ref handle);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00004C56 File Offset: 0x00002E56
		internal static ArchiveFileInfo[] Archive_GetFileInfo(ArchiveHandle handle)
		{
			return ArchiveFileInterface.Archive_GetFileInfo_Injected(ref handle);
		}

		// Token: 0x0600028F RID: 655
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MountAsync_Injected(ref ContentNamespace namespaceId, string filePath, string prefix, out ArchiveHandle ret);

		// Token: 0x06000290 RID: 656
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ArchiveHandle[] GetMountedArchives_Injected(ref ContentNamespace namespaceId);

		// Token: 0x06000291 RID: 657
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ArchiveStatus Archive_GetStatus_Injected(ref ArchiveHandle handle);

		// Token: 0x06000292 RID: 658
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Archive_GetJobHandle_Injected(ref ArchiveHandle handle, out JobHandle ret);

		// Token: 0x06000293 RID: 659
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Archive_IsValid_Injected(ref ArchiveHandle handle);

		// Token: 0x06000294 RID: 660
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Archive_UnmountAsync_Injected(ref ArchiveHandle handle, out JobHandle ret);

		// Token: 0x06000295 RID: 661
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Archive_GetMountPath_Injected(ref ArchiveHandle handle);

		// Token: 0x06000296 RID: 662
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CompressionType Archive_GetCompression_Injected(ref ArchiveHandle handle);

		// Token: 0x06000297 RID: 663
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Archive_IsStreamed_Injected(ref ArchiveHandle handle);

		// Token: 0x06000298 RID: 664
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ArchiveFileInfo[] Archive_GetFileInfo_Injected(ref ArchiveHandle handle);
	}
}
