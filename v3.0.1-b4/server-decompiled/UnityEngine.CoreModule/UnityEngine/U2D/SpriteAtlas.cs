using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.U2D
{
	// Token: 0x020002B8 RID: 696
	[NativeType(Header = "Runtime/2D/SpriteAtlas/SpriteAtlas.h")]
	[NativeHeader("Runtime/Graphics/SpriteFrame.h")]
	public class SpriteAtlas : Object
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001DA9 RID: 7593
		public extern bool isVariant
		{
			[NativeMethod("IsVariant")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001DAA RID: 7594
		public extern string tag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001DAB RID: 7595
		public extern int spriteCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001DAC RID: 7596
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool CanBindTo([NotNull("ArgumentNullException")] Sprite sprite);

		// Token: 0x06001DAD RID: 7597
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Sprite GetSprite(string name);

		// Token: 0x06001DAE RID: 7598 RVA: 0x00030EC0 File Offset: 0x0002F0C0
		public int GetSprites(Sprite[] sprites)
		{
			return this.GetSpritesScripting(sprites);
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x00030EDC File Offset: 0x0002F0DC
		public int GetSprites(Sprite[] sprites, string name)
		{
			return this.GetSpritesWithNameScripting(sprites, name);
		}

		// Token: 0x06001DB0 RID: 7600
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetSpritesScripting([Unmarshalled] Sprite[] sprites);

		// Token: 0x06001DB1 RID: 7601
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetSpritesWithNameScripting([Unmarshalled] Sprite[] sprites, string name);
	}
}
