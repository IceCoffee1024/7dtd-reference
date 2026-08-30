using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001702 RID: 5890
	public class PathShared
	{
		// Token: 0x0600B788 RID: 46984 RVA: 0x0044204C File Offset: 0x0044024C
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

		// Token: 0x0600B789 RID: 46985 RVA: 0x004420EC File Offset: 0x004402EC
		public void ConvertIdsToColors(byte[] ids, Color32[] dest)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				int num = (int)ids[i];
				dest[i] = this.IdToColor[num & 15];
			}
		}

		// Token: 0x04008934 RID: 35124
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008935 RID: 35125
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 CountryColor = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x04008936 RID: 35126
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 HighwayColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		// Token: 0x04008937 RID: 35127
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 WaterColor = new Color32(0, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008938 RID: 35128
		public readonly Color32[] IdToColor;
	}
}
