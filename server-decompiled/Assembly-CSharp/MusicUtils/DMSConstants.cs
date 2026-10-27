using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace MusicUtils
{
	// Token: 0x02001A68 RID: 6760
	public static class DMSConstants
	{
		// Token: 0x04009BD2 RID: 39890
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

		// Token: 0x04009BD3 RID: 39891
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

		// Token: 0x04009BD4 RID: 39892
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

		// Token: 0x04009BD5 RID: 39893
		public const int cChannels = 2;

		// Token: 0x04009BD6 RID: 39894
		public const int cFrequency = 44100;

		// Token: 0x04009BD7 RID: 39895
		public static List<SectionType> LayeredSections = new List<SectionType>
		{
			SectionType.Exploration,
			SectionType.Suspense,
			SectionType.Combat,
			SectionType.Bloodmoon
		};
	}
}
