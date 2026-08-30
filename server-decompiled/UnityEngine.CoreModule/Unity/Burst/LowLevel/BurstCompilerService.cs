using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace Unity.Burst.LowLevel
{
	// Token: 0x020000BC RID: 188
	[NativeHeader("Runtime/Burst/BurstDelegateCache.h")]
	[StaticAccessor("BurstCompilerService::Get()", StaticAccessorType.Arrow)]
	[NativeHeader("Runtime/Burst/Burst.h")]
	internal static class BurstCompilerService
	{
		// Token: 0x060003A2 RID: 930
		[NativeMethod("Initialize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string InitializeInternal(string path, BurstCompilerService.ExtractCompilerFlags extractCompilerFlags);

		// Token: 0x060003A3 RID: 931
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetDisassembly(MethodInfo m, string compilerOptions);

		// Token: 0x060003A4 RID: 932
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CompileAsyncDelegateMethod(object delegateMethod, string compilerOptions);

		// Token: 0x060003A5 RID: 933
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void* GetAsyncCompiledAsyncDelegateMethod(int userID);

		// Token: 0x060003A6 RID: 934
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void* GetOrCreateSharedMemory(ref Hash128 key, uint size_of, uint alignment);

		// Token: 0x060003A7 RID: 935
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetMethodSignature(MethodInfo method);

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003A8 RID: 936
		public static extern bool IsInitialized
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060003A9 RID: 937
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCurrentExecutionMode(uint environment);

		// Token: 0x060003AA RID: 938
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetCurrentExecutionMode();

		// Token: 0x060003AB RID: 939
		[FreeFunction("DefaultBurstLogCallback", true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void Log(void* userData, BurstCompilerService.BurstLogType logType, byte* message, byte* filename, int lineNumber);

		// Token: 0x060003AC RID: 940
		[FreeFunction("DefaultBurstRuntimeLogCallback", true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void RuntimeLog(void* userData, BurstCompilerService.BurstLogType logType, byte* message, byte* filename, int lineNumber);

		// Token: 0x060003AD RID: 941
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool LoadBurstLibrary(string fullPathToLibBurstGenerated);

		// Token: 0x060003AE RID: 942 RVA: 0x00006A40 File Offset: 0x00004C40
		public static void Initialize(string folderRuntime, BurstCompilerService.ExtractCompilerFlags extractCompilerFlags)
		{
			bool flag = folderRuntime == null;
			if (flag)
			{
				throw new ArgumentNullException("folderRuntime");
			}
			bool flag2 = extractCompilerFlags == null;
			if (flag2)
			{
				throw new ArgumentNullException("extractCompilerFlags");
			}
			bool flag3 = !Directory.Exists(folderRuntime);
			if (flag3)
			{
				Debug.LogError("Unable to initialize the burst JIT compiler. The folder `" + folderRuntime + "` does not exist");
			}
			else
			{
				string text = BurstCompilerService.InitializeInternal(folderRuntime, extractCompilerFlags);
				bool flag4 = !string.IsNullOrEmpty(text);
				if (flag4)
				{
					Debug.LogError("Unexpected error while trying to initialize the burst JIT compiler: " + text);
				}
			}
		}

		// Token: 0x020000BD RID: 189
		// (Invoke) Token: 0x060003B0 RID: 944
		public delegate bool ExtractCompilerFlags(Type jobType, out string flags);

		// Token: 0x020000BE RID: 190
		public enum BurstLogType
		{
			// Token: 0x0400024A RID: 586
			Info,
			// Token: 0x0400024B RID: 587
			Warning,
			// Token: 0x0400024C RID: 588
			Error
		}
	}
}
