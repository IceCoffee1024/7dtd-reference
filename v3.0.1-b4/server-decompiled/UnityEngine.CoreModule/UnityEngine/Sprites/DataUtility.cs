using System;

namespace UnityEngine.Sprites
{
	// Token: 0x020002AF RID: 687
	public sealed class DataUtility
	{
		// Token: 0x06001D61 RID: 7521 RVA: 0x0003074C File Offset: 0x0002E94C
		public static Vector4 GetInnerUV(Sprite sprite)
		{
			return sprite.GetInnerUVs();
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00030764 File Offset: 0x0002E964
		public static Vector4 GetOuterUV(Sprite sprite)
		{
			return sprite.GetOuterUVs();
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0003077C File Offset: 0x0002E97C
		public static Vector4 GetPadding(Sprite sprite)
		{
			return sprite.GetPadding();
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x00030794 File Offset: 0x0002E994
		public static Vector2 GetMinSize(Sprite sprite)
		{
			Vector2 vector;
			vector.x = sprite.border.x + sprite.border.z;
			vector.y = sprite.border.y + sprite.border.w;
			return vector;
		}
	}
}
