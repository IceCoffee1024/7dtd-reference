using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200015C RID: 348
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public static class RendererExtensions
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x00011EDE File Offset: 0x000100DE
		public static void UpdateGIMaterials(this Renderer renderer)
		{
			RendererExtensions.UpdateGIMaterialsForRenderer(renderer);
		}

		// Token: 0x06000B86 RID: 2950
		[FreeFunction("RendererScripting::UpdateGIMaterialsForRenderer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateGIMaterialsForRenderer(Renderer renderer);
	}
}
