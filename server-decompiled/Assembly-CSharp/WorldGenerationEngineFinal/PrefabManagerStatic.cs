using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001719 RID: 5913
	public static class PrefabManagerStatic
	{
		// Token: 0x0400898A RID: 35210
		public static readonly Dictionary<string, Vector2i> TileMinMaxCounts = new Dictionary<string, Vector2i>();

		// Token: 0x0400898B RID: 35211
		public static readonly Dictionary<string, float> TileMaxDensityScore = new Dictionary<string, float>();

		// Token: 0x0400898C RID: 35212
		public static readonly List<PrefabManager.POIWeightData> prefabWeightData = new List<PrefabManager.POIWeightData>();
	}
}
