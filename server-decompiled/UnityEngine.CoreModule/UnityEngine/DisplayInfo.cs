using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000146 RID: 326
	[NativeType("Runtime/Graphics/DisplayInfo.h")]
	[UsedByNativeCode]
	public struct DisplayInfo : IEquatable<DisplayInfo>
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(DisplayInfo other)
		{
			return this.handle == other.handle && this.width == other.width && this.height == other.height && this.refreshRate.Equals(other.refreshRate) && this.workArea.Equals(other.workArea) && this.name == other.name;
		}

		// Token: 0x04000418 RID: 1048
		[RequiredMember]
		internal ulong handle;

		// Token: 0x04000419 RID: 1049
		[RequiredMember]
		public int width;

		// Token: 0x0400041A RID: 1050
		[RequiredMember]
		public int height;

		// Token: 0x0400041B RID: 1051
		[RequiredMember]
		public RefreshRate refreshRate;

		// Token: 0x0400041C RID: 1052
		[RequiredMember]
		public RectInt workArea;

		// Token: 0x0400041D RID: 1053
		[RequiredMember]
		public string name;
	}
}
