using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001708 RID: 5896
	public class StreetTileShared
	{
		// Token: 0x0600B70C RID: 46860 RVA: 0x004421C0 File Offset: 0x004403C0
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

		// Token: 0x040088C2 RID: 35010
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x040088C3 RID: 35011
		public readonly FastTags<TagGroup.Poi> traderTag = FastTags<TagGroup.Poi>.Parse("trader");

		// Token: 0x040088C4 RID: 35012
		public readonly FastTags<TagGroup.Poi> wildernessTag = FastTags<TagGroup.Poi>.Parse("wilderness");

		// Token: 0x040088C5 RID: 35013
		public readonly string[] RoadShapes = new string[] { "rwg_tile_straight", "rwg_tile_t", "rwg_tile_intersection", "rwg_tile_cap", "rwg_tile_corner" };

		// Token: 0x040088C6 RID: 35014
		public readonly string[] RoadShapesDistrict = new string[] { "rwg_tile_{0}straight", "rwg_tile_{0}t", "rwg_tile_{0}intersection", "rwg_tile_{0}cap", "rwg_tile_{0}corner" };

		// Token: 0x040088C7 RID: 35015
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] RoadShapeExits = new int[] { 5, 7, 15, 4, 6 };

		// Token: 0x040088C8 RID: 35016
		public readonly List<int> RoadShapeExitCounts = new List<int> { 2, 3, 4, 1, 2 };

		// Token: 0x040088C9 RID: 35017
		public readonly List<int[]> RoadShapeExitsPerRotation = new List<int[]>();

		// Token: 0x040088CA RID: 35018
		public readonly Vector2i[] dir4way = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 0),
			new Vector2i(0, -1),
			new Vector2i(-1, 0)
		};

		// Token: 0x040088CB RID: 35019
		public readonly Vector2i[] dirDiagonal = new Vector2i[]
		{
			new Vector2i(1, 1),
			new Vector2i(1, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 1)
		};

		// Token: 0x040088CC RID: 35020
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

		// Token: 0x040088CD RID: 35021
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
