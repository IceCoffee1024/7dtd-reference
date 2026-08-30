using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000431 RID: 1073
	[NativeHeader("Runtime/Export/Graphics/RenderingCommandBufferExtensions.bindings.h")]
	[UsedByNativeCode]
	public static class CommandBufferExtensions
	{
		// Token: 0x06002421 RID: 9249
		[FreeFunction("RenderingCommandBufferExtensions_Bindings::Internal_SwitchIntoFastMemory")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SwitchIntoFastMemory([NotNull("NullExceptionObject")] CommandBuffer cmd, ref RenderTargetIdentifier rt, FastMemoryFlags fastMemoryFlags, float residency, bool copyContents);

		// Token: 0x06002422 RID: 9250
		[FreeFunction("RenderingCommandBufferExtensions_Bindings::Internal_SwitchOutOfFastMemory")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SwitchOutOfFastMemory([NotNull("NullExceptionObject")] CommandBuffer cmd, ref RenderTargetIdentifier rt, bool copyContents);

		// Token: 0x06002423 RID: 9251 RVA: 0x0003C002 File Offset: 0x0003A202
		[NativeConditional("UNITY_XBOXONE || UNITY_GAMECORE_XBOXONE")]
		public static void SwitchIntoFastMemory(this CommandBuffer cmd, RenderTargetIdentifier rid, FastMemoryFlags fastMemoryFlags, float residency, bool copyContents)
		{
			CommandBufferExtensions.Internal_SwitchIntoFastMemory(cmd, ref rid, fastMemoryFlags, residency, copyContents);
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0003C012 File Offset: 0x0003A212
		[NativeConditional("UNITY_XBOXONE || UNITY_GAMECORE_XBOXONE")]
		public static void SwitchOutOfFastMemory(this CommandBuffer cmd, RenderTargetIdentifier rid, bool copyContents)
		{
			CommandBufferExtensions.Internal_SwitchOutOfFastMemory(cmd, ref rid, copyContents);
		}
	}
}
