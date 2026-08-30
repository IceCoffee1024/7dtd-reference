using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C6 RID: 1222
	public struct Cookie
	{
		// Token: 0x06002AC3 RID: 10947 RVA: 0x00047D40 File Offset: 0x00045F40
		public static Cookie Defaults()
		{
			Cookie cookie;
			cookie.instanceID = 0;
			cookie.scale = 1f;
			cookie.sizes = new Vector2(1f, 1f);
			return cookie;
		}

		// Token: 0x04001012 RID: 4114
		public int instanceID;

		// Token: 0x04001013 RID: 4115
		public float scale;

		// Token: 0x04001014 RID: 4116
		public Vector2 sizes;
	}
}
