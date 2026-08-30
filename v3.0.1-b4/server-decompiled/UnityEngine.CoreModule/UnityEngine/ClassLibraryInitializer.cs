using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32.SafeHandles;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000238 RID: 568
	internal static class ClassLibraryInitializer
	{
		// Token: 0x06001861 RID: 6241 RVA: 0x00028588 File Offset: 0x00026788
		[RequiredByNativeCode]
		private static void Init()
		{
			UnityLogWriter.Init();
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00028594 File Offset: 0x00026794
		[RequiredByNativeCode]
		private static void InitStdErrWithHandle(IntPtr fileHandle)
		{
			SafeFileHandle safeFileHandle = new SafeFileHandle(fileHandle, false);
			bool flag = !safeFileHandle.IsInvalid;
			if (flag)
			{
				StreamWriter streamWriter = new StreamWriter(new FileStream(safeFileHandle, FileAccess.Write))
				{
					AutoFlush = true
				};
				Console.SetError(streamWriter);
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x000285D5 File Offset: 0x000267D5
		[RequiredByNativeCode]
		private static void InitAssemblyRedirections()
		{
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object _, ResolveEventArgs args)
			{
				AssemblyName assemblyName = new AssemblyName(args.Name);
				Assembly assembly2;
				try
				{
					Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName.Name);
					assembly2 = assembly;
				}
				catch
				{
					assembly2 = null;
				}
				return assembly2;
			};
		}
	}
}
