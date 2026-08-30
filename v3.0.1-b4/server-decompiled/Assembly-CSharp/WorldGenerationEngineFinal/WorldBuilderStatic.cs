using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200171B RID: 5915
	public static class WorldBuilderStatic
	{
		// Token: 0x04008943 RID: 35139
		public static readonly Dictionary<string, DynamicProperties> Properties = new Dictionary<string, DynamicProperties>();

		// Token: 0x04008944 RID: 35140
		public static readonly Dictionary<string, Vector2i> WorldSizeMapper = new Dictionary<string, Vector2i>();

		// Token: 0x04008945 RID: 35141
		public static readonly Dictionary<int, TownshipData> idToTownshipData = new Dictionary<int, TownshipData>();
	}
}
