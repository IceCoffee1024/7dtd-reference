using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D12 RID: 7442
	public class MicroSplatRuntimeUtil
	{
		// Token: 0x0600DC00 RID: 56320 RVA: 0x004EDBB0 File Offset: 0x004EBDB0
		public static Vector2 UnityUVScaleToUVScale(Vector2 uv, Terrain t)
		{
			float x = t.terrainData.size.x;
			float z = t.terrainData.size.z;
			uv.x = 1f / (uv.x / x);
			uv.y = 1f / (uv.y / z);
			return uv;
		}

		// Token: 0x0600DC01 RID: 56321 RVA: 0x004EDC0C File Offset: 0x004EBE0C
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
