using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AF8 RID: 6904
	public class MusicGroup : EnumDictionary<ThreatLevelLegacyType, ThreatLevel>
	{
		// Token: 0x0600D012 RID: 53266 RVA: 0x004B4638 File Offset: 0x004B2838
		public MusicGroup(int _sampleRate, byte _hbLength)
		{
			this.SampleRate = _sampleRate;
			this.HBLength = _hbLength;
			this.ConfigIDs = new List<int>();
		}

		// Token: 0x0600D013 RID: 53267 RVA: 0x004B4659 File Offset: 0x004B2859
		public static void InitStatic()
		{
			MusicGroup.AllGroups = new List<MusicGroup>();
		}

		// Token: 0x0600D014 RID: 53268 RVA: 0x004B4665 File Offset: 0x004B2865
		public static void Cleanup()
		{
			if (MusicGroup.AllGroups != null)
			{
				MusicGroup.AllGroups.Clear();
			}
		}

		// Token: 0x04009D7D RID: 40317
		public static List<MusicGroup> AllGroups;

		// Token: 0x04009D7E RID: 40318
		public List<int> ConfigIDs;

		// Token: 0x04009D7F RID: 40319
		public readonly int SampleRate;

		// Token: 0x04009D80 RID: 40320
		public readonly byte HBLength;
	}
}
