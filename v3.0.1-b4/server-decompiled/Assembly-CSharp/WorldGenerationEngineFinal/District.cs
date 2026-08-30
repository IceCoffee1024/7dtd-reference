using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016CF RID: 5839
	public class District
	{
		// Token: 0x0600B5D9 RID: 46553 RVA: 0x0043A1C7 File Offset: 0x004383C7
		public District()
		{
		}

		// Token: 0x0600B5DA RID: 46554 RVA: 0x0043A1E8 File Offset: 0x004383E8
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

		// Token: 0x0600B5DB RID: 46555 RVA: 0x0043A26C File Offset: 0x0043846C
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

		// Token: 0x040087F1 RID: 34801
		public string name;

		// Token: 0x040087F2 RID: 34802
		public string prefabName;

		// Token: 0x040087F3 RID: 34803
		public District.Type type;

		// Token: 0x040087F4 RID: 34804
		public FastTags<TagGroup.Poi> tag;

		// Token: 0x040087F5 RID: 34805
		public FastTags<TagGroup.Poi> townships;

		// Token: 0x040087F6 RID: 34806
		public float weight = 0.5f;

		// Token: 0x040087F7 RID: 34807
		public float spawnOrder;

		// Token: 0x040087F8 RID: 34808
		public Color preview_color;

		// Token: 0x040087F9 RID: 34809
		public bool spawnCustomSizePrefabs;

		// Token: 0x040087FA RID: 34810
		public List<string> avoidedNeighborDistricts = new List<string>();

		// Token: 0x020016D0 RID: 5840
		public enum Type
		{
			// Token: 0x040087FC RID: 34812
			None,
			// Token: 0x040087FD RID: 34813
			Commercial,
			// Token: 0x040087FE RID: 34814
			Downtown,
			// Token: 0x040087FF RID: 34815
			Gateway,
			// Token: 0x04008800 RID: 34816
			Industrial,
			// Token: 0x04008801 RID: 34817
			Residential,
			// Token: 0x04008802 RID: 34818
			Rural
		}
	}
}
