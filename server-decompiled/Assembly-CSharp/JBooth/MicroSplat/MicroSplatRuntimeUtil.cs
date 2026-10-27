using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D9D RID: 7581
	public class MicroSplatRuntimeUtil
	{
		// Token: 0x0600E030 RID: 57392 RVA: 0x004FCE0C File Offset: 0x004FB00C
		public static Vector2 UnityUVScaleToUVScale(Vector2 uv, Terrain t)
		{
			float x = t.terrainData.size.x;
			float z = t.terrainData.size.z;
			uv.x = 1f / (uv.x / x);
			uv.y = 1f / (uv.y / z);
			return uv;
		}

		// Token: 0x0600E031 RID: 57393 RVA: 0x004FCE68 File Offset: 0x004FB068
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
