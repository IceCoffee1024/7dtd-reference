using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001766 RID: 5990
	public class TownshipShared
	{
		// Token: 0x0600BA53 RID: 47699 RVA: 0x00450FB0 File Offset: 0x0044F1B0
		public TownshipShared(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x04008B00 RID: 35584
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B01 RID: 35585
		public int NextId;

		// Token: 0x04008B02 RID: 35586
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};
	}
}
