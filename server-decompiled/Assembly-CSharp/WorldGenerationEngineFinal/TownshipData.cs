using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001798 RID: 6040
	public class TownshipData
	{
		// Token: 0x0600BB88 RID: 48008 RVA: 0x00459378 File Offset: 0x00457578
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

		// Token: 0x04008C43 RID: 35907
		public string Name;

		// Token: 0x04008C44 RID: 35908
		public int Id;

		// Token: 0x04008C45 RID: 35909
		public List<string> SpawnableTerrain = new List<string>();

		// Token: 0x04008C46 RID: 35910
		public bool SpawnCustomSizes;

		// Token: 0x04008C47 RID: 35911
		public bool SpawnTrader = true;

		// Token: 0x04008C48 RID: 35912
		public bool SpawnGateway = true;

		// Token: 0x04008C49 RID: 35913
		public string OutskirtDistrict;

		// Token: 0x04008C4A RID: 35914
		public float OutskirtDistrictPercent;

		// Token: 0x04008C4B RID: 35915
		public FastTags<TagGroup.Poi> Biomes;

		// Token: 0x04008C4C RID: 35916
		public readonly TownshipData.eCategory Category;

		// Token: 0x02001799 RID: 6041
		public enum eCategory
		{
			// Token: 0x04008C4E RID: 35918
			Normal,
			// Token: 0x04008C4F RID: 35919
			Roadside,
			// Token: 0x04008C50 RID: 35920
			Rural,
			// Token: 0x04008C51 RID: 35921
			Wilderness
		}
	}
}
