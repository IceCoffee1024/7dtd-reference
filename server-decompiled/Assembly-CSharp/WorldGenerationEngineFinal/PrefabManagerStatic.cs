using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001776 RID: 6006
	public static class PrefabManagerStatic
	{
		// Token: 0x04008B38 RID: 35640
		public static readonly Dictionary<string, Vector2i> TileMinMaxCounts = new Dictionary<string, Vector2i>();

		// Token: 0x04008B39 RID: 35641
		public static readonly Dictionary<string, float> TileMaxDensityScore = new Dictionary<string, float>();

		// Token: 0x04008B3A RID: 35642
		public static readonly List<PrefabManager.POIWeightData> prefabWeightData = new List<PrefabManager.POIWeightData>();
	}
}
