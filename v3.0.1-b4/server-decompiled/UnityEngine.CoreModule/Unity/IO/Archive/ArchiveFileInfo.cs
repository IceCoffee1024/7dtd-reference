using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.IO.Archive
{
	// Token: 0x02000088 RID: 136
	[RequiredByNativeCode]
	[NativeHeader("Runtime/VirtualFileSystem/ArchiveFileSystem/ArchiveFileHandle.h")]
	public struct ArchiveFileInfo
	{
		// Token: 0x0400020B RID: 523
		public string Filename;

		// Token: 0x0400020C RID: 524
		public ulong FileSize;
	}
}
