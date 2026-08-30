using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016F3 RID: 5875
	public static class PrefabManagerStatic
	{
		// Token: 0x0400886E RID: 34926
		public static readonly Dictionary<string, Vector2i> TileMinMaxCounts = new Dictionary<string, Vector2i>();

		// Token: 0x0400886F RID: 34927
		public static readonly Dictionary<string, float> TileMaxDensityScore = new Dictionary<string, float>();

		// Token: 0x04008870 RID: 34928
		public static readonly List<PrefabManager.POIWeightData> prefabWeightData = new List<PrefabManager.POIWeightData>();
	}
}
