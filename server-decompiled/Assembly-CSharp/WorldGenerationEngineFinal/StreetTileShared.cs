using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200172E RID: 5934
	public class StreetTileShared
	{
		// Token: 0x0600B88F RID: 47247 RVA: 0x00447F38 File Offset: 0x00446138
		public StreetTileShared(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
			for (int i = 0; i < this.RoadShapeExits.Length; i++)
			{
				int num = this.RoadShapeExits[i];
				int[] array = new int[4];
				for (int j = 0; j < 4; j++)
				{
					array[j] = num;
					num = ((num << 1) & 15) | (num >> 3);
				}
				this.RoadShapeExitsPerRotation.Add(array);
			}
		}

		// Token: 0x040089DE RID: 35294
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x040089DF RID: 35295
		public readonly FastTags<TagGroup.Poi> traderTag = FastTags<TagGroup.Poi>.Parse("trader");

		// Token: 0x040089E0 RID: 35296
		public readonly FastTags<TagGroup.Poi> wildernessTag = FastTags<TagGroup.Poi>.Parse("wilderness");

		// Token: 0x040089E1 RID: 35297
		public readonly string[] RoadShapes = new string[] { "rwg_tile_straight", "rwg_tile_t", "rwg_tile_intersection", "rwg_tile_cap", "rwg_tile_corner" };

		// Token: 0x040089E2 RID: 35298
		public readonly string[] RoadShapesDistrict = new string[] { "rwg_tile_{0}straight", "rwg_tile_{0}t", "rwg_tile_{0}intersection", "rwg_tile_{0}cap", "rwg_tile_{0}corner" };

		// Token: 0x040089E3 RID: 35299
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] RoadShapeExits = new int[] { 5, 7, 15, 4, 6 };

		// Token: 0x040089E4 RID: 35300
		public readonly List<int> RoadShapeExitCounts = new List<int> { 2, 3, 4, 1, 2 };

		// Token: 0x040089E5 RID: 35301
		public readonly List<int[]> RoadShapeExitsPerRotation = new List<int[]>();

		// Token: 0x040089E6 RID: 35302
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};

		// Token: 0x040089E7 RID: 35303
		public readonly Vector2i[] dirDiagonal = new Vector2i[]
		{
			new Vector2i(1, 1),
			new Vector2i(1, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 1)
		};

		// Token: 0x040089E8 RID: 35304
		public readonly Vector2i[] dir8way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 1),
			new Vector2i(1, 0),
			new Vector2i(1, -1),
			new Vector2i(0, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 0),
			new Vector2i(-1, 1)
		};

		// Token: 0x040089E9 RID: 35305
		public readonly Vector2i[] dir9way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 1),
			new Vector2i(1, 0),
			new Vector2i(1, -1),
			new Vector2i(0, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 0),
			new Vector2i(-1, 1)
		};
	}
}
