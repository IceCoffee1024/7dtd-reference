using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001721 RID: 5921
	public class District
	{
		// Token: 0x0600B8B3 RID: 47283 RVA: 0x0044445F File Offset: 0x0044265F
		public District()
		{
		}

		// Token: 0x0600B8B4 RID: 47284 RVA: 0x00444480 File Offset: 0x00442680
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

		// Token: 0x0600B8B5 RID: 47285 RVA: 0x00444504 File Offset: 0x00442704
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

		// Token: 0x040089D2 RID: 35282
		public string name;

		// Token: 0x040089D3 RID: 35283
		public string prefabName;

		// Token: 0x040089D4 RID: 35284
		public District.Type type;

		// Token: 0x040089D5 RID: 35285
		public FastTags<TagGroup.Poi> tag;

		// Token: 0x040089D6 RID: 35286
		public FastTags<TagGroup.Poi> townships;

		// Token: 0x040089D7 RID: 35287
		public float weight = 0.5f;

		// Token: 0x040089D8 RID: 35288
		public float spawnOrder;

		// Token: 0x040089D9 RID: 35289
		public Color preview_color;

		// Token: 0x040089DA RID: 35290
		public bool spawnCustomSizePrefabs;

		// Token: 0x040089DB RID: 35291
		public List<string> avoidedNeighborDistricts = new List<string>();

		// Token: 0x02001722 RID: 5922
		public enum Type
		{
			// Token: 0x040089DD RID: 35293
			None,
			// Token: 0x040089DE RID: 35294
			Commercial,
			// Token: 0x040089DF RID: 35295
			Downtown,
			// Token: 0x040089E0 RID: 35296
			Gateway,
			// Token: 0x040089E1 RID: 35297
			Industrial,
			// Token: 0x040089E2 RID: 35298
			Residential,
			// Token: 0x040089E3 RID: 35299
			Rural
		}
	}
}
