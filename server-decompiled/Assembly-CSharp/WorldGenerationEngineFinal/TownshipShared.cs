using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001714 RID: 5908
	public class TownshipShared
	{
		// Token: 0x0600B779 RID: 46969 RVA: 0x00446D18 File Offset: 0x00444F18
		public TownshipShared(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0400891F RID: 35103
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008920 RID: 35104
		public int NextId;

		// Token: 0x04008921 RID: 35105
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};
	}
}
