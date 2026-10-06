using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001745 RID: 5957
	public static class PrefabManagerStatic
	{
		// Token: 0x04008A4F RID: 35407
		public static readonly Dictionary<string, Vector2i> TileMinMaxCounts = new Dictionary<string, Vector2i>();

		// Token: 0x04008A50 RID: 35408
		public static readonly Dictionary<string, float> TileMaxDensityScore = new Dictionary<string, float>();

		// Token: 0x04008A51 RID: 35409
		public static readonly List<PrefabManager.POIWeightData> prefabWeightData = new List<PrefabManager.POIWeightData>();
	}
}
