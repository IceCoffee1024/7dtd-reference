using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x02000086 RID: 134
	[NativeHeader("Runtime/VirtualFileSystem/VirtualFileSystem.h")]
	[StaticAccessor("GetFileSystem()", StaticAccessorType.Dot)]
	public static class VirtualFileSystem
	{
		// Token: 0x0600027B RID: 635
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetLocalFileSystemName(string vfsFileName, out string localFileName, out ulong localFileOffset, out ulong localFileSize);

		// Token: 0x0600027C RID: 636
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string ToLogicalPath(string physicalPath);
	}
}
