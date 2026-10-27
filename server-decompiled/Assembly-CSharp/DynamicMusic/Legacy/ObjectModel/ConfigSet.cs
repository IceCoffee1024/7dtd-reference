using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AFF RID: 6911
	public class ConfigSet : EnumDictionary<ThreatLevelLegacyType, ThreatLevelConfig>
	{
		// Token: 0x0600D032 RID: 53298 RVA: 0x004B4E33 File Offset: 0x004B3033
		public static void Cleanup()
		{
			if (ConfigSet.AllConfigSets != null)
			{
				ConfigSet.AllConfigSets.Clear();
			}
		}

		// Token: 0x04009DA3 RID: 40355
		public static Dictionary<int, ConfigSet> AllConfigSets = new Dictionary<int, ConfigSet>();
	}
}
