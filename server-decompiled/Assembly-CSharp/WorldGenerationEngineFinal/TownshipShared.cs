using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001797 RID: 6039
	public class TownshipShared
	{
		// Token: 0x0600BB67 RID: 47975 RVA: 0x0045495C File Offset: 0x00452B5C
		public TownshipShared(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x04008BE9 RID: 35817
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008BEA RID: 35818
		public int NextId;

		// Token: 0x04008BEB RID: 35819
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};
	}
}
