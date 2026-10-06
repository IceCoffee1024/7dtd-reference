using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200172E RID: 5934
	public class PathShared
	{
		// Token: 0x0600B8DF RID: 47327 RVA: 0x0044656C File Offset: 0x0044476C
		public PathShared(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
			this.IdToColor = new Color32[]
			{
				default(Color32),
				this.CountryColor,
				this.HighwayColor,
				this.CountryColor,
				this.WaterColor
			};
		}

		// Token: 0x0600B8E0 RID: 47328 RVA: 0x0044660C File Offset: 0x0044480C
		public void ConvertIdsToColors(byte[] ids, Color32[] dest)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				int num = (int)ids[i];
				dest[i] = this.IdToColor[num & 15];
			}
		}

		// Token: 0x040089F9 RID: 35321
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x040089FA RID: 35322
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 CountryColor = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x040089FB RID: 35323
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 HighwayColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		// Token: 0x040089FC RID: 35324
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 WaterColor = new Color32(0, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x040089FD RID: 35325
		public readonly Color32[] IdToColor;
	}
}
