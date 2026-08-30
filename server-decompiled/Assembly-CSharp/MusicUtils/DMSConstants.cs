using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace MusicUtils
{
	// Token: 0x02001A0B RID: 6667
	public static class DMSConstants
	{
		// Token: 0x04009A23 RID: 39459
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

		// Token: 0x04009A24 RID: 39460
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

		// Token: 0x04009A25 RID: 39461
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

		// Token: 0x04009A26 RID: 39462
		public const int cChannels = 2;

		// Token: 0x04009A27 RID: 39463
		public const int cFrequency = 44100;

		// Token: 0x04009A28 RID: 39464
		public static List<SectionType> LayeredSections = new List<SectionType>
		{
			SectionType.Exploration,
			SectionType.Suspense,
			SectionType.Combat,
			SectionType.Bloodmoon
		};
	}
}
