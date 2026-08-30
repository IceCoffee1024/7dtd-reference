using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001746 RID: 5958
	public class TownshipData
	{
		// Token: 0x0600B8AE RID: 47278 RVA: 0x0044F0E0 File Offset: 0x0044D2E0
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

		// Token: 0x04008A62 RID: 35426
		public string Name;

		// Token: 0x04008A63 RID: 35427
		public int Id;

		// Token: 0x04008A64 RID: 35428
		public List<string> SpawnableTerrain = new List<string>();

		// Token: 0x04008A65 RID: 35429
		public bool SpawnCustomSizes;

		// Token: 0x04008A66 RID: 35430
		public bool SpawnTrader = true;

		// Token: 0x04008A67 RID: 35431
		public bool SpawnGateway = true;

		// Token: 0x04008A68 RID: 35432
		public string OutskirtDistrict;

		// Token: 0x04008A69 RID: 35433
		public float OutskirtDistrictPercent;

		// Token: 0x04008A6A RID: 35434
		public FastTags<TagGroup.Poi> Biomes;

		// Token: 0x04008A6B RID: 35435
		public readonly TownshipData.eCategory Category;

		// Token: 0x02001747 RID: 5959
		public enum eCategory
		{
			// Token: 0x04008A6D RID: 35437
			Normal,
			// Token: 0x04008A6E RID: 35438
			Roadside,
			// Token: 0x04008A6F RID: 35439
			Rural,
			// Token: 0x04008A70 RID: 35440
			Wilderness
		}
	}
}
