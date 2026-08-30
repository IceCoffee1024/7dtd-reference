using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AA2 RID: 6818
	public class ConfigSet : EnumDictionary<ThreatLevelLegacyType, ThreatLevelConfig>
	{
		// Token: 0x0600CDC6 RID: 52678 RVA: 0x004ACE6B File Offset: 0x004AB06B
		public static void Cleanup()
		{
			if (ConfigSet.AllConfigSets != null)
			{
				ConfigSet.AllConfigSets.Clear();
			}
		}

		// Token: 0x04009BF4 RID: 39924
		public static Dictionary<int, ConfigSet> AllConfigSets = new Dictionary<int, ConfigSet>();
	}
}
