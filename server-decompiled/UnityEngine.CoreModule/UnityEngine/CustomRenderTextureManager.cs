using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200013D RID: 317
	[NativeHeader("Runtime/Graphics/CustomRenderTextureManager.h")]
	public static class CustomRenderTextureManager
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060008D2 RID: 2258 RVA: 0x0000E314 File Offset: 0x0000C514
		// (remove) Token: 0x060008D3 RID: 2259 RVA: 0x0000E348 File Offset: 0x0000C548
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<CustomRenderTexture> textureLoaded;

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000E37B File Offset: 0x0000C57B
		[RequiredByNativeCode]
		private static void InvokeOnTextureLoaded_Internal(CustomRenderTexture source)
		{
			Action<CustomRenderTexture> action = CustomRenderTextureManager.textureLoaded;
			if (action != null)
			{
				action(source);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060008D5 RID: 2261 RVA: 0x0000E390 File Offset: 0x0000C590
		// (remove) Token: 0x060008D6 RID: 2262 RVA: 0x0000E3C4 File Offset: 0x0000C5C4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<CustomRenderTexture> textureUnloaded;

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000E3F7 File Offset: 0x0000C5F7
		[RequiredByNativeCode]
		private static void InvokeOnTextureUnloaded_Internal(CustomRenderTexture source)
		{
			Action<CustomRenderTexture> action = CustomRenderTextureManager.textureUnloaded;
			if (action != null)
			{
				action(source);
			}
		}

		// Token: 0x060008D8 RID: 2264
		[FreeFunction(Name = "CustomRenderTextureManagerScripting::GetAllCustomRenderTextures", HasExplicitThis = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetAllCustomRenderTextures(List<CustomRenderTexture> currentCustomRenderTextures);

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060008D9 RID: 2265 RVA: 0x0000E40C File Offset: 0x0000C60C
		// (remove) Token: 0x060008DA RID: 2266 RVA: 0x0000E440 File Offset: 0x0000C640
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<CustomRenderTexture, int> updateTriggered;

		// Token: 0x060008DB RID: 2267 RVA: 0x0000E473 File Offset: 0x0000C673
		internal static void InvokeTriggerUpdate(CustomRenderTexture crt, int updateCount)
		{
			Action<CustomRenderTexture, int> action = CustomRenderTextureManager.updateTriggered;
			if (action != null)
			{
				action(crt, updateCount);
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060008DC RID: 2268 RVA: 0x0000E488 File Offset: 0x0000C688
		// (remove) Token: 0x060008DD RID: 2269 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<CustomRenderTexture> initializeTriggered;

		// Token: 0x060008DE RID: 2270 RVA: 0x0000E4EF File Offset: 0x0000C6EF
		internal static void InvokeTriggerInitialize(CustomRenderTexture crt)
		{
			Action<CustomRenderTexture> action = CustomRenderTextureManager.initializeTriggered;
			if (action != null)
			{
				action(crt);
			}
		}
	}
}
