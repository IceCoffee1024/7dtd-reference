using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200179E RID: 6046
	public static class WorldBuilderStatic
	{
		// Token: 0x04008C0D RID: 35853
		public static readonly Dictionary<string, DynamicProperties> Properties = new Dictionary<string, DynamicProperties>();

		// Token: 0x04008C0E RID: 35854
		public static readonly Dictionary<string, Vector2i> WorldSizeMapper = new Dictionary<string, Vector2i>();

		// Token: 0x04008C0F RID: 35855
		public static readonly Dictionary<int, TownshipData> idToTownshipData = new Dictionary<int, TownshipData>();
	}
}
