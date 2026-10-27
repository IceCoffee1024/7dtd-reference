using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001752 RID: 5970
	public class District
	{
		// Token: 0x0600B9C7 RID: 47559 RVA: 0x00447E0B File Offset: 0x0044600B
		public District()
		{
		}

		// Token: 0x0600B9C8 RID: 47560 RVA: 0x00447E2C File Offset: 0x0044602C
		public District(District _other)
		{
			this.name = _other.name;
			this.prefabName = _other.prefabName;
			this.tag = _other.tag;
			this.townships = _other.townships;
			this.weight = _other.weight;
			this.preview_color = _other.preview_color;
			this.avoidedNeighborDistricts = _other.avoidedNeighborDistricts;
			this.Init();
		}

		// Token: 0x0600B9C9 RID: 47561 RVA: 0x00447EB0 File Offset: 0x004460B0
		public void Init()
		{
			this.type = District.Type.None;
			if (this.name.EndsWith("commercial"))
			{
				this.type = District.Type.Commercial;
				this.spawnOrder = 1f;
				return;
			}
			if (this.name.EndsWith("downtown"))
			{
				this.type = District.Type.Downtown;
				this.spawnOrder = 99f;
				return;
			}
			if (this.name.EndsWith("gateway"))
			{
				this.type = District.Type.Gateway;
				return;
			}
			if (this.name.EndsWith("industrial"))
			{
				this.type = District.Type.Industrial;
				return;
			}
			if (this.name.EndsWith("residential"))
			{
				this.type = District.Type.Residential;
				this.spawnOrder = -1f;
				return;
			}
			if (this.name.EndsWith("rural"))
			{
				this.type = District.Type.Rural;
			}
		}

		// Token: 0x04008ABB RID: 35515
		public string name;

		// Token: 0x04008ABC RID: 35516
		public string prefabName;

		// Token: 0x04008ABD RID: 35517
		public District.Type type;

		// Token: 0x04008ABE RID: 35518
		public FastTags<TagGroup.Poi> tag;

		// Token: 0x04008ABF RID: 35519
		public FastTags<TagGroup.Poi> townships;

		// Token: 0x04008AC0 RID: 35520
		public float weight = 0.5f;

		// Token: 0x04008AC1 RID: 35521
		public float spawnOrder;

		// Token: 0x04008AC2 RID: 35522
		public Color preview_color;

		// Token: 0x04008AC3 RID: 35523
		public bool spawnCustomSizePrefabs;

		// Token: 0x04008AC4 RID: 35524
		public List<string> avoidedNeighborDistricts = new List<string>();

		// Token: 0x02001753 RID: 5971
		public enum Type
		{
			// Token: 0x04008AC6 RID: 35526
			None,
			// Token: 0x04008AC7 RID: 35527
			Commercial,
			// Token: 0x04008AC8 RID: 35528
			Downtown,
			// Token: 0x04008AC9 RID: 35529
			Gateway,
			// Token: 0x04008ACA RID: 35530
			Industrial,
			// Token: 0x04008ACB RID: 35531
			Residential,
			// Token: 0x04008ACC RID: 35532
			Rural
		}
	}
}
