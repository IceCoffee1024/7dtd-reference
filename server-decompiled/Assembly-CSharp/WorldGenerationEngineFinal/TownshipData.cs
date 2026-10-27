using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020017C9 RID: 6089
	public class TownshipData
	{
		// Token: 0x0600BC9C RID: 48284 RVA: 0x0045CD24 File Offset: 0x0045AF24
		public TownshipData(string _name, int _id)
		{
			this.Name = _name;
			this.Id = _id;
			if (_name.EndsWith("roadside"))
			{
				this.Category = TownshipData.eCategory.Roadside;
			}
			else if (_name.EndsWith("rural"))
			{
				this.Category = TownshipData.eCategory.Rural;
			}
			else if (_name.EndsWith("wilderness"))
			{
				this.Category = TownshipData.eCategory.Wilderness;
			}
			WorldBuilderStatic.idToTownshipData[this.Id] = this;
		}

		// Token: 0x04008D2C RID: 36140
		public string Name;

		// Token: 0x04008D2D RID: 36141
		public int Id;

		// Token: 0x04008D2E RID: 36142
		public List<string> SpawnableTerrain = new List<string>();

		// Token: 0x04008D2F RID: 36143
		public bool SpawnCustomSizes;

		// Token: 0x04008D30 RID: 36144
		public bool SpawnTrader = true;

		// Token: 0x04008D31 RID: 36145
		public bool SpawnGateway = true;

		// Token: 0x04008D32 RID: 36146
		public string OutskirtDistrict;

		// Token: 0x04008D33 RID: 36147
		public float OutskirtDistrictPercent;

		// Token: 0x04008D34 RID: 36148
		public FastTags<TagGroup.Poi> Biomes;

		// Token: 0x04008D35 RID: 36149
		public readonly TownshipData.eCategory Category;

		// Token: 0x020017CA RID: 6090
		public enum eCategory
		{
			// Token: 0x04008D37 RID: 36151
			Normal,
			// Token: 0x04008D38 RID: 36152
			Roadside,
			// Token: 0x04008D39 RID: 36153
			Rural,
			// Token: 0x04008D3A RID: 36154
			Wilderness
		}
	}
}
