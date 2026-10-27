using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200178B RID: 6027
	public class StreetTileShared
	{
		// Token: 0x0600BAFA RID: 47866 RVA: 0x0044FE04 File Offset: 0x0044E004
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

		// Token: 0x04008B8C RID: 35724
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B8D RID: 35725
		public readonly FastTags<TagGroup.Poi> traderTag = FastTags<TagGroup.Poi>.Parse("trader");

		// Token: 0x04008B8E RID: 35726
		public readonly FastTags<TagGroup.Poi> wildernessTag = FastTags<TagGroup.Poi>.Parse("wilderness");

		// Token: 0x04008B8F RID: 35727
		public readonly string[] RoadShapes = new string[] { "rwg_tile_straight", "rwg_tile_t", "rwg_tile_intersection", "rwg_tile_cap", "rwg_tile_corner" };

		// Token: 0x04008B90 RID: 35728
		public readonly string[] RoadShapesDistrict = new string[] { "rwg_tile_{0}straight", "rwg_tile_{0}t", "rwg_tile_{0}intersection", "rwg_tile_{0}cap", "rwg_tile_{0}corner" };

		// Token: 0x04008B91 RID: 35729
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] RoadShapeExits = new int[] { 5, 7, 15, 4, 6 };

		// Token: 0x04008B92 RID: 35730
		public readonly List<int> RoadShapeExitCounts = new List<int> { 2, 3, 4, 1, 2 };

		// Token: 0x04008B93 RID: 35731
		public readonly List<int[]> RoadShapeExitsPerRotation = new List<int[]>();

		// Token: 0x04008B94 RID: 35732
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};

		// Token: 0x04008B95 RID: 35733
		public readonly Vector2i[] dirDiagonal = new Vector2i[]
		{
			new Vector2i(1, 1),
			new Vector2i(1, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 1)
		};

		// Token: 0x04008B96 RID: 35734
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

		// Token: 0x04008B97 RID: 35735
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
