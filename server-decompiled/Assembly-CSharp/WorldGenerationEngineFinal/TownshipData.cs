using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200176C RID: 5996
	public class TownshipData
	{
		// Token: 0x0600BA31 RID: 47665 RVA: 0x00454E58 File Offset: 0x00453058
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

		// Token: 0x04008B7E RID: 35710
		public string Name;

		// Token: 0x04008B7F RID: 35711
		public int Id;

		// Token: 0x04008B80 RID: 35712
		public List<string> SpawnableTerrain = new List<string>();

		// Token: 0x04008B81 RID: 35713
		public bool SpawnCustomSizes;

		// Token: 0x04008B82 RID: 35714
		public bool SpawnTrader = true;

		// Token: 0x04008B83 RID: 35715
		public bool SpawnGateway = true;

		// Token: 0x04008B84 RID: 35716
		public string OutskirtDistrict;

		// Token: 0x04008B85 RID: 35717
		public float OutskirtDistrictPercent;

		// Token: 0x04008B86 RID: 35718
		public FastTags<TagGroup.Poi> Biomes;

		// Token: 0x04008B87 RID: 35719
		public readonly TownshipData.eCategory Category;

		// Token: 0x0200176D RID: 5997
		public enum eCategory
		{
			// Token: 0x04008B89 RID: 35721
			Normal,
			// Token: 0x04008B8A RID: 35722
			Roadside,
			// Token: 0x04008B8B RID: 35723
			Rural,
			// Token: 0x04008B8C RID: 35724
			Wilderness
		}
	}
}
