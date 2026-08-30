using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200173A RID: 5946
	public class TownshipShared
	{
		// Token: 0x0600B8FC RID: 47356 RVA: 0x0044CA90 File Offset: 0x0044AC90
		public TownshipShared(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x04008A3B RID: 35387
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008A3C RID: 35388
		public int NextId;

		// Token: 0x04008A3D RID: 35389
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};
	}
}
