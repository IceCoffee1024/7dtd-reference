using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001E2 RID: 482
	[NativeHeader("Runtime/Export/Input/Cursor.bindings.h")]
	public class Cursor
	{
		// Token: 0x060014AB RID: 5291 RVA: 0x0001E3EF File Offset: 0x0001C5EF
		private static void SetCursor(Texture2D texture, CursorMode cursorMode)
		{
			Cursor.SetCursor(texture, Vector2.zero, cursorMode);
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0001E3FF File Offset: 0x0001C5FF
		public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
		{
			Cursor.SetCursor_Injected(texture, ref hotspot, cursorMode);
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060014AD RID: 5293
		// (set) Token: 0x060014AE RID: 5294
		public static extern bool visible
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060014AF RID: 5295
		// (set) Token: 0x060014B0 RID: 5296
		public static extern CursorLockMode lockState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060014B2 RID: 5298
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCursor_Injected(Texture2D texture, ref Vector2 hotspot, CursorMode cursorMode);
	}
}
