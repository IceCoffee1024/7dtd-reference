using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200176D RID: 5997
	public static class WorldBuilderStatic
	{
		// Token: 0x04008B24 RID: 35620
		public static readonly Dictionary<string, DynamicProperties> Properties = new Dictionary<string, DynamicProperties>();

		// Token: 0x04008B25 RID: 35621
		public static readonly Dictionary<string, Vector2i> WorldSizeMapper = new Dictionary<string, Vector2i>();

		// Token: 0x04008B26 RID: 35622
		public static readonly Dictionary<int, TownshipData> idToTownshipData = new Dictionary<int, TownshipData>();
	}
}
