using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001BD RID: 445
	[StaticAccessor("GetQualitySettings()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Graphics/QualitySettings.h")]
	public static class TextureMipmapLimitGroups
	{
		// Token: 0x06001014 RID: 4116
		[NativeName("GetTextureMipmapLimitGroupNames")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetGroups();

		// Token: 0x06001015 RID: 4117
		[NativeName("HasTextureMipmapLimitGroup")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasGroup([NotNull("ArgumentNullException")] string groupName);
	}
}
