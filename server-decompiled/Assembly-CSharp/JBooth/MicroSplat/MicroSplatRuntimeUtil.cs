using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D3A RID: 7482
	public class MicroSplatRuntimeUtil
	{
		// Token: 0x0600DDA1 RID: 56737 RVA: 0x004F4624 File Offset: 0x004F2824
		public static Vector2 UnityUVScaleToUVScale(Vector2 uv, Terrain t)
		{
			float x = t.terrainData.size.x;
			float z = t.terrainData.size.z;
			uv.x = 1f / (uv.x / x);
			uv.y = 1f / (uv.y / z);
			return uv;
		}

		// Token: 0x0600DDA2 RID: 56738 RVA: 0x004F4680 File Offset: 0x004F2880
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
