using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001741 RID: 5953
	public static class WorldBuilderStatic
	{
		// Token: 0x04008A5F RID: 35423
		public static readonly Dictionary<string, DynamicProperties> Properties = new Dictionary<string, DynamicProperties>();

		// Token: 0x04008A60 RID: 35424
		public static readonly Dictionary<string, Vector2i> WorldSizeMapper = new Dictionary<string, Vector2i>();

		// Token: 0x04008A61 RID: 35425
		public static readonly Dictionary<int, TownshipData> idToTownshipData = new Dictionary<int, TownshipData>();
	}
}
