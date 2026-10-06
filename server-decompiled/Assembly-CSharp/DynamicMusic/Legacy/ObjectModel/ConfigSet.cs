using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001ACE RID: 6862
	public class ConfigSet : EnumDictionary<ThreatLevelLegacyType, ThreatLevelConfig>
	{
		// Token: 0x0600CF1E RID: 53022 RVA: 0x004B1337 File Offset: 0x004AF537
		public static void Cleanup()
		{
			if (ConfigSet.AllConfigSets != null)
			{
				ConfigSet.AllConfigSets.Clear();
			}
		}

		// Token: 0x04009CBA RID: 40122
		public static Dictionary<int, ConfigSet> AllConfigSets = new Dictionary<int, ConfigSet>();
	}
}
