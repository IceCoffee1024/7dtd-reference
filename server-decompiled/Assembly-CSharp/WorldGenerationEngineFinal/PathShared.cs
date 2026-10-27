using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200175F RID: 5983
	public class PathShared
	{
		// Token: 0x0600B9F3 RID: 47603 RVA: 0x00449F18 File Offset: 0x00448118
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

		// Token: 0x0600B9F4 RID: 47604 RVA: 0x00449FB8 File Offset: 0x004481B8
		public void ConvertIdsToColors(byte[] ids, Color32[] dest)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				int num = (int)ids[i];
				dest[i] = this.IdToColor[num & 15];
			}
		}

		// Token: 0x04008AE2 RID: 35554
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008AE3 RID: 35555
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 CountryColor = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x04008AE4 RID: 35556
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 HighwayColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		// Token: 0x04008AE5 RID: 35557
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 WaterColor = new Color32(0, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008AE6 RID: 35558
		public readonly Color32[] IdToColor;
	}
}
