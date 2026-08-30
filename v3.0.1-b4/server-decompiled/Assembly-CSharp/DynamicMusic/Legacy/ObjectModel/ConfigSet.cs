using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A7B RID: 6779
	public class ConfigSet : EnumDictionary<ThreatLevelLegacyType, ThreatLevelConfig>
	{
		// Token: 0x0600CC3D RID: 52285 RVA: 0x004A6A77 File Offset: 0x004A4C77
		public static void Cleanup()
		{
			if (ConfigSet.AllConfigSets != null)
			{
				ConfigSet.AllConfigSets.Clear();
			}
		}

		// Token: 0x04009AC3 RID: 39619
		public static Dictionary<int, ConfigSet> AllConfigSets = new Dictionary<int, ConfigSet>();
	}
}
