using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x0200007E RID: 126
	[NativeHeader("Runtime/File/AsyncReadManagerManagedApi.h")]
	public static class AsyncReadManager
	{
		// Token: 0x0600020C RID: 524 RVA: 0x00004470 File Offset: 0x00002670
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::Read", IsThreadSafe = true)]
		private unsafe static ReadHandle ReadInternal(string filename, void* cmds, uint cmdCount, string assetName, ulong typeID, AssetLoadingSubsystem subsystem)
		{
			ReadHandle readHandle;
			AsyncReadManager.ReadInternal_Injected(filename, cmds, cmdCount, assetName, typeID, subsystem, out readHandle);
			return readHandle;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00004490 File Offset: 0x00002690
		public unsafe static ReadHandle Read(string filename, ReadCommand* readCmds, uint readCmdCount, string assetName = "", ulong typeID = 0UL, AssetLoadingSubsystem subsystem = AssetLoadingSubsystem.Scripts)
		{
			return AsyncReadManager.ReadInternal(filename, (void*)readCmds, readCmdCount, assetName, typeID, subsystem);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000044B0 File Offset: 0x000026B0
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::GetFileInfo", IsThreadSafe = true)]
		private unsafe static ReadHandle GetFileInfoInternal(string filename, void* cmd)
		{
			ReadHandle readHandle;
			AsyncReadManager.GetFileInfoInternal_Injected(filename, cmd, out readHandle);
			return readHandle;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000044C8 File Offset: 0x000026C8
		public unsafe static ReadHandle GetFileInfo(string filename, FileInfoResult* result)
		{
			bool flag = result == null;
			if (flag)
			{
				throw new NullReferenceException("GetFileInfo must have a valid FileInfoResult to write into.");
			}
			return AsyncReadManager.GetFileInfoInternal(filename, (void*)result);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000044F8 File Offset: 0x000026F8
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::ReadWithHandles_NativePtr", IsThreadSafe = true)]
		private unsafe static ReadHandle ReadWithHandlesInternal_NativePtr(in FileHandle fileHandle, void* readCmdArray, JobHandle dependency)
		{
			ReadHandle readHandle;
			AsyncReadManager.ReadWithHandlesInternal_NativePtr_Injected(in fileHandle, readCmdArray, ref dependency, out readHandle);
			return readHandle;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00004514 File Offset: 0x00002714
		[FreeFunction("AsyncReadManagerManaged::ReadWithHandles_NativeCopy", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		private unsafe static ReadHandle ReadWithHandlesInternal_NativeCopy(in FileHandle fileHandle, void* readCmdArray)
		{
			ReadHandle readHandle;
			AsyncReadManager.ReadWithHandlesInternal_NativeCopy_Injected(in fileHandle, readCmdArray, out readHandle);
			return readHandle;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000452C File Offset: 0x0000272C
		public unsafe static ReadHandle ReadDeferred(in FileHandle fileHandle, ReadCommandArray* readCmdArray, JobHandle dependency)
		{
			bool flag = !fileHandle.IsValid();
			if (flag)
			{
				throw new InvalidOperationException("FileHandle is invalid and may not be read from.");
			}
			return AsyncReadManager.ReadWithHandlesInternal_NativePtr(in fileHandle, (void*)readCmdArray, dependency);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00004560 File Offset: 0x00002760
		public static ReadHandle Read(in FileHandle fileHandle, ReadCommandArray readCmdArray)
		{
			bool flag = !fileHandle.IsValid();
			if (flag)
			{
				throw new InvalidOperationException("FileHandle is invalid and may not be read from.");
			}
			return AsyncReadManager.ReadWithHandlesInternal_NativeCopy(in fileHandle, UnsafeUtility.AddressOf<ReadCommandArray>(ref readCmdArray));
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00004598 File Offset: 0x00002798
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::ScheduleOpenRequest", IsThreadSafe = true)]
		private static FileHandle OpenFileAsync_Internal(string fileName)
		{
			FileHandle fileHandle;
			AsyncReadManager.OpenFileAsync_Internal_Injected(fileName, out fileHandle);
			return fileHandle;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000045B0 File Offset: 0x000027B0
		public static FileHandle OpenFileAsync(string fileName)
		{
			bool flag = fileName.Length == 0;
			if (flag)
			{
				throw new InvalidOperationException("FileName is empty");
			}
			return AsyncReadManager.OpenFileAsync_Internal(fileName);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000045E0 File Offset: 0x000027E0
		[FreeFunction("AsyncReadManagerManaged::ScheduleCloseRequest", IsThreadSafe = true)]
		[ThreadAndSerializationSafe]
		internal static JobHandle CloseFileAsync(in FileHandle fileHandle, JobHandle dependency)
		{
			JobHandle jobHandle;
			AsyncReadManager.CloseFileAsync_Injected(in fileHandle, ref dependency, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000045F8 File Offset: 0x000027F8
		[ThreadAndSerializationSafe]
		[FreeFunction("AsyncReadManagerManaged::ScheduleCloseCachedFileRequest", IsThreadSafe = true)]
		public static JobHandle CloseCachedFileAsync(string fileName, JobHandle dependency = default(JobHandle))
		{
			JobHandle jobHandle;
			AsyncReadManager.CloseCachedFileAsync_Injected(fileName, ref dependency, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000218 RID: 536
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ReadInternal_Injected(string filename, void* cmds, uint cmdCount, string assetName, ulong typeID, AssetLoadingSubsystem subsystem, out ReadHandle ret);

		// Token: 0x06000219 RID: 537
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void GetFileInfoInternal_Injected(string filename, void* cmd, out ReadHandle ret);

		// Token: 0x0600021A RID: 538
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ReadWithHandlesInternal_NativePtr_Injected(in FileHandle fileHandle, void* readCmdArray, ref JobHandle dependency, out ReadHandle ret);

		// Token: 0x0600021B RID: 539
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ReadWithHandlesInternal_NativeCopy_Injected(in FileHandle fileHandle, void* readCmdArray, out ReadHandle ret);

		// Token: 0x0600021C RID: 540
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OpenFileAsync_Internal_Injected(string fileName, out FileHandle ret);

		// Token: 0x0600021D RID: 541
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CloseFileAsync_Injected(in FileHandle fileHandle, ref JobHandle dependency, out JobHandle ret);

		// Token: 0x0600021E RID: 542
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CloseCachedFileAsync_Injected(string fileName, ref JobHandle dependency = null, out JobHandle ret);
	}
}
