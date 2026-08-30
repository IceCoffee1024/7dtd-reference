using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016DC RID: 5852
	public class PathShared
	{
		// Token: 0x0600B605 RID: 46597 RVA: 0x0043C2D4 File Offset: 0x0043A4D4
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

		// Token: 0x0600B606 RID: 46598 RVA: 0x0043C374 File Offset: 0x0043A574
		public void ConvertIdsToColors(byte[] ids, Color32[] dest)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				int num = (int)ids[i];
				dest[i] = this.IdToColor[num & 15];
			}
		}

		// Token: 0x04008818 RID: 34840
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008819 RID: 34841
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 CountryColor = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x0400881A RID: 34842
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 HighwayColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		// Token: 0x0400881B RID: 34843
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Color32 WaterColor = new Color32(0, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400881C RID: 34844
		public readonly Color32[] IdToColor;
	}
}
