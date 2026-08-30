using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.U2D
{
	// Token: 0x020002B7 RID: 695
	[StaticAccessor("GetSpriteAtlasManager()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/2D/SpriteAtlas/SpriteAtlasManager.h")]
	[NativeHeader("Runtime/2D/SpriteAtlas/SpriteAtlas.h")]
	public class SpriteAtlasManager
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001DA1 RID: 7585 RVA: 0x00030DA0 File Offset: 0x0002EFA0
		// (remove) Token: 0x06001DA2 RID: 7586 RVA: 0x00030DD4 File Offset: 0x0002EFD4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<string, Action<SpriteAtlas>> atlasRequested;

		// Token: 0x06001DA3 RID: 7587 RVA: 0x00030E08 File Offset: 0x0002F008
		[RequiredByNativeCode]
		private static bool RequestAtlas(string tag)
		{
			bool flag = SpriteAtlasManager.atlasRequested != null;
			bool flag2;
			if (flag)
			{
				SpriteAtlasManager.atlasRequested(tag, new Action<SpriteAtlas>(SpriteAtlasManager.Register));
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001DA4 RID: 7588 RVA: 0x00030E44 File Offset: 0x0002F044
		// (remove) Token: 0x06001DA5 RID: 7589 RVA: 0x00030E78 File Offset: 0x0002F078
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<SpriteAtlas> atlasRegistered;

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00030EAB File Offset: 0x0002F0AB
		[RequiredByNativeCode]
		private static void PostRegisteredAtlas(SpriteAtlas spriteAtlas)
		{
			Action<SpriteAtlas> action = SpriteAtlasManager.atlasRegistered;
			if (action != null)
			{
				action(spriteAtlas);
			}
		}

		// Token: 0x06001DA7 RID: 7591
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Register(SpriteAtlas spriteAtlas);
	}
}
