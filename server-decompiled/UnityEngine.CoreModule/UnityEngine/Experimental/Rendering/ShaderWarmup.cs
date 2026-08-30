using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004F2 RID: 1266
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	public static class ShaderWarmup
	{
		// Token: 0x06002BD7 RID: 11223 RVA: 0x00049B4B File Offset: 0x00047D4B
		[FreeFunction(Name = "ShaderWarmupScripting::WarmupShader")]
		public static void WarmupShader(Shader shader, ShaderWarmupSetup setup)
		{
			ShaderWarmup.WarmupShader_Injected(shader, ref setup);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x00049B55 File Offset: 0x00047D55
		[FreeFunction(Name = "ShaderWarmupScripting::WarmupShaderFromCollection")]
		public static void WarmupShaderFromCollection(ShaderVariantCollection collection, Shader shader, ShaderWarmupSetup setup)
		{
			ShaderWarmup.WarmupShaderFromCollection_Injected(collection, shader, ref setup);
		}

		// Token: 0x06002BD9 RID: 11225
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WarmupShader_Injected(Shader shader, ref ShaderWarmupSetup setup);

		// Token: 0x06002BDA RID: 11226
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WarmupShaderFromCollection_Injected(ShaderVariantCollection collection, Shader shader, ref ShaderWarmupSetup setup);
	}
}
