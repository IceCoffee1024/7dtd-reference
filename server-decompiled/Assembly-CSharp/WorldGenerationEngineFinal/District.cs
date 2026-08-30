using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016F5 RID: 5877
	public class District
	{
		// Token: 0x0600B75C RID: 46940 RVA: 0x0043FF3F File Offset: 0x0043E13F
		public District()
		{
		}

		// Token: 0x0600B75D RID: 46941 RVA: 0x0043FF60 File Offset: 0x0043E160
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

		// Token: 0x0600B75E RID: 46942 RVA: 0x0043FFE4 File Offset: 0x0043E1E4
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

		// Token: 0x0400890D RID: 35085
		public string name;

		// Token: 0x0400890E RID: 35086
		public string prefabName;

		// Token: 0x0400890F RID: 35087
		public District.Type type;

		// Token: 0x04008910 RID: 35088
		public FastTags<TagGroup.Poi> tag;

		// Token: 0x04008911 RID: 35089
		public FastTags<TagGroup.Poi> townships;

		// Token: 0x04008912 RID: 35090
		public float weight = 0.5f;

		// Token: 0x04008913 RID: 35091
		public float spawnOrder;

		// Token: 0x04008914 RID: 35092
		public Color preview_color;

		// Token: 0x04008915 RID: 35093
		public bool spawnCustomSizePrefabs;

		// Token: 0x04008916 RID: 35094
		public List<string> avoidedNeighborDistricts = new List<string>();

		// Token: 0x020016F6 RID: 5878
		public enum Type
		{
			// Token: 0x04008918 RID: 35096
			None,
			// Token: 0x04008919 RID: 35097
			Commercial,
			// Token: 0x0400891A RID: 35098
			Downtown,
			// Token: 0x0400891B RID: 35099
			Gateway,
			// Token: 0x0400891C RID: 35100
			Industrial,
			// Token: 0x0400891D RID: 35101
			Residential,
			// Token: 0x0400891E RID: 35102
			Rural
		}
	}
}
