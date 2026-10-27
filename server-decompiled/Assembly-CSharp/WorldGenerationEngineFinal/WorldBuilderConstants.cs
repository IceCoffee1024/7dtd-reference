using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200179F RID: 6047
	public static class WorldBuilderConstants
	{
		// Token: 0x04008C10 RID: 35856
		public static readonly Color32 forestCol = new Color32(0, 64, 0, byte.MaxValue);

		// Token: 0x04008C11 RID: 35857
		public static readonly Color32 burntForestCol = new Color32(186, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008C12 RID: 35858
		public static readonly Color32 desertCol = new Color32(byte.MaxValue, 228, 119, byte.MaxValue);

		// Token: 0x04008C13 RID: 35859
		public static readonly Color32 snowCol = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008C14 RID: 35860
		public static readonly Color32 wastelandCol = new Color32(byte.MaxValue, 168, 0, byte.MaxValue);

		// Token: 0x04008C15 RID: 35861
		public static readonly Color32 waterCol = new Color32(0, 0, 100, byte.MaxValue);

		// Token: 0x04008C16 RID: 35862
		public static readonly List<Color32> biomeColorList = new List<Color32>
		{
			WorldBuilderConstants.forestCol,
			WorldBuilderConstants.burntForestCol,
			WorldBuilderConstants.desertCol,
			WorldBuilderConstants.snowCol,
			WorldBuilderConstants.wastelandCol
		};

		// Token: 0x04008C17 RID: 35863
		public const int ForestBiomeWeightDefault = 13;

		// Token: 0x04008C18 RID: 35864
		public const int BurntForestBiomeWeightDefault = 18;

		// Token: 0x04008C19 RID: 35865
		public const int DesertBiomeWeightDefault = 22;

		// Token: 0x04008C1A RID: 35866
		public const int SnowBiomeWeightDefault = 23;

		// Token: 0x04008C1B RID: 35867
		public const int WastelandBiomeWeightDefault = 24;

		// Token: 0x04008C1C RID: 35868
		public static readonly int[] BiomeWeightDefaults = new int[] { 13, 18, 22, 23, 24 };

		// Token: 0x04008C1D RID: 35869
		public const int PlainsWeightDefault = 4;

		// Token: 0x04008C1E RID: 35870
		public const int HillsWeightDefault = 4;

		// Token: 0x04008C1F RID: 35871
		public const int MountainsWeightDefault = 2;
	}
}
