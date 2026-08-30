using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.U2D
{
	// Token: 0x020004B9 RID: 1209
	[NativeHeader("Runtime/2D/Renderer/SpriteRendererGroup.h")]
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	internal class SpriteRendererGroup
	{
		// Token: 0x06002AB5 RID: 10933 RVA: 0x00047A3B File Offset: 0x00045C3B
		public static void AddRenderers(NativeArray<SpriteIntermediateRendererInfo> renderers)
		{
			SpriteRendererGroup.AddRenderers(renderers.GetUnsafeReadOnlyPtr<SpriteIntermediateRendererInfo>(), renderers.Length);
		}

		// Token: 0x06002AB6 RID: 10934
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void AddRenderers(void* renderers, int count);

		// Token: 0x06002AB7 RID: 10935
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Clear();
	}
}
