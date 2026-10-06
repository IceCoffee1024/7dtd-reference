using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace MusicUtils
{
	// Token: 0x02001A37 RID: 6711
	public static class DMSConstants
	{
		// Token: 0x04009AE9 RID: 39657
		public static EnumDictionary<SectionType, string> SectionAbbrvs = new EnumDictionary<SectionType, string>
		{
			{
				SectionType.Exploration,
				"Exp"
			},
			{
				SectionType.Suspense,
				"Sus"
			},
			{
				SectionType.Combat,
				"Cbt"
			},
			{
				SectionType.Bloodmoon,
				"Bld"
			}
		};

		// Token: 0x04009AEA RID: 39658
		public static EnumDictionary<LayerType, string> LayerAbbrvs = new EnumDictionary<LayerType, string>
		{
			{
				LayerType.Primary,
				"Pri"
			},
			{
				LayerType.PrimaryPairable1,
				"Pp1"
			},
			{
				LayerType.PrimarySupporting,
				"Psp"
			},
			{
				LayerType.Secondary,
				"Sec"
			},
			{
				LayerType.LongEffects,
				"Lfx"
			},
			{
				LayerType.ShortEffects,
				"Sfx"
			}
		};

		// Token: 0x04009AEB RID: 39659
		public static EnumDictionary<PlacementType, string> PlacementAbbrv = new EnumDictionary<PlacementType, string>
		{
			{
				PlacementType.Begin,
				"a"
			},
			{
				PlacementType.Loop,
				"b"
			},
			{
				PlacementType.End,
				"c"
			}
		};

		// Token: 0x04009AEC RID: 39660
		public const int cChannels = 2;

		// Token: 0x04009AED RID: 39661
		public const int cFrequency = 44100;

		// Token: 0x04009AEE RID: 39662
		public static List<SectionType> LayeredSections = new List<SectionType>
		{
			SectionType.Exploration,
			SectionType.Suspense,
			SectionType.Combat,
			SectionType.Bloodmoon
		};
	}
}
