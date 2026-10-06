using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200175A RID: 5978
	public class StreetTileShared
	{
		// Token: 0x0600B9E6 RID: 47590 RVA: 0x0044C458 File Offset: 0x0044A658
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

		// Token: 0x04008AA3 RID: 35491
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008AA4 RID: 35492
		public readonly FastTags<TagGroup.Poi> traderTag = FastTags<TagGroup.Poi>.Parse("trader");

		// Token: 0x04008AA5 RID: 35493
		public readonly FastTags<TagGroup.Poi> wildernessTag = FastTags<TagGroup.Poi>.Parse("wilderness");

		// Token: 0x04008AA6 RID: 35494
		public readonly string[] RoadShapes = new string[] { "rwg_tile_straight", "rwg_tile_t", "rwg_tile_intersection", "rwg_tile_cap", "rwg_tile_corner" };

		// Token: 0x04008AA7 RID: 35495
		public readonly string[] RoadShapesDistrict = new string[] { "rwg_tile_{0}straight", "rwg_tile_{0}t", "rwg_tile_{0}intersection", "rwg_tile_{0}cap", "rwg_tile_{0}corner" };

		// Token: 0x04008AA8 RID: 35496
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] RoadShapeExits = new int[] { 5, 7, 15, 4, 6 };

		// Token: 0x04008AA9 RID: 35497
		public readonly List<int> RoadShapeExitCounts = new List<int> { 2, 3, 4, 1, 2 };

		// Token: 0x04008AAA RID: 35498
		public readonly List<int[]> RoadShapeExitsPerRotation = new List<int[]>();

		// Token: 0x04008AAB RID: 35499
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};

		// Token: 0x04008AAC RID: 35500
		public readonly Vector2i[] dirDiagonal = new Vector2i[]
		{
			new Vector2i(1, 1),
			new Vector2i(1, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 1)
		};

		// Token: 0x04008AAD RID: 35501
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

		// Token: 0x04008AAE RID: 35502
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
