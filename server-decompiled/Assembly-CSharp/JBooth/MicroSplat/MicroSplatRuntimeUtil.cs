using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D6C RID: 7532
	public class MicroSplatRuntimeUtil
	{
		// Token: 0x0600DF1C RID: 57116 RVA: 0x004F926C File Offset: 0x004F746C
		public static Vector2 UnityUVScaleToUVScale(Vector2 uv, Terrain t)
		{
			float x = t.terrainData.size.x;
			float z = t.terrainData.size.z;
			uv.x = 1f / (uv.x / x);
			uv.y = 1f / (uv.y / z);
			return uv;
		}

		// Token: 0x0600DF1D RID: 57117 RVA: 0x004F92C8 File Offset: 0x004F74C8
		public static Vector2 UVScaleToUnityUVScale(Vector2 uv, Terrain t)
		{
			float x = t.terrainData.size.x;
			float z = t.terrainData.size.z;
			if (uv.x < 0f)
			{
				uv.x = 0.001f;
			}
			if (uv.y < 0f)
			{
				uv.y = 0.001f;
			}
			uv.x = x / uv.x;
			uv.y = z / uv.y;
			return uv;
		}
	}
}
